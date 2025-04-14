using UnityEngine;
using ppl.SimpleEventSystem;

namespace ppl.PBehaviourChain.Core
{
    public class BehaviourChainComponent : MonoBehaviour, IEventBindable
    {
        
        public BehaviourChain Chain => _chain;
        public BehaviourChain Instance => _instance;
        
        [SerializeField] 
        private BehaviourChain _chain;
        private BehaviourChain _instance;
        
        protected virtual void Awake()
        {
            _instance = _chain.Instantiate();
        }
    }
}