namespace ppl.PBehaviourChain.Core.Behaviours
{
    public abstract class Behaviour : Node
    {
        public Behaviour Child;

        public void ResetState()
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
