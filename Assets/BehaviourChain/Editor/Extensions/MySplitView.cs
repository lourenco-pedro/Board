using UnityEngine.UIElements;

namespace ppl.PBehaviourTree.Editor.Extensions
{
    public class MySplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<MySplitView, TwoPaneSplitView.UxmlTraits> { }
    }
}
