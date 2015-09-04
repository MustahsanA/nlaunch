To provide your desktop application with autoupdating capabilities just copy nlaunch.exe and nlaunch.xml to the target installation directory (this files can be both renamed but they must have the same filename), and modify the configuration file to suit your needs:

**nlaunch.xml** (see ConfigurationReference)
```
<?xml version="1.0" encoding="utf-8" ?>
<nlaunch>
  <protocol>ftp</protocol>                                           (1)
  <updatesLocation>ftp://user:password@server.com/</updatesLocation> (2)
  <applicationExe>Test.App.exe</applicationExe>                      (3)
</nlaunch>
```

Then replace the shortcuts (start menu or desktop) to your application with references to nlaunch.exe

That's all.

Really.

From now on, every time that the user runs your application, the following actions will take place:

**1. NLaunch checks if a new version of the application exists.** This is done by looking for all the files named after the following convention:
```
Test.App-1.2.3.4.zip
```
where the first part of the filename is derived from the name of the executable configured in the applicationExe node and the second part specifies the version contained in the zip file.
Once that the version of the last updated is obtained, it is compared with the version of the current application executable and it is decided if it needs to be updated or not.

**2. If acording to the versions information an update is required, then the update is downloaded and activated.** This is done through the the following steps:
  * downloads (or copy) the zip file containing the update to a local temp directory
  * copy the contents of the existing installation into a subdirectory called `PreviousVersion` within the application directory
  * deletes everything that does not belong to nlaunch itself
  * unzip the downloaded zip file to the application target directory
  * restores previous application configuration (Test.App.exe.config in this case) from the `PreviousVersion` directory

**3. The original application is launched and NLaunch ends its work.**

### Deploying updates ###
To deploy an update, just copy a new file named after the right convention (`Test.App-1.2.0.0.zip`) and the next time that the user starts your application, it will be downloaded and installed.