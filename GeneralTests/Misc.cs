using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GeneralTests
{
	struct SimpleStruct
	{
		public int X;
		public int Y;
	}

	class X
	{
		Y b = new Y();
		public X() { ResultSinkFactory.Get().Send("X"); }
	}

	class Y
	{
		public Y() { ResultSinkFactory.Get().Send("Y"); }
	}

	class Z : X
	{
		Y y = new Y();
		public Z() { ResultSinkFactory.Get().Send("Z"); }
	}



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

	static class Swapper
	{
		public static void Swap<T>(ref T a, ref T b)
		{
			T tmp = b;
			b = a;
			a = tmp;
		}
	}

	static class QSort
	{
		private static void _sort(int[] table, int first, int last)
		{
			if (first >= last) return;
			int mid = table[first];
			int axis = first;
			for (int i = first+1; i <= last; ++i)
				if (table[i] < mid)
					Swapper.Swap(ref table[++axis], ref table[i]);
			Swapper.Swap(ref table[axis], ref table[first]);
			_sort(table, first, axis - 1);
			_sort(table, axis + 1, last);
		}

		public static void Sort(int[] table)
		{
			_sort(table, 0, table.Length - 1);
		}
	}

	static class Bubble
	{
		private class DefaultComparer<T> : IComparer<T> where T: IComparable
		{
			#region IComparer<T> Members

			public int Compare(T x, T y)
			{
				return y.CompareTo(x); // reverses order
			}

			#endregion
		}

		public static void Sort<T>(T[] table, IComparer<T> cmp = null) where T: IComparable
		{
			int l = table.Length;
			if (cmp == null) cmp = new DefaultComparer<T>();
			while (l > 0)
			{
				for (int p = 1; p < l; ++p)
				{
					if (cmp.Compare(table[p - 1], table[p]) == 1)
						Swapper.Swap(ref table[p - 1], ref table[p]);
				}
				--l;
			}
		}
	}


	[TestCase("Misc tests")]
	class Misc
	{
		[TestCaseMethod("Bubble Sort Test")]
		public void TestSort(IResultSink irs)
		{
			int[] tbl = { 100, 4, 1, 10, 33, 3, 45, 18, 78, 0 };
			Bubble.Sort(tbl);
			for (int i = 0; i < tbl.Length; ++i)
				irs.Send(tbl[i].ToString());
		}

		[TestCaseMethod("QuickSort Test")]
		public void TestQuickSort(IResultSink irs)
		{
			int[] tbl = new int[100];
			Random rnd = new Random();
			for (int i = 0; i < tbl.Length; ++i)
				tbl[i] = rnd.Next(0, 1000);
			QSort.Sort(tbl);
			foreach (int k in tbl)
				irs.Send(k.ToString());
		}

		[TestCaseMethod("Primes Test")]
		public void TestPrimes(IResultSink irs)
		{
			const int len = 1000;
			bool[] marks = new bool[len];
			for (int i = 0; i < len; ++i)
				marks[i] = true;

			for (int i = 2; i < Math.Sqrt(len); ++i)
			{
				if (marks[i-1] == false) continue;

				int k = 2;
				while (i * k <= len)
					marks[(k++ * i)-1] = false;
			}
			for (int i = 0; i < len; ++i)
			{
				if (marks[i] == true)
					irs.Send((i+1).ToString());
			}

		}

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

		[TestCaseMethod("Inheritance test")]
		public void TestInheritance(IResultSink irs)
		{
			Z z = new Z();
		}

		[TestCaseMethod("Struct test")]
		public void TestStruct(IResultSink irs)
		{
			// creates new struct, copies it to s
			// default constructor initializes all fields
			SimpleStruct s = new SimpleStruct();
			irs.Send(s.X.ToString());

			SimpleStruct a;
			// a.X is unassigned here (would not compile)			
			//irs.Send(a.X.ToString());

			a.X = 1;
			a.Y = 2;
			
			// fields assigned, compiles ok
			irs.Send(a.X.ToString());
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
