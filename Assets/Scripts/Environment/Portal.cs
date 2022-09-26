using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Makardwaj.Environment
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private Transform m_playerSpawnMarker;

        private bool _isDoorOpen = false;
        public Vector2 PlayerPosition { get => m_playerSpawnMarker.position; }

        private Animator _animator;
        private Collider2D _collider;
        private Coroutine _coroutineCloseDoor;

        public UnityAction doorOpened;
        public UnityAction doorClosed;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _collider = GetComponent<Collider2D>();
            _collider.enabled = false;
        }

        public void Teleport(Vector2 position)
        {
            transform.position = position;
        }

        public void OpenAndCloseDoor()
        {
            OpenDoor();

            if(_coroutineCloseDoor != null)
            {
                StopCoroutine(_coroutineCloseDoor);
            }

            StartCoroutine(IE_CloseDoor());
        }

        private IEnumerator IE_CloseDoor()
        {
            yield return new WaitWhile(() => !_isDoorOpen);
            CloseDoor();
        }


        public void OpenDoor()
        {
            _animator.SetBool("open", true);
            _animator.SetBool("close", false);
            _collider.enabled = false;
        }

        public void CloseDoor()
        {
            _animator.SetBool("open", false);
            _animator.SetBool("close", true);
            _collider.enabled = false;
        }

        public void DoorOpenTrigger()
        {
            _isDoorOpen = true;
            doorOpened?.Invoke();
        }

        public void DoorCloseTrigger()
        {
            _isDoorOpen = false;
            doorClosed?.Invoke();
        }
    }
}