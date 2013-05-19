using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace GeneralTests
{
	[TestCase("Linq scratchpad")]
	class LinqScratchPad
	{
		[TestCaseMethod("List of processes")]
		public void ListProcesses(IResultSink res)
		{
			var processes =
				from p in Process.GetProcesses()
				orderby p.ProcessName, p.Id
				group p by p.ProcessName into g
				let tot_mem = g.Sum (p => (double)p.PrivateMemorySize64 / (1024 * 1024))
				orderby tot_mem descending
				select new { Name = g.Key, TotalMem = tot_mem };

			foreach (var pp in processes)
				res.Send(string.Format("{0,-20}{1,10:n2}", pp.Name, pp.TotalMem));
		}

		Task<IEnumerable<int>> GetPrimes(int min, int count)
		{
			return Task.Run(() =>
				(Enumerable.Range(min, count).Where
				(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i =>
					n % i > 0))).ToList().AsEnumerable());
		}

		[TestCaseMethod("GetPrimesAsync")]
		public async void GetPrimesAsync(IResultSink res)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			var result = await GetPrimes(1, 10000000);
			sw.Stop();
			res.Send("Elapsed: " + sw.ElapsedMilliseconds.ToString() + " ms");
			result.Take(100).ToList().ForEach(x => res.Send(x.ToString()));
		}
	}
}
