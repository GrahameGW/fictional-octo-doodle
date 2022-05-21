namespace FictionalOctoDoodle.Core
{
    public abstract class PlayerState
    {
        public abstract PlayerStateID ID { get; }
        
        public virtual void EnterState(PlayerMovement player) { }
        public virtual void Update() { }
        public virtual void ExitState() { }
    }

    public enum PlayerStateID
    {
        Idle,
        Running,
        Airborne,
        Climbing
    }
}

