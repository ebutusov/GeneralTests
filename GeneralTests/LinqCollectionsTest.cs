using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Linq.Expressions;

namespace GeneralTests
{

	public static class StringExtensions
	{
		public static IEnumerable<string> AllPermutations(this IEnumerable<char> s)
		{
			return s.SelectMany(x =>
			{
				var index = Array.IndexOf(s.ToArray(), x);
				return s.Where((y, i) =>
					i != index).AllPermutations().Select(y =>
						new string(new[] { x }.Concat(y).ToArray())).Union(new[] { new string(new[] { x }) });
			}).Distinct();
		}
	}

	[TestCase("Query the array")]
	public class LinqCollectionsTest
	{

		[TestCaseMethod("Empty method")]
		public void EmptyMethod(IResultSink res)
		{
		}

		private class Person
		{
			public string Name;
			public int Age;
		}

		private class AgeT
		{
			public int Age;
		}

		Person[] persons = {
												 new Person { Age = 16, Name = "Pierwszy" },
												 new Person { Age = 16, Name = "Drugi" },
												 new Person { Age = 17, Name = "Trzeci" },
												 new Person { Age = 17, Name = "Czwarty" },
												 new Person { Age = 18, Name = "Piaty" },
											 };

		AgeT[] ages = { 
									 new AgeT { Age = 18 }, 
									 new AgeT { Age = 17 }
								 };

	  string[] greets = { "Hi", "Hello", "Guten Tag", "Dzien dobry", "Hi", "Hello" };

		int[] ints = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

		[TestCaseMethod("Parallel Array Query")]
		public void TestParallelArray(IResultSink res)
		{
			var q = from p in persons.AsParallel()
							where p.Age < 18
							select p;
			foreach (var p in q)
				res.Send("Person: " + p.Name + " " + p.Age);
		}

		[TestCaseMethod("Group join")]
		public void TestGroupJoin(IResultSink res)
		{
			var gj = from a in ages
							 join p in persons on a.Age equals p.Age into g
							 select new { a.Age, Count = g.Count() };

			foreach (var g in gj)
				res.Send("Age: " + g.Age + " has count: " + g.Count);
		}

		[TestCaseMethod("Sum evens")]
		public void TestQueryInts(IResultSink res)
		{
			var sums =
					from e in ints
					group e by (e % 2 == 0) into g
					select new { Key = g.Key, Total = g.Sum() };

	    foreach(var sum in sums)
				res.Send(sum.Key.ToString() + " has sum: " + sum.Total.ToString());
		}

		[TestCaseMethod("Show array")]
		public void ListGreets(IResultSink res)
		{
			foreach (string s in greets)
				res.Send(s);
		}

		[TestCaseMethod("Query all > 2")]
		public void QueryGreaterThanTwo(IResultSink res)
		{
			var items =
			from greet in greets
			where greet.Length > 2
			select greet;

			foreach (var item in items)
				res.Send("Greet: " + item);
		}

		[TestCaseMethod("Display sorted greets")]
		public void DisplaySorted(IResultSink res)
		{
			string[] sorted = greets.Select(s => s).OrderBy(s => s).ToArray();
			foreach (string s in sorted)
				res.Send(s);
		}

		[TestCaseMethod("Select many")]
		public void SelectManyTest(IResultSink res)
		{
			var t = greets.SelectMany(g => g.ToArray());
			foreach (var x in t)
				res.Send(x.ToString());
		}

		[TestCaseMethod("Get distinct values")]
		public void GetDistictTest(IResultSink res)
		{
			var dist = from g in greets.Distinct()
								 select g;
			foreach (var x in dist)
				res.Send(x.ToString());
		}

		[TestCaseMethod("TakeWhile")]
		public void TakeWhileTest(IResultSink res)
		{
			var t = greets.TakeWhile(x => x.StartsWith("G"));
			foreach (var x in t)
				res.Send(x.ToString());
		}

		[TestCaseMethod("Enumerable generation")]
		public void TestGenerateSequence(IResultSink res)
		{
			var f = Enumerable.Range(1, 20);
			foreach (int x in f)
				res.Send(x.ToString());
		}

		public IEnumerable<string> GetVariations(int range)
		{
			for (int c = 0; c < range; ++c)
			{
				yield return new string(c.ToString().Select(x =>
					(char)('A' + int.Parse(x.ToString()))).ToArray());
			}
		}

		[TestCaseMethod("Variations Base System")]
		public void TestVariationBase(IResultSink res)
		{
			//res.Send ( char.ConvertFromUtf32('A' + 1).ToString());
			foreach(var r in GetVariations(99))
				res.Send(r);
			
		}

		[TestCaseMethod("Variations with repetition")]
		public void TestVariations(IResultSink res)
		{
			foreach (var p in "abdc".AllPermutations())
				res.Send(p);
		}
	}

	[TestCase("Anoumous and Generics")]
	public class AnonymousAndGenericsTest
	{
		[TestCaseMethod("Recursive generic anonymous Factorial")]
		public void TestRecursiveAnonymous(IResultSink res)
		{
			Func<long, long> Factorial =
				x => (x > 1 ? x * (long)(new StackTrace().GetFrame(0).GetMethod().Invoke(null, new object[] { x-1 })) : x);

			res.Send(Factorial(5).ToString());
		}

		[TestCaseMethod("Recursive Fibonacci")]
		public void TestFib(IResultSink res)
		{
			Func<long, long> Fib =
				x => x < 2 ? x : 
					((long)(new StackTrace().GetFrame(0).GetMethod().Invoke(null, new object[] { x-1 }))) +
					((long)(new StackTrace().GetFrame(0).GetMethod().Invoke(null, new object[] { x-2 })));
			for (int i = 0; i < 10; ++i)
				res.Send(Fib(i).ToString());
		}
	}	

	[TestCase("Partial methods")]
	public partial class PartialMethodsTest
	{
		int count = 0;
		partial void PMethodOne();
		partial void PMethodTwo();

		partial void PMethodTwo()
		{
			++count;
		}

		public void ResetTest()
		{
			count = 0;
		}

		[TestCaseMethod("Call absent partial")]
		public void CallAbsentPartial(IResultSink res)
		{
			PMethodOne();
			res.Send(count.ToString());
		}

		[TestCaseMethod("Call implemented partial")]
		public void CallImplPartial(IResultSink res)
		{
			PMethodTwo();
			res.Send(count.ToString());
		}
	}

	[TestCase("Expression trees")]
	public class ExpressionTreeTest
	{
		[TestCaseMethod("Create and show tree")]
		public void TestShowTree(IResultSink res)
		{
			Expression<Func<int, bool>> expEven = x => x % 2 == 0;
			res.Send("Code :" + expEven.ToString());
			res.Send("Exp body: " + expEven.Body.ToString());
			res.Send("Exp node type: " + expEven.NodeType.ToString());
			Func<int, bool> realExp = expEven.Compile();
			res.Send("Calling compiled on (2): " + realExp(2).ToString());
		}

		[TestCaseMethod("Create dynamic exression")]
		public void TestDynamicExpression(IResultSink res)
		{
			

		}
	}
}
