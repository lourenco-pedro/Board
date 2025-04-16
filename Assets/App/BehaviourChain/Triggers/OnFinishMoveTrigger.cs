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
        private bool _preventDefault = false;
        
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
            _preventDefault = false;
        }
        
        private void EventOnFinishMove(EventPayload<FinishMoveEventPayload> payload)
        {
            if(_preventDefault)
                return;
            
            if (Pawn.Id != payload.Args.PawnId)
                return;

            _preventDefault = true;
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

            node.Child = Child?.Instantiate() as BehaviourNode;
            
            return node;
        }
    }
}