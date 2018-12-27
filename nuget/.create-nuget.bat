rem This is not our official nuget build script.
rem This is used as a quick and dirty way create nuget packages used to test user issue reproductions.
rem This is not ideal, but it's better than nothing, and it usually works fine.

@echo off
rem stub uncommon targets
@setlocal enableextensions
@cd /d "%~dp0"
cd %CD%
set NUGET_EXE=%CD%\nuGet.exe

if "%DEBUG_VERSION%"=="" set DEBUG_VERSION=0
set /a DEBUG_VERSION=%DEBUG_VERSION%+1
%NUGET_EXE% pack Plugin.NetStandardStorage.nuspec -properties configuration=debug;platform=anycpu -Version 2.0.%DEBUG_VERSION%
pause