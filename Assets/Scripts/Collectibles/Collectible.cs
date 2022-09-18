using Makardwaj.UI;
using UnityEngine;

namespace Makardwaj.Collectibles
{
    public class Collectible : MonoBehaviour, ICollectible
    {
        [SerializeField] private int m_points = 1;
        [SerializeField] private float m_collectionTime = 1f;
        [SerializeField] private SpriteRenderer m_animation;


        private NumberHandler m_numberHandler;
        private SpriteRenderer _sr;
        private Collider2D _collider;
        private Rigidbody2D _rigidbody;

        private void OnEnable()
        {
            SetActive(true);
            _collider.enabled = false;

            Invoke(nameof(AllowCollection), m_collectionTime);
        }

        private void Awake()
        {
            m_numberHandler = FindObjectOfType<NumberHandler>();
            _sr = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Collect()
        {
            if (m_numberHandler)
            {

                m_animation.sprite = m_numberHandler.GetNumberImage(m_points);
                SetActive(false);
                m_animation.gameObject.SetActive(true);
                GameManager.Score += m_points;
                GameManager.collectibleCollected?.Invoke(GameManager.Score);
            }
        }

        private void SetActive(bool flag)
        {
            _sr.enabled = flag;
            _collider.enabled = flag;
            _rigidbody.bodyType = (flag) ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
        }

        private void AllowCollection()
        {
            _collider.enabled = true;
        }
    }

}
