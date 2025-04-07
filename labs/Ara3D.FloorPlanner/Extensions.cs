using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Ara3D.FloorPlanner;
using Plato;

namespace Ara2D.FloorPlanner;

public static class Extensions
{
    public static Rect2D Shrink(this Rect2D r, Rect2D padding)
        => Shrink(r, padding.Left, padding.Top, padding.Right, padding.Bottom);

    public static Rect2D Shrink(this Rect2D r, float left, float top, float right, float bottom) 
        => new((r.Left + left, r.Top + top), (r.Width - left - right, r.Height - top - bottom));

    public static Rect2D Shrink(this Rect2D r, float padding)
        => Shrink(r, padding, padding, padding, padding);

    public static Rect2D GetRow(this Rect2D r, int row, int rowCount)
        => new((r.Left, r.Top + r.Height * row / rowCount), (r.Width, r.Height / rowCount));

    public static Rect2D GetColumn(this Rect2D r, int col, int colCount)
        => new((r.Left + r.Width * col / colCount, r.Top), (r.Width / colCount, r.Bottom));

    public static Rect2D GetRowColumn(this Rect2D r, int row, int col, int rowCount, int colCount)
        => GetColumn(GetRow(r, row, rowCount), col, colCount);

    public static IEnumerable<Rect2D> GetGrid(this Rect2D r, int rowCount, int colCount)
        => Enumerable.Range(0, rowCount * colCount).Select(i => GetRowColumn(r, i / colCount, i % colCount, rowCount, colCount));

    /*
    public static IEnumerable<Rect2D> ComputeStackLayout(this Rect2D rect, IReadOnlyList<Rect2D> children, bool horizontalOrVertical = false, bool reverse = false)
        => horizontalOrVertical
            ? reverse
                ? ComputeHorizontalLayoutRight(rect, children)
                : ComputeHorizontalLayoutLeft(rect, children)
            : reverse
                ? ComputeVerticalLayoutUp(rect, children)
                : ComputeVerticalLayoutDown(rect, children);

    public static IEnumerable<Rect2D> ComputeVerticalLayoutDown(this Rect2D rect, IReadOnlyList<Rect2D> children)
    {

        var top = rect.Top;
        for (var i = 0; i < children.Count - 1; ++i)
            yield return new Rect2D((rect.Left, top), (rect.Width, top += children[i].Height));
        yield return new Rect2D(new Point2D(rect.Left, top), new Point2D(rect.Right, rect.Bottom));
    }

    public static IEnumerable<Rect2D> ComputeHorizontalLayoutRight(this Rect2D rect, IReadOnlyList<Rect2D> children)
    {
        var left = rect.Left;
        for (var i = 0; i < children.Count - 1; ++i)
            yield return new Rect2D(new Point2D(left, rect.Top), new Point2D(left += children[i].Width, rect.Bottom));
        yield return new Rect2D(new Point2D(left, rect.Top), new Point2D(rect.Right, rect.Bottom));
    }

    public static IEnumerable<Rect2D> ComputeVerticalLayoutUp(this Rect2D rect, IReadOnlyList<Rect2D> children)
    {
        var bottom = rect.Bottom;
        for (var i = 0; i < children.Count - 1; ++i)
            yield return new Rect2D(new Point2D(rect.Left, bottom -= children[i].Height), new Size2D(rect.Width, children[i].Height));
        yield return new Rect2D(new Point2D(rect.Left, rect.Top), new Point2D(rect.Right, bottom));
    }

    public static IEnumerable<Rect2D> ComputeHorizontalLayoutLeft(this Rect2D rect, IReadOnlyList<Rect2D> children)
    {
        var right = rect.Right;
        for (var i = 0; i < children.Count - 1; ++i)
            yield return new Rect2D(new Point2D(right -= children[i].Width, rect.Bottom), new Size2D(children[i].Width, rect.Height));
        yield return new Rect2D(new Point2D(rect.Left, rect.Top), new Point2D(right, rect.Bottom));
    }
    */

    public static Rect2D GetRect(this Window window)
        => new((0, 0), ((float)window.Width, (float)window.Height));

    public static WindowProps GetProps(this Window window)
        => new(GetRect(window), window.Title, window.Cursor);

    public static Window ApplyRect(this Window window, Rect2D rect)
    {
        window.Left = rect.Left;
        window.Top = rect.Top;
        window.Width = rect.Width;
        window.Height = rect.Height;
        return window;
    }

    public static Window ApplyCursor(this Window window, Cursor cursor)
        => (window.Cursor = cursor, window).Item2;

    public static Window ApplyTitle(this Window window, string title)
        => (window.Title = title, window).Item2;

    public static Window ApplyProps(this Window window, WindowProps props)
        => ApplyTitle(window.ApplyRect(props.Rect).ApplyCursor(props.Cursor), props.Title);

    public static Rect2D Resize(this Rect2D rect, Size2D size)
        => new(rect.TopLeft, size);

    public static Rect2D MoveTo(this Rect2D rect, Point2D point)
        => new(point, rect.Size);

