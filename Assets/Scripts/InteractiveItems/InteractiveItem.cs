using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using Makardwaj.Utils;
using UnityEngine;

namespace Makardwaj.InteractiveItems
{
    public class InteractiveItem : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField]
        protected bool _canInteract;
        [SerializeField] private float m_interactionRadius = 30f;
        [SerializeField] private Vector2 m_triggerArea = new Vector2(10, 20);
        [SerializeField] private LayerMask m_playerLayer;

        [ReadOnly]
        [SerializeField]
        protected MakardwajController _makar;
        protected Collider2D _makarCollider;

        private void Update()
        {
            _makarCollider = Physics2D.OverlapBox(transform.position, m_triggerArea, 0, m_playerLayer);
            if (_makarCollider)
            {
                if (!_makar)
                {
                    _makar = _makarCollider.GetComponent<MakardwajController>();
                    OnPlayerEnter();
                }
            }
            else
            {
                if (_makar)
                {
                    _makar = null;
                    OnPlayerExit();
                }
            }
        }

        protected virtual void OnPlayerEnter()
        {
            _canInteract = true;
        }

        protected virtual void OnPlayerExit()
        {
            _canInteract = false;
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, m_triggerArea);
        }
    }
}
