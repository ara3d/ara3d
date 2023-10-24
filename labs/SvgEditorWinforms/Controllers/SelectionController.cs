using SvgDemoWinForms;
using SvgEditorWinForms.Models;

namespace SvgEditorWinForms
{
    public class SelectionController
    {
        public event EventHandler SelectionChanged;

        public List<ElementModel> SelectedShapes { get; private set; } = new(); 

        public void ClearSelection()
        {
            SelectedShapes.Clear();
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void AddToSelection(ElementModel shape)
        {
            AddToSelection(new List<ElementModel>() { shape });
        }

        public void AddToSelection(IEnumerable<ElementModel> shapes)
        {
            SelectedShapes.AddRange(shapes);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Select(ElementModel? element)
        {
            Select(element == null 
                ? Array.Empty<ElementModel>() 
                : new[] { element });
        }

        public void Select(IEnumerable<ElementModel> elements)
        {
            // Don't send pointless notifications  
            if (SelectedShapes.UpdateIfChanged(elements))
                SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
