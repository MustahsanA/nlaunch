using System.IO;
using System.Threading;

namespace NLaunch.Impl
{
	public class FtpUpdatesProvider : IUpdatesProvider
	{
		private readonly LauncherConfiguration configuration;
		private readonly IFtpAccessor ftp;

		public FtpUpdatesProvider(LauncherConfiguration configuration, IFtpAccessor ftp)
		{
			this.configuration = configuration;
			this.ftp = ftp;
		}

		public string GetLastVersionFilename(INamingConvention namingConvention)
		{
			var fileList = ftp.ListFiles() ?? new string[] {};
			var lastNewFilename = namingConvention.GetLastVersionFilename(fileList, configuration);
			return lastNewFilename;
		}

		public string Download(string filename, IDownloadNotificator notificator)
		{
			var localFilename = Path.GetTempPath() + filename;
			if (File.Exists(localFilename))
				File.Delete(localFilename);

			notificator.Show();

			ThreadPool.QueueUserWorkItem(x => ftp.Download(filename, localFilename, notificator));

			notificator.WaitForCompletition();

			notificator.Close();

			return localFilename;
		}

	}
}