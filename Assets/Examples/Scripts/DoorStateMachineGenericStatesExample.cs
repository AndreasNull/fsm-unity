using ADikt.StateMachine;
using UnityEngine;

namespace ADikt.Examples
{
    /// <summary>
    /// This examples demonstrates a state machine driving a Unity Animator's state machine :)
    /// Using generic state ActionState.
    /// </summary>
    public class DoorStateMachineGenericStatesExample : MonoBehaviour
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

            // add generic action states
            m_SMManager.AddActionState(States.Closed, () => m_Animator.SetBool("Open", false));
            m_SMManager.AddActionState(States.Opened, () => m_Animator.SetBool("Open", true));

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
    }
}