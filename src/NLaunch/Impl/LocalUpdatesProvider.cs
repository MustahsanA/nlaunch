using System.IO;
using System.Linq;

namespace NLaunch.Impl
{
	public class LocalUpdatesProvider : IUpdatesProvider
	{
		private readonly LauncherConfiguration configuration;

		public LocalUpdatesProvider(LauncherConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public string GetLastVersionFilename(INamingConvention namingConvention)
		{
			var fileList = Directory.GetFiles(configuration.UpdatesLocation)
				.Select(x => Path.GetFileName(x));
			return namingConvention.GetLastVersionFilename(fileList, configuration);
		}

		public string Download(string filename, IDownloadNotificator notificator)
		{
			var sourcePath = Path.Combine(configuration.UpdatesLocation, filename);
			var targetPath = Path.Combine(Path.GetTempPath(), filename);
			File.Copy(sourcePath, targetPath, true);
			return targetPath;
		}
	}
}