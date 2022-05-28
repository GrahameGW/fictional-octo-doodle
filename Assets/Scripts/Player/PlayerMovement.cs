using UnityEngine;
using UnityEngine.InputSystem;


namespace FictionalOctoDoodle.Core
{
    public class PlayerMovement : MonoBehaviour
    {
        public PlayerInputMap Input { get; private set; }
        public bool InWater { get; private set; }
        public bool CanClimb { get; private set; }

        [SerializeField] PlayerMoveStats stats;
        [SerializeField] GameObject weapon;

        private Rigidbody2D rb;
        private Animator animator;
        private PlayerState activeState; // states enable & disable actions, and handle updating actions (may want to return that to the player)
        private LimbAssembly limbAssembly;
        private float distanceToGround;



        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
            limbAssembly = GetComponentInChildren<LimbAssembly>();
            //distanceToGround = GetComponentInChildren<Collider2D>().bounds.extents.y;
            distanceToGround = 100f;
        }

        private void OnEnable()
        {
            Input = new PlayerInputMap();
            Input.Player.Jump.performed += Jump;
            Input.Player.Fire.performed += Attack;
            limbAssembly.OnAnimatorChanged += ReloadState;

            Input.Player.Move.Enable(); // enabling all actions at startup; states will disable where necessary
            Input.Player.Jump.Enable();
            Input.Player.Fire.Enable();

            SetNewState(new IdleState());

            //weapon.SetActive(false);
        }
        private void OnDisable()
        {
            Input.Player.Jump.performed -= Jump;
            Input.Player.Fire.performed -= Attack;
            limbAssembly.OnAnimatorChanged -= ReloadState;

            Input.Player.Move.Disable(); 
            Input.Player.Jump.Disable();
            Input.Player.Fire.Disable();
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

        private void ReloadState()
        {
            var state = activeState;
            SetNewState(state);
        }

        public void Move(Vector2 value)
        {
            var vec = InWater ? value * stats.swimSpeed * Vector2.one : value * new Vector2(stats.moveSpeed, stats.climbSpeed);
            transform.Translate(vec * Time.fixedDeltaTime);

            if (value.x != 0f)
            {
                limbAssembly.FlipModels(value.x > 0f ? 0f : 180f);
            }
        }

        public void Jump(InputAction.CallbackContext ctx)
        {
            rb.AddForce(Vector2.up * stats.jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("jump");
            Debug.Log("jump");
        }

        public void Attack(InputAction.CallbackContext ctx)
        {
           // weapon.SetActive(true);
        }

        public bool IsGrounded()
        {
            // 1 << 3 gets the "Ground" layer
            return Physics2D.Raycast(transform.position, Vector2.down, distanceToGround + 0.1f, 1 << 3).collider != null;
        }

        public void ToggleGravity(float scale)
        {
            rb.gravityScale = scale;
            if (scale == 0f)
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
            if (!CanClimb)
            {
                CanClimb = LayerMask.NameToLayer("Climb") == other.gameObject.layer;
            }
            if (!InWater)
            {
                InWater = LayerMask.NameToLayer("Water") == other.gameObject.layer;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (CanClimb)
            {
                var filter = new ContactFilter2D();
                var mask = LayerMask.NameToLayer("Climb");
                filter.SetLayerMask(mask);
                CanClimb = rb.IsTouching(filter);
            }

            if (InWater && LayerMask.NameToLayer("Water") == other.gameObject.layer)
            {
                InWater = false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.down * (distanceToGround + 0.1f));
        }
    }
}

