using System.Collections.Generic;
using UnityEngine;

namespace ADikt.StateMachine
{
    public class ParametersController<T>
    {
#if UNITY_EDITOR
        public Dictionary<int, T> _editor_Dictionary { get { return m_Values; } }
        public string _Editor_GetParamName(int paramId)
        {
            return m_Names[paramId];
        }
#endif

        Dictionary<int, T> m_InitValues;
        Dictionary<int, T> m_Values;
        Dictionary<int, string> m_Names;
        string m_ValueTypeName;
        T m_DefaultValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ADikt.StateMachine.ParametersController`1"/> class.
        /// </summary>
        /// <param name="valueTypeName">Value type name.</param>
        /// <param name="defaultValue">Default value.</param>
        public ParametersController(string valueTypeName, T defaultValue)
        {
            m_InitValues = new Dictionary<int, T>();
            m_Values = new Dictionary<int, T>();
            m_Names = new Dictionary<int, string>();
            m_ValueTypeName = valueTypeName;
            m_DefaultValue = defaultValue;
        }

       /// <summary>
       /// Registers the parameter.
       /// </summary>
       /// <param name="id">Identifier.</param>
       /// <param name="initValue">Init value.</param>
       /// <param name="name">Name.</param>
        public void RegisterParam(int id, T initValue, string name)
        {
            if (m_Values.ContainsKey(id))
            {
                Debug.LogWarningFormat("{0} param with id {1} already exists.", m_ValueTypeName, id);
                return;
            }

            m_InitValues[id] = initValue;
            m_Values[id] = initValue;
            m_Names[id] = name;
        }

        /// <summary>
        /// Initialises all parameters.
        /// </summary>
        public void InitialiseAllParams ()
        {
            if(m_Values == null)
                return;

            foreach (KeyValuePair<int, T> pair in m_InitValues)
                SetParam(pair.Key, pair.Value);
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="value">Value.</param>
        public void SetParam(int id, T value)
        {
            if (!m_Values.ContainsKey(id))
            {
                Debug.LogWarningFormat("{0} param with id {1} does not exists.", m_ValueTypeName, id);
                return;
            }

            m_Values[id] = value;
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <returns>The parameter.</returns>
        /// <param name="id">Identifier.</param>
        public T GetParam(int id)
        {
            if (!m_Values.ContainsKey(id))
            {
                Debug.LogWarningFormat("{0} param with id {0} does not exists.", m_ValueTypeName, id);
                return m_DefaultValue;
            }

            return m_Values[id];
        }
    }
}
