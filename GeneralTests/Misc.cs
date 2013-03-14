using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralTests
{

	class SomeBaseClass<T>
	{
		public void CallMethod(IResultSink irs)
		{
			dynamic t = this;
			t.OnCallMethod(irs);
		}
	}

	class SomeOtherClass : SomeBaseClass<SomeOtherClass>
	{
		public void OnCallMethod(IResultSink irs)
		{
			irs.Send("OnCallMethod called");
		}
	}

	[TestCase("Misc tests")]
	class Misc
	{
		[TestCaseMethod("WTL test")]
		public void TestWTL(IResultSink irs)
		{
			SomeOtherClass soc = new SomeOtherClass();
			soc.CallMethod(irs);
		}
	}
}
