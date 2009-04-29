using NLaunch.Impl;
using NLaunch.Infrastructure;
using NUnit.Framework;
using Rhino.Mocks;

namespace NLaunch.Tests
{
	[TestFixture]
	public class FtpUpdatesProviderTests
	{
		private LauncherConfiguration config;
		private IFtpAccessor ftp;
		private INamingConvention namingConvention;

		[SetUp]
		public void SetUp()
		{
			config = new LauncherConfiguration
				{
					ApplicationExe = "Test.App.exe",
					UpdatesLocation = "ftp://user:pass@ftp.test.com/remotedir"
				};
			ftp = MockRepository.GenerateMock<IFtpAccessor>();
			namingConvention = new DefaultNamingConvention();
		}

		[Test]
		public void Download_returns_local_downloaded_filename()
		{
			var notificator = MockRepository.GenerateMock<IDownloadNotificator>();

			var updatesProvider = new FtpUpdatesProvider(config, ftp);

			const string filename = "Test.App-1.1.0.0.zip";
			var localFilename = updatesProvider.Download(filename, notificator);

			Assert.That(localFilename.EndsWith(filename));
			Assert.That(localFilename.Length, Is.GreaterThan(filename.Length));
		}

	}
}