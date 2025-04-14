using ppl.PBehaviourChain.Core;
using UnityEngine;
using Behaviour = ppl.PBehaviourChain.Core.Behaviours.Behaviour;

namespace App.BehaviourChain.BehaviourNodes
{
    public class MovePieceBehaviour : ppl.PBehaviourChain.Core.Behaviours.Behaviour
    {
        protected override void OnStart()
        {
            Debug.Log("Start Move Piece Behaviour for " + name);
        }

        protected override State OnUpdate()
        {
            Debug.Log("Update Move Piece Behaviour for " + name);
            return State.Success;
        }

        protected override void OnStop()
        {
            Debug.Log("Stop Move Piece Behaviour for " + name);
        }

        public override Node Instantiate()
        {
            MovePieceBehaviour node = ScriptableObject.CreateInstance<MovePieceBehaviour>();
            node.GUID = GUID;
            node.Started = Started;
            node.Child = Child;
            node.name = name;
            node.NodeState = node.NodeState;
            node.Child = Child?.Instantiate() as Behaviour;
            return node;
        }
    }
}