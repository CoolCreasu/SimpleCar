using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleSystems.Arcade
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private AnimationCurve m_TurningCurve =
            new AnimationCurve(new Keyframe(0.0f, 1.0f), new Keyframe(1.0f, 0.2f));
        [SerializeField] private AnimationCurve m_FrictionCurve =
            new AnimationCurve(new Keyframe(0.0f, 0.1f), new Keyframe(1.0f, 0.6f));

        private Rigidbody m_Rigidbody = default;
        private Vector3 m_CarVelocity = Vector3.zero;

        private WheelCollider[] m_WheelColliders = new WheelCollider[4];

        private void Awake()
        {
            m_Rigidbody = gameObject.GetComponent<Rigidbody>();
            if (!m_Rigidbody) m_Rigidbody = gameObject.AddComponent<Rigidbody>();

            m_WheelColliders = GetComponentsInChildren<WheelCollider>();
        }

        private void FixedUpdate()
        {
            Vector3 worldVelocity = m_Rigidbody.velocity;
            m_CarVelocity.x = Vector3.Dot(worldVelocity, m_Rigidbody.transform.right);
            m_CarVelocity.y = Vector3.Dot(worldVelocity, m_Rigidbody.transform.up);
            m_CarVelocity.z = Vector3.Dot(worldVelocity, m_Rigidbody.transform.forward);

            float turnValue = m_TurningCurve.Evaluate(m_CarVelocity.magnitude * 0.01f) * InputManager.Instance.InputSteering;
            float frictionValue = m_FrictionCurve.Evaluate(m_CarVelocity.magnitude * 0.01f) * m_Rigidbody.mass * 9.81f;

            #region TurnLogic
            if (m_CarVelocity.z > 0.1f)
            {
                m_Rigidbody.AddTorque(transform.up * turnValue * m_Rigidbody.mass * 9.81f);
            }
            else if (false)
            {

            }
            if (m_CarVelocity.z < -0.1f)
            {
                m_Rigidbody.AddTorque(transform.up * -turnValue * m_Rigidbody.mass * 9.81f);
            }
            #endregion

            #region Awake Wheels
            if (InputManager.Instance.InputThrottle > 1E-05f)
            {
                for (int i = 0; i < m_WheelColliders.Length; i++)
                {
                    m_WheelColliders[i].motorTorque = 1E-05f;
                }
            }
            else
            {
                for (int i = 0; i < m_WheelColliders.Length; i++)
                {
                    m_WheelColliders[i].motorTorque = 0.0f;
                }
            }
            #endregion

            float frictionAngle = (-Vector3.Angle(transform.up, Vector3.up) / 90.0f) + 1.0f;

            m_Rigidbody.AddForce(transform.right * frictionValue * frictionAngle * 100f * -m_CarVelocity.normalized.x);

            m_Rigidbody.AddForce(m_Rigidbody.mass * 9.81f * m_Rigidbody.transform.forward * InputManager.Instance.InputThrottle);
        }


    }
}