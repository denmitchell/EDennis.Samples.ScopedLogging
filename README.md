# EDennis.Samples.ScopedLogging
This project demos an approach to logging in which multiple loggers (of different levels) are added to the DI and then, based upon predefined criteria and scope-level data, a particular logger (if enabled) is chosen for logging.  A LoggersController (and its base class) provides a sample interface for enabling/disabling particular loggers and adding/removing logger selection criteria (in this case, the logged-on user).

## Demo Steps
To explore the demo, do the following:
1. Install a local version of Seq at port 5341 (or remove Seq settings from appsettings.development.json)
2. Launch Visual Studio using the AutoLogin=Moe profile
3. In the Swagger Interface, do the following:
    - [POST] /api/Loggers/Enable/1/Moe
    - [POST] /api/Loggers/Enable/2/Larry
    - [POST] /api/Loggers/Enable/2/Curly
    - [GET]  /api/Colors
4. Examine the logs in the Console and/or Seq
5. Relaunch Visual Studio using AutoLogin=Larry or AutoLogin=Curly and repeat steps 3 and 4.

## Observations
- When the user is Moe and you have associated Moe with the Verbose logger (logger index = 1), you should see verbose logs (which logs the entire returned array of colors) and debug logs (which logs just the input parameters).
- When the user is Larry or Curly and you have associated the user with the Debug logger (logger index = 2), you should see debug logs (which logs just the input parameters).

## Setup In Another Project 
Note that to get everything working correctly in another project, you need to setup things correctly in various classes.

### Program.cs
- Create the default logger with ```Log.Logger = ```
- Also call ```UseSerilog()``` on IWebHostBuilder.  See the Program.cs file in this project.

### Startup.cs
- Setup ScopeProperties for DI as 
```c#
services.AddScoped<ScopeProperties>();
```
- Setup each secondary logger as singletons:
```c#
services.AddSingleton(typeof(ILogger<>), typeof(SerilogVerboseLogger<>));
services.AddSingleton(typeof(ILogger<>), typeof(SerilogDebugLogger<>));
```
- Setup the ILoggerChooser as a singleton using a factory method (in order to inject sample loggers):
```c#
services.AddSingleton<ILoggerChooser>(f => {
    var loggers = f.GetRequiredService<IEnumerable<ILogger<object>>>();
    return new DefaultLoggerChooser(loggers);
});
```

### Serilog and IConfiguration 
- If using Serilog, ensure that you have all of the NuGet packages installed.
- If using Serilog, you can read configurations for each logger from appsettings.{env}.json.

### Classes that inject loggers
- Ensure that all classes that will need to use alternate loggers inject IEnumerable<ILogger<T>> and ScopeProperies and follow the approach demonstrated in the ColorsController constructor:
```c#
private readonly ColorDbContext _context;
private ILogger _logger;
public ColorsController(ColorDbContext context, 
    IEnumerable<ILogger<ColorsController>> loggers, 
    ScopeProperties scopeProperties)
{
    _context = context;
    _logger = loggers.ElementAt(scopeProperties.LoggerIndex);
}
```

### Other
- If using DefaultLoggerChooser, you may wish to setup AutoLogins, which requires configuration in Startup.cs, appsettings.json, and launchSettings.json.
- Note that although the current example used Serilog, the same approach could be used with other providers.
