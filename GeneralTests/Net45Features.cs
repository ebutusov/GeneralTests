using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeneralTests
{
	[TestCase("New feautres in .NET 4.5")]
	class Net45Features
	{
		public void ResetTest() { }

		private string GetSomeStringTakesLongTime()
		{
			Thread.Sleep(5000);
			return "Sorry I'm soooo sloooow...";
		}

		[TestCaseMethod("Await")]
		public async void TestAwait(IResultSink sink)
		{
			sink.Send("Waiting for the result...");
			sink.SetResult(false);
			//string result = await Task<string>.Factory.StartNew(() => GetSomeStringTakesLongTime());
			Task<string> t1 = new Task<string>(() => GetSomeStringTakesLongTime());
			t1.Start();
			string result = await t1; 
			sink.Send(result);
			sink.SetResult(true);
		}
	}
}
