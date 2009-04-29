using System;

namespace NLaunch
{
	public interface ICurrentVersionProvider
	{
		Version GetCurrentVersion();
	}
}