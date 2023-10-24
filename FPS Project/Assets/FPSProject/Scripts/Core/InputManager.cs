using RolePoloGame.Core;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.FPSProject.Scripts.Core
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField]
        private bool isDebugging = false;

        private InputSettings Input => m_Input ??= new InputSettings();
        private InputSettings m_Input;

        [field: SerializeField]
        public Vector2 Move { get; private set; }
        [field: SerializeField]
        public Vector2 Look { get; private set; }
        public bool IsController => Gamepad.current.enabled;

        public event Action OnFireStarted;
        public event Action OnFireCanceled;
        public event Action<bool> OnRun;
        public event Action OnReload;
        public event Action OnJump;
        public event Action<bool> OnWeaponSwap;
        public event Action OnWeaponWheel;
        public event Action OnMenu;

        private void OnEnable()
        {
            Input.Enable();
            AddInputActions();
        }

        private void OnDisable()
        {
            RemoveInputActions();
            Input.Disable();
        }

        private void AddInputActions()
        {
            Input.FPS.Movement.started += Movement_performed;
            Input.FPS.Movement.performed += Movement_performed;
            Input.FPS.Movement.canceled += Movement_performed;
            Input.FPS.Fire.started += Fire_started;
            Input.FPS.Fire.canceled += Fire_canceled;
            Input.FPS.Reload.performed += Reload_performed;
            Input.FPS.Jump.performed += Jump_performed;
            Input.FPS.WeaponSwap.performed += WeaponSwap_performed;
            Input.FPS.WeaponWheel.performed += WeaponWheel_performed;
            Input.FPS.Menu.performed += Menu_performed;
            Input.FPS.Run.started += Run_started;
            Input.FPS.Run.canceled += Run_canceled;
            Input.FPS.Look.started += Look_performed;
            Input.FPS.Look.performed += Look_performed;
            Input.FPS.Look.canceled += Look_performed;
        }

        private void RemoveInputActions()
        {
            Input.FPS.Movement.started -= Movement_performed;
            Input.FPS.Movement.performed -= Movement_performed;
            Input.FPS.Movement.canceled -= Movement_performed;
            Input.FPS.Fire.performed -= Fire_started;
            Input.FPS.Reload.performed -= Reload_performed;
            Input.FPS.Jump.performed -= Jump_performed;
            Input.FPS.WeaponSwap.performed -= WeaponSwap_performed;
            Input.FPS.WeaponWheel.performed -= WeaponWheel_performed;
            Input.FPS.Menu.performed -= Menu_performed;
            Input.FPS.Run.started -= Run_started;
            Input.FPS.Run.canceled -= Run_canceled;
            Input.FPS.Look.started -= Look_performed;
            Input.FPS.Look.performed -= Look_performed;
            Input.FPS.Look.canceled -= Look_performed;
        }
        private void Menu_performed(InputAction.CallbackContext obj)
        {
            Log(nameof(Menu_performed));
            OnMenu?.Invoke();
        }

        private void WeaponWheel_performed(InputAction.CallbackContext obj)
        {
            Log(nameof(WeaponWheel_performed));
            OnWeaponWheel?.Invoke();
        }

        private void WeaponSwap_performed(InputAction.CallbackContext obj)
        {
            Log(nameof(WeaponSwap_performed));
            float value = obj.ReadValue<float>();
            if (value == 0) return;
            OnWeaponSwap?.Invoke(value > 0);
        }

        private void Jump_performed(InputAction.CallbackContext obj)
        {
            Log(nameof(Jump_performed));
            OnJump?.Invoke();
        }

        private void Reload_performed(InputAction.CallbackContext obj)
        {
            Log(nameof(Reload_performed));
            OnReload?.Invoke();
        }

        private void Fire_started(InputAction.CallbackContext obj)
        {
            Log(nameof(Fire_started));
            OnFireStarted?.Invoke();
        }

        private void Fire_canceled(InputAction.CallbackContext obj)
        {
            Log(nameof(Fire_canceled));
            OnFireCanceled?.Invoke();
        }

        private void Movement_performed(InputAction.CallbackContext obj)
        {
            Log(nameof(Movement_performed));
            Move = obj.ReadValue<Vector2>();
        }

        private void Look_performed(InputAction.CallbackContext obj)
        {
            Log(nameof(Look_performed));
            Look = obj.ReadValue<Vector2>();
        }

        private void Run_started(InputAction.CallbackContext obj)
        {
            OnRun?.Invoke(true);
        }

        private void Run_canceled(InputAction.CallbackContext obj)
        {
            OnRun?.Invoke(false);
        }

        private void Log(string value)
        {
            if (!isDebugging) return;
            Debug.Log(value);
        }
    }
}