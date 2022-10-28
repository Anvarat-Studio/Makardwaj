using CCS.SoundPlayer;
using Makardwaj.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Makardwaj.Environment
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private Transform m_playerSpawnMarker;
        [SerializeField] private GameObject m_interactiveIcon;

        private bool _isDoorOpen = false;
        public Vector2 PlayerPosition { get => m_playerSpawnMarker.position; }

        private Animator _animator;
        private Collider2D _collider;
        private Coroutine _coroutineCloseDoor;

        public UnityAction doorOpened;
        public UnityAction doorClosed;

        [ReadOnly]
        [SerializeField]
        private bool _enableEntering;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _collider = GetComponent<Collider2D>();
            _collider.enabled = false;
            m_interactiveIcon.SetActive(false);
        }

        public void Teleport(Vector2 position)
        {
            transform.position = position;
        }

        public void OpenAndCloseDoor(UnityAction onComplete = null)
        {
            OpenDoor();

            if(_coroutineCloseDoor != null)
            {
                StopCoroutine(_coroutineCloseDoor);
            }

            StartCoroutine(IE_CloseDoor(onComplete));
        }

        private IEnumerator IE_CloseDoor(UnityAction onComplete = null)
        {
            yield return new WaitWhile(() => !_isDoorOpen);
            CloseDoor();
            yield return new WaitForSeconds(0.5f);
            onComplete?.Invoke();
        }


        public void OpenDoor(bool enableEntering = false)
        {
            _animator.SetBool("open", true);
            _animator.SetBool("close", false);
            _collider.enabled = false;
            m_interactiveIcon.SetActive(false);
            _enableEntering = enableEntering;
            SoundManager.Instance.PlaySFX(MixerPlayer.Extra, "doorOpen", 0.5f, false);
        }

        public void CloseDoor()
        {
            _animator.SetBool("open", false);
            _animator.SetBool("close", true);
            _collider.enabled = false;
            m_interactiveIcon.SetActive(false);
            SoundManager.Instance.PlaySFX(MixerPlayer.Extra, "doorClose", 0.5f, false);
        }

        public void DoorOpenTrigger()
        {
            _isDoorOpen = true;
            _collider.enabled = _enableEntering;
            m_interactiveIcon.SetActive(_enableEntering);
            doorOpened?.Invoke();
        }

        public void DoorCloseTrigger()
        {
            _isDoorOpen = false;
            doorClosed?.Invoke();
        }
    }
}