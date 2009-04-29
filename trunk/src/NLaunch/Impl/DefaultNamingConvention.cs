using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NLaunch.Impl
{
	public class DefaultNamingConvention : INamingConvention
	{
		public Version GetVersionFromFilename(string filename)
		{
			var withoutExtension = Path.GetFileNameWithoutExtension(filename);
			var versionString = withoutExtension.Substring(withoutExtension.IndexOf("-") + 1);
			return new Version(versionString);
		}

		public string GetLastVersionFilename(IEnumerable<string> fileList, LauncherConfiguration configuration)
		{
			return (from f in fileList ?? new string[] {}
			        where verifiesConvention(f, configuration)
			        let v = GetVersionFromFilename(Path.GetFileNameWithoutExtension(f))
			        orderby v descending
			        select f).FirstOrDefault();
		}

		private bool verifiesConvention(string filename, LauncherConfiguration configuration)
		{
			var withoutExtension = Path.GetFileNameWithoutExtension(configuration.ApplicationExe);
			return filename.StartsWith(withoutExtension);
		}
	}
}