using NUnit.Framework;

namespace NLaunch.Tests
{
	[TestFixture]
	public class ConfigurationReaderTests
	{
		private ConfigurationReader configurationReader;
		private LauncherConfiguration cfg;

		[SetUp]
		public void SetUp()
		{
			configurationReader = new ConfigurationReader();
			cfg = configurationReader.Read();
		}

		[Test]
		public void ReadConfig_returns_a_LauncherConfig_instance()
		{
			Assert.That(cfg, Is.TypeOf(typeof(LauncherConfiguration)));
			Assert.That(cfg, Is.Not.Null);
		}

		[Test]
		public void ReadConfig_reads_correct_protocol()
		{
			Assert.That(cfg.Protocol, Is.EqualTo("ftp"));
		}	
		
		[Test]
		public void ReadConfig_reads_correct_updatesLocation()
		{
			Assert.That(cfg.UpdatesLocation, Is.EqualTo("ftp://user%40server:password123@server.com/"));
		}		

		[Test]
		public void ReadConfig_reads_correct_applicationExe()
		{
			Assert.That(cfg.ApplicationExe, Is.EqualTo("Test.App.exe"));
		}
	}
}