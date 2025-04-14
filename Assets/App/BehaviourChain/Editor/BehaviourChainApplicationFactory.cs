using System.Linq;
using App.BehaviourChain.BehaviourNodes;
using App.BehaviourChain.Triggers;
using ppl.PBehaviourChain.Editor;
using UnityEditor;

namespace App.BehaviourChain.Editor
{
    public static class BehaviourChainApplicationFactory
    {
        [UnityEditor.Callbacks.DidReloadScripts]
        public static void OnApplicationRecompile()
        {
            BehaviourChainEditorFactory.RegisterContextMenuItem("Add/Behavior/MovePieceBehavior", (wrapper) => wrapper.AddNode<MovePieceBehaviour>());
            BehaviourChainEditorFactory.RegisterTriggerTypeProvider(() => TypeCache.GetTypesDerivedFrom<AppLevelTrigger>().ToArray());
        }
    }
}