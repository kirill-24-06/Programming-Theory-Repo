using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Match3
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        InputAction _select;
        InputAction _click;

        public Vector2 SelectedPos => _select.ReadValue<Vector2>();

        public Action Select;

        private void Awake()
        {
            var input = GetComponent<PlayerInput>();
            _select = input.actions["Select"];
            _click = input.actions["Click"];

            _click.performed += OnClick;

        }

        public void OnClick(InputAction.CallbackContext context)
        {
            Select?.Invoke();
        }

        private void OnDestroy()
        {
            _click.performed -= OnClick;
        }
    }
}
