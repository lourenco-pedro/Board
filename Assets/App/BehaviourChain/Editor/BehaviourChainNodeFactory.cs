using App.BehaviourChain.BehaviourNodes;
using ppl.PBehaviourChain.Editor;

namespace App.BehaviourChain.Editor
{
    public class BehaviourChainNodeFactory
    {
        [UnityEditor.Callbacks.DidReloadScripts]
        public static void OnApplicationRecompile()
        {
            ApplicationBehaviourProvider.RegisterContextMenuItem("Add/Behavior/MovePieceBehavior", (wrapper) => wrapper.AddNode<MovePieceBehaviour>());
        }
    }
}