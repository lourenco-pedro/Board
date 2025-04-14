using ppl.PBehaviourChain.Core.Behaviours;

namespace ppl.PBehaviourChain.Core.Triggers
{
    public abstract class TriggerNode : Node
    {
        public Behaviour Child; 

        public override State Update()
        {
            Node nodeToUpdate = Child.GetNextChild();
            //Se node to update for nulo, quer dizer que chegou na ponta do grafo. Não tendo mais child para atualizar
            //Pode entender que é um caminho concluido
            if (null == nodeToUpdate)
                return State.Success;
                
            nodeToUpdate.Update();
            return State.Running;
        }
    }
}