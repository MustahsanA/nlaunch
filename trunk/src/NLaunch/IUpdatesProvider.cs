namespace NLaunch
{
	public interface IUpdatesProvider
	{
		string GetLastVersionFilename(INamingConvention namingConvention);
		string Download(string filename, IDownloadNotificator notificator);
	}
}