using UnityEditor;
using System.Collections.Generic;

namespace ADikt.StateMachine
{
    [CustomEditor(typeof(StateMachineManager))]
    public class StateMachineInspector : Editor
    {
        StateMachineManager m_Target;

        public override void OnInspectorGUI()
        {
            m_Target = (StateMachineManager)target;
            //base.OnInspectorGUI();

            EditorGUILayout.LabelField("Status", m_Target.status.ToString());
            EditorGUILayout.LabelField("Current State", m_Target.currentState);

            EditorGUILayout.Space();

            if (m_Target.editor_StateMachine._editor_TriggerValuesController != null
                && m_Target.editor_StateMachine._editor_TriggerValuesController._editor_Dictionary.Count > 0)
            {
                EditorGUILayout.LabelField("Trigger Params");
                foreach (KeyValuePair<int, bool> pair in m_Target.editor_StateMachine._editor_TriggerValuesController._editor_Dictionary)
                {
                    EditorGUILayout.LabelField("id: " + pair.Key.ToString() + ", "
                        + m_Target.editor_StateMachine._editor_TriggerValuesController._Editor_GetParamName(pair.Key)
                    , "(trigger)");
                }
            }

            if (m_Target.editor_StateMachine._editor_BoolValuesController != null
                && m_Target.editor_StateMachine._editor_BoolValuesController._editor_Dictionary.Count > 0)
            {
                EditorGUILayout.LabelField("Bool Params");
                foreach (KeyValuePair<int, bool> pair in m_Target.editor_StateMachine._editor_BoolValuesController._editor_Dictionary)
                {
                    EditorGUILayout.LabelField("id: " + pair.Key.ToString() + ", "
                        + m_Target.editor_StateMachine._editor_BoolValuesController._Editor_GetParamName(pair.Key)
                    , pair.Value.ToString());
                }
            }


            if (m_Target.editor_StateMachine._editor_IntegerValuesController != null
                && m_Target.editor_StateMachine._editor_IntegerValuesController._editor_Dictionary.Count > 0)
            {
                EditorGUILayout.LabelField("Int Params");
                foreach (KeyValuePair<int, int> pair in m_Target.editor_StateMachine._editor_IntegerValuesController._editor_Dictionary)
                {
                    EditorGUILayout.LabelField("id: " + pair.Key.ToString() + ", "
                        + m_Target.editor_StateMachine._editor_IntegerValuesController._Editor_GetParamName(pair.Key)
                    , pair.Value.ToString());
                }
            }

            if (m_Target.editor_StateMachine._editor_FloatValuesController != null 
                && m_Target.editor_StateMachine._editor_FloatValuesController._editor_Dictionary.Count > 0 )
            {
                EditorGUILayout.LabelField("Float Params");
                foreach (KeyValuePair<int, float> pair in m_Target.editor_StateMachine._editor_FloatValuesController._editor_Dictionary)
                {
                    EditorGUILayout.LabelField("id: " + pair.Key.ToString() + ", "
                        + m_Target.editor_StateMachine._editor_FloatValuesController._Editor_GetParamName(pair.Key)
                    , pair.Value.ToString());
                }
            }

            if (EditorApplication.isPlaying)
                Repaint();
        }
    }
}
