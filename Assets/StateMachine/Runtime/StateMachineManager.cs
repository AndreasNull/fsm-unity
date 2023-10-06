using System;
using UnityEngine;

namespace ADikt.StateMachine
{
    public class StateMachineManager : MonoBehaviour
    {
        #if UNITY_EDITOR
        public StateMachine editor_StateMachine { get { return m_StateMachine; } }
#endif

        #region Debug
        private static bool s_DebugLogEnabled = false;
        public static bool debugLogEnabled
        {
            get { return s_DebugLogEnabled; }
            set { s_DebugLogEnabled = value; }
        }
        #endregion

        #region API
        public static StateMachineManager Create(GameObject gameObject)
        {
            return gameObject.AddComponent<StateMachineManager>().Init();
        }

        public StateMachine.Status status { get { return m_StateMachine.status; } }
        public string currentState { get { return m_StateMachine.currentStateName; } }

        public void AddHCondition(Func<bool> condition, string state, Action onEnter,
            Action onExit = null, Action onUpdate = null)
        {
            m_StateMachine.AddHCondition(condition, state, onEnter, onUpdate, onExit);
        }
        public void AddState(IState state, Enum stateEnum, params object[] payload)
        {
            m_StateMachine.AddState(state, Convert.ToString(stateEnum), payload);
        }
        public void AddTransition(IState sourceState, IState targetState, params Condition[] conditions)
        {
            m_StateMachine.AddTransition(new Transition(sourceState.name, targetState.name, conditions));
        }
        public void AddTransition(Enum sourceState, Enum targetState, params Condition[] conditions)
        {
            m_StateMachine.AddTransition(new Transition(sourceState.ToString(), targetState.ToString(), conditions));
        }
        public void AddTransitionFromAny(Enum targetState, params Condition[] conditions)
        {
            m_StateMachine.AddTransitionFromAny(targetState.ToString(), conditions);
        }
        public void AddExitTransition(IState sourceState, params Condition[] conditions)
        {
            m_StateMachine.AddTransition(new Transition(sourceState.name, conditions));
        }
        public void AddExitTransition(Enum sourceState, params Condition[] conditions)
        {
            m_StateMachine.AddTransition(new Transition(sourceState.ToString(), conditions));
        }
        public void StartStateMachine(IState initState = null, Action onExit = null)
        {
            m_StateMachine.StartStateMachine(initState, onExit);
        }
        public void ResumeStateMachine(IState initState = null, Action onExit = null)
        {
            m_StateMachine.ResumeStateMachine(initState, onExit);
        }
        public void PauseStateMachine() { m_StateMachine.PauseStateMachine(); }
        public void PlayStateMachine() { m_StateMachine.PlayStateMachine(); }
        public void StopStateMachine(bool callOnExit = true) { m_StateMachine.StopStateMachine(callOnExit); }

        //public void SendStateMachineMessage(int message, params object[] payload) { m_StateMachine.SendMessage(message, payload); }
        public void RegisterTriggerParam(int paramId, string name) { m_StateMachine.RegisterTriggerParam(paramId, name); }
        public void SetTriggerParam (int paramId) { m_StateMachine.SetTriggerParam(paramId); }

        public void RegisterParam(int paramId, bool value, string name) { m_StateMachine.RegisterParam(paramId, value, name); }
        public void RegisterParam(int paramId, int value, string name) { m_StateMachine.RegisterParam(paramId, value, name); }
        public void RegisterParam(int paramId, float value, string name) { m_StateMachine.RegisterParam(paramId, value, name); }
        public void SetParam(int paramId, bool value) { m_StateMachine.SetParam(paramId, value); }
        public void SetParam(int paramId, int value) { m_StateMachine.SetParam(paramId, value); }
        public void SetParam(int paramId, float value) { m_StateMachine.SetParam(paramId, value); }
        public bool GetParamBool(int paramId) { return m_StateMachine.GetBoolValue(paramId); }
        public int GetParaInteger(int paramId) { return m_StateMachine.GetIntegerValue(paramId); }
        public float GetParamFloat(int paramId) { return m_StateMachine.GetFoatValue(paramId); }

        // enum solution
        public void RegisterTriggerParam(Enum paramId) { m_StateMachine.RegisterTriggerParam(Convert.ToInt32(paramId), paramId.ToString()); }
        public void RegisterParam(Enum paramId, bool value) { m_StateMachine.RegisterParam(Convert.ToInt32(paramId), value, paramId.ToString()); }
        public void RegisterParam(Enum paramId, int value) { m_StateMachine.RegisterParam(Convert.ToInt32(paramId), value, paramId.ToString()); }
        public void RegisterParam(Enum paramId, float value) { m_StateMachine.RegisterParam(Convert.ToInt32(paramId), value, paramId.ToString()); }

        #endregion

        StateMachine m_StateMachine;

        // Update is called once per frame
        void Update() { m_StateMachine.Update(); }

        StateMachineManager Init()
        {
            m_StateMachine = new StateMachine();
            return this;
        }
    }
}
