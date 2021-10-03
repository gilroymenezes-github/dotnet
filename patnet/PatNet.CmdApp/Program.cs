using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

const string inputFile = @"C:\\Users\\gilroy\\Downloads\\24 Sept part 2";
//const string regexString = @":?[A-Z]\d\d[A-Z]\d\d\d\d\d\d\d\d\d\d,?";
const string regexString = @":?[A-Z]\d{2}[A-Z]\d{10},?";

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


//using (StreamReader sr = new StreamReader($"{inputFile}.txt"))
//{
//    var ln = string.Empty;
//    while ((ln = sr.ReadLine()) != null) {
//        Console.WriteLine(ln);
//    }
//    sr.Close();
//}

//Console.WriteLine("Done!");

//Console.ReadKey();

using (PdfDocument document = PdfDocument.Open($"{inputFile}.pdf"))
{
    var result = new List<string>();
    foreach(Page page in document.GetPages()) {
        IEnumerable<Word> words = page.GetWords();
        result.Add(Check21Phrase(words));   
    }
    await File.WriteAllLinesAsync($"{inputFile}-out.txt", result);
}

Console.WriteLine("Done PDF!");

Console.ReadKey();

static string Check21Phrase(IEnumerable<Word> words)
{
    var is21Start = default(bool);
    var post21Count = default(int);
    var sb = new StringBuilder();
    var result = string.Empty;
    foreach (Word word in words)
    {
        if (is21Start)
        {
            sb.Append($" {word.Text} ");
            if (++post21Count < 3) continue;
            is21Start = default(bool);
            var appNo = sb.ToString().Split(" A ")[0];
            //Console.WriteLine(appNo);
            result = appNo + Check51Phrase(words);
        }
        is21Start = string.Compare(word.Text, "(21)") == 0
            ? true
            : default(bool);
    }
    return result;
}


static string Check51Phrase(IEnumerable<Word> words)
{
    var is51Start = default(bool);
    var is51End = default(bool);
    var items = new List<string>();
    var sb = new StringBuilder();
    foreach(Word word in words)
    {
        if (is51Start)
        {
            is51End = string.Compare(word.Text, "(57)") == 0
                ? true
                : default(bool);
            if (!is51End)
            {
                if (useRegex(word.Text))
                {
                    items.Add(useTrim(word.Text));
                }
                continue;
            }
            foreach(var kvp in useAggregateCount(items))
            {
                sb.Append($",{kvp.Key}:{kvp.Value}");
            }
        }
        is51Start = string.Compare(word.Text, "(54)") == 0
            ? true
            : default(bool);
    }
    return sb.ToString();
}

static bool useRegex(string input)
{
    Regex regex = new Regex(regexString, RegexOptions.IgnoreCase);
    return regex.IsMatch(input);
}

static string useTrim(string input)
    => input switch
    {
        { Length: 16 } => input.Split(',')[0].Split(':')[1],
        { Length: 15 } => input.Split(',')[0],
        { Length: 14 } => input,
        _ => string.Empty
    };

static IDictionary<string, int> useAggregateCount(IEnumerable<string> inputs)
{
    string firstCharString = inputs
        .Where(s => !string.IsNullOrEmpty(s))
        .Aggregate("", (xs, x) => xs + x.First());
    var totalCount = 0;

    IDictionary<string, int> result = new Dictionary<string, int>();
    foreach(var first in firstCharString.ToCharArray())
    {
        var count = inputs.Count(f => f.First() == first);
        result[first.ToString()] = count;
        totalCount += count;
        if (totalCount < 5) continue;
    }
    return result;
}
