The configuration file must have the same file name as the NLaunch executable but with XML extension (yourapp.nlaunch.exe & yourapp.nlaunch.xml)

```
<?xml version="1.0" encoding="utf-8" ?>
<nlaunch>
  <protocol>ftp</protocol>                                           (1)
  <updatesLocation>ftp://user:password@server.com/</updatesLocation> (2)
  <applicationExe>Test.App.exe</applicationExe>                      (3)
</nlaunch>
```

(1)   `protocol`: `ftp` or `local` are supported

(2)   `updatesLocation`: if protocol is `ftp` here goes the ftp url with the authentication credentials and remote directory included; if protocol is `local` here goes the the full path of the local folder that will contain the updates

(3)   `applicationExe`: filename of the main executable of the application.