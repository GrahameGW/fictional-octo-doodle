using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkRandomizer : StateMachineBehaviour
{
    [SerializeField]
    private int blinkMinTime = 1;

    [SerializeField]
    private int blinkMaxTime = 5;

    float blinkTimer = 0.0f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RefreshTimer();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (blinkTimer <= 0)
        {
            animator.SetTrigger("blinkTrigger");
            RefreshTimer();
        }

        blinkTimer -= Time.deltaTime;
    }

    private void RefreshTimer()
    {
        blinkTimer = Random.Range(blinkMinTime, blinkMaxTime);
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
