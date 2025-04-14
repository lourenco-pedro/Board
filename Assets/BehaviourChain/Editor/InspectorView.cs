using UnityEngine.UIElements;
using UnityEditor;

namespace ppl.PBehaviourChain.Editor
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        private UnityEditor.Editor editor = null;
        
        public void UpdateSelection(NodeView nodeView)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);
            editor = UnityEditor.Editor.CreateEditor(nodeView.Node);
            IMGUIContainer container = new IMGUIContainer(()=> editor.OnInspectorGUI());
            Add(container);
        }
    }
}
