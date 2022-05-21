namespace FictionalOctoDoodle.Core
{
    public abstract class PlayerState
    {
        public abstract PlayerStateID ID { get; }
        
        public virtual void EnterState(PlayerInputMap inputMap) { }
        public virtual void Update(Player player) { }
        public virtual void ExitState() { }
    }

    public enum PlayerStateID
    {
        Idle,
        Moving
    }
}

