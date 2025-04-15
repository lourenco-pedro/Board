using System.Collections.Generic;
using UnityEngine;

namespace ppl.PBehaviourChain.Core
{
    public abstract class Node : ScriptableObject
    {
        public enum State
        {
            Idle,
            Running,
            Failure,
            Success
        }    
        
        [HideInInspector] public State NodeState = State.Idle;
        
        [HideInInspector] public bool Started;
        [HideInInspector] public string GUID;
        [HideInInspector] public Vector2 Position;
        
        protected abstract void OnStart(Dictionary<string, object> args = null);
        protected abstract State OnUpdate();
        protected abstract void OnStop();
        public abstract Node Instantiate();
        
        public virtual State Update(Dictionary<string, object> args = null)
        {
            if (!Started)
            {
                OnStart(args);
                Started = true;
            }
            
            NodeState = OnUpdate();

            if (NodeState == State.Failure || NodeState == State.Success)
            {
                Started = false;
                OnStop();
            }

            return NodeState;
        }
    }
}
