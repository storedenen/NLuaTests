using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLua;
using Bogus;

var faker = new Faker();
var state = new Lua();

var formula = @"
	function IsEqual (val1, val2)
		return val1 == val2
	end";

state.DoString(formula);

var scriptFunc = state["IsEqual"] as LuaFunction;

var stopWatch = Stopwatch.StartNew();

var stringTestCounts = 10_000;
var intTestCounts = 10_000;
var customFormulaTestCounts = 10_000;
var results = new List<object?>();

for (int i = 0; i < stringTestCounts; i++)
{
	var res = scriptFunc?.Call(faker.Random.String2(5), faker.Random.String2(5)).First();
	results.Add(res);
}

stopWatch.Stop();
Console.WriteLine($"String tests took {stopWatch.ElapsedMilliseconds} ms with {stringTestCounts} iteration");

stopWatch.Restart();
for (int i = 0; i < intTestCounts; i++)
{
	var res = scriptFunc?.Call(faker.Random.Int(0, 10), faker.Random.Int(0, 10)).First();
	results.Add(res);
}

stopWatch.Stop();
Console.WriteLine($"Int tests took {stopWatch.ElapsedMilliseconds} ms with {intTestCounts} iteration");

var testFormula = "signal == 'test'";
stopWatch.Restart();
for (int i = 0; i < customFormulaTestCounts; i++)
{
	var newFormulaId = Guid.NewGuid().ToString("N");
	var newFormula = $"function Formula{newFormulaId}(signal)\n\treturn {testFormula}\nend";
	state.DoString(newFormula);
	var res = scriptFunc?.Call(faker.Random.String2(10)).First();
	results.Add(res);
}

stopWatch.Stop();
Console.WriteLine($"Custom formula tests took {stopWatch.ElapsedMilliseconds} ms with {customFormulaTestCounts} iteration");

Console.WriteLine($"Result count: {results.Count}");
Console.WriteLine($"Null values in result: {results.Count(x => x == null)}");

Console.WriteLine("Hit any key to exit...");
Console.ReadKey();



