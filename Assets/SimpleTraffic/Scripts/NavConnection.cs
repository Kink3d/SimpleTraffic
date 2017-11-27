using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic
{
	[RequireComponent(typeof(Collider))]
	public class NavConnection : MonoBehaviour 
	{
		private NavSection m_NavSection;
		public NavSection navSection 
		{
			get { return m_NavSection; }
			set { m_NavSection = value; }
		}

		// -------------------------------------------------------------------
		// Properties

		public NavConnection[] outConnections;

		// -------------------------------------------------------------------
		// Get Data

		public NavConnection GetOutConnection()
		{
			if(outConnections.Length > 0)
			{
				int index = UnityEngine.Random.Range(0, outConnections.Length);
				return outConnections[index];
			}
			return null;
		}

		// -------------------------------------------------------------------
		// Debug

		private void OnDrawGizmos()
		{
			if(TrafficSystem.Instance.drawGizmos)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawSphere(transform.position - new Vector3(0, transform.localScale.y * 0.5f, 0), 0.05f);
			}
		}
	}
}

