using UnityEngine;

namespace ADikt.StateMachine
{
    public class State : IState
    {
        /// <summary>
        /// The state's name. Must be unique in a state machine.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; private set; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        /// <value>The payload.</value>
        protected object[] payload { get; private set; }

        /// <summary>
        /// The state machine.
        /// </summary>
        private StateMachine m_StateMachine;

        /// <summary>
        /// Init this state instance.
        /// </summary>
        /// <param name="machine">State Machine.</param>
        /// <param name="name">State Name.</param>
        /// <param name="payload">Payload.</param>
        public void Init (StateMachine machine, string name, params object[] payload)
        {
            if(StateMachineManager.debugLogEnabled)
                Debug.LogFormat("+Init state {0}.", name);

            this.name = name;
            m_StateMachine = machine;
            this.payload = payload;

            OnInit();
        }

        public void Enter ()
        {
            if (StateMachineManager.debugLogEnabled)
                Debug.LogFormat("->Enter state {0}.", name);

            OnEnter();
        }

        public void Exit()
        {
            if (StateMachineManager.debugLogEnabled)
                Debug.LogFormat("<-Exit state {0}.", name);

            OnExit();
        }

        protected void SetTriggerParam (int id)
        {
            m_StateMachine.SetTriggerParam(id);
        }
        protected void SetParam (int id, bool value)
        {
            m_StateMachine.SetParam(id, value);
        }
        protected void SetParam(int id, int value)
        {
            m_StateMachine.SetParam(id, value);
        }
        protected void SetParam(int id, float value)
        {
            m_StateMachine.SetParam(id, value);
        }

        public virtual void OnInit () { }

        /// <summary>
        /// Called on enter state.
        /// </summary>
        public virtual void OnEnter() { }

        /// <summary>
        /// Called on exit state.
        /// </summary>
        public virtual void OnExit() { }

        /// <summary>
        /// Ons the message received.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="payload">Payload.</param>
        public virtual void OnMessageReceived(int message, params object[] payload)
        {
            if (StateMachineManager.debugLogEnabled)
                Debug.LogFormat("Message {0}, received.", message);
        }

        /// <summary>
        /// Called on boolean param value change.
        /// </summary>
        /// <param name="paramId">Parameter identifier.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public virtual void OnValueChange(int paramId, bool value)
        {
            if (StateMachineManager.debugLogEnabled)
                Debug.LogFormat("On param value change {0}, {1}", paramId, value);
        }

        /// <summary>
        /// Called on integer param value change.
        /// </summary>
        /// <param name="paramId">Parameter identifier.</param>
        /// <param name="value">Value.</param>
        public virtual void OnValueChange(int paramId, int value)
        {
            if (StateMachineManager.debugLogEnabled)
                Debug.LogFormat("On param value change {0}, {1}", paramId, value);
        }

        /// <summary>
        /// Called on float param value change.
        /// </summary>
        /// <param name="paramId">Parameter identifier.</param>
        /// <param name="value">Value.</param>
        public virtual void OnValueChange(int paramId, float value)
        {
            if (StateMachineManager.debugLogEnabled)
                Debug.LogFormat("On param value change {0}, {1}", paramId, value);
        }

        /// <summary>
        /// Called on every frame when this state is the current state.
        /// </summary>
        public virtual void OnUpdate() { }
    }
}
