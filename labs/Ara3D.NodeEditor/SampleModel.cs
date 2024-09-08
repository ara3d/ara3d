using System.Windows;

namespace Ara3D.NodeEditor
{
    public class SampleModel
    {
        public GraphModel Graph(Rect rect)
        {
            return new GraphModel(
                Guid.NewGuid(), Nodes(8), Connectors(), rect);
        }

        public Rect LastRect;

        public string[] NodeNames =
        {
            "NodeA", 
            "Another Node", 
            "Yet another node", 
            "More nodes", 
            "Second Node", 
            "2nd Node", 
            "1st Node"
        }; 

        public string[] OpNames =
        {
            "OperatorA", 
            "OperatorB", 
            "Move", 
            "Distort",
            "Random",
            "Select",
            "Range",
            "Slice",
            "Distance",
            "Select"
        };
        public int OpIndex;

        public NodeModel Node(int nodeIndex)
        {
            var name = NodeNames[nodeIndex % NodeNames.Length];
            var rect = new Rect(LastRect.TopRight.Add(new Vector(20, 0)), new Size(40, 60));
            LastRect = rect;
            var r = new NodeModel(Guid.NewGuid(), name, rect, Operators(5));
            return r;
        }

        public OperatorModel Operator(int i)
        {
            var name = OpNames[OpIndex++ % OpNames.Length];
            var r = new OperatorModel(Guid.NewGuid(), name, i, Array.Empty<PropertyModel>());
            return r;
        }

        public IReadOnlyList<OperatorModel> Operators(int count)
        {
            return Enumerable.Range(0, count).Select(Operator).ToList();
        }

        public IReadOnlyList<ConnectorModel> Connectors()
        {
            return Array.Empty<ConnectorModel>();
        }

        public IReadOnlyList<NodeModel> Nodes(int count)
        {
            return Enumerable.Range(0, count).Select(Node).ToList();
        }
    }
}
