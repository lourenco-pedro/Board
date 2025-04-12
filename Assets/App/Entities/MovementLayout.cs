using UnityEngine;

namespace App.Entities
{
    [CreateAssetMenu(fileName = "MovementLayout", menuName = "App/Entities/MovementLayout")]
    public class MovementLayout : ScriptableObject
    {
        public MovementPattern[] Patterns;
    }
}
