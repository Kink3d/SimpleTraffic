using UnityEngine;

namespace kTools.Traffic
{
    [ExecuteAlways]
    public class Section : MonoBehaviour
    {
#region Properties
        public Transform[] connectors;
#endregion

#region Data
        [SerializeField] private TrafficSystem m_System;
        private Vector3 m_PreviousPosition;
        private Quaternion m_PreviousRotation;
        private Vector3 m_PreviousScale;
#endregion

#region Initialization
        public void Init(TrafficSystem system)
        {
            // Initiailize a Section instance
            m_System = system;
        }
#endregion

#region Update
        private void Update()
        {
            // If System is null the instance was created in error
            if(m_System == null)
                return;
            
#if UNITY_EDITOR
            // If in Editor and not in Play mode
            if(!Application.isPlaying)
            {
                // If Transform has been edited in this Update
                // Set the TrafficSystem as dirty
                if(IsTransformUpdated())
                {
                    m_System.SetDirty();

                    // Update previous transforms
                    m_PreviousPosition = transform.position;
                    m_PreviousRotation = transform.rotation;
                    m_PreviousScale = transform.localScale;
                }
            }
#endif
        }

        private bool IsTransformUpdated()
        {
            // Calculate if any Transform values have been altered this Update.
            bool isMoved = !transform.position.Equals(m_PreviousPosition);
            bool isRotated = !transform.rotation.Equals(m_PreviousRotation);
            bool isScaled = !transform.localScale.Equals(m_PreviousScale);
            return isMoved || isRotated || isScaled;
        }
#endregion   
    }
}
