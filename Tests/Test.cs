using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeneralTests;

namespace Tests
{
	[TestClass]
	public class Test
	{
		[TestMethod]
		public void TestShiftArray()
		{
			GeneralTests.ShiftArray<char> arr = new GeneralTests.ShiftArray<char>("tapir".ToArray());
			arr.ShiftLeft(0);

			string a = new string(arr.Get());
			Assert.AreEqual(a, "apirt");
			
			arr.ShiftLeft(1);
			// should be 'airtp'
			a = new string(arr.Get());
			Assert.AreEqual(a, "airtp");
		}

	}
}
