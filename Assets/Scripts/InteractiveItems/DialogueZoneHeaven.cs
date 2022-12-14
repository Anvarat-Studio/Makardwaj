using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Graphs;
using Makardwaj.Managers;
using Makardwaj.Utils;
using UnityEngine;

namespace Makardwaj.InteractiveItems
{
    public class DialogueZoneHeaven : DialogueZone
    {
        [Header("Dialogues")]
        [SerializeField] protected DialogueGraph m_firstVisitDialogueGraph;
        [SerializeField] protected DialogueGraph m_secondVisitDialogueGraph;
        [SerializeField] protected List<DialogueGraph> m_defaultDialogueGraph;

        [ReadOnly]
        [SerializeField]
        protected bool _hasAlreadyVisitedHeaven;
        protected bool _isHeavenDoorOpen;

        protected override void Awake()
        {
            base.Awake();

            EventHandler.heavenActivated += OnHeavenActivated;
            EventHandler.heavenDeactivated += OnHeavenDeactivated;
        }

        private void OnHeavenActivated()
        {
            if (_hasAlreadyVisitedHeaven)
            {
                m_dialogues = m_secondVisitDialogueGraph;
            }
            else
            {
                m_dialogues = m_firstVisitDialogueGraph;
            }
            _isHeavenDoorOpen = false;
        }

        private void OnHeavenDeactivated()
        {
            _hasAlreadyVisitedHeaven = true;
        }

        protected override void OnDialogueEnd()
        {
            base.OnDialogueEnd();

            m_dialogues = m_defaultDialogueGraph[Random.Range(0, m_defaultDialogueGraph.Count)];

            if (!_isHeavenDoorOpen && !_hasAlreadyVisitedHeaven)
            {
                _isHeavenDoorOpen = true;
                EventHandler.heavenMainDialogueCompleted?.Invoke();
            }
        }
    }
}