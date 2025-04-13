using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviourChainEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("BehaviourChain/Editor")]
    public static void Open()
    {
        BehaviourChainEditor wnd = GetWindow<BehaviourChainEditor>();
        wnd.titleContent = new GUIContent("BehaviourChainEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        m_VisualTreeAsset.CloneTree(rootVisualElement);
        rootVisualElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviourChain/Editor/BehaviourChainEditor.uss"));
    }
}
