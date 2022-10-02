using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues;
using CleverCrow.Fluid.Dialogues.Graphs;
using UnityEngine;

namespace Makardwaj.InteractiveItem
{
    public class DialogueZone : MonoBehaviour
    {
        [SerializeField] private DialogueGraph m_dialogueGraph;

        private DatabaseInstance _databaseInstance;
        private DialogueController _dialogueController;
        public bool IsInteracting { get; private set; }

        private Vector2 PlayerDialoguePosition { get; set; }

        private void Awake()
        {
            _databaseInstance = new DatabaseInstance();
            _dialogueController = new DialogueController(_databaseInstance);

            _dialogueController.Events.Speak.AddListener(Speak);
            _dialogueController.Events.End.AddListener(End);
        }

        public void Initialize(Vector2 playerDialgouePosition)
        {
            PlayerDialoguePosition = playerDialgouePosition;
            _dialogueController.Play(m_dialogueGraph);
        }

        public void GetNextDialogue()
        {
            IsInteracting = true;
            _dialogueController.Next();
        }

        private void Speak(IActor actor, string dialogue)
        {
            Debug.Log($"{actor.DisplayName}: {dialogue}");
        }

        private void End()
        {
            IsInteracting = false;
            _dialogueController.Stop();

            Debug.Log($"Interaction End");
        }
    }

}
