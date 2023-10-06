using System;

namespace ADikt.StateMachine
{
    public static class GenericStatesExtensions
    {
        public enum ReservedParams
        {
            DelayTrigger = -10001
        }

        public static void AddActionState(this StateMachineManager smm, Enum state, Action onEnter = null, Action onUpdate = null, Action onExit = null)
            => smm.AddState(new ActionState(), state, onEnter, onUpdate, onExit);

        public static void AddDelayState(this StateMachineManager smm, Enum state, float delay, Enum nextState = null)
        {
            smm.AddState(new DelayState(), state, delay, ReservedParams.DelayTrigger);
            smm.RegisterTriggerParam(ReservedParams.DelayTrigger);
            if (nextState == null)
                smm.AddExitTransition(state, new TriggerCondition(ReservedParams.DelayTrigger));
            else
                smm.AddTransition(state, nextState, new TriggerCondition(ReservedParams.DelayTrigger));
        }
    }
}