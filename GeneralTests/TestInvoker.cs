// 
//	Simple test framework.
//	---------------------------------------------
// 	E.Butusov <ebutusov@gmail.com> (24.07.2009)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;


namespace GeneralTests
{
	class TestInvoker
	{
		private class MetaMethod
		{
			public string Name;
			public string Description;
			public bool AcceptsSink = false;
		}

		private class TestCell
		{
			public string TestName;
			public Type TestType;
			public object Instance;
			public Dictionary<string, MetaMethod> TestMethods = new Dictionary<string, MetaMethod>();
		}

		Assembly asm = null;
		Dictionary<string, TestCell> testCells = new Dictionary<string, TestCell>();

		public TestInvoker(Assembly assembly)
		{
			asm = assembly;
			ReadTests();
		}

		private void ReadTests()
		{
			foreach (Type t in asm.GetTypes())
			{
				if (t.IsClass)
				{
					object[] attrs = t.GetCustomAttributes(typeof(TestCaseAttribute), false);
					if (attrs.Length == 0) continue;
					TestCaseAttribute attr = (TestCaseAttribute)attrs[0];
					TestCell cell = new TestCell() { TestType = t, TestName = attr.Name };

					foreach (MethodInfo mi in t.GetMethods())
					{
						object[] mattrs = mi.GetCustomAttributes(typeof(TestCaseMethodAttribute), false);
						if (mattrs.Length == 0) 
							continue;
						TestCaseMethodAttribute mattr = (TestCaseMethodAttribute)mattrs[0];
						ParameterInfo[] pi = mi.GetParameters();
						MetaMethod mm = new MetaMethod() { Name = mi.Name, Description = mattr.Description, AcceptsSink = false };
						if (pi.Length == 1 && pi[0].ParameterType == typeof(IResultSink))
							mm.AcceptsSink = true;
						cell.TestMethods.Add(mm.Name, mm);
					}
					if (cell.TestMethods.Count > 0)
						testCells.Add(attr.Name, cell);
				}
			}
		}

		public string[] GetTests()
		{
			if (testCells.Count == 0)
				return null;
			string[] ret = new string[testCells.Count];
			int i = 0;
			foreach (string s in testCells.Keys)
				ret[i++] = s;
			return ret;
		}

		public TestMethodInfo[] GetMethods(string testName)
		{
			TestCell tc = testCells[testName];
			if (tc.TestMethods.Count == 0) return null;
			TestMethodInfo[] methods = new TestMethodInfo[tc.TestMethods.Count];
			int i = 0;
			foreach (MetaMethod met in tc.TestMethods.Values)
				methods[i++] = new TestMethodInfo(met.Name, met.Description);
			return methods;
		}

		public void Invoke(string testName, string testMethod, IResultSink sink)
		{
			TestCell t = testCells[testName];
			if (t.Instance == null)
				t.Instance = t.TestType.GetConstructor(Type.EmptyTypes).Invoke(null);
			MetaMethod mm = t.TestMethods[testMethod];
			t.TestType.InvokeMember(testMethod, BindingFlags.InvokeMethod, 
				Type.DefaultBinder, t.Instance, mm.AcceptsSink ? new object[] { sink } : null);
		}

		public void ResetTest(string testName)
		{
			TestCell t = testCells[testName];
			if (t.Instance == null) return;
			dynamic c = t.Instance;
			try
			{
				c.ResetTest();
			}
			catch {}
		}
	}
}
