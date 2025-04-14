using App.CommonEventsPayload;
using ppl.PBehaviourChain.Core;
using ppl.PBehaviourChain.Core.Triggers;
using ppl.SimpleEventSystem;
using UnityEngine;

namespace App.BehaviourChain.Triggers
{
    public class OnFinishMoveTrigger : TriggerNode, IEventBindable
    {
        protected override void OnStart()
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
        }

        public override void SetupTrigger()
        {
            this.ListenToEvent<FinishMoveEventPayload>(EventsConstants.EVENT_BEHAVIOURCHAIN_ON_FINISH_MOVE, EventOnFinishMove);
        }
        
        public override void Dispose()
        {
            this.StopListenForEvent<FinishMoveEventPayload>(EventsConstants.EVENT_BEHAVIOURCHAIN_ON_FINISH_MOVE);
        }

        public override Node Instantiate()
        {
            OnFinishMoveTrigger node = ScriptableObject.CreateInstance<OnFinishMoveTrigger>();
            node.GUID = GUID;
            node.Started = Started;
            node.Child = Child;
            node.name = name;
            node.NodeState = node.NodeState;
            return node;
        }
    }
}