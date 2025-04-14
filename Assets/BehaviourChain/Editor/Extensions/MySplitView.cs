using UnityEngine.UIElements;

namespace ppl.PBehaviourChain.Editor.Extensions
{
    public class MySplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<MySplitView, TwoPaneSplitView.UxmlTraits> { }
    }
}
