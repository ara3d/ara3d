using System;
using Plato.Geometry.Scenes;

namespace Ara3D.Viewer.Api
{
    public interface IApi
    {
        Scene Scene { get; set; }
        void LoadIfc(string filePath);
        void LoadPly(string file);
        void SaveViewAsImage(string filePath);
    }
}
