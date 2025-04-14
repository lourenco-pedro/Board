using UnityEngine;

namespace ppl.PBehaviourChain.Core
{
    public class BehaviourChainComponent : MonoBehaviour
    {
        [SerializeField] 
        private BehaviourChain _chain;
        private BehaviourChain _instance;
        
        void Awake()
        {
            _instance = _chain.Instantiate();
        }

        void OnDestroy()
        {
            if (null == _instance)
                return;
            
            _instance.Dispose();
        }
    }
}