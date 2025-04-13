using System;

namespace ppl.PBehaviourTree.Core.Triggers
{
    public abstract class TriggerNode : Node, IDisposable
    {
        public Node Child;

        public abstract void SetupTrigger();
        public abstract void Dispose();
    }
}