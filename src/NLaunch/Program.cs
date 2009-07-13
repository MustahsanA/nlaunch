using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NLaunch.Impl;

namespace NLaunch
{
	static class Program
	{
		private static readonly ILog log = LogManager.GetLogger(typeof (Program));

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			initializeLogging();

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
					log.InfoFormat("Current version: {0}", currentVersion);
					log.InfoFormat("Last version available: {0}", lastVersion);

					// 3. if there is a new version, download it
					var downloadProgressNotificator = new DownloadProgressForm();
					var localFilename = updatesProvider.Download(lastVersionFilename,
					                                             downloadProgressNotificator);

					// 4. then activate it
					var activator = new DefaultActivator(config, null);
					activator.Activate(localFilename);
				}

			}
			catch (UpdatesProviderInaccessibleException ex)
			{
				// ignore errors when updates provider is inaccessible
			}
			catch (Exception ex)
			{
				log.Error("Error updating", ex);
			}

			try
			{
				Process.Start(config.ApplicationExe);
			}
			catch (Exception ex)
			{
				log.Fatal("Cannot start application", ex);
			}
		}

		private static void initializeLogging()
		{
			var thisFilename = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			var logFilename = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(thisFilename, "txt"));

			var layout = new log4net.Layout.PatternLayout("%d [%t]%-5p %c [%x] - %m%n");
			layout.ActivateOptions();

			log4net.Config.BasicConfigurator.Configure(
				new log4net.Appender.FileAppender(
					layout, logFilename));
		}
	}
}
