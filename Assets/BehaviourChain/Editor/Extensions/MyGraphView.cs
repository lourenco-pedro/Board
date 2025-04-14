using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace ppl.PBehaviourChain.Editor.Extensions
{
    public class MyGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<MyGraphView, GraphView.UxmlTraits> { }
    
        public Action<NodeView> OnNodeSelected;    
        
        private BehaviourChainEditorWrapper _behaviourChainEditorWrapper;
        
        public MyGraphView()
        {
            Insert(0, new GridBackground());            
         
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviourChain/Editor/BehaviourChainEditor.uss");
            styleSheets.Add(styleSheet);
        }

        public void SetWrapper(BehaviourChainEditorWrapper chainWrapper)
        {
            _behaviourChainEditorWrapper = chainWrapper;
        }
        
        public void PopulateGraph(ppl.PBehaviourChain.Core.Node[] nodes)
        {
            graphViewChanged -= OnGhraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGhraphViewChanged;
            
            Array.ForEach(nodes, CreateNodeView);
            
            Array.ForEach(nodes, (node) =>
            {
                ppl.PBehaviourChain.Core.Node child = _behaviourChainEditorWrapper.GetChildNode(node);
                
                if(null == child)
                    return;
                
                NodeView parentNodeView = GetNodeView(node);
                NodeView childNodeView = GetNodeView(child);

                Edge edge = parentNodeView.OutputPort.ConnectTo(childNodeView.InputPort);
                AddElement(edge);
            });
        }
        
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(port =>
            {
                return port.direction != startPort.direction && port.node != startPort.node;
            }).ToList();
        }

        private NodeView GetNodeView(ppl.PBehaviourChain.Core.Node node)
        {
            return GetNodeByGuid(node.GUID) as NodeView;
        }

        private GraphViewChange OnGhraphViewChanged(GraphViewChange graphviewchange)
        {
            if (graphviewchange.elementsToRemove != null)
            {
                graphviewchange.elementsToRemove.ForEach(elem =>
                {
                    NodeView nodeView = elem as NodeView;
                    if (null != nodeView)
                        _behaviourChainEditorWrapper.RemoveNode(nodeView.Node);
                    
                    Edge edge = elem as Edge;
                    if (null != edge)
                    {
                        NodeView parent = edge.output.node as NodeView;

                        if(null == parent)
                            return;
                        
                        _behaviourChainEditorWrapper.RemoveChild(parent.Node);
                    }
                });
            }

            if (graphviewchange.edgesToCreate != null)
            {
                graphviewchange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parent = edge.output.node as NodeView;
                    NodeView child = edge.input.node as NodeView;
                    
                    if(null == parent || null == child)
                        return;
                        
                    _behaviourChainEditorWrapper.AddChild(parent.Node, child.Node);
                });
            }
            
            return graphviewchange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            (string contextMenu, BehaviourChainEditorFactory.BehaviourProviderEventHandler evt)[] menus =
                BehaviourChainEditorFactory.GetContextMenuItems();
            foreach (var menu in menus)
            {
                evt.menu.AppendAction(menu.contextMenu, a =>
                {
                    ppl.PBehaviourChain.Core.Node node = menu.evt(_behaviourChainEditorWrapper);
                    CreateNodeView(node);
                });
            }
        }

        void CreateNodeView(ppl.PBehaviourChain.Core.Node node)
        {
            NodeView nodeView = new NodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            AddElement(nodeView);
        }
    }
}
