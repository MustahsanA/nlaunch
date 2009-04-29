using System;
using System.Collections.Generic;

namespace NLaunch
{
	public interface INamingConvention
	{
		Version GetVersionFromFilename(string filename);
		string GetLastVersionFilename(IEnumerable<string> fileList, LauncherConfiguration configuration);
	}
}