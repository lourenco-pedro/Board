using ppl.PBehaviourChain.Core.Triggers;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = ppl.PBehaviourChain.Core.Node;

namespace ppl.PBehaviourChain.Editor
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public System.Action<NodeView> OnNodeSelected;
        
        public Port InputPort;
        public Port OutputPort;
        
        private Node _node;
        
        public sealed override string title
        {
            get { return base.title; }
            set { base.title = value; }
        }

        public Node Node
        {
            get { return _node; }
        }
        
        public NodeView(Node node)
        {
            _node = node;
            this.title = node.name;
            this.viewDataKey = node.GUID;
            
            style.left = node.Position.x;
            style.top = node.Position.y;

            if (node is TriggerNode)
            {
                titleContainer.style.backgroundColor = new StyleColor(new Color32(156, 103, 51, 255));
                titleContainer.Q<Label>().style.unityFontStyleAndWeight = FontStyle.Bold;
            }
            
            CreateInputPorts();
            CreateOutputPorts();
        }
        
        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }

        private void CreateOutputPorts()
        {
            if (_node is ppl.PBehaviourChain.Core.Behaviours.Behaviour)
            {
                OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                    typeof(bool));
            }
            
            if (_node is ppl.PBehaviourChain.Core.Triggers.TriggerNode)
            {
                OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                    typeof(bool));
            }

            if (null != OutputPort)
            {
                OutputPort.portName = "";
                outputContainer.Add(OutputPort);
            }
        }

        private void CreateInputPorts()
        {
            if (_node is ppl.PBehaviourChain.Core.Behaviours.Behaviour)
            {
                InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                    typeof(bool));
            }

            if (null != InputPort)
            {
                InputPort.portName = "";
                inputContainer.Add(InputPort);
            }
        }

        public override void SetPosition(Rect position)
        {
            base.SetPosition(position);
            _node.Position.x = position.xMin;
            _node.Position.y = position.yMin;
        }
    }
}
