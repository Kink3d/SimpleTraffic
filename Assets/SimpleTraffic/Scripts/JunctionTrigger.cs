using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic
{
	public class JunctionTrigger : MonoBehaviour 
	{
		// -------------------------------------------------------------------
		// Enum

		public enum TriggerType { Enter, Exit }

		// -------------------------------------------------------------------
		// Properties

		public TriggerType type;

		// -------------------------------------------------------------------
		// Junction

		private Junction m_Junction;
		public Junction junction 
		{
			get { return m_Junction; }
			set { m_Junction = value; }
		}

		public void TriggerJunction()
		{
            junction.TryChangePhase();
		}
	}
}
