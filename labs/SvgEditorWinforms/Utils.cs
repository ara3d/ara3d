using Svg;
using SvgEditorWinForms.Models;

namespace SvgDemoWinForms
{
    public static class Utils
    {
        public static Color ToDrawingColor(this ColorModel color)
        {
            return Color.FromArgb(color.R, color.G, color.B);
        }

        public static SvgPaintServer ToSvg(this ColorModel color)
        {
            return new SvgColourServer(color.ToDrawingColor());
        }


        public static SvgDocument ToSvg(this DocumentModel documentModel)
        {
            var r = new SvgDocument();
            foreach (var layer in documentModel.Layers)
            {
                if (layer.IsActive)
                {
                    foreach (var e in layer.Elements)
                    {
                        r.Children.Add(e.ToSvg());
                    }
                }
            }

            foreach (var e in documentModel.Elements)
            {
                var tmp = e.ToSvg();
                if (tmp != null) 
                    r.Children.Add(tmp);
            }
            return r;
        }

        public static IEnumerable<ToolStripMenuItem> AllMenuItems(this ToolStripItemCollection collection)
        {
            foreach (var m in collection.OfType<ToolStripMenuItem>())
            {
                yield return m;
                foreach (var m2 in m.DropDownItems.AllMenuItems())
                    yield return m2;
            }
        }

        public static void PreventFormDisposal(this Form form)
        {
            form.FormClosing += (sender, args) =>
            {
                form.Hide();
                args.Cancel = true;
            };
        }

        public static ColorModel ToModel(this Color color)
            => new() { R = color.R, B = color.B, G = color.G };

        public static bool UpdateIfChanged<T>(this List<T> self, IEnumerable<T> newValues)
        {
            var tmp = newValues.ToList();
            if (tmp.Count != self.Count || !self.SequenceEqual(tmp))
            {
                self.Clear();
                self.AddRange(tmp);
                return true;
            }

            return false;
        }
    }
}
