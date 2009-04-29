using System;
using NLaunch.Impl;
using NUnit.Framework;

namespace NLaunch.Tests
{
	[TestFixture]
	public class DefaultNamingConventionTests
	{
		[Test]
		public void GetVersion_returns_version_part_of_filename()
		{
			var namingConvention = new DefaultNamingConvention();

			var version = namingConvention.GetVersionFromFilename("Test.App-1.0.0.1.zip");
			
			Assert.That(version, Is.EqualTo(new Version(1, 0, 0, 1)));
		}

		[Test]
		public void GetLastVersionFilename_returns_last_version_filename_that_verifies_convention()
		{
			var namingConvention = new DefaultNamingConvention();
			var configuration = new LauncherConfiguration {ApplicationExe = "Test.App.exe"};
			var fileList = new[]
				{
					"Test.App-1.0.0.0.zip",
					"Test.App-1.0.2.0.zip",
					"Test.Other-2.0.2.0.zip",
				};

			var lastVersionFilename = namingConvention.GetLastVersionFilename(fileList, configuration);

			Assert.That(lastVersionFilename, Is.EqualTo("Test.App-1.0.2.0.zip"));
		}
	}
}