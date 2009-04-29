using System;
using System.IO;
using System.Linq;

namespace NLaunch.Infrastructure
{
	public static class DirectoryExtensions
	{
		public static void DeleteAllExcept(string directory, bool recursive, params string[] exceptions)
		{
			var dir = new DirectoryInfo(directory);

			// if the source dir doesn't exist, throw
			if (!dir.Exists)
			{
				throw new ArgumentException("source dir doesn't exist -> " + directory);
			}

			var files = dir.GetFiles().Where(x => !exceptions.Select(e => e.ToLower()).Contains(x.Name.ToLower()));

			foreach (var file in files)
			{
				file.Delete();
			}

			if (!recursive)
			{
				return;
			}

			var dirs = dir.GetDirectories().Where(x => !exceptions.Select(e => e.ToLower()).Contains(x.Name.ToLower()));

			foreach (var d in dirs)
			{
				var subdir = Path.Combine(directory, d.Name);
				DeleteAllExcept(subdir, true, exceptions);
			}
		}

		public static void CopyExcept(string sourceDirectory, string destinationDirectory, bool recursive, params string[] exceptions)
		{
			// determine if the destination directory exists, if not create it
			if (!Directory.Exists(destinationDirectory))
			{
				Directory.CreateDirectory(destinationDirectory);
			}

			var dir = new DirectoryInfo(sourceDirectory);

			// if the source dir doesn't exist, throw
			if (!dir.Exists)
			{
				throw new ArgumentException("directory " + sourceDirectory + "does not exist", "sourceDirectory");
			}

			// get all files in the current dir
			var files = dir.GetFiles().Where(x => !exceptions.Select(e => e.ToLower()).Contains(x.Name.ToLower()));

			// loop through each file
			foreach (var f in files)
			{
				// create the path to where this file should be in destinationDirectory
				var file = Path.Combine(destinationDirectory, f.Name);

				// copy file to dest dir
				f.CopyTo(file, false);
			}

			// if not recursive, all work is done
			if (!recursive)
			{
				return;
			}

			// otherwise, get dirs
			var dirs = dir.GetDirectories().Where(x => !exceptions.Select(e => e.ToLower()).Contains(x.Name.ToLower()));

			// loop through each sub directory in the current dir
			foreach (var d in dirs)
			{
				// create the path to the directory in destinationDirectory
				var subdir = Path.Combine(destinationDirectory, d.Name);

				// recursively call this function over and over again
				// with each new dir.
				CopyExcept(d.FullName, subdir, true, exceptions);
			}
		}
	}
}