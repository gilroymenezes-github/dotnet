using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

const string inputFile = @"C:\\Users\\gilroy\\Downloads\\24 Sept part 2";
//const string regexString = @":?[A-Z]\d\d[A-Z]\d\d\d\d\d\d\d\d\d\d,?";
const string regexString = @":?[A-Z]\d{2}[A-Z]\d{10},?";

Console.WriteLine("Started Process");

using (PdfDocument document = PdfDocument.Open($"{inputFile}.pdf"))
{
    var result = new Dictionary<string, int>() { { "A", 0}, { "B", 0}, { "C", 0}, { "D", 0}, { "E", 0}, { "F", 0}, { "G", 0}, { "H", 0} };
    var output = new List<string>();
    foreach(Page page in document.GetPages()) {
        IEnumerable<Word> words = page.GetWords();
        foreach(var kvp in UseApplicationNumber(words))
        {
            result[kvp.Key] += kvp.Value;
        }
    }
    foreach(var kvp in result)
    {
        output.Add($"{kvp.Key}:{kvp.Value}");
    }
    await File.WriteAllLinesAsync($"{inputFile}-out.txt", output);
}

Console.WriteLine("Completed Process");


static IDictionary<string, int> UseApplicationNumber(IEnumerable<Word> words)
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


static IDictionary<string, int> UseClassificationCode(IEnumerable<Word> words)
{
    var is51Start = default(bool);
    var is51End = default(bool);
    var items = new List<string>();
    var result = new Dictionary<string, int>();
    foreach(Word word in words)
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

static IDictionary<string, int> UseAggregateCount(IEnumerable<string> inputs)
{
    string firstCharString = inputs
        .Where(s => !string.IsNullOrEmpty(s))
        .Aggregate("", (xs, x) => xs + x.First());
    //var totalCount = 0;

    IDictionary<string, int> result = new Dictionary<string, int>();
    foreach(var first in firstCharString.ToCharArray().Distinct())
    {
        //var count = inputs.Count(f => f.First() == first);
        result[first.ToString()] = 1;
        //totalCount += count;
        //if (totalCount < 5) continue;
    }
    return result;
}

static bool UseRegex(string input)
{
    Regex regex = new Regex(regexString, RegexOptions.IgnoreCase);
    return regex.IsMatch(input);
}

static string UseTrim(string input)
    => input switch
    {
        { Length: 16 } => input.Split(',')[0].Split(':')[1],
        { Length: 15 } => input.Split(',')[0],
        { Length: 14 } => input,
        _ => string.Empty
    };