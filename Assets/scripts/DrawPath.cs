using JetBrains.Annotations;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Analytics;

public class DrawPath : MonoBehaviour
{
    public float deltaStep;
    public int nmbrOfSteps;

    public bool draw;   
    public LineRenderer[] lrs;

    public Rigidbody BodyRelativeTo;
    private GravityGhost GhostRelativeTo;
    private GravityBody[] bodies;
    private GravityGhost[] ghosts;

    public Rigidbody[] bodysToDraw;

    private GravityGhost[] ghostsToDraw;

  

    public float width;
    public GravitySystem gs;

    private Vector3[][] linePoints;

    void Awake()
    {
        if (gs == null)
        {
            gs = GetComponent<GravitySystem>();
        }

        foreach (LineRenderer lr in lrs)
        {
            SetLineProperties(lr);
        }

        if (bodysToDraw == null) bodysToDraw = new Rigidbody[0];

        linePoints = new Vector3[bodysToDraw.Length][];
        for (int i = 0; i < linePoints.Length; ++i)
        {
            linePoints[i] = new Vector3[nmbrOfSteps];
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (draw)
        {
            getGhosts();

            for (int i = 0; i < nmbrOfSteps; ++i)
            {
                    Vector3 offset = (GhostRelativeTo != null) ? GhostRelativeTo.position : Vector3.zero;

                for (int j = 0; j < linePoints.Length; j++)
                {
                    if (ghostsToDraw == null || ghostsToDraw.Length <= j || ghostsToDraw[j] == null)
                    {
                        linePoints[j][i] = Vector3.zero;
                    }
                    else
                    {
                        linePoints[j][i] = ghostsToDraw[j].position - offset;
                    }
                }
                Simulate(deltaStep);
            }
            for (int i = 0; i < linePoints.Length; ++i)
            {
                DrawLine(linePoints[i], lrs[i]);
            }
        }
        else
        {
            foreach (LineRenderer lr in lrs)
            {
                lr.positionCount = 0;
            }
        }
    }
    
    void getGhosts()
    {
        bodies = null;
        bodies = FindObjectsByType<GravityBody>(FindObjectsSortMode.None);
        ghosts = new GravityGhost[bodies.Length];
        ghostsToDraw = new GravityGhost[bodysToDraw.Length];
        GhostRelativeTo = null;

        for (int i = 0; i < bodies.Length; ++i)
        {
            ghosts[i] = new GravityGhost(bodies[i]);

            for (int j = 0; j < bodysToDraw.Length; ++j)
            {
                if (bodies[i]._rb == bodysToDraw[j])
                {
                    ghostsToDraw[j] = ghosts[i];
                }
            }
            // compare GravityBody to BodyRelativeTo (we store Rigidbody in BodyRelativeTo)
            if (BodyRelativeTo != null && bodies[i]._rb == BodyRelativeTo)
            {
                // use the ghost instance so its position is updated by Simulate()
                GhostRelativeTo = ghosts[i];
            }
        }
    }

    void Simulate(float deltaStep)
    {
        float dt = deltaStep;
        foreach (var ghost in ghosts)
        {
            Vector3 force = ComputeForce(ghost);
            Vector3 acceleration = force / ghost.mass;
            ghost.velocity += acceleration * dt;
        }
        foreach (var ghost in ghosts)
        {
            ghost.position += ghost.velocity * dt;
        }
    }
    Vector3 ComputeForce(GravityGhost current)
    {
        Vector3 sumOfForces = new Vector3(0,0,0);
        float thisMass = current.mass;
        foreach (var that in ghosts)
        {
            if (that == current)
            {
                continue;
            }
            float thatMass = that.mass;
            Vector3 vDistance = that.position - current.position;
            float distance = vDistance.magnitude +0.1f;
            vDistance = vDistance.normalized;
            float gForceMagnitude = (gs.G)*(thatMass*thisMass)/(distance*distance);
            sumOfForces += vDistance * gForceMagnitude;
        }
        return sumOfForces;
    }
    private void SetLineProperties(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = nmbrOfSteps;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }
    private void DrawLine(Vector3[] pathPoints, LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = pathPoints.Length;
        lineRenderer.SetPositions(pathPoints);
    }

}
