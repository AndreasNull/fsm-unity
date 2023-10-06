using System;

namespace ADikt.StateMachine
{
    /// <summary>
    /// Action state.
    /// payload[0] -> OnEnter : Action
    /// payload[1] -> OnUpdate : Action
    /// payload[2] -> OnExit : Action
    /// </summary>
    public class ActionState : State
    {
        public override void OnEnter()
        {
            if (payload != null && payload.Length > 0 && payload[0] != null)
                ((Action)payload[0]).Invoke();
        }

        public override void OnUpdate()
        {
            if (payload != null && payload.Length > 1 && payload[1] != null)
                ((Action)payload[1]).Invoke();
        }

        public override void OnExit()
        {
            if (payload != null && payload.Length > 2 && payload[2] != null)
                ((Action)payload[2]).Invoke();
        }
    }
}