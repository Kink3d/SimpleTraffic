using UnityEngine;
using System.Collections.Generic;

namespace kTools.Traffic
{
    [CreateAssetMenu(fileName = "SectionResources", menuName = "kTools/kTraffic/SectionResources", order = 1)]
    public class SectionResources : ScriptableObject 
    {
#region Properties
        public List<Section> sections;
#endregion
    }
}