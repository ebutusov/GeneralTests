using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GeneralTests
{
	public partial class HundredRects : Form
	{
		public HundredRects()
		{
			InitializeComponent();
		}

		Rectangle[] rects = new Rectangle[100];
		Timer timer = new Timer();

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			Random rand = new Random(DateTime.Now.Millisecond);
			int x, y, w, h;
			for (int i = 0; i < rects.Length; ++i)
			{
				x = rand.Next(ClientRectangle.Width / 2);
				y = rand.Next(ClientRectangle.Width / 2);
				h = rand.Next(200);
				w = rand.Next(200);
				rects[i] = new Rectangle(x, y, w, h);
			}
			timer.Tick += new EventHandler(timer_Tick);
			timer.Interval = 1000;
		}

		void timer_Tick(object sender, EventArgs e)
		{
			Invalidate();
		}

		public void StartShow()
		{
			timer.Start();
		}

		public void EndShow()
		{
			timer.Stop();
		}

		private void HundredRects_Paint(object sender, PaintEventArgs e)
		{
			Random rand = new Random(Environment.TickCount);
			Predicate<Rectangle> area = rect =>
				(rect.Width * rect.Height) <= (rand.Next(200) * rand.Next(200));

			Rectangle[] matches = Array.FindAll(rects, area);
			e.Graphics.Clear(BackColor);
			foreach (var rect in matches)
				e.Graphics.DrawRectangle(Pens.Red, rect);
			e.Graphics.DrawString("Found: " + matches.Length.ToString(),
				Font, Brushes.Black, 10, 0);
		}
	}

	[TestCase("Hundred rectangles")]
	public class HundredRectsTest : IDisposable
	{
		HundredRects form = null;
		bool m_disposed = false;

		[TestCaseMethod("Start")]
		public void StartTest()
		{
			if (form == null)
			{
				form = new HundredRects();
				form.StartShow();
			}
			form.Show();
		}

		[TestCaseMethod("Stop")]
		public void StopTest()
		{
			form.EndShow();
			form.Close();
			form = null;
		}

		#region IDisposable Members

		protected virtual void Dispose(bool disposing)
		{
			if (!m_disposed)
			{
				form.Dispose();
			}
			m_disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion
	}
}
