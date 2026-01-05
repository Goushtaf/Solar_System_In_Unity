using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GravityGhost
{
    public float mass {get; set;}
    public Vector3 position {get; set;}
    public Vector3 velocity {get;set;}
    // use GravityBody so we pick up the initial velocity you set on the body
    public GravityGhost(GravityBody body)
    {
        mass = body._rb != null ? body._rb.mass : 1f;
        position = body.transform.position;
        velocity = body.velocity;
    }
}