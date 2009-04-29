using System;

namespace NLaunch.Impl
{
	public class UpdatesProviderFactory
	{
		private readonly LauncherConfiguration configuration;

		public UpdatesProviderFactory(LauncherConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public IUpdatesProvider Create()
		{
			if (configuration.Protocol == "ftp")
			{
				return new FtpUpdatesProvider(configuration, new FtpAccessor(configuration.UpdatesLocation));
			}
			if (configuration.Protocol == "local")
			{
				return new LocalUpdatesProvider(configuration);
			}
			throw new NotImplementedException();
		}
	}
}