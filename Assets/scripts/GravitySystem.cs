using UnityEngine;
public class GravitySystem : MonoBehaviour
{
    public float G;
    private GravityBody[] bodies;

    public bool running = false;
    void Awake()
    {
        bodies = FindObjectsByType<GravityBody>(FindObjectsSortMode.None);
        running = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    Vector3 ComputeForce(GravityBody current)
    {
        Vector3 sumOfForces = new Vector3(0,0,0);
        float thisMass = current._rb.mass;
        foreach (var that in bodies)
        {
            if (that == current)
            {
                continue;
            }
            float thatMass = that._rb.mass;
            Vector3 vDistance = that.transform.position - current.transform.position;
            float distance = vDistance.magnitude +0.1f;
            vDistance = vDistance.normalized;
            float gForceMagnitude = G*(thatMass*thisMass)/(distance*distance);
            sumOfForces += vDistance * gForceMagnitude;
        }
        return sumOfForces;
    }
    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        if (running)
        {
            foreach (var body in bodies)
            {
                Vector3 force = ComputeForce(body);
                Vector3 acceleration = force / body._rb.mass;
                body.velocity += acceleration * dt;
            }
            foreach (var body in bodies)
            {
                body.transform.position += body.velocity * dt;
            }
        }

        
        
        
        
    }
}