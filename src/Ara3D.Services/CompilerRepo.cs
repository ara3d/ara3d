using System;
using System.Collections.Generic;
using Ara3D.Domo;
using Ara3D.Utils;

namespace Ara3D.Services
{
    public class TextSpan
    {
        public int Index;
        public int Length;
    }

    public class CompilationSource
    {
        public FilePath File;
        public DateTimeOffset Modified;
        public int Size;
    }

    public class CompilationDiagnostic
    {
        public TextSpan Location;
        public string Category;
        public string Title;
        public string Description;
    }

    public class CompilationModel
    {
        public string Settings; 
        public IReadOnlyList<CompilationSource> InputFiles = Array.Empty<CompilationSource>();
        public IReadOnlyList<CompilationDiagnostic> Diagnostics = Array.Empty<CompilationDiagnostic>();
        public FilePath Output;
        public bool Success;
    }

    public class CompilerRepo : SingletonRepository<CompilationModel>
    {
    }
}