using System.Collections;
using UnityEngine;

namespace Makardwaj.Environment
{
    public class PortalAnimationTriggers : MonoBehaviour
    {
        private Portal _portal;

        private void Awake()
        {
            _portal = GetComponentInParent<Portal>();    
        }

        private void DoorOpenTrigger()
        {
            _portal.DoorOpenTrigger();
        }

        private void DoorCloseTrigger()
        {
            _portal.DoorCloseTrigger();
        }
    }
}