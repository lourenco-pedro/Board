using System;
using System.Linq;
using ppl.PBehaviourChain.Core;
using ppl.PBehaviourChain.Core.Triggers;
using UnityEditor;
using UnityEngine;
using Behaviour = ppl.PBehaviourChain.Core.Behaviours.Behaviour;

namespace ppl.PBehaviourChain.Editor
{
    public class BehaviourChainEditorWrapper
    {
        private BehaviourChain _behaviourChain;

        public BehaviourChainEditorWrapper(BehaviourChain behaviourChain)
        {
            _behaviourChain = behaviourChain;
            SetupTriggers();
        }
        
        public TNode AddNode<TNode>()
        where TNode : Node
        {
            TNode node = ScriptableObject.CreateInstance<TNode>();
            node.name = typeof(TNode).Name;
            node.GUID = System.Guid.NewGuid().ToString();
            _behaviourChain.Nodes.Add(node);
            
            AssetDatabase.AddObjectToAsset(node, _behaviourChain);
            AssetDatabase.SaveAssets();
            
            return node;
        }

        public Node AddNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.GUID = System.Guid.NewGuid().ToString();
            _behaviourChain.Nodes.Add(node);
            
            AssetDatabase.AddObjectToAsset(node, _behaviourChain);
            AssetDatabase.SaveAssets();
            
            return node;
        }

        public void SetupTriggers()
        {
            var triggers = BehaviourChainEditorFactory.GetTriggerDerivedTypes();
            if (triggers.Length == 0)
            {
                TypeCache.TypeCollection collection = TypeCache.GetTypesDerivedFrom<TriggerNode>();
                triggers = new Type[collection.Count];
                collection.CopyTo(triggers, 0);
            }
            
            foreach (var trigger in triggers)
            {
                if (_behaviourChain.TriggerNodes.Count(t => t.GetType() == trigger) == 0)
                {
                    TriggerNode newTriggerNode = AddNode(trigger) as TriggerNode;
                    _behaviourChain.TriggerNodes.Add(newTriggerNode);
                }
            }
        }

        public void RemoveNode(Node node)
        {
            if (node is TriggerNode)
                _behaviourChain.TriggerNodes.Remove(node as TriggerNode);
            
            _behaviourChain.Nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            ppl.PBehaviourChain.Core.Triggers.TriggerNode triggerNode = parent as ppl.PBehaviourChain.Core.Triggers.TriggerNode;
            if (null != triggerNode)
            {
                triggerNode.Child = child as Behaviour;
            }
            
            ppl.PBehaviourChain.Core.Behaviours.Behaviour behaviourNode = parent as ppl.PBehaviourChain.Core.Behaviours.Behaviour;
            if (null != behaviourNode)
            {
                behaviourNode.Child = child as Behaviour;
            }
        }

        public void RemoveChild(Node parent)
        {
            ppl.PBehaviourChain.Core.Triggers.TriggerNode triggerNode = parent as ppl.PBehaviourChain.Core.Triggers.TriggerNode;
            if (null != triggerNode)
            {
                triggerNode.Child = null;
            }
            
            ppl.PBehaviourChain.Core.Behaviours.Behaviour behaviourNode = parent as ppl.PBehaviourChain.Core.Behaviours.Behaviour;
            if (null != behaviourNode)
            {
                behaviourNode.Child = null;
            }
        }

        public Node GetChildNode(Node parent)
        {
            ppl.PBehaviourChain.Core.Triggers.TriggerNode triggerNode = parent as ppl.PBehaviourChain.Core.Triggers.TriggerNode;
            if (null != triggerNode)
            {
                return triggerNode.Child;
            }
            
            ppl.PBehaviourChain.Core.Behaviours.Behaviour behaviourNode = parent as ppl.PBehaviourChain.Core.Behaviours.Behaviour;
            if (null != behaviourNode)
            {
                return behaviourNode.Child;
            }

            return null;
        }
    }
}
