using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GeneralTests
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

		static class ResultSinkFactory
		{
			class FakeSink : IResultSink
			{

				#region IResultSink Members

				public void Send(string res)
				{
					// ignore
				}

				public void SetResult(bool passed)
				{
					// ignore
				}

				#endregion
			}

			static IResultSink sink = new FakeSink();
			public static void Register(IResultSink sink_imp) { sink = sink_imp; }
			public static IResultSink Get() { return sink; }
		}
}
