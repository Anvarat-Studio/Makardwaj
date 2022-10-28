using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Projectiles
{
    public class Poison : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        [SerializeField] private GameObject m_vfxParticles;
        [SerializeField] private LayerMask m_groundLayer;
        [SerializeField] private float m_groundCheckLength = 10f;

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

            LookInVelocityDirection();
        }

        public void Shoot(Vector2 startPos, float angle, float speed)
        {
            angle *= Mathf.Deg2Rad;
            _workbench.x = Mathf.Cos(angle);
            _workbench.y = Mathf.Sin(angle);
            transform.position = startPos;
            _rigidbody.velocity = _workbench * speed;
            _rigidbody.gravityScale = 0;

            LookInVelocityDirection();
        }

        public void Drop(Vector2 startPos, float dropSpeed = 1f)
        {
            _workbench = _rigidbody.velocity;
            transform.position = startPos;
            _workbench.Set(_workbench.x, -dropSpeed);
            _rigidbody.velocity = _workbench;

            LookInVelocityDirection();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.collider.GetComponent<MakardwajController>();
            var collisionPoint = collision.GetContact(0);
            Vector2 point = collisionPoint.point;
            float rotation = 0;
            if (player)
            {
                point = GetGroundPosition();
                _vfxParticles = Instantiate(m_vfxParticles, player.transform.position, Quaternion.identity);
                Invoke(nameof(DestroyParticles), 1);
            }
            else
            {

                rotation = GetRotation(collisionPoint.normal);
            }

            

            gameObject.SetActive(false);
            _pool.InstantiatePoisonSpill(point, rotation);
        }

        private float GetRotation(Vector2 normal)
        {
            int x = (int)normal.x;
            int y = (int)normal.y;

            float rotation = 0;

            if(x != 0)
            {
                rotation = (x > 0) ? 270 : 90;  
            }else if(y != 0)
            {
                rotation = (y > 0) ? 0 : 180;
            }

            return rotation;
        }

        private void DestroyParticles()
        {
            Destroy(_vfxParticles, 0);
        }

        private Vector2 GetGroundPosition()
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.down, m_groundCheckLength, m_groundLayer);
            Debug.DrawRay(transform.position, Vector2.down * m_groundCheckLength, Color.magenta, 5f);
            return hit.point;
        }

        private void LookInVelocityDirection()
        {
            var dir = _rigidbody.velocity;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = q;
        }
    }
}