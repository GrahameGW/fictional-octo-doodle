using System;
using UnityEngine;

public class AIIdle : IAIBehavior
{
    private float elapsed;

    private readonly float duration;
    private readonly Action OnIdleComplete;


    public AIIdle (float idleDuration, Action idleCompleteAction)
    {
        duration = idleDuration;
        OnIdleComplete = idleCompleteAction;
    }

    
    public void Initialize(Transform transform)
    {
        elapsed = 0f;
    }

    public void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= duration)
        {
            OnIdleComplete();
            elapsed = 0f;
        }
    }
}

