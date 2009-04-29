namespace NLaunch
{
	public interface IDownloadNotificator
	{
		void UpdateProgress(int perc);
		void Show();
		void Close();
		void Complete();
		void WaitForCompletition();
	}
}