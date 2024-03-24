# Backend
Scalability Backend Project

## How to run
``` 
C:\ >Backend.Program.exe
```
### Supported Command Line Arguments
* `-dir` The fully qualified path to a folder containing `facts.json` and `actions.json`.  If
         this is not passed, it is assumed that the files are in the same directory as the executable.
* `-by`  This controls the final output of the program to be displayed grouped by loans or by
         borrowers.  If no value is passed, it outputs by loan.  To group by borrower, pass the
         word "borrower".  It is not case sensitive.
```
C:\ >Backend.Program.exe -dir c:\TheFolderWithTheFiles -by borrower
```

### appsettings.json
A few settings are controlled in the `appsettings.json` file

* <strong>ConnectionStrings:BackendDb</strong> The file to write the SQLite data to.  The file is
  deleted at the beginning of each run.  You can examine the data with a SQLite browser when the
  program completes.
* <strong>EnableSensitiveSqlLogging</strong> A boolean value to set if you would like to log the
  SQLite parameter values used for the call.  To see this output, the LogLevel parameter must be
  at least `Information`
* <strong>Logging</strong> The standard format of `appsettings.json` logging configuration.  For
  additional information, go to: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-6.0

### Limitations
* The program starts by deleting any existing database file.  If this is locked by a SQLite
  browser, the program will halt with an Exception.
