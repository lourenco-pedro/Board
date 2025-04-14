using System.Collections.Generic;
using ppl.PBehaviourChain.Core.Triggers;
using UnityEngine;

namespace ppl.PBehaviourChain.Core
{
    [CreateAssetMenu(fileName = "BehaviourChain", menuName = "Behaviour Chain/New BehaviourChain")]
    public class BehaviourChain : ScriptableObject, System.IDisposable
    {
        public List<TriggerNode> TriggerNodes;
        public List<Node> Nodes = new List<Node>();
        public Node.State State;

        private bool _isInstance;

        public Node.State Update()
        {
            return State;
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
                    triggerNode.SetupTrigger();
                    chainInstance.TriggerNodes.Add(triggerNode);
                }
            }
            
            return chainInstance;
        }

        public void Dispose()
        {
            if(!_isInstance)
                return;

            foreach (TriggerNode triggerNdoe in TriggerNodes)
            {
                triggerNdoe.Dispose();
            }
        }
    }
}
