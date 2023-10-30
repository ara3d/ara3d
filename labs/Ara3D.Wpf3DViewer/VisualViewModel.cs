// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisualViewModel.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Wpf3DViewer
{
    public class VisualViewModel
    {
        public IEnumerable<VisualViewModel> Children
        {
            get
            {
                var mv = element as ModelVisual3D;
                if (mv != null)
                {
                    if (mv.Content != null)
                    {
                        yield return new VisualViewModel(mv.Content);
                    }

                    foreach (var mc in mv.Children)
                    {
                        yield return new VisualViewModel(mc);
                    }
                }

                var mg = element as Model3DGroup;
                if (mg != null)
                {
                    foreach (var mc in mg.Children)
                    {
                        yield return new VisualViewModel(mc);
                    }
                }

                var gm = element as GeometryModel3D;
                if (gm != null)
                {
                    yield return new VisualViewModel(gm.Geometry);
                }

                //int n = VisualTreeHelper.GetChildrenCount(element);
                //for (int i = 0; i < n; i++)
                //    yield return new VisualViewModel(VisualTreeHelper.GetChild(element, i));
                foreach (DependencyObject c in LogicalTreeHelper.GetChildren(element))
                {
                    yield return new VisualViewModel(c);
                }
            }
        }

        public string Name => element.GetName();

        public string TypeName => element.GetType().Name;

        public Brush Brush
        {
            get
            {
                if (element.GetType() == typeof(ModelVisual3D))
                    return Brushes.Orange;
                if (element.GetType() == typeof(GeometryModel3D))
                    return Brushes.Green;
                if (element.GetType() == typeof(Model3DGroup))
                    return Brushes.Blue;
                if (element.GetType() == typeof(Visual3D))
                    return Brushes.Gray;
                if (element.GetType() == typeof(Model3D))
                    return Brushes.Black;
                return null;
            }
        }

        public override string ToString()
        {
            return element.GetType().ToString();
        }

        private DependencyObject element;

        public VisualViewModel(DependencyObject e)
        {
            element = e;
        }
    }
}