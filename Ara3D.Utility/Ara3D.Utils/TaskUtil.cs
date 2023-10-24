using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ara3D.Utils
{
    public static class TaskUtil
    {
        public static async Task WhenAll(params Action[] actions)
            => await Task.WhenAll(actions.Select(Task.Run));
    }
}