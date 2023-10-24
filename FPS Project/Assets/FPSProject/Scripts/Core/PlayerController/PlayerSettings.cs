using UnityEngine;

namespace Assets.FPSProject.Scripts.Core.PlayerController
{
    [CreateAssetMenu(menuName = "FPS/Player/Settings")]
    public class PlayerSettings : ScriptableObject
    {
        [field: SerializeField, Header("Movement")]
        public float MovementSpeed { get; private set; } = 10;
        [field: SerializeField]
        public float RunSpeed { get; private set; } = 20;
        [field: SerializeField]
        public float AirSpeed { get; private set; } = 20;
        [field: SerializeField]
        public float GroundDrag { get; private set; } = .3f;
        [field: SerializeField]
        public float AirDrag { get; private set; } = 0.0f;
        [field: SerializeField]
        public float GroundDistance { get; private set; } = 0.5f;
        [field: SerializeField]
        public LayerMask GroundLayer { get; private set; }
        [field: SerializeField]
        public Vector3 GroundOffset { get; private set; } = new Vector3(0, -.2f, 0);

        [field: SerializeField]
        public float JumpForce { get; private set; } = 5.0f;
        [field: SerializeField, Header("Camera")]
        public Vector2 LookSensivity { get; private set; } = new(100, 1);
        [field: SerializeField, Header("Controller")]
        public float ControllerLookBoost = 8.0f;
    }
}