using Assets.Scripts.Projectiles.PoisionDrop;
using UnityEngine;

namespace Makardwaj.Projectiles
{
    public class Poison : MonoBehaviour
    {
        [SerializeField] private PoisonSpillVfx m_spillVFXPrefab;

        private Rigidbody2D _rigidbody;
        private PoisonSpillVfx _spillVFX;

        private Vector2 _workbench;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spillVFX = Instantiate(m_spillVFXPrefab, transform.position, Quaternion.identity);
            _spillVFX.gameObject.SetActive(false);
        }

        public void Shoot(Vector2 startPos, float speed, int dir)
        {
            _workbench = _rigidbody.velocity;
            transform.position = startPos;
            _workbench.Set(speed * dir, _workbench.y);
            _rigidbody.velocity = _workbench;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var collisionPoint = collision.GetContact(0);

            gameObject.SetActive(false);
            _spillVFX.Activate(collisionPoint.point);
        }
    }
}