using System.Collections.Generic;
using UnityEngine;

namespace App.Entities
{
    [CreateAssetMenu(fileName = "EntitiesCatalog", menuName = "App/Entities/EntitiesCatalog")]
    public class EntitiesCatalog : ScriptableObject
    {
        [SerializeField]
        private AYellowpaper.SerializedCollections.SerializedDictionary<string, BoardEntity> _catalog;
    
        public BoardEntity GetEntity(string id)
        {
            return _catalog.GetValueOrDefault(id);
        }
        
        public TEntity GetEntityAs<TEntity>(string id) 
            where TEntity : BoardEntity
        {
            if (_catalog.TryGetValue(id, out BoardEntity entity))
            {
                return entity as TEntity;
            }
            
            return default(TEntity);
        }
    }
}