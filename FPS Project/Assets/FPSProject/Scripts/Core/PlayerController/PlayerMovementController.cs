using RolePoloGame.Core.Extensions;
using System;
using UnityEngine;

namespace Assets.FPSProject.Scripts.Core.PlayerController
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementController : MonoBehaviour
    {
        #region Properties & Fields
        private const int MAX_COLLISIONS = 1;

        [SerializeField]
        private PlayerSettings playerSettings;
        [SerializeField]
        private Transform lookTransform;

        [SerializeField]
        private bool invertYAxis = true;

        private RaycastHit[] groundCollided = new RaycastHit[MAX_COLLISIONS];

        public Rigidbody Rigidbody
        {
            get
            {
                if (m_Rigidbody != null) return m_Rigidbody;
                if (TryGetComponent(out m_Rigidbody)) return m_Rigidbody;
                Debug.LogWarning($"{name} has no Rigidbody! This should never happen!");
                enabled = false;
                throw new NullReferenceException();
            }
        }
        private Rigidbody m_Rigidbody;

        [SerializeField] private Vector3 m_Movement = Vector3.zero;
        [SerializeField] private Vector2 m_Look = Vector2.zero;
        [SerializeField] private bool m_IsSprinting = false;
        [SerializeField] private bool m_IsGrounded = false;
        [SerializeField] private bool m_IsJumping = false;

        private bool m_HasJumped = false;

        private float m_CurrentSpeed = 0.0f;
        private float m_CurrentDrag = .3f;

        #endregion

        #region Unity Methods
        private void Start()
        {
            SubscribeToInputManager();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDestroy()
        {
            UnsubscribeFromInputManager();
        }

        private void Update()
        {
            HandleGrounded();
            HandleDrag();
            HandleLookInput();
            HandleMovementInput();
            HandleMovementSpeed();
        }
        public void FixedUpdate()
        {
            UpdateJump();
            UpdateLook();
            UpdateMovement();
            UpdateVelocity();
        }
        #endregion

        #region Private methods
        private bool SubscribeToInputManager()
        {
            if (!InputManager.Instance)
            {
                Debug.LogWarning("Failed to subscribe to input manager!");
                return false;
            }
            InputManager.Instance.OnRun += HandleRunEvent;
            InputManager.Instance.OnJump += HandleJumpEvent;
            return true;
        }

        private void UnsubscribeFromInputManager()
        {
            if (!InputManager.Instance) return;
            InputManager.Instance.OnRun -= HandleRunEvent;
            InputManager.Instance.OnJump -= HandleJumpEvent;
        }

        private void HandleLookInput()
        {
            Vector2 lookInput = InputManager.Instance.Look;
            m_Look = lookInput;
            if (!invertYAxis) return;
            m_Look = m_Look.WithY(-m_Look.y);
        }

        private void HandleMovementInput()
        {
            Vector2 moveInput = InputManager.Instance.Move;
            m_Movement = new Vector3(moveInput.x, 0.0f, moveInput.y);
        }

        private void HandleMovementSpeed()
        {
            m_CurrentSpeed = m_IsSprinting ? playerSettings.RunSpeed : playerSettings.MovementSpeed;
        }
        private void HandleDrag()
        {
            m_CurrentDrag = m_IsGrounded ? playerSettings.GroundDrag : playerSettings.AirDrag;
            Rigidbody.drag = m_CurrentDrag;
        }

        private void HandleGrounded()
        {
            var hitsCount = Physics.SphereCastNonAlloc(transform.position - playerSettings.GroundOffset, .5f, Vector3.down, groundCollided, playerSettings.GroundDistance, playerSettings.GroundLayer);
            m_IsGrounded = hitsCount > 0;
            if (!m_IsGrounded) return;
            m_HasJumped = false;
        }


        private void UpdateLook()
        {
            if (m_Look.magnitude <= .001f) return;

            var lookSentivity = m_Look * playerSettings.LookSensivity;
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + lookSentivity.x, 0);
            lookTransform.localEulerAngles = new Vector3(lookTransform.localEulerAngles.x + lookSentivity.y, 0, 0);
        }

        private void UpdateMovement()
        {
            var movement = this.m_Movement;
            if (movement.magnitude <= .001f) return;
            if (!m_IsGrounded) return;

            Vector3 forwardVector = movement.z * m_CurrentSpeed * transform.forward;
            Vector3 sideVector = movement.x * m_CurrentSpeed * transform.right;
            var movementVector = forwardVector + sideVector;
            movementVector = movementVector.WithY(0);
            Rigidbody.AddForce(movementVector, ForceMode.Force);
        }

        private void UpdateVelocity()
        {
            var moveVelocity = Rigidbody.velocity.WithY(0);
            if (moveVelocity.magnitude < m_CurrentSpeed) return;
            Rigidbody.velocity = (moveVelocity.normalized * m_CurrentSpeed).WithY(Rigidbody.velocity.y);
        }

        public Vector2 FindVelRelativeToLook()
        {
            float lookAngle = lookTransform.transform.eulerAngles.y;
            float moveAngle = Mathf.Atan2(Rigidbody.velocity.x, Rigidbody.velocity.z) * Mathf.Rad2Deg;

            float u = Mathf.DeltaAngle(lookAngle, moveAngle);
            float v = 90 - u;

            float magnitue = Rigidbody.velocity.magnitude;
            float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
            float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

            return new Vector2(xMag, yMag);
        }

        private void UpdateJump()
        {
            if (!m_IsGrounded) return;
            if (!m_IsJumping) return;
            if (m_HasJumped) return;
            m_HasJumped = true;
            Rigidbody.AddForce(transform.up * playerSettings.JumpForce, ForceMode.Impulse);
            m_IsJumping = false;
        }

        #region Event Handlers

        private void HandleJumpEvent()
        {
            m_IsJumping = true;
        }

        private void HandleRunEvent(bool value)
        {
            m_IsSprinting = value;
        }
        #endregion
        #endregion
    }
}