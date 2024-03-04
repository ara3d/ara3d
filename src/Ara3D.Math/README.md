**This repository is a fork of Vim.Math3D which was originally written by Ara 3D Inc.**

# Ara3D.Mathematics

**Ara3D.Mathematics** is a portable, safe, and efficient 3D math library written in C# 
targeting .NET Standard 2.0 without any dependencies. 

It is intended primarily as a feature rich drop-in replacement for System.Numerics that assures consistent serialization
across platforms, enforces immutability, and offers many additional structures and functionality. 

Ara3D.Mathematics is compatible with Unity and has been used in production on many different platforms including Windows, 
Android, iOS, WebGL, and Magic Leap. 

# History and the Math3D Origins

The first version of this library ([Ara3D.Math3D](https://github.com/ara3d/math3d) was written by Christopher Diggins for Ara 3D. 

The original library was based on the CoreFX implementation of System.Numerics with 
additional algorithms and structures taken from MonoGame, an open-source cross 
platform port of the XNA game development framework.

Development and maintenace was taken over by [VIM](https://vimaec.com) 
a software company specializing in BIM data analysis and visualization when 
Christopher Diggins joined them in 2019.

VIM continues to support and publish Math3D repository at [Vim.Math3D](https://github.com/vimaec/math3d)
which they use in commercial products and open-source libraries. 

Today Christopher is back working at Ara 3D Inc full-time and using 
this library as a corner-stone for a 3D modeling tool we are building. 

## Motivation for Forking 

There are a few things that we need to do differently:

1. Extend the number of data-structure and algorithms provided 
1. Support auto-generation of code in other languages (especially JavaScript)
1. Move geometrical structures and algorithms into a geometry specific library 
1. Default to using double precision floating point numbers throughout 
1. Improve the efficiency of the compiled libraries 

## How we will Achieve the Goals

To achieve better performance and cross-language code-generation, the core algorithms are being ported 
to another language called [Plato](https://github.com/cdiggins/plato). 

Plato is designed to make it easy to express generic mathematical concepts and algorithms
in a manner that is easy to analyze, optimize, and port to different languages and platforms.

## What Next?

For now, the Ara3D.Mathematics surface API is very similar to Vim.Math3D with only a 
few minor differences. The [Plato library](https://github.com/cdiggins/plato/tree/main/PlatoStandardLibrary) is quite different
and is undergoing testing and development.

Currently our priority is finalizing the design of the 
[Ara3D.Geometry](https://github.com/ara3d/ara3d/tree/main/src/Ara3D.Geometry) 
library. 

Once the geometry library design is stabilizied, the names of the Ara3D.Mathematics structures will be mapped 
to the new Plato names. Afterwards, the new implementations will be used, and Plato will be 
introduced as part of the build process. 

Once this is complete the library will be separated into its own repository. 

During this process it may be possible that we have to create a dependency on 
[Ara3D.Collections](https://github.com/ara3d/ara3d/tree/main/src/Ara3D.Collections) and follow
a similar process. This is because the most basic collections like arrays and enumerable, 
may be replaced by Plato primitives. 

## Design Goals

The Ara3D.Mathematics design goals remain mostly the same as the Math3D library:

1. Portability
	* The library must be pure C# 
	* No unsafe code 
	* Fixed binary layout of structs in memory
	* Double and Single precision implementation of most structures 
2. Robustness
	* Functions are well covered by unit tests 
	* Functions are easy to read, understand, and verify
3. Ease of Use and Discoverability
	* Consistent with Microsoft coding styles
	* Consistent API with System.Numerics
	* Can use fluent syntax (object-oriented "dot" notation)
	* We don't have to pass arguments by reference
4. Performance 
	* Excellent performance, but not at cost of readability and discoverability

## Related Libraries 

* [Vim.Math3D](https://github.com/vimaec/math3d)
* [System.Numerics](https://referencesource.microsoft.com/#System.Numerics,namespaces)
* [SharpDX Mathematics](https://github.com/sharpdx/SharpDX/tree/master/Source/SharpDX.Mathematics)
* [MonoGame](https://github.com/MonoGame/MonoGame)
* [Math.NET Spatial](https://github.com/mathnet/mathnet-spatial)
* [Math.NET Numerics](https://github.com/mathnet/mathnet-numerics)
* [Unity.Mathematics](https://github.com/Unity-Technologies/Unity.Mathematics)
* [Unity Reference](https://github.com/Unity-Technologies/UnityCsReference/tree/master/Runtime/Export)
* [Abacus](https://github.com/sungiant/abacus)
* [Geometry3Sharp](https://github.com/gradientspace/geometry3Sharp)
* [FNA-XNA](https://github.com/FNA-XNA/FNA/tree/master/src)
* [Stride](https://github.com/stride3d/stride/tree/master/sources/core/Stride.Core.Mathematics)
* [A Vector Type for C# - R Potter via Code Project](https://www.codeproject.com/Articles/17425/A-Vector-Type-for-C)
* [Godot Engine C# Libraries](https://github.com/godotengine/godot/tree/master/modules/mono/glue/GodotSharp/GodotSharp/Core)
* [GeometRi - Simple and lightweight computational geometry library for .Net](https://github.com/RiSearcher/GeometRi.CSharp)
* [Veldrid](https://github.com/mellinoe/veldrid/tree/master/src/Veldrid.Utilities)