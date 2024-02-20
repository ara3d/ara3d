# Ara 3D

A set of open-source libraries for rapid application development in C#. 
This is the main development repository for work done by [Ara3D.com](https://ara3d.com).

## Ara3D.Utils

This is also the home repository for the [Ara3D.Utils Nuget package](https://www.nuget.org/packages/Ara3D.Utils). 

**Ara3D.Utils** is a collection of hundreds of useful functions and classes written in portable C# 
(C# version 7.3 and .NET Standard 2.0). 
The library is intended to reduce the amount of code we have to write, and to lighten the cognitive load 
making us more effective at programming. 

Many of the code was adapted from solutions to problems found on StackOverflow.com. Most functions are 
implemented as extension methods to improve discoverability. 

The code is written in a simple and straightforward manner, and should be self-explanatory.
We encourage you to take a [quick read through it](https://github.com/ara3d/ara3d/tree/main/src/Ara3D.Utils)
and see if it could be useful for you!

## Using 

To clone this repository then the following command can be used:

> `git clone --recursive git://github.com/ara3d/ara3d.git`

## Submodules 

There are several submodules included with  this repository.   

* [Domo](https://github.com/ara3d/domo) - A state management library.
* [Parakeet](https://github.com/ara3d/parakeet) - A text parsing library.
* [Bowerbird](https://github.com/ara3d/bowerbird) - A C# scripting library.
* [Plato](https://github.com/ara3d/plato) - A simple and efficient cross-platform programming language.  

## Organization

Code hosted in this repsitory is divided into two main sections:

1. `dev` - code which is intended for eventual use in production environments
2. `labs` - code which is used for experiments and investigation

## Design Goals 

The Ara 3D library design goals are:

* Portability
* Simplicity
* Robustness 
* Flexibility
* Minimize dependence

Most libraries are .NET Standard 2.0, so that they can be used in a wide range of scenarios
such as for plug-ins. 

The libraries use functional programming idioms heavily. 

## Contributing 

We welcome contributions. We expect the following:

1. You are familiar with the structure and contents of existing libraries.
2. You have validated that a contribution does not exist already.
3. You follow the coding, naming, and documentation styles and conventions of the existing code.     
4. Code works well and is robust.
5. You follow the design principles. 

Everything will be code-reviewed kindly and rigorously. 

# License 

All code is licensed under the commercially friendly [MIT License](https://github.com/ara3d/ara3d?tab=MIT-1-ov-file#readme)

