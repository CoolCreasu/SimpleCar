using UnityEngine;

[DisallowMultipleComponent]
public class VisualWheelController : MonoBehaviour
{
    [SerializeField] private CustomWheelCollider _customWheelCollider = default;

    private void Awake()
    {
        if (_customWheelCollider == null)
        {
            Debug.LogWarning("VisualWheelController requires a reference to a CustomWheelCollider to function.");
        }
    }

    private void FixedUpdate()
    {
        if (_customWheelCollider == null)
        {
            return;
        }

        _customWheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);

        transform.position = position;
        transform.rotation = rotation;
    }
}