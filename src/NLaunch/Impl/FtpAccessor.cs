using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using log4net;

namespace NLaunch.Impl
{
	public class FtpAccessor : IFtpAccessor
	{
		private static readonly ILog log = LogManager.GetLogger(typeof (FtpAccessor));

		private readonly Uri serverUri;

		public FtpAccessor(string stringUri)
		{
			serverUri = new Uri(stringUri);
		}

		public IEnumerable<string> ListFiles()
		{
			FtpWebResponse response = null;

			log.Debug("Listing files from " + serverUri);

			try
			{
				var request = WebRequest.Create(serverUri);
				request.Method = WebRequestMethods.Ftp.ListDirectory;

				response = (FtpWebResponse) request.GetResponse();
				var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

				var files = responseString.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

				return files;
			}
			catch (WebException ex)
			{
				log.Warn("Cannot list files", ex);
				throw new UpdatesProviderInaccessibleException("Cannot list files", ex);
			}
			finally
			{
				if (response != null) response.Close();
			}
		}

		public void Download(string remoteFilename, string localFilename, IDownloadNotificator notificator)
		{
			FtpWebResponse response1 = null;
			FtpWebResponse response2 = null;
			FileStream outputStream = null;
			Stream responseStream = null;

			log.DebugFormat("Downloading file {0} with local name {1} from {2}", remoteFilename, localFilename, serverUri);

			try
			{
				var remoteFileUri = serverUri + remoteFilename;

				var request1 = (FtpWebRequest) WebRequest.Create(remoteFileUri);
				request1.Method = WebRequestMethods.Ftp.GetFileSize;
				response1 = (FtpWebResponse) request1.GetResponse();
				long fileSize = response1.ContentLength;

				var request2 = (FtpWebRequest)WebRequest.Create(remoteFileUri);
				request2.Method = WebRequestMethods.Ftp.DownloadFile;
				request2.UseBinary = true;

				response2 = (FtpWebResponse)request2.GetResponse();
				responseStream = response2.GetResponseStream();
				
				outputStream = File.Create(localFilename);
				var buffer = new byte[1024];
				while (true)
				{
					int bytesRead = responseStream.Read(buffer, 0, buffer.Length);
					if (bytesRead == 0)
						break;
					outputStream.Write(buffer, 0, bytesRead);

					notificator.UpdateProgress((int) (outputStream.Position*100/fileSize));
				}

				notificator.Complete();
			}
			catch (WebException ex)
			{
				log.Warn("Cannot download update", ex);
				throw new UpdatesProviderInaccessibleException("Cannot download update", ex);
			}
			finally
			{
				if (response1 != null) response1.Close();
				if (response2 != null) response2.Close();
				if (outputStream != null) outputStream.Close();
				if (responseStream != null) responseStream.Close();
			}
		}
	}
}