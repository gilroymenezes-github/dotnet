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

namespace Business.Process.Classifications
{
    internal class PdfBlobProcessor : BackgroundService
    {
        const string inputFile = @"C:\\Users\\gilroy\\Downloads\\24 Sept part 2";
        const string regexString = @":?[A-Z]\d{2}[A-Z]\d{10},?";


        ILogger<PdfBlobProcessor> logger; 
        IBlobStorage blobStorage;
        CountsTableStore countsStore;
        bool isProcessed = default(bool);

        public PdfBlobProcessor(
            ILogger<PdfBlobProcessor> logger,
            IBlobStorage blobStorage,
            CountsTableStore countsStore
            )
        {
            this.blobStorage = blobStorage;
            this.countsStore = countsStore;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested && !isProcessed)
            {
                await Task.Delay(2000, stoppingToken);
                try
                {
                    var data = await blobStorage.DownloadBlobAsync("vihkh3vo.z0h");
                    var countModel = GetCountModelFromPdfData(data);
                    var countModelCreated =await countsStore.CreateItemAsync(countModel);
                }
                catch(Exception ex)
                {
                    logger.LogError(ex.Message);
                }
                finally
                {
                    isProcessed = true;
                }
            }
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            logger.LogInformation($"Received: {body}");

            //var stream = await blobStorage.DownloadBlobAsync("vihkh3vo.z0h");

            await args.CompleteMessageAsync(args.Message);
            
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            logger.LogError(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private CountModel GetCountModelFromPdfData(byte[] data)
        {
            var result = new Dictionary<string, int>() { { "A", 0 }, { "B", 0 }, { "C", 0 }, { "D", 0 }, { "E", 0 }, { "F", 0 }, { "G", 0 }, { "H", 0 } };

            using (var document = PdfDocument.Open(data))
            {
                foreach (Page page in document.GetPages())
                {
                    IEnumerable<Word> words = page.GetWords();
                    foreach (var kvp in UseApplicationNumber(words))
                    {
                        result[kvp.Key] += kvp.Value;
                    }
                }
            }
            var countModel = new CountModel();
            countModel = countModel.CreateFromCountModel("classifications", new DateTime(2021, 9, 24));
            countModel.TokenSeparatedFields = string.Join(countModel.TsvToken, result.Keys);
            countModel.TokenSeparatedValues = string.Join(countModel.TsvToken, result.Values);
            return countModel;
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
                    //var appNo = sb.ToString().Split(" A ")[0];
                    result = (Dictionary<string, int>)UseClassificationCode(words);
                }
                is21Start = string.Compare(word.Text, "(21)") == 0
                    ? true
                    : default(bool);
            }
            return result;
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
            string firstCharString = inputs
                .Where(s => !string.IsNullOrEmpty(s))
                .Aggregate("", (xs, x) => xs + x.First());
            //var totalCount = 0;

            IDictionary<string, int> result = new Dictionary<string, int>();
            foreach (var first in firstCharString.ToCharArray().Distinct())
            {
                //var count = inputs.Count(f => f.First() == first);
                result[first.ToString()] = 1;
                //totalCount += count;
                //if (totalCount < 5) continue;
            }
            return result;
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
