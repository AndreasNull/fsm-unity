using ADikt.StateMachine;
using UnityEngine;

namespace ADikt.Examples
{
    /// <summary>
    /// This examples demonstrates a state machine driving a Unity Animator's state machine :)
    /// Basic version.
    /// </summary>
    public class DoorStateMachineExample : MonoBehaviour
    {
        #region Inspector
        [SerializeField] Animator m_Animator;
        #endregion

        private StateMachineManager m_SMManager;
        private enum States { Opened, Closed }
        private enum Params { SpacePressed }

        // Start is called before the first frame update
        void Start()
        {
            // create state machine
            m_SMManager = StateMachineManager.Create(this.gameObject);

            // add states
            m_SMManager.AddState(new ClosedState(), States.Closed, m_Animator);
            m_SMManager.AddState(new OpenedState(), States.Opened, m_Animator);

            // register trigger parameter
            m_SMManager.RegisterTriggerParam(Params.SpacePressed);

            // add transitions
            m_SMManager.AddTransition(States.Opened, States.Closed, new TriggerCondition(Params.SpacePressed)); // first state
            m_SMManager.AddTransition(States.Closed, States.Opened, new TriggerCondition(Params.SpacePressed));

            // start state machine
            m_SMManager.StartStateMachine();
        }

        private void Update()
        {
            // fire SpacePressed trigger param on Space key pressed
            if (Input.GetKeyDown(KeyCode.Space))
                m_SMManager.SetTriggerParam((int)Params.SpacePressed);
        }

        /// <summary>
        /// Opened state.
        /// </summary>
        private class OpenedState : State
        {
            Animator m_Animator;

            // get animator component from payload
            public override void OnInit() => m_Animator = (Animator)payload[0];
            public override void OnEnter() => m_Animator.SetBool("Open", true);
            
        }

        /// <summary>
        /// Closed state.
        /// </summary>
        private class ClosedState : State
        {
            Animator m_Animator;

            // get animator component from payload
            public override void OnInit() => m_Animator = (Animator)payload[0];
            public override void OnEnter() => m_Animator.SetBool("Open", false);
            
        }
    }
}