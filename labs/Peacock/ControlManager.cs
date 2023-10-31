using System.Windows;

namespace Peacock
{
    /// <summary>
    /// The ControlManager class contains the current control tree, which it creates from a control factory.
    /// It also maintains the list of associated behaviors with each control. 
    /// The control factory can be updated at any time. 
    /// </summary>
    [Mutable]
    public class ControlManager
    {
        public ControlManager(IControlFactory factory)
            => Factory = factory;

        public void UpdateControlTree(IModel model, Rect rect, IControlFactory? newFactory = null)
        {
            Factory = newFactory ?? Factory;
            Controls = Factory.Create(model, rect).ToList();
            UpdateBehaviors();
        }

        public Dictionary<object, List<IBehavior>> Behaviors { get; } = new();
        public IControlFactory Factory { get; set; }
        public IReadOnlyList<IControl> Controls { get; set; }
        public IEnumerable<IControl> AllControls
            => Controls.SelectMany(c => c.Descendants());

        private void UpdateBehaviors()
        {
            var dict = AllControls.ToDictionary(c => c.View.Id, c => c);
            // Remove behaviors no longer used 
            foreach (var id in Behaviors.Keys)
                if (!dict.ContainsKey(id))
                    Behaviors.Remove(id);
            // Add default behaviors
            foreach (var id in dict.Keys)
                if (!Behaviors.ContainsKey(id))
                    Behaviors.Add(id, dict[id].GetDefaultBehaviors().ToList());
        }        

        public ICanvas Draw(ICanvas canvas)
            => Controls.Aggregate(canvas, Draw);

        public ICanvas Draw(ICanvas canvas, IControl control)
        {
            canvas = canvas.SetRect(control.Measures.RelativeRect);
            canvas = GetBehaviors(control).Aggregate(canvas, (localCanvas, b) => b.PreDraw(localCanvas, control));
            canvas = control.Draw(canvas);    
            foreach (var child in control.Children)
                canvas = Draw(canvas, child);
            canvas = GetBehaviors(control).Aggregate(canvas, (localCanvas, b) => b.PostDraw(localCanvas, control));
            canvas = canvas.PopRect();
            return canvas;
        }

        public IUpdates ProcessBehaviorInput(IControl control, InputEvent input, IUpdates updates, IEnumerable<IBehavior> behaviors)
            => behaviors.Aggregate(updates, (current, behavior) => behavior.Process(control, input, current));

        public IEnumerable<IBehavior> GetBehaviors(IControl control)
            => Behaviors.ContainsKey(control.View.Id) ? Behaviors[control.View.Id] : Array.Empty<IBehavior>();

        public IUpdates ProcessControlInput(InputEvent input, IUpdates updates, IControl control)
            => control.Process(input, ProcessBehaviorInput(control, input, updates, GetBehaviors(control)));

        /// <summary>
        /// Process the input for all the controls, and their associated behaviors. Behaviors are given the chance to
        /// process input before the control.
        /// TODO: does that make sense? 
        /// </summary>
        public IUpdates ProcessInput(InputEvent input, IUpdates? updates = null)
            => AllControls.Aggregate(updates ?? new Updates(), (current, control) => ProcessControlInput(input, current, control));

        public void ApplyChanges(IUpdates updates)
        {
            // TODO: Add new behaviors from the updates. 

            // Update behaviors 
            foreach (var controlId in Behaviors.Keys.ToList())
                Behaviors[controlId] = Behaviors[controlId]
                    .Select(updates.Apply).ToList();

            // Update controls
            foreach (var control in AllControls)
            {
                var newControl = updates.Apply(control);
                if (!control.Equals(newControl))
                    updates = newControl.Callback.Invoke(updates, control, newControl);
            }
        }
    }
}
