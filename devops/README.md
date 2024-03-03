# DevOps

This repository has several sub-modules (and more added all the time). 
Several of the projects have associated nuget packages. 
These scripts make it easier to submit changes, and submit the changes. 

## Projects are Missing: Don't Panic! 

Some projects won't load unless you have a specifically named repository 
in the same folder as this repository. This is because they 
are private to Ara 3D Inc., and are reserved for usage by employees.

Even though those projects won't load everything else should work fine despite that. 

## Pushing 

To facilitate updating the submodule and the main repository call the `push.bat`
batch script from the root folder like-this:

```
.\devops\push.bat <commit-message> 
```

This will go through all of the submodules one by one, and then the root folder and call the 
following commands:

```
git add .
git commit -m <commit-message>
git push
```

This makes pushing changes to the remote repository and sub-modules quick and easy.

If an additional version parameter is specified as so:

```
.\devops\push.bat <commit-message> <version> 
```

Then a [Git tag](https://git-scm.com/book/en/v2/Git-Basics-Tagging) 
will also be created with the version information (with a "v" prepended to it)  
and pushed online.

## Updating a Local Nuget

Some projects reference a nuget package, but to avoid the slow turnaround of publishing 
online, [a local nuget feed can be easily configured on your computer](https://learn.microsoft.com/en-us/nuget/hosting-packages/local-feeds). 

A local nuget feed is a special directory. It is created by calling a `nuget init` command.
The script `.\devops\nuget-init.bat` is provided to simplify creating the nuget feed process. 

## Publishing to Nuget 

Publishing to nuget is reserved for Ara 3D Inc. developers. You will need an API key, etc. 

The recommended (and supported) workflow for Ara 3D Inc. developers is: 

1. place your api-key in the following path: `C:\Users\<username>\api-keys\nuget-key.txt`
2. run a script to push all of the generated nuget files to nuget.

The script I use looks like this: 

```
for %%f in ("<ara3d-git-repo-folder>\nuget-output\*.nupkg") do dotnet nuget push %%f --api-key <apikey> --source https://api.nuget.org/v3/index.json
```

## Upgrading a Local Nuget Dependency 

1. Rebuild all 
1. Run the `.\devops\nuget-init.bat` folder 
1. Open the folder `.\nuget-output\` and inspect that the latest `.nupkg` files are there 
1. Upgrade projects manually or using tool to use the new version number 

## Nuget Publishing Check-List

1. Increment the version numbers in `Directory.Build.props` (version, assemblyversion, packageversion). 
1. Assure that dependencies on local nugets are up to date (see above)
1. Assure that the solution configuration is release 
1. Rebuild all
1. Run all tests 
1. Open the folder `.\nuget-output\` and inspect that the latest `.nupkg` files are there
1. Manually delete the older `.nupkg` files
1. Open `cmd.exe` in `C:\Users\<user>\api-keys` folder and run `nuget-push.bat`
1. Note: this process will re-upload the old files and some failures 
1. Push all of your local code using a tag option `.\devops\push.bat <message> <version>`