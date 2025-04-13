using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace ppl.PBehaviourTree.Editor.Extensions
{
    public class MyGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<MyGraphView, GraphView.UxmlTraits> { }
        public MyGraphView()
        {
            Insert(0, new GridBackground());            
         
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviourChain/Editor/BehaviourChainEditor.uss");
            styleSheets.Add(styleSheet);
        }
    }
}
