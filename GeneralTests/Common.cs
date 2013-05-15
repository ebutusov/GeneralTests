using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralTests
{
	[AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=false)]
	public class TestCaseAttribute : System.Attribute
	{
		private string name;
		public string Name
		{
			get { return name; }
		}
		public TestCaseAttribute(string testName)
		{
			name = testName;
		}
	}

	[AttributeUsage(AttributeTargets.Method, Inherited=false, AllowMultiple=false)]
	public class TestCaseMethodAttribute : System.Attribute
	{
		private string desc;

		public string Description
		{
			get { return desc; }
		}

		public TestCaseMethodAttribute(string description)
		{
			desc = description;
		}
	}

	public interface IResultSink
	{
		void Send(string res);
		void SetResult(bool passed);
	}

	public class TestMethodInfo
	{
		public string Name;
		public string Description;

		public TestMethodInfo(string name, string description)	
		{
			Name = name;
			Description = description;
		}

		public override string ToString()
		{
			return Description;
		}
	}
}
