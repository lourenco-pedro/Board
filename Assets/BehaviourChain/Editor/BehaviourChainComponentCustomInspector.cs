using ppl.PBehaviourChain.Core;
using UnityEngine;
using UnityEditor;

namespace ppl.PBehaviourChain.Editor
{
    [CustomEditor(typeof(BehaviourChainComponent), true)]
    public class BehaviourChainComponentCustomInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            BehaviourChainComponent component = (BehaviourChainComponent)target;
            
            if(null == component)
                return;
            
            if(GUILayout.Button("Edit piece Behaviour Chain"))
                BehaviourChainEditor.Open(component.Chain);
            
            base.OnInspectorGUI();
        }
    }
}
