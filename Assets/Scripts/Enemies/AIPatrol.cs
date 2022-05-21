using UnityEngine;

public class AIPatrol : IAIBehavior
{
    public float patrolSpeed;
    public Vector3[] waypoints;

    private Transform transform;
    private int index = 0;

    public void Initialize(Transform transform)
    {
        this.transform = transform;
    }

    public void Update()
    {
        if (waypoints.Length <= 0) return;

        var dir = waypoints[index] - transform.position;
        dir.Normalize();
        var speed = patrolSpeed * Time.deltaTime;
        transform.Translate(speed * dir);

        if (Vector3.Distance(transform.position, waypoints[index]) <= speed)
        {
            index = index == waypoints.Length - 1 ? 0 : index + 1;
        }
    }
}

