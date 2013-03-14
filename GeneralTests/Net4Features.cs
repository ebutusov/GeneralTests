using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Contracts;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GeneralTests
{
	[TestCase("New features in .NET 4.0")]
	class Net4Features
	{
		public void ResetTest()
		{

		}

		int Factorial(int x)
		{
			int ret = 1;
			for (int i = 1; i <= x; ++i)
				ret *= i;
			// simulate complex calculations
			System.Threading.Thread.Sleep(10);
			return ret;
		}

		[TestCaseMethod("Tasks")]
		public void TestTasks(IResultSink res)
		{
			Task<int> t1 = new Task<int>(() => Factorial(5));
			Task<int> t2 = new Task<int>(() => Factorial(10));
			t1.Start(); t2.Start();

			Task.WaitAll(t1, t2);
			res.Send("Factorials: " + t1.Result.ToString() + " " + t2.Result.ToString());
		}

		[TestCaseMethod("Parallel For")]
		public void TestParallelFor(IResultSink res)
		{
			int[] tbl = new int[20];
			Stopwatch psw = new Stopwatch();
			psw.Start();
			Parallel.For(1, 21, i => tbl[i-1] = Factorial(i));
			psw.Stop();
			foreach (int i in tbl)
				res.Send(i.ToString());
			res.Send("Parallelized: " + psw.Elapsed.Milliseconds.ToString());
			psw = Stopwatch.StartNew();
			for (int i = 1; i <= 20; ++i)
				tbl[i - 1] = Factorial(i);
			psw.Stop();
			res.Send("Non-parallelized: " + psw.Elapsed.Milliseconds.ToString());
		}

		[TestCaseMethod("Contracts")]
		public void TestContracts(IResultSink res)
		{
			res.SetResult(false);
		}

		[TestCaseMethod("Default args")]
		public void TestDefaultArgs(IResultSink res)
		{
			Peter p = new Peter();
			p.SetSink(res);
			p.TellYourAge(); // expected -1, result: -1
			p.DoSomething(); // expected -1, result: 0

			Fred f = new Fred();
			f.SetSink(res);
			f.TellYourAge(1); // expected 1, result: 1
			f.DoSomething(); // expected 1, result: 1

		}

		public abstract class Person
		{
			protected IResultSink sink;
			public void SetSink(IResultSink sink) { this.sink = sink; }
			public abstract void TellYourAge(int age); // abstract method without default value
		}

		public class Peter : Person
		{
			public override void TellYourAge(int age = -1) // override with default value
			{
				sink.Send("Peter: " + age);
			}

			public void DoSomething()
			{
				TellYourAge();
			}
		}

		public class Fred : Person
		{
			public override void TellYourAge(int age) // override without default value
			{
				sink.Send("Fred: " + age);
			}

			public void DoSomething()
			{
				TellYourAge(1);
			}
		}


		[TestCaseMethod("Copy stream")]
		public void TestCopyStream(IResultSink res)
		{
			using (MemoryStream ms = new MemoryStream())
			using (FileStream fs = File.Open(@"..\..\test.txt", FileMode.Open))
			{
				fs.CopyTo(ms);
				ms.Seek(0, SeekOrigin.Begin);
				using (StreamReader sr = new StreamReader(ms))
				{
					res.Send(sr.ReadToEnd());
				}
			}
		}
	}
}
