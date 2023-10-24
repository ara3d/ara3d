namespace SvgDemoWinForms
{
    public class UndoController<T>
    {
        public T Current { get; private set; }
        public List<T> UndoStack { get; } = new();
        public List<T> RedoStack { get; } = new();

        public UndoController(T current)
        {
            Current = current;
        }

        public void Reset(T current)
        {
            Current = current;
            UndoStack.Clear();
            RedoStack.Clear();
            NotifyListeners();
        }

        public T Undo()
        {
            if (!CanUndo())
            {
                throw new InvalidOperationException("Can't undo");
            }

            RedoStack.Add(Current); 
            Current = PopStack(UndoStack);
            NotifyListeners();
            return Current;
        }

        public void NewItem(T item)
        {
            UndoStack.Add(Current);
            Current = item;
            RedoStack.Clear();
            NotifyListeners();
        }

        public void NotifyListeners()
        {
            UndoStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public static T PopStack(List<T> stack)
        {
            var r = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1); 
            return r;
        }

        public T Redo()
        {
            if (!CanRedo())
            {
                throw new InvalidOperationException("Can't redo");
            }

            UndoStack.Add(Current);
            Current = PopStack(RedoStack);
            NotifyListeners();
            return Current;
        }

        public bool CanUndo()
            => UndoStack.Count > 0;

        public bool CanRedo()
            => RedoStack.Count > 0;

        public event EventHandler? UndoStateChanged;
    }
}