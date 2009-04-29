using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace NLaunch.Infrastructure
{
	public static class ZipHelper
	{
		public static void Unzip(string zipFile)
		{
			Unzip(zipFile, ".");
		}

		public static void Unzip(string zipFile, string targetDirectory)
		{
			using (var s = new ZipInputStream(File.OpenRead(zipFile)))
			{
				ZipEntry theEntry;
				while ((theEntry = s.GetNextEntry()) != null)
				{
					var directoryName = Path.GetDirectoryName(theEntry.Name);
					var fileName = Path.GetFileName(theEntry.Name);

					// create directory
					if (directoryName.Length > 0)
					{
						Directory.CreateDirectory(Path.Combine(targetDirectory, directoryName));
					}

					if (fileName != String.Empty)
					{
						using (var streamWriter = File.Create(Path.Combine(targetDirectory, theEntry.Name)))
						{
							var data = new byte[2048];
							while (true)
							{
								var size = s.Read(data, 0, data.Length);
								if (size > 0)
								{
									streamWriter.Write(data, 0, size);
								}
								else
								{
									break;
								}
							}
						}
					}
				}
			}
		}

	}
}