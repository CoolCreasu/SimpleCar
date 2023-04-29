using UnityEngine;

[DisallowMultipleComponent]
public class CustomWheelCollider : MonoBehaviour
{
    [Header("Suspension")]
    [SerializeField] private float _suspensionDistance = 0.3f;
    [SerializeField] private float _suspensionSpring = 35000.0f;
    [SerializeField] private float _suspensionDamper = 4500.0f;
    [Header("Wheel")]
    [SerializeField] private float _radius = 0.5f;

    private float _fixedDeltaTime = 0.0f;

    private Vector3 _transformPositon = Vector3.zero;
    private Vector3 _wheelRight = Vector3.zero;
    private Vector3 _wheelUp = Vector3.zero;
    private Vector3 _wheelForward = Vector3.zero;

    private bool _isGrounded = false;
    private RaycastHit _wheelHit = default;
    private Vector3 _wheelPosition = Vector3.zero;

    private float _suspensionCompression = 0.0f;
    private float _suspensionCompressionPrevious = 0.0f;
    private float _suspensionForce = 0.0f;

    private float _load = 0.0f;

    private Rigidbody _rigidbody = default;

    public float steerAngle { get; set; } = 0.0f;

    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();

        if (_rigidbody == null)
        {
            Debug.LogWarning("CustomWheelCollider requires an attached Rigidbody to function.");
        }
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        _transformPositon = transform.position;

        // wheel orientations in world space
        Quaternion steerRotation = Quaternion.Euler(0.0f, steerAngle, 0.0f);
        _wheelRight = transform.TransformDirection(steerRotation * Vector3.right);
        _wheelUp = transform.TransformDirection(steerRotation * Vector3.up);
        _wheelForward = transform.TransformDirection(steerRotation * Vector3.forward);

        // Ground detection
        _isGrounded = Physics.Raycast(_transformPositon, -_wheelUp, out _wheelHit, _suspensionDistance + _radius);

        // Wheel center position
        _wheelPosition = _transformPositon - _wheelUp * (_isGrounded ? _wheelHit.distance : _suspensionDistance);

        // Suspension
        _suspensionCompressionPrevious = _isGrounded ? _suspensionCompression : 0.0f;
        _suspensionCompression = _isGrounded ? (_suspensionDistance + _radius) - _wheelHit.distance : 0.0f;
        _suspensionForce = (_suspensionCompression * _suspensionSpring) + ((_suspensionCompression - _suspensionCompressionPrevious) / _fixedDeltaTime * _suspensionDamper);
        _rigidbody.AddForceAtPosition(_wheelUp * _suspensionForce, _transformPositon);

        // Load
        _load = Mathf.Max(_suspensionForce, 0.0f);
    }

    public void GetWorldPose(out Vector3 position, out Quaternion rotation)
    {
        position = transform.position - _wheelUp * (_suspensionDistance - _suspensionCompression);
        rotation = transform.rotation * Quaternion.Euler(0.0f, steerAngle, 0.0f);
    }
}