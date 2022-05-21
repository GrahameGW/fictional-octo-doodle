using UnityEngine;

public class AIChase : IAIBehavior
{
    private Transform transform;

    public void Initialize(Transform transform)
    {
        this.transform = transform;
    }

    public void Update()
    {

    }
}

