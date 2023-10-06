using System.Collections.Generic;

namespace ADikt.StateMachine
{
    public partial class StateMachine
    {
        private void InitialiseAllParams ()
        {
            m_TriggerValuesController.InitialiseAllParams(); // maybe we do no this for triggers (?)
            m_BoolValuesController.InitialiseAllParams();
            m_IntegerValuesController.InitialiseAllParams();
            m_FloatValuesController.InitialiseAllParams();
        }

#if UNITY_EDITOR
        public ParametersController<bool> _editor_TriggerValuesController { get { return m_TriggerValuesController; } }
        public ParametersController<bool> _editor_BoolValuesController { get { return m_BoolValuesController; } }
        public ParametersController<int> _editor_IntegerValuesController { get { return m_IntegerValuesController; } }
        public ParametersController<float> _editor_FloatValuesController { get { return m_FloatValuesController; } }
#endif

        #region Trigger Values
        ParametersController<bool> m_TriggerValuesController = new ParametersController<bool>("Trigger", false);
        List<int> m_DirtyTriggerValuesList = new List<int>();

        public void RegisterTriggerParam(int id, string name)
        {
            m_TriggerValuesController.RegisterParam(id, false, name);
        }

        public void SetTriggerParam(int id)
        {
            m_TriggerValuesController.SetParam(id, true);

            m_DirtyTriggerValuesList.Add(id);

            if (m_CurrentState != null)
                m_CurrentState.OnValueChange(id, true);
        }

        public bool GetTriggerValue (int id)
        {
            return m_TriggerValuesController.GetParam(id);
        }

        private void ClearDirtyTriggerParams ()
        {
            if (m_DirtyTriggerValuesList.Count == 0)
                return;

            for (int i = 0; i < m_DirtyTriggerValuesList.Count; i++)
                m_TriggerValuesController.SetParam(m_DirtyTriggerValuesList[i], false);

            m_DirtyTriggerValuesList.Clear();
        }
        #endregion

        #region Bool Values
        ParametersController<bool> m_BoolValuesController = new ParametersController<bool>("Bool", false);

        public void RegisterParam(int id, bool initValue, string name)
        {
            m_BoolValuesController.RegisterParam(id, initValue, name);
        }

        public void SetParam(int id, bool value)
        {
            m_BoolValuesController.SetParam(id, value);

            if (m_CurrentState != null)
                m_CurrentState.OnValueChange(id, value);
        }

        public bool GetBoolValue(int id)
        {
            return m_BoolValuesController.GetParam(id);
        }
        #endregion

        #region Integer Values
        ParametersController<int> m_IntegerValuesController = new ParametersController<int>("Integer", 0);

        public void RegisterParam(int id, int initValue, string name)
        {
            m_IntegerValuesController.RegisterParam(id, initValue, name);
        }

        public void SetParam(int id, int value)
        {
            m_IntegerValuesController.SetParam(id, value);

            if (m_CurrentState != null)
                m_CurrentState.OnValueChange(id, value);
        }

        public int GetIntegerValue(int id)
        {
            return m_IntegerValuesController.GetParam(id);
        }
        #endregion

        #region Float Values
        ParametersController<float> m_FloatValuesController = new ParametersController<float>("Float", 0f);

        public void RegisterParam(int id, float initValue, string name)
        {
            m_FloatValuesController.RegisterParam(id, initValue, name);
        }

        public void SetParam(int id, float value)
        {
            m_FloatValuesController.SetParam(id, value);

            if (m_CurrentState != null)
                m_CurrentState.OnValueChange(id, value);
        }

        public float GetFoatValue(int id)
        {
            return m_FloatValuesController.GetParam(id);
        }
        #endregion
    }
}
