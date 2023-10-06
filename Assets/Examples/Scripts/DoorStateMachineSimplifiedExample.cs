using ADikt.StateMachine;
using UnityEngine;

namespace ADikt.Examples
{
    /// <summary>
    /// This examples demonstrates a state machine driving a Unity Animator's state machine :)
    /// Simplified version.
    /// </summary>
    public class DoorStateMachineSimplifiedExample : MonoBehaviour
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
            m_SMManager.AddState(new AnimatorBoolState(), States.Closed, m_Animator, "Open", false);
            m_SMManager.AddState(new AnimatorBoolState(), States.Opened, m_Animator, "Open", true);

            // register trigger parameter
            m_SMManager.RegisterTriggerParam(Params.SpacePressed);

            // add transitions
            m_SMManager.AddTransition(States.Opened, States.Closed, new TriggerCondition(Params.SpacePressed));
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
        private class AnimatorBoolState : State
        {
            Animator m_Animator;
            string m_AnimParamName;
            bool m_AnimParamValue;

            public override void OnInit()
            {
                // get animator component from payload
                m_Animator = (Animator)payload[0];
                // get animator param name
                m_AnimParamName = (string)payload[1];
                // get animator param value
                m_AnimParamValue = (bool)payload[2];
            }

            public override void OnEnter() => m_Animator.SetBool(m_AnimParamName, m_AnimParamValue);
            
        }
    }
}