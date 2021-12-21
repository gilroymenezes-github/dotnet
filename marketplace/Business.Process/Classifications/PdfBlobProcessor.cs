using Azure.Messaging.ServiceBus;
using Business.Shared.Models;
using Business.Shared.Repositories;
using Business.Shared.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using System.Globalization;
using System.Text.Json;

namespace Business.Process.Classifications
{
    internal class PdfBlobProcessor : BackgroundService
    {
        const string regexString = @":?[A-Z]\d{2}[A-Z]\d{10},?";
        const string validCharString = "ABCDEFGH";


        ILogger<PdfBlobProcessor> logger; 
        IBlobStorage blobStorage;
        CountsTableStore countsStore;
        FilesTableStore filesStore;

        public PdfBlobProcessor(
            ILogger<PdfBlobProcessor> logger,
            IBlobStorage blobStorage,
            CountsTableStore countsStore,
            FilesTableStore filesStore
            )
        {
            this.blobStorage = blobStorage;
            this.countsStore = countsStore;
            this.filesStore = filesStore;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(2000, stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateFiles();
            }
            logger.LogInformation("Processed files");
        }


        private async Task UpdateFiles()
        {
            var files = await filesStore.ReadItemsAsync();
            foreach (var file in files)
            {
                if (!string.IsNullOrEmpty(file.Status)) continue;
                await UpdateFileItemForCounts(file);
            }
        }

