To build the project just run build.bat

There is only one requirement: you must install ILMerge before building NLaunch.
The build script assumes that ILMerge is installed at the default location: \Program Files\Microsoft\ILMerge, if it is anywhere else, just rename the file "local.properties.xml.template" to "local.properties.xml" and modify its contents to specify your ILMerge installation path.
(I'd bundle ilmerge.exe whithin source control but it seems to be restricted by its license)