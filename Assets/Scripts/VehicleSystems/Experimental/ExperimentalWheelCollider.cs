using UnityEngine;

namespace VehicleSystems.Experimental
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(WheelCollider))]
    public class ExperimentalWheelCollider : MonoBehaviour
    {
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