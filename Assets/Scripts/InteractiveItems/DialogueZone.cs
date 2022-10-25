using CleverCrow.Fluid.Dialogues;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Databases;
using UnityEngine;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using Makardwaj.Utils;
using UnityEngine.Events;

namespace Makardwaj.InteractiveItems
{
    public class DialogueZone : MonoBehaviour, InteractiveItem
    {
        [SerializeField] private DialogueGraph m_dialogues;
        [SerializeField] private GameObject m_interactionIcon;

        [SerializeField] private float m_interactionRadius = 30f;
        [SerializeField] private Vector2 m_triggerArea = new Vector2(10, 20);
        [SerializeField] private LayerMask m_playerLayer;

        [ReadOnly]
        [SerializeField]
        private MakardwajController _makar;

        private DialogueController _controller;
        private DatabaseInstance _database;
        private Collider2D _makarCollider;
        public bool IsInteracting { get; set ;}

        public UnityAction<IActor, string> dialogueChange;
        public UnityAction dialogueEnd;

        private void Awake()
        {
            _database = new DatabaseInstance();
            _controller = new DialogueController(_database);
            m_interactionIcon?.SetActive(false);

            _controller.Events.Speak.AddListener(OnSpeak);
            _controller.Events.End.AddListener(OnDialogueEnd);
        }

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
                    OnPlayerExit();
                    _makar = null;
                }
            }
        }

        private void OnSpeak(IActor actor, string dialogue)
        {
            Debug.Log($"{actor.DisplayName}: {dialogue}");
            dialogueChange?.Invoke(actor, dialogue.ToUpper());
        }

        private void OnDialogueEnd()
        {
            _controller.Stop();
            //_controller.Play(m_dialogues);
            IsInteracting = false;
            m_interactionIcon.SetActive(true);
            dialogueEnd?.Invoke();
        }

        public void PlayNextDialogue()
        {
            if (_controller.ActiveDialogue == null)
            {
                _controller.Play(m_dialogues);
            }
            else
            {
                _controller.Next();
            }
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, m_triggerArea);
        }

        public void OnPlayerEnter()
        {
            m_interactionIcon?.SetActive(true);
            _makar.InteractiveItemNearby = this;
        }

        public void OnPlayerExit()
        {
            m_interactionIcon?.SetActive(false);
            if (_makar)
            {
                _makar.InteractiveItemNearby = null;
            }
        }

        public void Interact()
        {
            if (!IsInteracting)
            {
                IsInteracting = true;
                m_interactionIcon.SetActive(false);
            }
            PlayNextDialogue(); ;
        }
    }
}
