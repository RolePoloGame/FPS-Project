using UnityEngine;

namespace AgeOfPiracy.Scripts.Core
{
    public class GUIBehaviour : MonoBehaviour
    {
        public Canvas Canvas => m_Canvas ??= GetComponentInParent<Canvas>();
        private Canvas m_Canvas;

        public RectTransform RectTransform => m_Rect ??= GetComponent<RectTransform>();
        private RectTransform m_Rect;

        public virtual void UpdateGUI() { }
    }
}