    public static Point2D Add(this Point2D self, Vector2 v)
        => new(self.X + v.X, self.Y + v.Y);

    public static Point2D Add(this Point2D self, Point2D point)
        => new(self.X + point.X, self.Y + point.Y);

    public static Rect2D MoveBy(this Rect2D rect, Point2D point)
        => new(rect.TopLeft.Add(point), rect.Size);

    public static Point2D Negate(this Point2D point)
        => new(-point.X, -point.Y);

    public static Point2D Subtract(this Point2D self, Point2D point)
        => self.Add(Negate(point));

    public static Point2D GetScreenPosition(this MouseEventArgs args, Window window)
        => window.PointToScreen(args.GetPosition(window)).ToPlato();

    public static float HalfHeight(this Rect2D rect)
        => rect.Height.Half;

    public static float HalfHeight(this Size2D size)
        => size.Height.Half;

    public static float HalfWidth(this Rect2D rect)
        => rect.Width.Half;

    public static float HalfWidth(this Size2D size)
        => size.Width.Half;

    public static Point2D Center(this Rect2D r)
        => new(r.Left + r.HalfWidth(), r.Top + r.HalfHeight());

    public static Point2D LeftCenter(this Rect2D r)
        => new(r.Left, r.Top + r.HalfHeight());

    public static Point2D RightCenter(this Rect2D r)
        => new(r.Right, r.Top + r.HalfHeight());

    public static Point2D TopCenter(this Rect2D r)
        => new(r.Left + r.HalfWidth(), r.Top);

    public static Point2D BottomCenter(this Rect2D r)
        => new(r.Left + r.HalfWidth(), r.Bottom);

    public static Size2D Half(this Size2D size)
        => new(size.HalfWidth(), size.HalfHeight());

    public static Point2D Subtract(this Point2D p, Size2D size)
        => new(p.X - size.Width, p.Y - size.Height);

    public static Point2D CenterTopLeftOfSubarea(this Rect2D rect, Size2D size)
        => Center(rect).Subtract(Half(size));

    public static Point2D GetAlignedLocation(this Rect2D rect, Size2D size, Alignment alignment)
        =>
            new(
                alignment.X switch
                {
                    AlignmentX.Right => rect.Right - size.Width,
                    AlignmentX.Center => Center(rect).X - size.HalfWidth(),
                    _ => rect.Left
                },
                alignment.Y switch
                {
                    AlignmentY.Bottom => rect.Bottom - size.Height,
                    AlignmentY.Center => Center(rect).Y - size.HalfHeight(),
                    _ => rect.Top
                });

    public static bool NonZero(this Rect2D self)
        => self.Width > 0 && self.Height > 0;

    public static Size2D GetSize(this FormattedText Text)
        => new((float)Text.Width, (float)Text.Height);

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> self) where T : class
        => self.Where(x => x != null);

    public static IEnumerable<T> WhereNotNull<T>(params T?[] self) where T : class
        => WhereNotNull<T>((IEnumerable<T?>)self);

    public static Rect2D SetSize(this Rect2D rect, Size2D size)
        => new(rect.TopLeft, size);

    public static Rect2D SetHeight(this Rect2D rect, float height)
        => rect.SetSize(new(rect.Width, height));
    
    public static Rect2D SetWidth(this Rect2D rect, float width)
        => rect.SetSize(new(width, rect.Height));

    public static Size2D Subtract(this Size2D size, Size2D amount)
        => new(Math.Max(0, size.Width - amount.Width), Math.Max(0, size.Height - amount.Height));

    public static Rect2D Shrink(this Rect2D rect, Size2D size)
        => new(rect.TopLeft, rect.Size.Subtract(size));

    public static Rect2D ShrinkFromCenter(this Rect2D rect, Size2D size)
        => new(rect.TopLeft.Add(size.Half()), rect.Size.Subtract(size));

    public static Rect2D Offset(this Rect2D rect, Size2D size)
        => new(rect.TopLeft.Add(size), rect.Size);

    public static Point2D Add(this Point2D point, Size2D size)
        => new(point.X + size.Width, point.Y + size.Height);

    public static Rect2D ShrinkAndOffset(this Rect2D rect, Size2D size)
        => new(rect.TopLeft.Add(size), rect.Size.Subtract(size));

    public static Point2D Multiply(this Point2D point, float factor)
        => new(point.X * factor, point.Y * factor);

    public static IReadOnlyList<T> Add<T>(this IReadOnlyList<T> self, T item)
        => self.Append(item).ToList();

    public static IReadOnlyList<T> Remove<T>(this IReadOnlyList<T> self, T item)
        => self.Where(x => x != null && !ReferenceEquals(x, item) && !x.Equals(item)).ToList();

    public static Rect2D ToSquareWithCenter(this Point2D point, float side)
        => new((point.X - side / 2, point.Y - side / 2), (side, side));

    public static Rect2D ToRect(this Point2D point, Size2D size)
        => new(point, size);
}