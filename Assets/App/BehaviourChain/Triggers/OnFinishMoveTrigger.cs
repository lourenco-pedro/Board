using System.Collections.Generic;
using App.CommonEventsPayload;
using ppl.PBehaviourChain.Core;
using ppl.PBehaviourChain.Core.Behaviours;
using ppl.SimpleEventSystem;
using UnityEngine;

namespace App.BehaviourChain.Triggers
{
    public class OnFinishMoveTrigger : AppLevelTrigger, IEventBindable
    {
        protected void OnDestroy()
        {
            Dispose();
        }

        protected override void OnStart(Dictionary<string, object> args = null)
        {
        }
        
        protected override State OnUpdate()
        {
            return State.Success;
        }

        protected override void OnStop()
        {
        }
        
        private void EventOnFinishMove(EventPayload<FinishMoveEventPayload> payload)
        {
            if (Pawn.Id != payload.Args.PawnId)
                return;

            NodeState = State.Running;
        }
        
        public override void Dispose()
        {
            this.StopListenForEvent<FinishMoveEventPayload>(EventsConstants.EVENT_BEHAVIOURCHAIN_ON_FINISH_MOVE);
        }

        public override void SetupTrigger()
        {
            this.ListenToEvent<FinishMoveEventPayload>(EventsConstants.EVENT_BEHAVIOURCHAIN_ON_FINISH_MOVE, EventOnFinishMove);
        }
        public override Node Instantiate()
        {
            OnFinishMoveTrigger node = ScriptableObject.CreateInstance<OnFinishMoveTrigger>();
            node.GUID = GUID;
            node.Started = Started;
            node.Child = Child;
            node.name = name;
            node.NodeState = node.NodeState;

            node.Child = Child.Instantiate() as BehaviourNode;
            
            return node;
        }
    }
}