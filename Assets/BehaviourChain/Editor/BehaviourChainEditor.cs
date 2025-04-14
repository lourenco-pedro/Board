using System.Collections.Generic;
using ppl.PBehaviourChain.Core;
using ppl.PBehaviourChain.Editor.Extensions;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace ppl.PBehaviourChain.Editor
{

    public class BehaviourChainEditor : EditorWindow
    {
        [FormerlySerializedAs("m_VisualTreeAsset")] [SerializeField]
        private VisualTreeAsset _visualTreeAsset = default;

        private MyGraphView _myGraphView = default;
        private InspectorView _inspectorView = default;

        private BehaviourChainEditorWrapper _chainWrapper;

        [MenuItem("BehaviourChain/Editor")]
        public static void Open()
        {
            BehaviourChainEditor wnd = GetWindow<BehaviourChainEditor>();
            wnd.titleContent = new GUIContent("BehaviourChainEditor");
            wnd.OnSelectionChange();
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            _visualTreeAsset.CloneTree(rootVisualElement);
            rootVisualElement.styleSheets.Add(
                AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviourChain/Editor/BehaviourChainEditor.uss"));

            _myGraphView = rootVisualElement.Q<MyGraphView>();
            _inspectorView = rootVisualElement.Q<InspectorView>();

            _myGraphView.focusable = true;
            _myGraphView.OnNodeSelected = OnNodeSelectionChanged;
            
            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            BehaviourChain chain = Selection.activeObject as BehaviourChain;
            if (null == chain)
                return;

            _chainWrapper = new BehaviourChainEditorWrapper(chain);
            _myGraphView.SetWrapper(_chainWrapper);
            _myGraphView.PopulateGraph(chain.Nodes.ToArray());
        }

        private void OnNodeSelectionChanged(NodeView nodeView)
        {
            _inspectorView.UpdateSelection(nodeView);
        }
    }
}
