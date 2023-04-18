using UnityEngine;

namespace VehicleSystems.Experimental
{
    public class ExperimentalCarController : MonoBehaviour
    {
        [SerializeField] private ExperimentalWheelCollider _wheelColliderFL = default;
        [SerializeField] private ExperimentalWheelCollider _wheelColliderFR = default;
        [SerializeField] private ExperimentalWheelCollider _wheelColliderRL = default;
        [SerializeField] private ExperimentalWheelCollider _wheelColliderRR = default;

        private void FixedUpdate()
        {
            _wheelColliderFL.MotorTorque = InputManager.Instance.InputThrottle != 0.0f ? 1E-05f : 0.0f;
            _wheelColliderFR.MotorTorque = InputManager.Instance.InputThrottle != 0.0f ? 1E-05f : 0.0f;

            _wheelColliderRL.MotorTorque = _wheelColliderRL.RPM < 1200 ? InputManager.Instance.InputThrottle * 1000f * 20 : 0.0f;
            _wheelColliderRR.MotorTorque = _wheelColliderRR.RPM < 1200 ? InputManager.Instance.InputThrottle * 1000f * 20: 0.0f;
        }
    }
}