using UnityEngine;

namespace ppl.PBehaviourTree.Core
{
    public abstract class Node : ScriptableObject
    {
        public enum State
        {
            Running,
            Failure,
            Success
        }    
        
        public State NodeState;
        public bool Started;
        
        protected abstract void OnStart();
        protected abstract State OnUpdate();
        protected abstract void OnStop();
        public abstract Node Instantiate();
        
        internal State Update()
        {
            if (!Started)
            {
                OnStart();
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
