using System;
using System.Threading;
using System.Windows.Forms;

namespace NLaunch
{
	public partial class DownloadProgressForm : Form, IDownloadNotificator
	{
		private readonly AutoResetEvent downloadComplete = new AutoResetEvent(false);

		public DownloadProgressForm()
		{
			InitializeComponent();
		}

		public void UpdateProgress(int perc)
		{
			invokeIfRequired(() =>
				{
					progressBar1.Value = perc;
				});
		}

		public void Complete()
		{
			downloadComplete.Set();
		}

		public void WaitForCompletition()
		{
			while (!downloadComplete.WaitOne(200))
			{
				Application.DoEvents();
			}
		}

		private void invokeIfRequired(Action action)
		{
			if (InvokeRequired)
			{
				BeginInvoke(action);
			}
			else
			{
				action();
			}
		}
	}
}
