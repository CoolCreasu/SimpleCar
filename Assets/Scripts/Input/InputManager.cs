using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("InputManager: Duplicate instance detected. Destroying the duplicate instance.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private float _inputThrottle = 0.0f;
    private float _inputBrake = 0.0f;
    private float _inputSteering = 0.0f;

    public float InputThrottle { get => _inputThrottle; }
    public float InputBrake { get => _inputBrake; }
    public float InputSteering { get => _inputSteering; }

    public event Action InputGearUp = delegate { };
    public event Action InputGearDown = delegate { };

    private void OnThrottle(InputValue value)
    {
        _inputThrottle = value.Get<float>();
    }

    private void OnBrake(InputValue value)
    {
        _inputBrake = value.Get<float>();
    }

    private void OnSteering(InputValue value)
    {
        _inputSteering = value.Get<float>();
    }

    private void OnGearUp()
    {
        InputGearUp.Invoke();
    }

    private void OnGearDown()
    {
        InputGearDown.Invoke();
    }
}