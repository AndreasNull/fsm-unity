using ADikt.StateMachine;
using UnityEngine;

namespace ADikt.Examples
{
    /// <summary>
    /// This examples demonstrates a state machine driving a Unity Animator's state machine :)
    /// Using generic state ActionState & Delay.
    /// </summary>
    public class DoorStateMachineGenericStatesAutoExample : MonoBehaviour
    {
        #region Inspector
        [SerializeField] Animator m_Animator;
        [SerializeField] float m_Delay;
        #endregion

        private StateMachineManager m_SMManager;
        private enum States { Opened, Delay, Closed }
        private enum Params { SpacePressed }

        // Start is called before the first frame update
        void Start()
        {
            // create state machine
            m_SMManager = StateMachineManager.Create(this.gameObject);

            // add generic action states
            m_SMManager.AddActionState(States.Closed, () => m_Animator.SetBool("Open", false));
            m_SMManager.AddActionState(States.Opened, () => m_Animator.SetBool("Open", true));
            m_SMManager.AddDelayState(States.Delay, m_Delay, States.Closed);

            // register trigger parameter
            m_SMManager.RegisterTriggerParam(Params.SpacePressed);

            // add transitions
            m_SMManager.AddTransition(States.Closed, States.Opened, new TriggerCondition(Params.SpacePressed));
            m_SMManager.AddTransition(States.Opened, States.Delay);

            // start state machine
            m_SMManager.StartStateMachine();
        }

        private void Update()
        {
            // fire SpacePressed trigger param on Space key pressed
            if (Input.GetKeyDown(KeyCode.Space))
                m_SMManager.SetTriggerParam((int)Params.SpacePressed);
        }
    }
}