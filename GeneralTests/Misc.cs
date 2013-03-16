using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GeneralTests
{

	partial class SomeBaseClass<T>
		where T : SomeBaseClass<T>
	{
		public void CallMethod(IResultSink irs)
		{
			//OnCallMethod(irs);
			//T t = (T)(object)this;
			//((T)this).OnCallmethod();
			dynamic t = this;
			t.OnCallMethod(irs);
		}
	}

	partial class SomeOtherClass : SomeBaseClass<SomeOtherClass>
	{
		public void OnCallMethod(IResultSink irs)
		{
			irs.Send("OnCallMethod called");
		}
	}

	class Callable
	{
		public void Call(IResultSink irs)
		{
			irs.Send("Callable method Call called");
		}
	}

	public static class DynamicExtension
	{
		public static void CallDynamic(this Object obj, string method, IResultSink irs)
		{
			Type delType = typeof(Action<IResultSink>);
			Type objType = obj.GetType();
			MethodInfo mi = objType.GetTypeInfo().GetDeclaredMethod(method);
			Delegate d = mi.CreateDelegate(delType, obj);
			d.DynamicInvoke(irs);
		}
	}

	[TestCase("Misc tests")]
	class Misc
	{
		[TestCaseMethod("Dynamic extension")]
		public void TestDynExt(IResultSink irs)
		{
			Callable c = new Callable();
			c.CallDynamic("Call", irs);
		}

		[TestCaseMethod("WTL test")]
		public void TestWTL(IResultSink irs)
		{
			SomeOtherClass soc = new SomeOtherClass();
			soc.CallMethod(irs);
		}

		[TestCaseMethod("BitConverter test")]
		public void TestBitConv(IResultSink irs)
		{
			int i = 0xAABB; // AA=170, BB=187
			byte[] bytes = new byte[4];
			bytes[0] = 0xBB;
			bytes[1] = 0xAA;
			int k = (int)(bytes[0] << 0 | bytes[1] << 8);
			irs.Send(k.ToString());
			//byte[] bytes = BitConverter.GetBytes(i);
		}
	}
}
