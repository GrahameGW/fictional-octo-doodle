using UnityEngine;
using UnityEngine.InputSystem;

namespace FictionalOctoDoodle.Core
{
    public class PlayerMovement : MonoBehaviour
    {
        public PlayerInputMap Input { get; private set; }
        public bool canClimb;

        [SerializeField] float moveSpeed;
        [SerializeField] float climbingSpeed;
        [SerializeField] float jumpHeight;
        [SerializeField] Animator animator;

        private Rigidbody2D rb;
        private PlayerState activeState; // states enable & disable actions, and handle updating actions (may want to return that to the player)
        private float distanceToGround;



        private void OnEnable()
        {
            Input = new PlayerInputMap();
            Input.Player.Jump.performed += Jump;

            Input.Player.Move.Enable(); // enabling all actions at startup; states will disable where necessary
            Input.Player.Jump.Enable();

            SetNewState(new IdleState());

            rb = GetComponent<Rigidbody2D>();
            distanceToGround = GetComponentInChildren<Collider2D>().bounds.extents.y;
        }
        private void OnDisable()
        {
            Input.Player.Jump.performed -= Jump;
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

        public void Move(Vector2 value)
        {
            var vec = value * new Vector2(moveSpeed, climbingSpeed);
            transform.Translate(vec * Time.fixedDeltaTime);
        }

        public void Jump(InputAction.CallbackContext ctx)
        {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            animator.SetTrigger("jump");
        }

        public bool IsGrounded()
        {
            // 1 << 3 gets the "Ground" layer
            return Physics2D.Raycast(transform.position, Vector2.down, distanceToGround + 0.1f, 1 << 3).collider != null;
        }

        public void ToggleGravity(bool onOff)
        {
            rb.gravityScale = onOff ? 1f : 0f;
            if (!onOff)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }
        }

        private void ChangeAnimation(PlayerState state)
        {
            animator.SetBool("airborne", false);
            animator.SetBool("moving", false);

            switch (state.ID)
            {
                case PlayerStateID.Running:
                    animator.SetBool("moving", true);
                    return;
                case PlayerStateID.Airborne:
                    animator.SetBool("airborne", true);
                    return;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!canClimb)
            {
                canClimb = LayerMask.NameToLayer("Climb") == other.gameObject.layer;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (canClimb)
            {
                var filter = new ContactFilter2D();
                var mask = LayerMask.NameToLayer("Climb");
                filter.SetLayerMask(mask);
                canClimb = rb.IsTouching(filter);
            }
        }
    }
}

