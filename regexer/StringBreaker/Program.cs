// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

var inputString = @"P22101,TTM Technologies,"" $ 27,894 "",1/2/2020,1/2/2020,0,On-Time,Material,12/19/2019,  14 ,  -   ,1,5,"" $ 21,029 "","" $ 10,000 "","" $ 100,000 "",Comcast,215,1";

Console.WriteLine($"Input String: {inputString}");

// typescript regex : string.split("/,(?=(?:(?:[^"]"){2})[^"]*$)/")

var regex = new Regex("(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)");

var splitString = regex.Matches(inputString).ToList();

foreach (var str in splitString) Console.WriteLine(str);

Console.ReadKey();

var arr = inputString.Split("\"");
var arrResult = new List<string>();

foreach(var arrStr in arr)
{
    if (arrStr.Contains("$")) arrResult.Add(arrStr);
    else
    {
        var arrComma = arrStr.Split(",");
        arrResult.AddRange(arrComma);
    }
}

foreach(var str in arrResult) Console.WriteLine(str);

Console.ReadKey();