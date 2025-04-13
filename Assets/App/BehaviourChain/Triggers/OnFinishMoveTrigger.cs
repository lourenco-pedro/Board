using App.CommonEventsPayload;
using ppl.PBehaviourTree.Core;
using ppl.PBehaviourTree.Core.Triggers;
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
            return ScriptableObject.CreateInstance<OnFinishMoveTrigger>();
        }
    }
}