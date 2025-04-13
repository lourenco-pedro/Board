using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.CommonEventsPayload;
using ppl.SimpleEventSystem;
using UnityEngine;

namespace App.Services.MatchService.Implementations
{
    public class MatchService : IMatchService, IEventBindable
    {
        public string Name => nameof(MatchService);
        
        private Match _match;
        private int _matchSnapshotIndex;
        private int _previusIndex;

        private void EmitEvent(string eventName)
        {
            switch (eventName)
            {
                case EventsConstants.EVENT_ON_MATCH_CHANGE:
                    this.EmitEvent(EventsConstants.EVENT_ON_MATCH_CHANGE, 
                        new MatchUpdateEventPayload(
                            _match, 
                            _matchSnapshotIndex, 
                            _previusIndex, 
                            _matchSnapshotIndex == _match.Snapshots.Count - 1, 
                            _match.Teams,
                            _match.BlockedTiles));
                    break;
            }
        }

        private void EventOnMovementData(EventPayload<MatchSnapshot> payload)
        {
            _previusIndex = _matchSnapshotIndex;
            _match.Snapshots.Add(payload.Args);
            _matchSnapshotIndex = _match.Snapshots.Count - 1;
            EmitEvent(EventsConstants.EVENT_ON_MATCH_CHANGE);
        }

        public Task AsyncSetup(Dictionary<string, object> args = null)
        {
            this.ListenToEvent<MatchSnapshot>(EventsConstants.EVENT_ON_MOVEMENT_DATA, EventOnMovementData);
            return Task.CompletedTask;
        }

        public bool SetMatch(Match match, int overrideSnapshotIndex = -1)
        {
            _match = match;
            
            int snapshotIndex = overrideSnapshotIndex == -1 ? Mathf.Max(match.Snapshots.Count - 1, 0) : overrideSnapshotIndex;
            
            SetHistoryIndex(snapshotIndex);
            return true;
        }

        public int GetHistoryIndex()
        {
            return _matchSnapshotIndex;
        }
        
        public void SetHistoryIndex(int index)
        {
            _previusIndex = _matchSnapshotIndex;
            _matchSnapshotIndex = Mathf.Clamp(index, 0, _match.Snapshots.Count - 1);
            EmitEvent(EventsConstants.EVENT_ON_MATCH_CHANGE);
        }

        public void RegisterMatchHistory(MatchSnapshot snapshot)
        {
            _match.Snapshots.Add(snapshot);
        }

#if UNITY_EDITOR
        public void DebugService()
        {
            int index = 0;
            foreach (MatchSnapshot snapshot in _match.Snapshots)
            {
                string pawns = string.Join(",", snapshot.Pieces.Keys.ToArray());
                GUILayout.Label($"Snapshot { index }: { pawns }");
                index++;
            }
        }
#endif
    }
}
