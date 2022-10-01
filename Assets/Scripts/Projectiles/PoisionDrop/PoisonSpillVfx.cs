using System.Collections;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using UnityEngine;

namespace Assets.Scripts.Projectiles.PoisionDrop
{
    public class PoisonSpillVfx : MonoBehaviour
    {
        [SerializeField] private float m_activeTime;

        private Coroutine _coroutineDeactivate;
        private WaitForSeconds _deactivateTime;

        private void Awake()
        {
            _deactivateTime = new WaitForSeconds(m_activeTime);
        }

        public void Activate(Vector2 position)
        {
            transform.position = position;
            gameObject.SetActive(true);

            Deactivate();
        }

        public void Deactivate()
        {
            if(_coroutineDeactivate != null)
            {
                StopCoroutine(_coroutineDeactivate);
            }

            _coroutineDeactivate = StartCoroutine(IE_Deactivate());
        }

        private IEnumerator IE_Deactivate()
        {
            yield return _deactivateTime;
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.GetComponent<MakardwajController>();

            if (player)
            {
                player.StartDebuff();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var player = collision.GetComponent<MakardwajController>();

            if (player)
            {
                player.StopDebuff();
            }
        }
    }
}