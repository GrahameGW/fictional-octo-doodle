using UnityEngine;


namespace FictionalOctoDoodle.Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float moveSpeed;
        [SerializeField] Animator animator;

        private PlayerInputMap inputMap;
        private PlayerState activeState;


        private void Awake()
        {
            inputMap = new PlayerInputMap();
            activeState = new IdleState();
            activeState.EnterState(inputMap);
        }

        private void FixedUpdate()
        {
            activeState.Update(this);
        }

        public void SetNewState(PlayerState newState)
        {
            activeState.ExitState();
            activeState = newState;
            newState.EnterState(inputMap);
            ChangeAnimation(newState);
        }

        public void Move(Vector2 value)
        {
            transform.Translate(moveSpeed * Time.fixedDeltaTime * value);
        }

        private void ChangeAnimation(PlayerState state)
        {
            switch (state.ID)
            {
                case PlayerStateID.Idle:
                    animator.SetBool("moving", false);
                    return;
                case PlayerStateID.Moving:
                    animator.SetBool("moving", true);
                    return;
            }
        }
    }
}

