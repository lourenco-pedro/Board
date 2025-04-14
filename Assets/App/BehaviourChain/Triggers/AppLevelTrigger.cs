using System;
using App.Entities;
using UnityEngine;

namespace App.BehaviourChain.Triggers
{
    public abstract class AppLevelTrigger : ppl.PBehaviourChain.Core.Triggers.TriggerNode, IDisposable
    {
        [HideInInspector] public Pawn Pawn;
        public abstract void Dispose();
        public abstract void SetupTrigger();
    }
}
