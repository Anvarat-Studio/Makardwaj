using UnityEngine;

namespace Makardwaj.Projectiles.Bubble.Utils
{
    public class BubblePlayerCollider : MonoBehaviour
    {
        private Bubble _bubble;

        private void Awake()
        {
            _bubble = GetComponent<Bubble>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _bubble?.OnCollisionEnter2D(collision);
        }
    }
}
