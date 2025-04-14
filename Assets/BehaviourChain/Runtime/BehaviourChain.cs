using System.Collections.Generic;
using ppl.PBehaviourChain.Core.Triggers;
using UnityEngine;

namespace ppl.PBehaviourChain.Core
{
    [CreateAssetMenu(fileName = "BehaviourChain", menuName = "Behaviour Chain/New BehaviourChain")]
    public class BehaviourChain : ScriptableObject
    {
        public List<TriggerNode> TriggerNodes;
        public List<Node> Nodes = new List<Node>();

        private bool _isInstance;
        
        public void Update()
        {
            foreach (var triggeredNode in TriggerNodes)
            {
                if(triggeredNode.NodeState != Node.State.Running)
                    continue;

                if (triggeredNode.Update() != Node.State.Running)
                {
                    triggeredNode.NodeState = Node.State.Idle;
                    triggeredNode.Child.ResetState();
                }
            }
        }

        public BehaviourChain Instantiate()
        {
            BehaviourChain chainInstance = ScriptableObject.CreateInstance<BehaviourChain>();
            chainInstance._isInstance = true;
            
            chainInstance.TriggerNodes = new List<TriggerNode>();
            foreach (TriggerNode rootNode in TriggerNodes)
            {
                TriggerNode triggerNode = rootNode.Instantiate() as TriggerNode;

                if (null != triggerNode)
                {
                    chainInstance.TriggerNodes.Add(triggerNode);
                    chainInstance.Nodes.Add(triggerNode);
                }
            }
            
            return chainInstance;
        }
    }
}
