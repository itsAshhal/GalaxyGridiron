using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GalaxyGridiron
{
    /// <summary>
    /// Handles the main input for all types of players in the game
    /// </summary>
    public class InputController : MonoBehaviour
    {
        #region ActionCallbacks

        /// <summary>
        /// This touch hold event identifies when the user tries to interact with the gameplay like touching and holding the
        /// ball and dragging it to somewhere else
        /// </summary>
        public Action<InputAction.CallbackContext> touchHoldEvent;

        /// <summary>
        /// We need another event to actually check if we've clicked or not
        /// </summary>
        /// 
        public Action<InputAction.CallbackContext> clickEvent;

        public Action<InputAction.CallbackContext> pointEvent;

        // lets save the mouse current position
        public Vector2 mouseCurrentScreenPosition = Vector2.zero;

        #endregion

        public static InputController Instance;
        private GameInput _gameInput;

        private void Awake()
        {
            // make a default instance to make it accessible
            Instance = this;

            // make a new object for the gameInput
            _gameInput = new();

            // bind the callbacks
            _gameInput.Player.TouchHold.started += ctx => touchHoldEvent?.Invoke(ctx);
            _gameInput.Player.TouchHold.performed += ctx => touchHoldEvent?.Invoke(ctx);
            _gameInput.Player.TouchHold.canceled += ctx => touchHoldEvent?.Invoke(ctx);

            // check for clicks and touch
            _gameInput.UI.Click.started += ctx => clickEvent?.Invoke(ctx);
            _gameInput.UI.Click.performed += ctx => clickEvent?.Invoke(ctx);
            _gameInput.UI.Click.canceled += ctx => clickEvent?.Invoke(ctx);

            _gameInput.UI.Point.performed += ctx =>
            {
                pointEvent?.Invoke(ctx);
                mouseCurrentScreenPosition = ctx.ReadValue<Vector2>();
                Debug.Log($"New point event {Accessories.GetWorldPosition(ctx.ReadValue<Vector2>())}");

            };

        }

        private void OnEnable()
        {
            _gameInput.Player.Enable();
            _gameInput.UI.Enable();
        }
        private void OnDisable()
        {
            _gameInput.Player.Disable();
            _gameInput.UI.Disable();
        }


    }

}