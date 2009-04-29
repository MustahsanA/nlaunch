using System.Collections.Generic;

namespace NLaunch
{
	public interface IFtpAccessor
	{
		IEnumerable<string> ListFiles();
		void Download(string remoteFilename, string localFilename, IDownloadNotificator notificator);
	}
}