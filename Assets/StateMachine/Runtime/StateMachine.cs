using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADikt.StateMachine
{
    public partial class StateMachine
    {
        #region Exposed to friend classes
        internal IState editor_InitState { get { return m_InitState; } }
        internal List<IState> editor_States { get { return m_States.Values.ToList(); } }
        #endregion

        /// <summary>
        /// State machine's status.
        /// </summary>
        public enum Status
        {
            Stopped,
            HCondition,
            Running,
            Paused
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public Status status { get; private set; }

        private IState m_InitState;
        private IState m_CurrentState;
        private HCondition m_CurrentHCondition;
        private Dictionary<string, IState> m_States = new Dictionary<string, IState>();
        private Dictionary<string, List<Transition>> m_Transitions = new Dictionary<string, List<Transition>>();
        private List<Transition> m_AnyTransitions = new List<Transition>();
        private Action m_OnExitCallback;
        // state -> list of StateMachineManager
        private Dictionary<string, List<HCondition>> m_StateHConditions = new Dictionary<string, List<HCondition>>();

        public string currentStateName
        {
            get
            {
                if (m_CurrentState == null)
                    return "None";

                return m_CurrentState.name;
            }
        }

        public IState currentState
        {
            get { return m_CurrentState; }
        }

        public void AddHCondition (Func<bool> condition, string state, Action onEnter,
            Action onExit = null, Action onUpdate = null)
        {
            if (!m_StateHConditions.ContainsKey(state))
                m_StateHConditions[state] = new List<HCondition>();

            m_StateHConditions[state].Add(new HCondition(condition, onEnter, onExit, onUpdate));
        }

        public void AddState(IState state, string stateName, params object[] payload)
        {
            if (m_InitState == null)
                m_InitState = state;

            if (m_States.ContainsKey(stateName))
            {
                Debug.LogWarningFormat("State with name {0} already exists.", stateName);
            }

            m_States.Add(stateName, state);
            state.Init(this, stateName, payload);
        }

        public void AddTransition(Transition transition)
        {
            if(transition.isAnyTransition)
            {
                m_AnyTransitions.Add(transition);
                return;
            }

            if (m_Transitions.ContainsKey(transition.sourceState))
            {
                m_Transitions[transition.sourceState].Add(transition);
            }
            else
            {
                m_Transitions.Add(transition.sourceState, new List<Transition>());
                m_Transitions[transition.sourceState].Add(transition);
            }
        }

        public void AddTransitionFromAny (string targetState, params Condition[] conditions)
        {
            AddTransition(new Transition(null, targetState, conditions));
        }

        public void StartStateMachine(IState initState = null, Action onExit = null)
        {
            // Start state machine and initialise parameters
            StartStateMachineInternal(initState, onExit, true);
        }

        public void ResumeStateMachine(IState initState = null, Action onExit = null)
        {
            // Start state machine without initialising parameters
            StartStateMachineInternal(initState, onExit, false);
        }

        private void StartStateMachineInternal(IState initState, Action onExit, bool initParams)
        {
            if (status == Status.Running)
            {
                Debug.LogWarning("State machine is already running!");
                return;
            }

            if (m_InitState == null && initState == null)
            {
                Debug.LogWarning("State machine has no init state.");
                status = Status.Stopped;
                return;
            }

            // define init state
            m_InitState = initState != null ? initState : m_InitState;

            // set params to their init values
            if (initParams)
                InitialiseAllParams();

            // define exit callback
            m_OnExitCallback = onExit;

            // change state to init state
            ChangeState(m_InitState);
        }

        private void ExitStateMachine ()
        {
            // stop state machine
            StopStateMachine(callOnExit:true);
            // call on exit call back if exists
            if (m_OnExitCallback != null)
                m_OnExitCallback();

            if(StateMachineManager.debugLogEnabled)
                Debug.Log("~ExitStateMachine");
        }

        public void StopStateMachine(bool callOnExit = true)
        {
            if (status == Status.Stopped)
            {
                Debug.LogWarning("State machine is already stopped!");
                return;
            }

            if (callOnExit)
                m_CurrentState.Exit();

            status = Status.Stopped;
            m_CurrentState = null;
        }

        public void PlayStateMachine()
        {
            if (status != Status.Paused)
            {
                Debug.LogWarning("State machine is not in paused state!");
                return;
            }

            status = Status.Running;
        }

        public void PauseStateMachine()
        {
            status = Status.Paused;
        }

        public void SendMessage(int message, params object[] payload)
        {
            if (m_CurrentState != null)
                m_CurrentState.OnMessageReceived(message, payload);
        }

        #region State Machine Logic
        void ChangeState(IState state)
        {
            ChangeState(state.name);
        }

        void ChangeState(string stateName)
        {
            if (!m_States.ContainsKey(stateName))
            {
                Debug.LogWarningFormat("{0} state does not exists.", stateName);
                return;
            }

            IState newState = m_States[stateName];

            // In this state machine implementation we allow 
            // repetitive states.
            //if (m_CurrentState == newState)
            //return;

            // On Exit Current State
            if (m_CurrentState != null)
                m_CurrentState.Exit();

            // Change State
            m_CurrentState = newState;

            // Check HConditions for this state
            if (CheckForStateHConditions(newState))
                status = Status.HCondition;
            else
                EnterState(newState);
        }

        void EnterState (IState state)
        {
            // change status
            status = Status.Running;

            // On Enter Next State
            state.Enter();

            // Check For Transitions
            CheckForTransitions();
        }

        void CheckForTransitions ()
        {
            // we check first for transitions from any
            if (CheckForTransition(m_AnyTransitions))
                return;

            if (m_CurrentState == null)
                return;

            // check for current state's transitions
            if (m_Transitions.ContainsKey(m_CurrentState.name))
                CheckForTransition(m_Transitions[m_CurrentState.name]);
        }

        private bool CheckForTransition(List<Transition> transitions)
        {
            if (transitions.Count == 0)
                return false;

            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].Evaluate(this))
                {
                    ApplyTransition(transitions[i]);
                    return true;
                }
            }

            return false;
        }

        private void ApplyTransition (Transition transition)
        {
            // Clear Dirty Triggers Params here too to prevent
            // change to another state automaticaly
            ClearDirtyTriggerParams();

            // check if this is a transition to exit state
            if (transition.isExitTransition)
                ExitStateMachine();
            else
                ChangeState(transition.targetState);
        }

        public void Update()
        {
            if(status == Status.HCondition)
            {
                if (!CheckForStateHConditions(m_CurrentState))
                    EnterState(m_CurrentState);
                else if(m_CurrentHCondition != null)
                {
                    m_CurrentHCondition.OnUpdate();
                    return;
                }
            }

            if (status != Status.Running)
                return;

            if (m_CurrentState != null)
                m_CurrentState.OnUpdate();

            // Check For Transitions
            CheckForTransitions();

            // Clear Dirty Triggers Params
            ClearDirtyTriggerParams();
        }


        private bool CheckForStateHConditions(IState state)
        {
            if (!m_StateHConditions.ContainsKey(state.name))
                return false;

            return CheckHCondition(m_StateHConditions[state.name]);
        }

        private bool CheckHCondition (List<HCondition> hConditions)
        {
            if (hConditions == null || hConditions.Count == 0)
            {
                m_CurrentHCondition = null;
                return false;
            }

            // check conditions one by one
            for (int i = 0; i < hConditions.Count; i++)
            {
                // if condition is false
                if (!hConditions[i].Evaluate())
                {
                    // if there is a different condition
                    if (m_CurrentHCondition != hConditions[i])
                    {
                        // onExit prev condition
                        if (m_CurrentHCondition != null)
                            m_CurrentHCondition.OnExit();
                        // assign new current condition
                        m_CurrentHCondition = hConditions[i];
                        // onEnter new condition
                        m_CurrentHCondition.OnEnter();
                    }

                    return true;
                }
            }

            // if all conditions are true
            m_CurrentHCondition = null;
            return false;
        }
        #endregion
    }
}
