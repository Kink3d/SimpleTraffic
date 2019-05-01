using UnityEngine;
using UnityEditor;
using kTools.Traffic;

namespace kTools.TrafficEditor
{
    [CustomEditor(typeof(TrafficSystem))]
    public class TrafficSystemEditor : Editor
    {
#region Styles
		internal class Styles
        {
            public static GUIContent propertiesText = EditorGUIUtility.TrTextContent("Properties");
            public static GUIContent sectionResourceText = EditorGUIUtility.TrTextContent("Section Resources", "Scriptable Object that defines Sections that can be placed.");
            public static GUIContent toolsText = EditorGUIUtility.TrTextContent("Tools");
            public static GUIContent createSectionText = EditorGUIUtility.TrTextContent("Create Section", "Open the Create Section window to create new Sections.");
        }
#endregion

#region Serialized Properties
        SerializedProperty m_SectionResourcesProp;
#endregion

#region Data
		TrafficSystem m_ActualTarget;
#endregion

#region Initializtion
		private void OnEnable()
        {
			m_SectionResourcesProp = serializedObject.FindProperty("m_SectionResources");
        }
#endregion

#region InspectorGUI
		public override void OnInspectorGUI()
		{
            // Get target
			m_ActualTarget = (TrafficSystem)target;

			serializedObject.Update();

            // Draw Properties
            EditorGUILayout.LabelField(Styles.propertiesText, EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(m_SectionResourcesProp, Styles.sectionResourceText);
            EditorGUILayout.Space();

            // Draw Tools
            EditorGUILayout.LabelField(Styles.toolsText, EditorStyles.boldLabel);
            if(GUILayout.Button(Styles.createSectionText))
			{
                CreateSectionWindow window = (CreateSectionWindow)EditorWindow.GetWindow(typeof(CreateSectionWindow));
				window.Init(m_ActualTarget);
            }

			serializedObject.ApplyModifiedProperties();
		}
#endregion
    }
}
