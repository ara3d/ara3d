# Ara 3D

A collection of open-source libraries for rapid application development in C#. 
This is the main development repository for work done by [Ara3D.com](https://ara3d.com).

## Ara3D.Utils

This is also the home repository for the [Ara3D.Utils Nuget package](https://www.nuget.org/packages/Ara3D.Utils). 

**Ara3D.Utils** is a collection of hundreds of useful functions and classes written in portable C# 
(C# version 7.3 and .NET Standard 2.0). 
The library is intended to reduce the amount of code we have to write, and to lighten the cognitive load 
making us more effective at programming. 

Many of the code was adapted from solutions to problems found on StackOverflow.com. 

## Documentation 

The code is written in a simple and straightforward manner, and should serve mostly as its own documentation.
Most functions are implemented as extension methods to improve discoverability. 

We encourage you to take a [quick read through it](https://github.com/ara3d/ara3d/tree/main/src/Ara3D.Utils)
and see if it could be useful for you!

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

## Organization

Code hosted in this repsitory is divided into two main sections:

1. `dev` - code which is intended for eventual use in production environments
2. `labs` - code which is used for experiments and investigation

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
* Interfaces are a good thing, use them
* Keep functions small, simple, and without side-effects
* When it makes sense, prefer extension methods

## Contributing 

We welcome contributions. We expect the following:

1. You are familiar with the structure and contents of existing libraries.
2. You have validated that a contribution does not exist already.
3. You follow the coding, naming, and documentation styles and conventions established in existing code.     
4. Code works well and is robust.
5. You follow the design goals and programming principles. 

Everything will be code-reviewed kindly and rigorously. 

# License 

All code is licensed under the commercially friendly [MIT License](https://github.com/ara3d/ara3d?tab=MIT-1-ov-file#readme)

# F.A.Q.

Q: Some projects can't be found. 

> A : It is normal that projects in "private" can't be found. You might not have recursively all submodules. 
See the top of the README.md. 

Q: Why so many projects? 

> A : Small projects that do one thing and do it well (and are reliable, tested, and simple), 
are easier to reuse, making us more productive in the long run.

Q: Why use submodules?

> A : Some of the work is of interest to the community as a standalone. This is the best way to help everyone 
get the code, and consume it independently. 




