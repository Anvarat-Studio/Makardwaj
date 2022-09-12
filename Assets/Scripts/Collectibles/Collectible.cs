using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Makardwaj.Collectibles
{
    public class Collectible : MonoBehaviour, ICollectible
    {
        [SerializeField] private float m_points = 1f;

        public void Collect()
        {

        }
    }

}
