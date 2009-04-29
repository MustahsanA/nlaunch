@echo off
cls
tools\nant\nant.exe -buildfile:nlaunch.build %*
pause
