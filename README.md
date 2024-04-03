# Ara 3D

A collection of open-source libraries for rapid application development in C#. 
This is the main development repository for work done by [Ara3D.com](https://ara3d.com).

## Documentation 

The code is written in a simple and straightforward manner, and should serve mostly as its own documentation.
Most functions are implemented as extension methods to improve discoverability. 

## Using 

Some of the code in this repository is available as [NuGet packages](https://www.nuget.org/profiles/Ara3D).

To clone this repository the following command can be used:

> `git clone --recursive git://github.com/ara3d/ara3d.git`

Note: there are a couple of libraries that will be missing. This is normal, because those libraries 
are in a private repository only available for Ara 3D employees. The solution should still build and work
fine. 

## Submodules 

There are several submodules included with this repository.   

* [Domo](https://github.com/ara3d/domo) - A state management library.
* [Parakeet](https://github.com/ara3d/parakeet) - A text parsing library.
* [Bowerbird](https://github.com/ara3d/bowerbird) - A C# scripting library.
* [Plato](https://github.com/ara3d/plato) - A simple and efficient cross-platform programming language.  
* [Ara3D.Mathematics](https://github.com/ara3d/mathematics) - A mathematical library.
* [Ara3D.Geometry](https://github.com/ara3d/geometry) - A library of geometric algorithms and data structures.
* [Ara3D.Collections](https://github.com/ara3d/collections) - A library of LINQ inspired immutable containers.  
* [Ara3D.Utils](https://github.com/ara3d/utils) - A library of miscellaneous utility algorithms and data structures.

## Organization

Code hosted in this repsitory (excluding code in submodules) 
is divided into the following main sections:

1. [`src`](https://github.com/ara3d/ara3d/tree/main/src) - projects intended for use in production environments
1. [`labs`](https://github.com/ara3d/ara3d/tree/main/labs) - projects for experiments and investigation
1. [`tests`](https://github.com/ara3d/ara3d/tree/main/tests) - NUnit-based test projects 
1. [`unity-projects`](https://github.com/ara3d/ara3d/tree/main/unity-projects) - Unity projects 
1. [`devops`](https://github.com/ara3d/ara3d/tree/main/devops) - scripts and tools to simplify develolpment  

## Design Goals 

The primary Ara 3D library design goals are roughly ordered as:

* Correctness 
* Robustness 
* Simplicity
* Portability
* Flexibility
* Discoverability
* Performance

This is not to say that performance is not important, but the other requirements must be addressed before performance 
can be considered. Performance is irrelevant if code is not correct, and well-written code is much easier to
validate and optimize.     

You will notice that a lot of care is placed into managing dependencies between the different libraries,
and that 3rd party dependencies are very sparse. A perfect library

Most libraries are .NET Standard 2.0, so that they can be used in a wide range of scenarios
such as for plug-ins. 

## Coding Style and Programming Principles

There is no formal written-down coding style guidelines. Upon reading the code
the established style should become apparent quickly. 

In general we follow the [Microsoft coding conventions for C#](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).
We also use the [JetBrains ReSharper plug-in](https://www.jetbrains.com/resharper/) and follow many of the default coding recommendations it makes. 

Some rules of thumb:

* Prefer immutable data structures
* Prefer explicit APIs to implicit (e.g., minimize annotations)
* Keep interfaces small and single purpose
* Prefer easy to read code to micro-optimizations
* Default to making things public
* Don't repeat yourself
* Interfaces are a good thing, use them frequently 
* Keep functions small, simple, and without side-effects
* When it makes sense use extension methods, they make code more generic, readable, and discoverable 
* Support as many platforms as you can (e.g., default to using .NET Standard 2.0)

## Contributing 

We welcome contributions. We expect the following:

1. You have familiarized yourself with the structure and contents of existing libraries.
2. You have validated that a contribution does not exist already.
3. You follow the coding, naming, and documentation styles and conventions established in existing code.     
4. Code works well and is robust.
5. You follow the design goals and programming principles. 

Everything will be code-reviewed with rigour and kindness.  

# License 

All code is licensed under the commercially friendly 
[MIT License](https://github.com/ara3d/ara3d?tab=MIT-1-ov-file#readme)

# F.A.Q.

Q: Some projects can't be found. 

> A : It is normal that projects in "private" can't be found. You also might not have recursively all submodules. 
See the top of the README.md. 

Q: Why so many projects? 

> A : Small projects that do one thing and do it well (and are reliable, tested, and simple), 
are easier to understand, test, and reuse, making us more productive.

Q: Why use submodules?

> A : Some of the work is of interest to the community as a standalone. This is the best way to help everyone 
get the code, and consume it independently. 

Q: Where are the build tasks and properties?  

> Many of the build tasks and properties are controlled within the `Directory.Build.props` file
whic is automatically included by all projects. 


