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
            // A definição dos itens que compõem o context menu deve ser feita no nível da aplicação, e não no nível do serviço.
            // Isso garante que o menu reflita as necessidades específicas do domínio da aplicação, mantendo o serviço desacoplado.
            BehaviourChainEditorFactory.RegisterContextMenuItem("Add/Behavior/MovePieceBehavior", (wrapper) => wrapper.AddNode<MovePieceBehaviourNode>());
            
            // Esta função permite adicionar um nível extra de triggers na hierarquia.
            // Por padrão, o sistema utiliza o TypeCache para buscar todas as derivações da classe TriggerNode ao adicionar triggers nas BehaviourChains.
            // No entanto, neste projeto foi necessário criar uma subclasse adicional a partir de TriggerNode.
            // Com isso, ao adicionar os triggers na BehaviourChain, o sistema passará a considerar esse novo nível de implementação.
            BehaviourChainEditorFactory.RegisterTriggerTypeProvider(() => TypeCache.GetTypesDerivedFrom<AppLevelTrigger>().ToArray());
        }
    }
}