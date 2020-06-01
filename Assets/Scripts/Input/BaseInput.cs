using System;
using UnityEngine;

namespace Input
{
    public abstract class BaseInput : MonoBehaviour
    {
        public enum State
        {
            Dragging,
            Started,
            Idle
        }

        protected Camera _mainCamera;
        protected State _currentState;
        private Vector3 _drag;
        private Vector3 _lastMousePosition;
        private Vector3 _startPosition;

        public Action<Vector3> onPositionChanged;

        public State CurrentState => _currentState;

        protected virtual void Start()
        {
            _mainCamera = Camera.main;
            _drag = transform.position;
            _currentState = State.Idle;
        }


        private void Update()
        {
         //   BeginInput();
           // UpdatePosition();
            HandleInput();
        }

        protected abstract void HandleInput();

        private void UpdatePosition()
        {
            switch (_currentState)
            {
                case State.Dragging:
                {
                    _drag = GetMousePosition() - _lastMousePosition;
                    _lastMousePosition = GetMousePosition();
                    break;
                }
            }
        }

        private Vector3 GetMousePosition()
        {
            /*var startPoint = _mainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            startPoint.z = 15;*/
            return UnityEngine.Input.mousePosition;
        }

        private void BeginInput()
        {
            var mousePosition = UnityEngine.Input.mousePosition;
            var ray = _mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Striker")))
                {
                    Debug.Log("Base Input Hit COllide " + hit.collider.tag);
                    if (hit.collider.CompareTag("Striker"))
                    {
                        _currentState = State.Started;
                        _startPosition = GetMousePosition();
                        _lastMousePosition = _startPosition;
                    }
                }
            }

            if (UnityEngine.Input.GetMouseButton(0) && _currentState == State.Started)
            {
                _currentState = State.Dragging;
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                _currentState = State.Idle;
            }
        }

        public Vector3 GetPosition()
        {
            return _drag;
        }
    }
}