using NLaunch.Impl;
using NUnit.Framework;

namespace NLaunch.Tests
{
	[TestFixture]
	public class UpdatesProviderFactoryTest
	{
		[Test]
		public void Create_returns_instance_of_FtpUpdatesProvider_when_protocol_configuration_is_ftp()
		{
			var config = new LauncherConfiguration
				{
					Protocol = "ftp",
					UpdatesLocation = "ftp://server"
				};

			var factory = new UpdatesProviderFactory(config);

			var updatesProvider = factory.Create();

			Assert.That(updatesProvider, Is.TypeOf(typeof(FtpUpdatesProvider)));
			Assert.That(updatesProvider, Is.Not.Null);
		}

	}
}