using UnityEngine;

namespace ADikt.StateMachine
{
    /// <summary>
    /// Delay state.
    /// { duration:float, paramId:int }
    /// </summary>
    public class DelayState : State
    {
        float m_Timer;
        float m_Duration;
        int m_TriggerParamId;

        public override void OnEnter()
        {
            m_Duration = (float)payload[0];
            m_TriggerParamId = (int)payload[1];
            m_Timer = 0;
        }

        public override void OnUpdate()
        {
            if (m_Timer > m_Duration)
                SetTriggerParam(m_TriggerParamId);

            m_Timer += Time.deltaTime;
        }
    }
}
