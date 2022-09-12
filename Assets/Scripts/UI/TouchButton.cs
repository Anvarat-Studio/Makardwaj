
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.UI;

////TODO: custom icon for OnScreenButton component

namespace UnityEngine.InputSystem.OnScreen
{
    /// <summary>
    /// A button that is visually represented on-screen and triggered by touch or other pointer
    /// input.
    /// </summary>
    [AddComponentMenu("Input/Touch Button")]
    public class TouchButton : OnScreenControl, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Sprite m_activeSprite;
        [SerializeField] private Sprite m_inActiveSprite;

        [SerializeField] private Image m_image;

        public void OnPointerUp(PointerEventData eventData)
        {
            SendValueToControl(0.0f);
            m_image.sprite = m_inActiveSprite;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SendValueToControl(1.0f);
            m_image.sprite = m_activeSprite;
        }

        ////TODO: pressure support
        /*
        /// <summary>
        /// If true, the button's value is driven from the pressure value of touch or pen input.
        /// </summary>
        /// <remarks>
        /// This essentially allows having trigger-like buttons as on-screen controls.
        /// </remarks>
        [SerializeField] private bool m_UsePressure;
        */

        [InputControl(layout = "Button")]
        [SerializeField]
        private string m_ControlPath;

        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }
    }
}
