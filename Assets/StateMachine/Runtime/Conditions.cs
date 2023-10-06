using System;

namespace ADikt.StateMachine
{
    public class Condition
    {
        public enum IntegerComparisonType
        {
            Greater,
            Less,
            Equals,
            NotEquals
        }

        public enum FloatComparisonType
        {
            Greater,
            Less
        }

        protected int m_ParamId;

        public Condition(int paramId)
        {
            m_ParamId = paramId;
        }

        public Condition(Enum paramId)
        {
            m_ParamId = Convert.ToInt32(paramId);
        }

        public virtual bool Evaluate(StateMachine stateMachine)
        {
            return false;
        }
    }

    public class TriggerCondition : Condition
    {
        public TriggerCondition(int paramId) : base(paramId) { }
        public TriggerCondition(Enum paramId) : base(paramId) { }

        public override bool Evaluate(StateMachine stateMachine)
        {
            return stateMachine.GetTriggerValue(m_ParamId);
        }
    }

    public class BoolCondition : Condition
    {
        private readonly bool m_ComparisonValue;

        public BoolCondition(int paramId, bool comparisonValue) : base(paramId)
        {
            m_ComparisonValue = comparisonValue;
        }
        public BoolCondition(Enum paramId, bool comparisonValue) : base(paramId)
        {
            m_ComparisonValue = comparisonValue;
        }

        public override bool Evaluate(StateMachine stateMachine)
        {
            return m_ComparisonValue == stateMachine.GetBoolValue(m_ParamId);
        }
    }

    public class IntegerCondition : Condition
    {
        private readonly IntegerComparisonType m_ComparisonType;
        private readonly int m_ComparisonValue;

        public IntegerCondition(int paramId, IntegerComparisonType comparisonType, int comparisonValue) : base(paramId)
        {
            m_ComparisonType = comparisonType;
            m_ComparisonValue = comparisonValue;
        }
        public IntegerCondition(Enum paramId, IntegerComparisonType comparisonType, int comparisonValue) : base(paramId)
        {
            m_ComparisonType = comparisonType;
            m_ComparisonValue = comparisonValue;
        }

        public override bool Evaluate(StateMachine stateMachine)
        {
            switch (m_ComparisonType)
            {
                case IntegerComparisonType.Greater:
                    return m_ComparisonValue > stateMachine.GetIntegerValue(m_ParamId);
                case IntegerComparisonType.Less:
                    return m_ComparisonValue < stateMachine.GetIntegerValue(m_ParamId);
                case IntegerComparisonType.Equals:
                    return m_ComparisonValue == stateMachine.GetIntegerValue(m_ParamId);
                case IntegerComparisonType.NotEquals:
                    return m_ComparisonValue != stateMachine.GetIntegerValue(m_ParamId);
            }

            return false;
        }
    }

    public class FloatCondition : Condition
    {
        private readonly FloatComparisonType m_ComparisonType;
        private readonly float m_ComparisonValue;

        public FloatCondition(int paramId, FloatComparisonType comparisonType, float comparisonValue) : base(paramId)
        {
            m_ComparisonType = comparisonType;
            m_ComparisonValue = comparisonValue;
        }
        public FloatCondition(Enum paramId, FloatComparisonType comparisonType, float comparisonValue) : base(paramId)
        {
            m_ComparisonType = comparisonType;
            m_ComparisonValue = comparisonValue;
        }

        public override bool Evaluate(StateMachine stateMachine)
        {
            switch (m_ComparisonType)
            {
                case FloatComparisonType.Greater:
                    return m_ComparisonValue > stateMachine.GetFoatValue(m_ParamId);
                case FloatComparisonType.Less:
                    return m_ComparisonValue < stateMachine.GetFoatValue(m_ParamId);
            }

            return false;
        }
    }
}