using System;
using System.Diagnostics;
using System.IO;

namespace NLaunch.Impl
{
	public class CurrentVersionProvider : ICurrentVersionProvider
	{
		private readonly LauncherConfiguration configuration;

		public CurrentVersionProvider(LauncherConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public Version GetCurrentVersion()
		{
			var assemblyFullPath = Path.GetFullPath(configuration.ApplicationExe);
			var versionInfo = FileVersionInfo.GetVersionInfo(assemblyFullPath);

			return new Version(versionInfo.FileVersion);
		}
	}
}