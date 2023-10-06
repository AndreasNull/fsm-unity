using System;

namespace ADikt.StateMachine
{
    public class HCondition
    {
    
        public HCondition (Func<bool> condition, Action onEnter, 
            Action onExit = null, Action onUpdate = null)
        {
            this.condition = condition;
            this.onEnter = onEnter;
            this.onExit = onExit;
            this.onUpdate = onUpdate;
        }

        private Func<bool> condition { get; set; }
        private Action onEnter { get; set; }
        private Action onExit { get; set; }
        private Action onUpdate { get; set; }

        public bool Evaluate ()
        {
            return condition == null || condition();
        }

        public void OnEnter ()
        {
            if (onEnter != null)
                onEnter();
        }

        public void OnUpdate()
        {
            if (onUpdate != null)
                onUpdate();
        }

        public void OnExit()
        {
            if (onExit != null)
                onExit();
        }
    }
}
