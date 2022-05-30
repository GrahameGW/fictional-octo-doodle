using UnityEngine;

public class AIChase : IAIBehavior
{
    public float chaseSpeed;
    public Transform player;
    private Transform transform;

    public void Initialize(Transform transform)
    {
        this.transform = transform;
    }

    public void Update()
    {
        var dir = player.position - transform.position;
        dir.Normalize();
        transform.Translate(new Vector3(chaseSpeed * Time.deltaTime * dir.x, 0f, 0f));
    }
}

