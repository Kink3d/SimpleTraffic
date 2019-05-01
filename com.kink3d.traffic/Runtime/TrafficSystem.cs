using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace kTools.Traffic
{
    [ExecuteAlways]
    public class TrafficSystem : MonoBehaviour
    {
#region Properties
        public SectionResources sectionResources => m_SectionResources;
#endregion

#region Data
        [SerializeField] private SectionResources m_SectionResources;
        [SerializeField] private List<Section> m_Sections;
        private bool m_IsDirty;

        [SerializeField] private Dictionary<Transform, Transform> m_ValidConnections;
        [SerializeField] private List<Transform> m_InvalidConnectors;
#endregion

#region Initialization
#if UNITY_EDITOR
        [MenuItem("GameObject/kTools/Traffic System", false, 10)]
        static void CreateTrafficSystem(MenuCommand menuCommand)
        {
            // Add a menu item for creating Traffic Systems
            // Create a new Traffic System
            // Parent, register undo and select
            TrafficSystem trafficSystem = TrafficSystem.Create();
            GameObjectUtility.SetParentAndAlign(trafficSystem.gameObject, menuCommand.context as GameObject);
        }

        /// <summary>
        /// Create a new Traffic System.
        /// </summary>
        public static TrafficSystem Create()
        {
            // Create a new Traffic System object
            GameObject go = new GameObject("Traffic System", typeof(TrafficSystem));
            Undo.RegisterCreatedObjectUndo(go, "Create Traffic System");
            Selection.activeObject = go;

            // Initiailize Traffic System
            TrafficSystem trafficSystem = go.GetComponent<TrafficSystem>();
            trafficSystem.Init();

            // Finalise
            return trafficSystem;
        }

        private void Init()
        {
            // Initiailize collections
            m_Sections = new List<Section>();
            m_ValidConnections = new Dictionary<Transform, Transform>();
            m_InvalidConnectors = new List<Transform>();
        }
#endif
#endregion

#region State
        /// <summary>
        /// Instruct the Traffic System to validate on the next Update.
        /// </summary>
        public void SetDirty()
        {
            // Set the TafficSystem as dirty
            // Force validation on next Update
            m_IsDirty = true;
        }
#endregion

#region Update
        private void Update()
        {
#if UNITY_EDITOR
            if(m_IsDirty)
            {
                // If TafficSystem was set dirty on last Update
                // Reset dirty flag and validate
                m_IsDirty = false;
                Validate();
            }
#endif
        }
#endregion

#region Validation
        private void Validate()
        {
            // Clear collections
            m_ValidConnections.Clear();
            m_InvalidConnectors.Clear();

            // First get a list of all connectors from Sections
            List<Transform> m_AllConnectors = new List<Transform>();
            foreach(Section section in m_Sections)
            {
                foreach(Transform connector in section.connectors)
                    m_AllConnectors.Add(connector);
            }

            // Use a for loop as we need to remove entries from the list
            // as we verify they are already in the Dictionary
            for(int i = 0; i < m_AllConnectors.Count; i++)
            {
                Transform connector = m_AllConnectors[i];

                // If we already have this connector as part of another pair
                // Remove it from the list and continue
                if(m_ValidConnections.Values.Contains(connector))
                {
                    m_AllConnectors.Remove(connector);
                    continue;
                }
                
                // Attempt to get another connector from the list
                // Where position matches but is not the same Transform
                Transform[] connections = m_AllConnectors
                    .Where(x => !x.Equals(connector))
                    .Where(x => x.transform.position == connector.position)
                    .ToArray();
                Transform connection = connections.Length > 0 ? connections[0] : null;

                // If not valid connection was found this connection is invalid
                // Leave it in the list and continue
                if(connection == null)
                    continue;

                // If we have a valid connection then add to Dictionary
                // Remove both Transforms from the list
                m_ValidConnections.Add(connector, connection);
                m_AllConnectors.Remove(connector);
                m_AllConnectors.Remove(connection);
            }

            // If we still have invalid connectors track them
            if(m_AllConnectors.Count > 0)
                m_InvalidConnectors = m_AllConnectors;
        }
#endregion

#region Sections
        public void CreateSection(Section section)
        {
            // Create a new insatnce of the Section and track it
            // Parent the instance to the TafficSystem
            // Name to instance with list index and definition name
            Section instance = Instantiate(section, Vector3.zero, Quaternion.identity);
            instance.transform.SetParent(transform);
            instance.gameObject.name = string.Format("Section{0}_{1}", m_Sections.Count, section.name);
            m_Sections.Add(instance);

            // Initiailize the new instance
            // Always validate when creating new instances
            instance.Init(this);
            Validate();
        }
#endregion

#region Gizmos
#if UNITY_EDITOR
        private void OnDrawGizmos()
		{
            if(m_ValidConnections == null)
                return;

            if(m_InvalidConnectors == null)
                return;

            // Draw valid connections
            foreach(KeyValuePair<Transform, Transform> validConnection in m_ValidConnections)
            {
                // Draw sphere gizmo at connection position
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(validConnection.Key.position, 0.1f);
            }

            // Draw invalid connectors
            foreach(Transform invalidConnector in m_InvalidConnectors)
            {
                // Draw sphere gizmo at connector position
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(invalidConnector.position, 0.1f);
            }
		}
#endif
#endregion
    }
}
