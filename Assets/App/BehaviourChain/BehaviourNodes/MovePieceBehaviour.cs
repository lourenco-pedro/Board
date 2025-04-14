using ppl.PBehaviourChain.Core;
using UnityEngine;

namespace App.BehaviourChain.BehaviourNodes
{
    public class MovePieceBehaviour : ppl.PBehaviourChain.Core.Behaviours.Behaviour
    {
        protected override void OnStart()
        {
            throw new System.NotImplementedException();
        }

        protected override State OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnStop()
        {
            throw new System.NotImplementedException();
        }

        public override Node Instantiate()
        {
            MovePieceBehaviour node = ScriptableObject.CreateInstance<MovePieceBehaviour>();
            node.GUID = GUID;
            node.Started = Started;
            node.Child = Child;
            node.name = name;
            node.NodeState = node.NodeState;
            return node;
        }
    }
}