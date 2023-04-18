using UnityEngine;

namespace VehicleSystems.Experimental
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(WheelCollider))]
    public class ExperimentalWheelCollider : MonoBehaviour
    {
        public float MotorTorque { get; set; } = 0.0f;
        public float RPM { get; private set; } = 0.0f;

        private WheelCollider _wheelCollider = default;
        private Rigidbody _rigidbody = default;

        private void Awake()
        {
            _wheelCollider = GetWheelCollider();

            _rigidbody = GetComponentInParent<Rigidbody>();

            if (_rigidbody == null)
            {
                Debug.LogWarning("ExperimentalWheelCollider requires an attached Rigidbody to function.");
            }
        }

        private void FixedUpdate()
        {
            RPM = _wheelCollider.rpm;
            _wheelCollider.motorTorque = MotorTorque;
        }

        private WheelCollider GetWheelCollider()
        {
            // Get the wheel collider on this gameobject
            _wheelCollider = gameObject.GetComponent<WheelCollider>();
            if (_wheelCollider == null)
            {
                // Add a new wheel collider on this gameobject if there wasn't one
                _wheelCollider = gameObject.AddComponent<WheelCollider>();
            }
            return _wheelCollider;
        }
    }
}