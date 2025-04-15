using App.CommonEventsPayload;
using ppl.SimpleEventSystem;
using UnityEngine;

namespace App.UI
{
    public class SnapshotButtonNavigator : EventEmitterButton
    {
        [SerializeField] protected bool _isForward;

        protected override void Awake()
        {
            _button.onClick.AddListener(()=> this.EmitEvent<bool>(EventsConstants.EVENT_UI_NAVIGATE_SPANSHOT, _isForward));
            
            this.ListenToEvent<MatchUpdateEventPayload>(EventsConstants.EVENT_ON_MATCH_CHANGE, EventMatchUpdate);
        }

        protected void OnDestroy()
        {
            this.StopListenForEvent<MatchUpdateEventPayload>(EventsConstants.EVENT_ON_MATCH_CHANGE);
        }

        private void EventMatchUpdate(EventPayload<MatchUpdateEventPayload> payload)
        {
            _button.interactable = (!_isForward && payload.Args.SnapshotIndex > 0) ||
                                   (_isForward && !payload.Args.IsNow);
        }
    }
}
