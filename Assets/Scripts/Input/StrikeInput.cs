using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

namespace Input
{
    public class StrikeInput : BaseInput
    {
        [SerializeField] private float maxPower = 250f;
        [SerializeField] private float minPower = 5f;
        [SerializeField] private float power = 5f;

        [SerializeField] private LineRenderer lrShootForce;
        [SerializeField] private LineRenderer lrShootDirection;
        [SerializeField] private Striker striker;

        private float _power;

        private Vector3 startPoint;
        private Vector3 endPoint;
        private Vector3 force = Vector3.zero;

        public Action<Vector3> onShoot;
        private Vector3 _strikerPosition;

        private Vector3 _strikerDrag;
        private Boolean _charging = false;
        private Boolean _allowInput = true;

        protected override void HandleInput()
        {
            if (_currentState == State.Idle && !striker.IsMoving())
            {
                Debug.Log("!_charging  && _allowInput :  " +(!_charging  && _allowInput));
                if (UnityEngine.Input.GetMouseButtonDown(0) && !_charging  && _allowInput)
                {
                    _strikerPosition = striker.transform.position;
                    lrShootForce.positionCount = 2;
                    var ray = _mainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Striker")))
                    {
                        if (hit.collider.CompareTag("StrikerSelectionArea"))
                        {
                            _charging = true;
                            startPoint = hit.point;
                            _strikerDrag = startPoint;
                            var position = new Vector3(_strikerPosition.x, _strikerPosition.y, _strikerPosition.z);
                            lrShootForce.SetPosition(0, position);
                            lrShootDirection.SetPosition(0, position);
                            lrShootForce.SetPosition(1, position);
                            lrShootDirection.SetPosition(1, position);
                            lrShootForce.enabled = true;
                            lrShootDirection.enabled = true;
                        }
                    }
                }

                if (UnityEngine.Input.GetMouseButton(0) && _charging)
                {
                    var ray = _mainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        _strikerPosition = striker.transform.position;
                        var position = new Vector3(_strikerPosition.x, _strikerPosition.y, _strikerPosition.z);
                        lrShootForce.SetPosition(0, position);
                        lrShootDirection.SetPosition(0, position);
                        var point = _strikerPosition - _strikerDrag;
                        _strikerDrag = startPoint - hit.point;

                        RaycastHit rayHit;
                        var pos = new Vector3(hit.point.x, _strikerPosition.y, hit.point.z);
                        Debug.DrawRay(_strikerPosition, -(pos - _strikerPosition) * 12, Color.magenta);

                        if (Physics.Raycast(_strikerPosition, (_strikerPosition - pos), out rayHit))
                        {
                            lrShootDirection.SetPosition(1, rayHit.point);
                        }
                    }
                }

                if (UnityEngine.Input.GetMouseButtonUp(0) && _charging)
                {
                    var ray = _mainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        lrShootForce.enabled = false;
                        lrShootDirection.enabled = false;
                        endPoint = hit.point;
                        var pos = new Vector3(hit.point.x, _strikerPosition.y, hit.point.z);
                        var dir = -(_strikerPosition - pos);

                        //Calculate Power

                        _strikerDrag = Vector3.zero;
                        var forceX = Mathf.Clamp((startPoint.x - endPoint.x) * power, minPower, maxPower);
                        var forceY = Mathf.Clamp((startPoint.y - endPoint.y) * power, minPower, maxPower);
                        var forceZ = Mathf.Clamp((startPoint.z - endPoint.z) * power, minPower, maxPower);


                        force = new Vector3(dir.x * forceX, 0, dir.z * forceZ);
                        if (!hit.collider.CompareTag("StrikerSelectionArea"))
                        {
                            AllowInput(false);
                            onShoot?.Invoke(-(dir * power));
                        }

                        _charging = false;
                    }
                }
            }
        }

        public Vector3 GetForce()
        {
            return force;
        }

        public void AllowInput(bool allow)
        {
            _charging = false;
            _allowInput = allow;
        }
    }
}