using UnityEngine;

namespace Makardwaj.Common
{
    public abstract class BaseData : ScriptableObject
    {
        [Header("Movement")]
        public float movementSpeed = 1f;
    }
}