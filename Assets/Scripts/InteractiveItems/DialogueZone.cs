using CleverCrow.Fluid.Dialogues;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace Makardwaj.InteractiveItems
{
    public class DialogueZone : InteractiveItem
    {
        [SerializeField] private DialogueGraph m_dialogues;

        private DialogueController _controller;
        private DatabaseInstance _database;

        private void Awake()
        {
            _database = new DatabaseInstance();
            _controller = new DialogueController(_database);

            _controller.Events.Speak.AddListener(OnSpeak);
            _controller.Events.End.AddListener(OnDialogueEnd);
        }

        private void OnSpeak(IActor actor, string dialogue)
        {
            Debug.Log($"{actor.DisplayName}: {dialogue}");
        }

        private void OnDialogueEnd()
        {
            _controller.Stop();
            _controller.Play(m_dialogues);
        }

        protected override void OnPlayerEnter()
        {
            base.OnPlayerEnter();
        }

        protected override void OnPlayerExit()
        {
            base.OnPlayerExit();
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
    }
}
