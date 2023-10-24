using Assets.FPSProject.Scripts.Core.PlayerController;
using TMPro;
using UnityEngine;

namespace Assets.FPSProject.Scripts.GUI
{
    public class GUIDebugInfo : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovementController movementController;

        [SerializeField, Header("GUI References")]
        private TextMeshProUGUI m_VelocityTMP;

        void Update()
        {
            if (m_VelocityTMP == null || movementController == null)
                return;
            m_VelocityTMP.SetText(movementController.FindVelRelativeToLook().ToString());
        }
    }
}