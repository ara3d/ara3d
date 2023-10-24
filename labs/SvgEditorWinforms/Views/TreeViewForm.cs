using SvgEditorWinForms;
using SvgEditorWinForms.Models;

namespace SvgDemoWinForms
{
    public partial class TreeViewForm : Form
    {
        public SelectionController SelectionController { get; }
        public DataModelController DataModelController { get; }

        public TreeViewForm(SelectionController selectionController, DataModelController dataModelController)
        {
            InitializeComponent();
            SelectionController = selectionController;
            DataModelController = dataModelController;
        }

        public void Update(DocumentModel d)
        {
            // Get the previous selection 
            var oldSel = treeView1.SelectedNode?.Name;
            
            // Remove and recreate all of the nodes
            treeView1.Nodes.Clear();
            for (var i = 0; i < d.Elements.Count; i++)
            {
                var e = d.Elements[i];
                var n = treeView1.Nodes.Add(e.Id, e.Name);
                n.Tag = e;
            }

            // Restore the old selection (if it existed)
            if (oldSel != null)
            {
                treeView1.SelectedNode = treeView1.Nodes[oldSel];
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid1.SelectedObject = null;
            var selected = treeView1.SelectedNode;
            if (selected == null)
                return;
            var element = selected.Tag as ElementModel;
            SelectionController.Select(element);
            if (element == null)
                return;
            propertyGrid1.SelectedObject = element;
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            DataModelController.NotifyOfChange();
        }
    }
}
