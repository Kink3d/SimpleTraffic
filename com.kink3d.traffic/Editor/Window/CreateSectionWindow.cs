using System.Linq;
using UnityEngine;
using UnityEditor;
using kTools.Traffic;

namespace kTools.TrafficEditor
{
    public class CreateSectionWindow : EditorWindow
    {
#region Styles
        internal static class Styles
        {
            internal static string noSectionResourceText = 
@"No Section Resources set on Traffic System. 
Set a Section Resource asset to place sections.";
        }
#endregion

#region Data
        private static Vector2 s_WindowSize = new Vector2(340, 84); 
        private TrafficSystem m_Target;
        private int m_SectionIndex;
#endregion

#region Initialization
        public void Init(TrafficSystem target)
        {
            // Set data
            m_Target = target;

            // Initialize Window
            titleContent = new GUIContent("Create Section");
            minSize = s_WindowSize;
            maxSize = s_WindowSize;
            Show();
        }
#endregion

#region GUI
        private void OnGUI()
        {
            // If TrafficSystem is null return
            if(m_Target == null)
                return;

            // If TrafficSystem has no SectionResources set display error and return
            SectionResources sectionResources = m_Target.sectionResources;
            if(sectionResources == null)
            {
                EditorGUILayout.HelpBox(Styles.noSectionResourceText, MessageType.Error);
                return;
            }

            // Build Popup from available Sections in SectionResources
            string[] sections = sectionResources.sections.Select(x => x.name).ToArray();
            m_SectionIndex = EditorGUILayout.Popup(m_SectionIndex, sections);

            // Create Section button
            if(GUILayout.Button("Create"))
                Create();
        }

        private void Create()
        {
            Section section = m_Target.sectionResources.sections[m_SectionIndex];
            m_Target.CreateSection(section);
            Close();
        }
#endregion
    }
}
