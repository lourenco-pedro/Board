using System;
using System.Collections.Generic;
using App.Entities;
using UnityEngine;

namespace App.BehaviourChain.Triggers
{
    public abstract class AppLevelTrigger : ppl.PBehaviourChain.Core.Triggers.TriggerNode, IDisposable
    {
        [HideInInspector] public Pawn Pawn;
        public abstract void Dispose();
        public abstract void SetupTrigger();

        public override State Update(Dictionary<string, object> args = null)
        {
            return base.Update(new Dictionary<string, object>()
            {
                { "pawnId", Pawn.Id },
            });
        }
    }
}
