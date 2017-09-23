using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.IO.IsolatedStorage;

namespace GeneralTests
{
	public partial class MainForm : Form, IResultSink
	{
		TestInvoker ti = null;

		[Serializable]
		public class TestState
		{
			public string TestName = string.Empty;
			public string MethodName = string.Empty;
			public TestState() { }
			public TestState(string test, string method)
			{
				TestName = test;
				MethodName = method;
			}
		}


		public MainForm()
		{
			InitializeComponent();
			ResultSinkFactory.Register(this);
		}

		private void InitTests()
		{
			ti = new TestInvoker(Assembly.GetExecutingAssembly());
			string[] tests = ti.GetTests();
			comboTests.Items.Clear();
			if (tests != null)
			{
				foreach (string t in tests.OrderBy(x => x))
					comboTests.Items.Add(t);
			}
		}

		const string storagefile = "gentest.st";

		private void StoreState()
		{
			try
			{
				
				TestState state = new TestState();
				if (comboTests.SelectedItem != null)
					state.TestName = comboTests.SelectedItem.ToString();
				if (comboMethods.SelectedItem != null)
					state.MethodName = comboMethods.SelectedItem.ToString();

				XmlSerializer ser = new XmlSerializer(typeof(TestState));
				IsolatedStorageFile isoStore =
					IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly,
					null, null);
				using (IsolatedStorageFileStream isfs =
					new IsolatedStorageFileStream(storagefile, FileMode.Create, isoStore))
				{
					ser.Serialize(isfs, state);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		private void RestoreState()
		{
			try
			{
				IsolatedStorageFile isoStore =
					IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly,
					null, null);
				XmlSerializer ser = new XmlSerializer(typeof(TestState));
				TestState ti = null;
				using (IsolatedStorageFileStream isfs =
					new IsolatedStorageFileStream(storagefile, FileMode.Open, isoStore))
				{
					ti = (TestState)ser.Deserialize(isfs);
				}
				comboTests.SelectedIndex = comboTests.FindStringExact(ti.TestName);
				comboMethods.SelectedIndex = comboMethods.FindStringExact(ti.MethodName);
			
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
			
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			InitTests();
			RestoreState();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			StoreState();
		}

		public void Send(string res)
		{
			textResults.Text += res + "\r\n";
		}

		public void SetResult(bool passed)
		{
			if (passed)
			{
				lbRes.Text = "OK";
				lbRes.BackColor = Color.LightGreen;
			}
			else
			{
				lbRes.Text = "FAILED";
				lbRes.BackColor = Color.Red;
			}
			// indicate that test was successful, or failed.
		}

		private void btRunTest_Click(object sender, EventArgs e)
		{
			SetResult(true);
			try
			{
				if (comboTests.SelectedItem == null || comboMethods.SelectedItem == null)
					return;
				ti.Invoke(comboTests.SelectedItem.ToString(), ((TestMethodInfo)comboMethods.SelectedItem).Name, this);
			}
			catch(Exception ex)
			{
				textResults.Text = ex.Message;
				if (ex.InnerException != null && ex.InnerException.Message != null)
					textResults.Text += ex.InnerException.Message;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			textResults.Clear();
		}

		private void comboTests_SelectedIndexChanged(object sender, EventArgs e)
		{
			comboMethods.Items.Clear();
			TestMethodInfo[] methods = ti.GetMethods(comboTests.SelectedItem.ToString());
			btRunTest.Enabled = false;
			btResetTest.Enabled = comboMethods.Enabled = (methods != null);
			if (methods == null)
				return;
			foreach (TestMethodInfo tmi in methods.OrderBy(x => x.Description))
				comboMethods.Items.Add(tmi);
		}

		private void btResetTest_Click(object sender, EventArgs e)
		{
			string item = comboTests.SelectedItem.ToString();
			ti.ResetTest(item);
		}

		private void comboMethods_SelectedIndexChanged(object sender, EventArgs e)
		{
			TestMethodInfo tmi = comboMethods.SelectedItem as TestMethodInfo;
			btRunTest.Enabled = (tmi != null);
		}
	}
}
