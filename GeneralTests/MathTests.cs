using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GeneralTests
{
	public class ShiftArray<T>
	{
		T[] m_arr;

		public ShiftArray(T[] arr) { m_arr = arr; }

		public T[] Get() { return m_arr; }

		public int Length
		{
			get { return m_arr.Length; }
		}

		public void ShiftLeft(int prefix)
		{
			if (m_arr.Length - prefix < 2)
				return;

			T first = m_arr[prefix];

			if (m_arr.Length == 2 && prefix == 0)
			{
				m_arr[0] = m_arr[1];
				m_arr[1] = first;
				return;
			}

			for (int i = 0; i < m_arr.Length - prefix - 1; ++i)
				m_arr[prefix+i] = m_arr[prefix+1+i];
			m_arr[m_arr.Length-1] = first;
		}
	}


	public class PermEnumerator<T> : IEnumerator<T[]>
	{
		private Permutator<T> m_gen;

		public void Dispose() { }

		public PermEnumerator(Permutator<T> gen)
		{
			m_gen = gen;
		}

		#region IEnumerator<T> Members

		public T[] Current
		{
			get { return m_gen.GetNext(); }
		}

		#endregion

		#region IEnumerator Members

		object IEnumerator.Current
		{
			get { return m_gen.GetNext(); }
		}

		public bool MoveNext()
		{
			// if HasNext is true, next permutation was already calculated
			return m_gen.HasNext;
		}

		public void Reset()
		{
			m_gen.Reset();
		}

		#endregion
	}

	public class Permutator<T> : IEnumerable<T[]>
	{
		int[] m_indices;
		T[] m_state, m_orig;
		Int64 m_total = 0, m_left = 0;

		public Permutator(T[] arr)
		{
			m_orig = new T[arr.Length];
			arr.CopyTo(m_orig,0);
			m_state = new T[m_orig.Length];
			m_indices = new int[m_orig.Length];
			m_total = Factorial(m_orig.Length);
			Reset();
		}

		public void Reset()
		{
			m_orig.CopyTo(m_state, 0);
			for (int i = 0; i < m_orig.Length; ++i)
				m_indices[i] = i;
			m_left = m_total;
		}

#if DEBUG
		public void ShowIndices()
		{
			StringBuilder sb = new StringBuilder("Indices: ");
			foreach (int i in m_indices)
				sb.Append(i.ToString() + ",");
			Debug.WriteLine(sb.ToString());
		}
#endif

		private Int64 Factorial(int n)
		{
			Int64 ret = 1;
			for (int i = 1; i <= n; ++i)
				ret *= i;
			return ret;
		}

		public bool HasNext
		{
			get
			{
				return m_left > 0;
			}
		}

		public T[] GetNext()
		{
			T[] ret = new T[m_state.Length];
			m_state.CopyTo(ret,0);
			if (HasNext)
			{
				NextPermutation();
				--m_left;
			}
			return ret;
		}

		private void NextPermutation()
		{
			// Find largest index j with a[j] < a[j+1]

			int j = m_indices.Length - 2;
			while (j >= 0 && m_indices[j] > m_indices[j + 1])
				--j;

			if (j < 0)
				j = 0;
			// Find index k such that a[k] is smallest integer
			// greater than a[j] to the right of a[j]

			int k = m_indices.Length - 1;
			while (k >= 0 && m_indices[j] > m_indices[k])
				--k;

			if (k < 0)
				k = 0;
			// Interchange a[j] and a[k]

			int temp = m_indices[k];
			m_indices[k] = m_indices[j];
			m_indices[j] = temp;

			// Put tail end of permutation after jth position in increasing order

			int r = m_indices.Length - 1;
			int s = j + 1;

			while (r > s)
			{
				temp = m_indices[s];
				m_indices[s] = m_indices[r];
				m_indices[r] = temp;
				r--;
				s++;
			}

			// reorganize the state array
			for (int i = 0; i < m_indices.Length; ++i)
				m_state[i] = m_orig[m_indices[i]];
			
		}

		#region IEnumerable<T> Members

		public IEnumerator<T[]> GetEnumerator()
		{
			return (IEnumerator<T[]>)new PermEnumerator<T>(this);
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator)new PermEnumerator<T>(this);
		}

		#endregion
	}

	[TestCase("Mathematics and algorithm tests")]
	public class MathTests
	{

		[TestCaseMethod("Permutate")]
		public void TestPermuatations(IResultSink res)
		{
			Permutator<char> p = new Permutator<char>("abc".ToArray());
			foreach(char[] arr in p)
				res.Send(new string(arr) + "\n");
		}
	}
}
