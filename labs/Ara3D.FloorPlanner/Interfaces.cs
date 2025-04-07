using System.Windows;
using Plato;

namespace Ara3D.FloorPlanner
{
    /// <summary>
    /// A drawing abstraction.
    /// This can be used to support different drawing platforms. 
    /// </summary>
    public interface ICanvas
    {
        ICanvas Draw(StyledText text);
        ICanvas Draw(StyledLine line);
        ICanvas Draw(StyledEllipse ellipse);
        ICanvas Draw(StyledRect rect);
        Size2D MeasureText(StyledText text);
        ICanvas SetRect(Rect rect);
        ICanvas PopRect();
    }

    public interface IController
    {
        public void Update(UserInput input);
        public ICanvas Draw(ICanvas canvas);
    }
}