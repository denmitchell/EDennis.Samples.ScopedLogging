# EDennis.Samples.ScopedLogging
This project demos an approach to logging in which multiple loggers (of different levels) are added to the DI and then, based upon predefined criteria and scope-level data, a particular logger (if enabled) is chosen for logging.  A LoggersController (and its base class) provides a sample interface for enabling/disabling particular loggers and adding/removing logger selection criteria (in this case, the logged-on user).
To explore the demo, do the following
1. Install a local version of Seq at port 5341 (or remove Seq settings from appsettings.development.json)
2. Launch Visual Studio using the AutoLogin=Moe profile
3. In the Swagger Interface, do the following:
   a. [POST] /api/Loggers/Enable/1/Moe
   b. [POST] /api/Loggers/Enable/2/Larry
   c. [POST] /api/Loggers/Enable/2/Curly
   d. [GET]  /api/Colors
4. Examine the logs in the Console and/or Seq
5. Relaunch Visual Studio using AutoLogin=Larry or AutoLogin=Curly and repeat steps 3 and 4.
When the user is Moe and you have associated Moe with the Verbose logger (logger index = 1), you should see verbose logs (which logs the entire returned array of colors) and debug logs (which logs just the input parameters).
When the user is Larry or Curly and you have associated the user with the Debug logger (logger index = 2), you should see debug logs (which logs just the input parameters).
Note that to get everything working correctly in another project, you need to setup things correctly in Program.cs, Startup.cs, Configuration (e.g., appsettings.{env}.json), and each class that injects loggers in the constructor (e.g, see the ColorsController).  Although the current example used Serilog, the same approach could be used with other providers.
