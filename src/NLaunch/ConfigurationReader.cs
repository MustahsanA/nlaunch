using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace NLaunch
{
	public class ConfigurationReader
	{
		private readonly string configurationFilename;

		public ConfigurationReader()
		{
			var launcherFilename = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
			configurationFilename = Path.ChangeExtension(launcherFilename, "xml");
		}

		public ConfigurationReader(string configurationFilename)
		{
			this.configurationFilename = configurationFilename;
		}

		public LauncherConfiguration Read()
		{
			var configuration = new LauncherConfiguration
				{
					ConfigurationFullPath = Path.GetFullPath(configurationFilename)
				};

			FileStream str = null;

			try {
				str = new FileStream(configurationFilename, FileMode.Open);

				var settings = new XmlReaderSettings {IgnoreWhitespace = true, IgnoreComments = true};

				using (XmlReader reader = XmlReader.Create(str, settings)) {
					reader.MoveToContent();

					while (reader.Read()) {
						if (reader.Name == "protocol") {
							reader.ReadStartElement();
							configuration.Protocol = reader.ReadString();
						}
						else if (reader.Name == "updatesLocation") {
							reader.ReadStartElement();
							configuration.UpdatesLocation = reader.ReadString();
						}
						else if (reader.Name == "localDirectory") {
							reader.ReadStartElement();
							configuration.LocalDirectory = reader.ReadString();
						}
						else if (reader.Name == "applicationExe")
						{
							reader.ReadStartElement();
							configuration.ApplicationExe = reader.ReadString();
						}
					}
				}
			}
			catch (XmlException ex) {
				throw new InvalidOperationException("Wrong configuration file.", ex);
			}
			finally {
				if (str != null) str.Close();
			}

			return configuration;
		}
	}
}