        private async Task UpdateFileItemForCounts(FileModel file)
        {
            try
            {
                var data = await blobStorage.DownloadBlobAsync(file.Name);
                await UpdateCounts(file.Name, data);
                file.Status = "processed";
                await filesStore.UpdateItemAsync(file);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            finally { }
        }

       
        private async Task UpdateCounts(string blobName, byte[] blobData)
        {
            var result = UpdateCountsFromData(blobData);
            var countModel = new CountModel() { Name = result.PublicationDate.ToShortDateString(), PublicationDate = result.PublicationDate.AddDays(1) };    // correct for UTC
            countModel = countModel.CreateFromCountModel();
            countModel.JsonData = JsonSerializer.Serialize(result.Counts);
            await countsStore.CreateItemAsync(countModel, blobName, "classifications");
        }


        private (DateTime PublicationDate, IDictionary<string, int> Counts) UpdateCountsFromData(byte[] data)
        {
            var result = new Dictionary<string, int>() { { "A", 0 }, { "B", 0 }, { "C", 0 }, { "D", 0 }, { "E", 0 }, { "F", 0 }, { "G", 0 }, { "H", 0 } };
            var hasPublicationDate = default(bool);
            var publicationDate = DateTime.MinValue;
            using (var document = PdfDocument.Open(data))
            {
                foreach (Page page in document.GetPages())
                {
                    logger.LogInformation($"Processing page {page.Number}");
                    IEnumerable<Word> words = page.GetWords();
                    if (!hasPublicationDate)
                    {
                        var response = UsePublicationDate(words);
                        publicationDate = response.PublicationDate;
                        hasPublicationDate = response.HasPublicationDate;
                    }
                    foreach (var kvp in UseApplicationNumber(words))
                    {
                        result[kvp.Key] += kvp.Value;
                    }
                }
            }
            return (publicationDate, result);
        }

        private IDictionary<string, int> UseApplicationNumber(IEnumerable<Word> words)
        {
            var is21Start = default(bool);
            var post21Count = default(int);
            var sb = new StringBuilder();
            var result = new Dictionary<string, int>();
            foreach (Word word in words)
            {
                if (is21Start)
                {
                    sb.Append($" {word.Text} ");
                    if (++post21Count < 3) continue;
                    is21Start = default(bool);
                    result = (Dictionary<string, int>)UseClassificationCode(words);
                }
                is21Start = string.Compare(word.Text, "(21)") == 0
                    ? true
                    : default(bool);
            }
            return result;
        }

        private (DateTime PublicationDate, bool HasPublicationDate) UsePublicationDate(IEnumerable<Word> words)
        {
            var validPublicationDateStrings = new List<string> { "Publication", "Date", ":" };
            var is43Start = default(bool);
            var post43Count = default(int);
            var publicationDate = DateTime.MinValue;
            var sb = new StringBuilder();   
            foreach (Word word in words)
            {
                if (is43Start)
                {
                    if (!validPublicationDateStrings.Contains(word.Text) && post43Count != 3) // ensures '(43) Publication Date : {date}'
                    {
                        is43Start = default(bool);
                        continue;
                    }
                    sb.Append(word.Text);
                    if (++post43Count < 4) continue;
                    publicationDate = DateTime.TryParseExact(word.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out publicationDate) 
                        ? publicationDate
                        : default(DateTime);
                    break;
                }
                is43Start = string.Compare(word.Text, "(43)") == 0
                    ? true
                    : default(bool);
            }
            return (publicationDate, is43Start);
        }


        private IDictionary<string, int> UseClassificationCode(IEnumerable<Word> words)
        {
            var is51Start = default(bool);
            var is51End = default(bool);
            var items = new List<string>();
            var result = new Dictionary<string, int>();
            foreach (Word word in words)
            {
                if (is51Start)
                {
                    is51End = string.Compare(word.Text, "(57)") == 0
                        ? true
                        : default(bool);
                    if (!is51End)
                    {
                        if (UseRegex(word.Text))
                        {
                            items.Add(UseTrim(word.Text));
                        }
                        continue;
                    }
                    result = (Dictionary<string, int>)UseAggregateCount(items);
                }
                is51Start = string.Compare(word.Text, "(54)") == 0
                    ? true
                    : default(bool);
            }
            return result;
        }

        private IDictionary<string, int> UseAggregateCount(IEnumerable<string> inputs)
        {
            var firstCharString = UseFirstCharString(inputs);            
            IDictionary<string, int> result = new Dictionary<string, int>();
            foreach (var first in firstCharString.ToCharArray().Distinct())
            {
                result[first.ToString()] = 1;
            }
            return result;
        }

        private string UseFirstCharString(IEnumerable<string> inputs)
        {
            var validKeyChars = validCharString.ToCharArray(); 
            string firstCharString = inputs
                .Where(s => !string.IsNullOrEmpty(s))
                .Aggregate("", (xs, x) => xs + x.First());
            var badFirstChar = firstCharString.ToCharArray().Where(c => !validKeyChars.Contains(c));
            if (badFirstChar.Any())
            {
                logger.LogInformation($"Attempting fix for first char in {firstCharString}");
                firstCharString = UseFirstCharStringWithFix(inputs);
            }
            return firstCharString;
        }

        private string UseFirstCharStringWithFix(IEnumerable<string> inputs)
        {
            var sb = new StringBuilder();
            foreach (var input in inputs)
            {
                if (string.IsNullOrEmpty(input)) continue;
                var firstChar = input.Take(1).First();
                if (validCharString.Contains(firstChar))
                {
                    sb.Append(firstChar);
                }
                else 
                {
                    var inputMinusFirst = input.Trim(firstChar);
                    firstChar = inputMinusFirst.Take(1).First();
                    if (validCharString.Contains(firstChar))
                    {
                        sb.Append(firstChar);
                    }
                    else sb.Append(firstChar);
                }
            }
            return sb.ToString();
        }

        private bool UseRegex(string input)
        {
            Regex regex = new Regex(regexString, RegexOptions.IgnoreCase);
            return regex.IsMatch(input);
        }

        private string UseTrim(string input)
            => input switch
            {
                { Length: 16 } => input.Split(',')[0].Split(':')[1],
                { Length: 15 } => input.Split(',')[0],
                { Length: 14 } => input,
                _ => string.Empty
            };
    }
}
