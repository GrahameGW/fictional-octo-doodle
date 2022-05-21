using UnityEngine;


namespace FictionalOctoDoodle.Core
{
    public class Player : MonoBehaviour
    {
        public PlayerInputMap Input { get; private set; }

        [SerializeField] float moveSpeed;
        [SerializeField] float jumpHeight;
        [SerializeField] Animator animator;

        private Rigidbody2D rb;
        private PlayerState activeState;
        private float distanceToGround;


        private void Awake()
        {
            Input = new PlayerInputMap();
            SetNewState(new IdleState());

            rb = GetComponent<Rigidbody2D>();
            distanceToGround = GetComponentInChildren<Collider2D>().bounds.extents.y;
        }

        private void FixedUpdate()
        {
            activeState.Update();
        }

        public void SetNewState(PlayerState newState)
        {
            activeState?.ExitState();
            activeState = newState;
            newState.EnterState(this);
            ChangeAnimation(newState);
        }

        public void Move(float xAxis)
        {
            transform.Translate(moveSpeed * Time.fixedDeltaTime * xAxis * Vector3.right);
        }

        public void Jump()
        {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
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

        public bool IsGrounded()
        {
            // 1 << 3 gets the "Ground" layer
            return Physics2D.Raycast(transform.position, Vector2.down, distanceToGround + 0.1f, 1 << 3).collider != null;
        }

        private void OnDrawGizmos()
        {
            Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.down * (distanceToGround + 0.1f));
        }
    }
}

