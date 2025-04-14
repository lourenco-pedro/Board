using System.Collections.Generic;
using System.Linq;
using ppl.PBehaviourChain.Core;

namespace ppl.PBehaviourChain.Editor
{
    public static class ApplicationBehaviourProvider
    {
        public delegate Node BehaviourProviderEventHandler(BehaviourChainEditorWrapper wrapper);

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
        
        public static (string, BehaviourProviderEventHandler)[] GetContextMenuItems()
        {
            return _handlers.Select(data => (contextMenu: data.Key, evt: data.Value)).ToArray();
        }
    }
}