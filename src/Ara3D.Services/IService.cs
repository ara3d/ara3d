using System.Collections.Generic;
using Ara3D.Utils;

namespace Ara3D.Services
{
    public interface IService
    {
        IReadOnlyList<INamedCommand> Commands { get; }
    }
}