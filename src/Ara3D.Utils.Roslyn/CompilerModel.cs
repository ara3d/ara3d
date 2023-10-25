using System;
using System.Collections.Generic;
using System.Linq;

namespace Ara3D.Utils.Roslyn
{
    public class CompilerModel
    {
        public CompilerModel(Compilation compilation, string directory)
            => (Result, Directory) = (compilation, directory);

        public string Directory { get; }
        public Compilation Result { get; }
        public string OutputDll 
            => Compiled ? Result?.Options?.OutputFileName : "failure";
        public IEnumerable<string> InputFiles 
            => Result?.InputFileLookup?.Keys?.ToArray() ?? Array.Empty<string>();
        public bool Compiled 
            => Result?.EmitResult?.Success ?? false;
        public IEnumerable<string> Diagnostics 
            => Result?.EmitResult == null
                ? new[] { "Unknown error" }
                : Result?.EmitResult?.Diagnostics.Select(d => d.ToString()).ToArray();
    }
}
