using Assets.Scripts.Projectiles.PoisionDrop;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Projectiles
{
    public class Poison : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        [SerializeField] private GameObject m_vfxParticles;

        private PoisonPool _pool;
        private Vector2 _workbench;
        private GameObject _vfxParticles;

        private void Awake()
        {
            _pool = FindObjectOfType<PoisonPool>();
            _rigidbody = GetComponent<Rigidbody2D>();
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
            var player = collision.collider.GetComponent<MakardwajController>();
            var collisionPoint = collision.GetContact(0);
            Vector2 point = collisionPoint.point;
            if (player)
            {
                point = player.FeetPosition;
                _vfxParticles = Instantiate(m_vfxParticles, player.transform.position, Quaternion.identity);
                Invoke(nameof(DestroyParticles), 1);
            }

            

            gameObject.SetActive(false);
            _pool.InstantiatePoisonSpill(point);
        }

        private void DestroyParticles()
        {
            Destroy(_vfxParticles, 0);
        }
    }
}