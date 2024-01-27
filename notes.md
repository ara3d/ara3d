# Notes

* Ara3D.Nuget.sln is a solution file that contains projects intended for release on Nuget.
* The "...Nuget.csproj" projects reference each other via a local Nuget feed. 
* The version of all of the nuget projects must be incremented at the same time. 
* Nuget version information is tracked in the directory.props file.