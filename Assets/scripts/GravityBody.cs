using UnityEngine;
using UnityEngine.Rendering;

public class GravityBody : MonoBehaviour
{
    public Rigidbody _rb;
    public Vector3 initialVelocity;

    public Vector3 velocity;
    
    void Awake()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();

        velocity = initialVelocity;

        // stop Unity physics from moving the object — we drive transform ourselves
        if (_rb != null) _rb.isKinematic = true;
    }
    void Start()
    {
        
    }
}