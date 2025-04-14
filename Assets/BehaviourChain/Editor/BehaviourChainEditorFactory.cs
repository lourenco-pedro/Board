using System;
using System.Collections.Generic;
using System.Linq;
using ppl.PBehaviourChain.Core;

namespace ppl.PBehaviourChain.Editor
{
    public static class BehaviourChainEditorFactory
    {
        public delegate Node BehaviourProviderEventHandler(BehaviourChainEditorWrapper wrapper);

        private static Func<Type[]> _triggerTypes;
        private static Dictionary<string, BehaviourProviderEventHandler> _handlers =
            new Dictionary<string, BehaviourProviderEventHandler>();

        public static void RegisterContextMenuItem(string menu, BehaviourProviderEventHandler handler)
        {
            _handlers.TryAdd(menu, handler);
        }
        
        public static void RemoveContextMenuItem(string menu)
        {
            _handlers.Remove(menu);
        }

        public static void RegisterTriggerTypeProvider(Func<Type[]> e)
        {
            _triggerTypes = e;
        }

        public static Type[] GetTriggerDerivedTypes()
        {
            return _triggerTypes == null ? Type.EmptyTypes : _triggerTypes();
        }

        public static (string, BehaviourProviderEventHandler)[] GetContextMenuItems()
        {
            return _handlers.Select(data => (contextMenu: data.Key, evt: data.Value)).ToArray();
        }
    }
}