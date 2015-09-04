The only prerequisite for building NLaunch locally is to download and install [ILMerge](http://research.microsoft.com/en-us/people/mbarnett/ilmerge.aspx).
If you install it in the default path (C:\Program Files\Microsoft\ILMerge) then you are done, if you install it anywhere else you need to tell this fact to the build script: copy the file `local.properties.xml.template` to `local.properties.xml` and edit its contents accordingly.

To build NLaunch locally you just need to run `build.bat`.

To open the Visual Studio solution you will need to build the project through the build script at least once in order for it to generate the `AssemblyInfo.cs` that is referenced by the NLaunch project.