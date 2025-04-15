namespace ppl.PBehaviourChain.Core.Behaviours
{
    public abstract class BehaviourNode : Node
    {
        public BehaviourNode Child;

        public virtual void ResetState()
        {
            NodeState = State.Idle;
            Child?.ResetState();
        }
        
        public Node GetNextChild()
        {
            if (NodeState == State.Idle || NodeState == State.Running)
            {
                return this;
            }

            return Child != null ? Child.GetNextChild() : null;
        }
    }
}
