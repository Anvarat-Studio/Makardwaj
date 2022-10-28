using CleverCrow.Fluid.Dialogues;
using Makardwaj.InteractiveItems;
using UnityEngine;
using UnityEngine.UI;

namespace Makardwaj.Characters
{
    public class ChitraguptController : MonoBehaviour
    {
        [SerializeField] private DialogueZone m_dialogueZone;
        [SerializeField] private GameObject m_speechBubble;
        [SerializeField] private Text m_speechText;
        [SerializeField] private string m_characterName = "Chitragupt";

        private void OnEnable()
        {
            m_dialogueZone.dialogueChange += OnDialogueChange;
            m_dialogueZone.dialogueEnd += OnDialogueEnd;
        }

        private void OnDisable()
        {
            m_dialogueZone.dialogueChange -= OnDialogueChange;
            m_dialogueZone.dialogueEnd -= OnDialogueEnd;
        }

        private void OnDialogueChange(IActor actor, string dialogue)
        {
            if (actor.DisplayName.Equals(m_characterName))
            {
                m_speechBubble.SetActive(true);
                m_speechText.text = dialogue;
            }
            else
            {
                m_speechBubble.SetActive(false);
            }
        }

        private void OnDialogueEnd()
        {
            m_speechBubble.SetActive(false);
        }
    }
}

