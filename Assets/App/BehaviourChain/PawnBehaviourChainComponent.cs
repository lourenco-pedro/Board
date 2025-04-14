using App.BehaviourChain.Triggers;
using App.Entities;
using ppl.PBehaviourChain.Core;
using UnityEngine;

namespace App.BehaviourChain
{
    public class PawnBehaviourChainComponent : BehaviourChainComponent
    {
        [SerializeField] private Pawn _pawn;

        protected override void Awake()
        {
            base.Awake();
            Instance.TriggerNodes.ForEach(trigger =>
            {
                AppLevelTrigger appLevelTrigger = trigger as AppLevelTrigger;
                if (null == appLevelTrigger)
                    return;
                
                appLevelTrigger.Pawn = _pawn;
                appLevelTrigger.SetupTrigger();
            });
        }

        private void Update()
        {
            Instance.Update();
        }

        private void OnValidate()
        {
            if(null == _pawn)
                _pawn = this.GetComponent<Pawn>();
        }
    }
}
