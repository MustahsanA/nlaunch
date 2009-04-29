using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NLaunch.Infrastructure;

namespace NLaunch.Impl
{
	public class DefaultActivator : IActivator
	{
		private const string backupDirectory = "PreviousVersion";

		private readonly LauncherConfiguration configuration;
		private readonly string[] otherFilesToPreserve;

		public DefaultActivator(LauncherConfiguration configuration, string[] otherFilesToPreserve)
		{
			this.configuration = configuration;
			this.otherFilesToPreserve = otherFilesToPreserve;
		}


		public void Activate(string localFilename)
		{
			var appDirectory = Path.GetFullPath(".");

			// if the directory PreviousVersion exists, deletes it
			if (Directory.Exists(backupDirectory))
				Directory.Delete(backupDirectory, true);

			// creates the PreviousVersion directory
			Directory.CreateDirectory(backupDirectory);

			// copy all the files to the PreviousVersion directory
			DirectoryExtensions.CopyExcept(appDirectory, backupDirectory, true, backupDirectory);

			// deletes current files (except for this executable, configuration, and previous version)
			var launcherFilename = Path.GetFileName(Assembly.GetExecutingAssembly().Location);

			var exceptions = new List<string>(otherFilesToPreserve ?? new string[] {});
			exceptions.AddRange(new[]
				{
					backupDirectory,
					launcherFilename,
					Path.GetFileName(configuration.ConfigurationFullPath),
					Path.ChangeExtension(launcherFilename, "pdb"),			// for debugging
					Path.ChangeExtension(launcherFilename, "vshost.exe")	// for debugging
				});
			exceptions.AddRange(
				Assembly
					.GetExecutingAssembly()
					.GetReferencedAssemblies()
					.Select(x => Path.GetFileName(Assembly.Load(x).Location))
				);

			DirectoryExtensions.DeleteAllExcept(appDirectory, true, exceptions.ToArray());

			// uncompress the localfile to the current directory
			ZipHelper.Unzip(localFilename);

			// restore application configuration from previous version
			var appConfigFilename = configuration.ApplicationExe + ".config";
			var previousConfiguration = Path.Combine(backupDirectory, appConfigFilename);
			File.Copy(previousConfiguration, appConfigFilename, true);
		}
	}

}