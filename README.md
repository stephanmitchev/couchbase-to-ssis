couchbase-to-ssis
================

A plugin for Microsoft SQL Server Integration Services that allows a Couchbase cluster to be used as a source in a Data Flow task.

For Couchbase to Oracle 10g synchronization, visit: https://github.com/stephanmitchev/couchbase-to-oracle

OSS Mentions
------------------
This solution contains the .NET Couchbase Driver downloaded from http://www.couchbase.com. You can re-download it and extract it over the existing files if you want the latest release - it should work fine. I have used the Couchbase logo to create an icon (couchbase.ico) to identify the plugin in the SSIS Data Tools IDE. You will also see Calvin Rien's adaptation of the JSON parser.

Installation
------------------

1. Get the source
2. Make sure that you have VS2010, SQLServer 2012 Integration Services (and tools), and Microsoft SDK (for gacutil) installed
3. Open the solution and build it. If you run Windows 8 or Windows Server 2012, You might need to change the paths post-build events from ..\v7.0A\.. to ..\v8.0A\..

Inspect the output log. Make sure that the library build without errors (there are currently 4 warnings) and that it successfully installed itself and the dependent assemblies to the GAC. THe first time you may get errors that It cannot find assembly to uninstall, but that's fine...

Now open SSIS Data Tools and create a new SSIS Project with a data flow task. You should see in the sources that there is a new guy - red with a white couch. I'll let you play with the mouse at this point.

Tips
-------------------
The Couchbase view that you want to synchronize must return as key a timestamp that is guaranteed to be updated if you change the JSON; the values of the view are ignored.

After you configure the view credentials, press "Test and Use Connection". You need to test the connection every time before you open the Data Mapping Screen

When you Generate a Model in the Data Mapping screen, most often your Root XPath will be empty (scan the entire JSON document)

When you configure the component, View URL must be in the form of "http://hostname.com:8091/"

You can use variables for Start and End Key such as @User::lastSync. This is key if you want to implement synchronization as you need to select a last updated timestamp from your tables and start your view query with that key.



... more to come
