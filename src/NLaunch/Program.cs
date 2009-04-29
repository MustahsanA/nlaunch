using System;
using System.Diagnostics;
using System.Windows.Forms;
using NLaunch.Impl;

namespace NLaunch
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// Workflow
			
			// 1. read configuration
			var config = new ConfigurationReader().Read();

			// 2. check whether there is a new version or not
			var updatesProvider = new UpdatesProviderFactory(config).Create();
			var namingConvention = new DefaultNamingConvention();

			try
			{
				var lastVersionFilename = updatesProvider.GetLastVersionFilename(namingConvention);
				var lastVersion = namingConvention.GetVersionFromFilename(lastVersionFilename);
				var currentVersion = new CurrentVersionProvider(config).GetCurrentVersion();

				if (lastVersion > currentVersion)
				{
					// 3. if there is a new version, download it
					var downloadProgressNotificator = new DownloadProgressForm();
					var localFilename = updatesProvider.Download(lastVersionFilename,
					                                             downloadProgressNotificator);

					// 4. then activate it
					var activator = new DefaultActivator(config, null);
					activator.Activate(localFilename);
				}

			}
			catch (UpdatesProviderInaccessibleException)
			{
				// ignore errors when updates provider is inaccessible
			}

			Process.Start(config.ApplicationExe);
		}
	}
}
