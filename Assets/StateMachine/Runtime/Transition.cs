namespace ADikt.StateMachine
{
    public struct Transition
    {
        public string sourceState { get; private set; }
        public string targetState { get; private set; }
        Condition[] m_Conditions;

        public Transition(string sourceState, string targetState, params Condition[] conditions)
        {
            this.sourceState = sourceState;
            this.targetState = targetState;
            m_Conditions = conditions;
        }

        public Transition(string sourceState, params Condition[] conditions)
        {
            this.sourceState = sourceState;
            this.targetState = null;
            m_Conditions = conditions;
        }

        public Transition(string sourceState, string targetState)
        {
            this.sourceState = sourceState;
            this.targetState = targetState;
            m_Conditions = null;
        }

        public bool isAnyTransition
        {
            get { return sourceState == null; }
        }

        public bool isExitTransition
        {
            get { return targetState == null; }
        }

        public bool Evaluate(StateMachine stateMachine)
        {
            if (m_Conditions == null)
                return true;

            for (int i = 0; i < m_Conditions.Length; i++)
            {
                if (!m_Conditions[i].Evaluate(stateMachine))
                    return false;
            }

            return true;
        }
    }
}
