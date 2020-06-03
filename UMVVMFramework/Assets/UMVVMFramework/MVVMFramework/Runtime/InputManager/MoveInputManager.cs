using System.Collections.Generic;
using UnityEngine;

namespace MVVM
{
    public class MoveInputManager : MonoBehaviour
    {
        #region Interfaces
        public interface IInputBound
        {
            void OnInputStarted(Vector2 input);
            void OnInputPerformed(Vector2 input);
            void OnInputCancelled(Vector2 input);
        }

        public interface IJoystickInputBound : IInputBound
        {
        }

        public interface IKeyboardInputBound : IInputBound
        {
        }
        #endregion

        private static MoveInputManager _instance;
        public static MoveInputManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<MoveInputManager>();

                return _instance;
            }
        }

        public bool EnableJoystick;
        public bool EnableKeyboard;
        public bool IgnoreZeroDir;

        private Dictionary<MoveInputControllerBase, List<IInputBound>> _inputReceiverCollection;
        private bool _isInited = false;

        private void Awake()
        {
            if (!_isInited)
                Init();
        }

        private void Init()
        {
            _isInited = true;

            _inputReceiverCollection = new Dictionary<MoveInputControllerBase, List<IInputBound>>()
        {
            { new KeyboardMoveInputController(EnableKeyboard), new List<IInputBound>() },
            { new JoystickMoveInputController(EnableJoystick, IgnoreZeroDir), new List<IInputBound>() }
        };

            StartListeningEvents();
        }

        private void OnDestroy()
        {
            _instance = null;

            StopListeningEvents();
        }

        private void StartListeningEvents()
        {
            Dictionary<MoveInputControllerBase, List<IInputBound>>.KeyCollection controllers = _inputReceiverCollection.Keys;

            foreach (MoveInputControllerBase value in controllers)
            {
                value.OnInputStarted += OnInputStarted;
                value.OnInputPerformed += OnInputPerformed;
                value.OnInputCancelled += OnInputCancelled;
            }
        }

        private void StopListeningEvents()
        {
            Dictionary<MoveInputControllerBase, List<IInputBound>>.KeyCollection controllers = _inputReceiverCollection.Keys;

            foreach (MoveInputControllerBase value in controllers)
            {
                value.OnInputStarted -= OnInputStarted;
                value.OnInputPerformed -= OnInputPerformed;
                value.OnInputCancelled -= OnInputCancelled;
            }
        }

        private void Update()
        {
            Dictionary<MoveInputControllerBase, List<IInputBound>>.KeyCollection controllers = _inputReceiverCollection.Keys;

            foreach (MoveInputControllerBase value in controllers)
            {
                if (value.IsActive)
                    value.CheckInput();
            }
        }

        #region Register/Unregister
        public void RegisterInputReceiver(IInputBound inputReceiver)
        {
            if (inputReceiver is IKeyboardInputBound)
                AddReceiverTo<KeyboardMoveInputController>(inputReceiver);

            if (inputReceiver is IJoystickInputBound)
                AddReceiverTo<JoystickMoveInputController>(inputReceiver);
        }

        private void AddReceiverTo<T>(IInputBound inputReceiver) where T : MoveInputControllerBase
        {
            if (!_isInited)
                Init();

            MoveInputControllerBase targetController = null;

            foreach (MoveInputControllerBase value in _inputReceiverCollection.Keys)
            {
                if (value is T)
                {
                    targetController = value;

                    break;
                }
            }

            if (targetController != null)
                _inputReceiverCollection[targetController].Add(inputReceiver);
        }

        public void UnregisterInputReceiver(IInputBound inputReceiver)
        {
            if (inputReceiver is IKeyboardInputBound)
                RemoveReceiverFrom<KeyboardMoveInputController>(inputReceiver);

            if (inputReceiver is IJoystickInputBound)
                RemoveReceiverFrom<JoystickMoveInputController>(inputReceiver);
        }

        private void RemoveReceiverFrom<T>(IInputBound inputReceiver) where T : MoveInputControllerBase
        {
            MoveInputControllerBase targetController = null;

            foreach (MoveInputControllerBase value in _inputReceiverCollection.Keys)
            {
                if (value is T)
                {
                    targetController = value;

                    break;
                }
            }

            if (targetController != null)
                _inputReceiverCollection[targetController].Remove(inputReceiver);
        }
        #endregion

        private void OnInputStarted(Vector2 input, MoveInputControllerBase controller)
        {
            _inputReceiverCollection[controller].ForEach(val => val.OnInputStarted(input));
        }

        private void OnInputPerformed(Vector2 input, MoveInputControllerBase controller)
        {
            _inputReceiverCollection[controller].ForEach(val => val.OnInputPerformed(input));
        }

        private void OnInputCancelled(Vector2 input, MoveInputControllerBase controller)
        {
            _inputReceiverCollection[controller].ForEach(val => val.OnInputCancelled(input));
        }
    }
}