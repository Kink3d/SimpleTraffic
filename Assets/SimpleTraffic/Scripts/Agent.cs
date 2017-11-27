using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Kawaiiju.Traffic;

namespace Kawaiiju
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class Agent : MonoBehaviour 
	{
		private NavMeshAgent m_Agent;
		public NavMeshAgent agent
		{
			get
			{
				if(!m_Agent)
					m_Agent = GetComponent<NavMeshAgent>();
				return m_Agent;
			}
		}

		// -------------------------------------------------------------------
		// Properties

		[Header("Agent")]
		public TrafficType type = TrafficType.Pedestrian;
		public int maxSpeed = 20;

		// -------------------------------------------------------------------
		// Initialization

		public void Initialize()
		{
			agent.enabled = true;
			speed = TrafficSystem.Instance.GetAgentSpeedFromKPH(maxSpeed);
			agent.speed = speed;
			m_Destination = TrafficSystem.Instance.GetPedestrianDestination();
			if(m_Destination)
				agent.destination = m_Destination.position;
		}

		// -------------------------------------------------------------------
		// Update

		float m_Speed;
		public float speed
		{
			get { return m_Speed; }
			set { m_Speed = value; }
		}
		Transform m_Destination;

		public virtual void Update()
		{
			if(agent.isOnNavMesh)
			{
				if(CheckStop())
					agent.velocity = Vector3.zero;
				CheckWaitZone();
				if(type == TrafficType.Pedestrian)
					TestDestination();
			}
		}

		private void TestDestination()
		{
			if(m_Destination)
			{
				float distanceToDestination = Vector3.Distance(transform.position, m_Destination.position);
				if(distanceToDestination < 1f)
				{
					m_Destination = TrafficSystem.Instance.GetPedestrianDestination();
					agent.destination = m_Destination.position;
				}
			}
		}

		// -------------------------------------------------------------------
		// Collisions

		public virtual void OnTriggerEnter(Collider col)
		{
			if(col.tag == "WaitZone")
			{
				WaitZone waitZone = col.GetComponent<WaitZone>();
				if(waitZone.type == type)
				{
					if(type == TrafficType.Pedestrian)
					{
						if(CheckOppositeWAitZone(waitZone))
							return;
					}
					m_CurrentWaitZone = waitZone;
					if(!waitZone.canPass)
						m_IsWaiting = true;
				}
			}
		}

		private bool CheckOppositeWAitZone(WaitZone waitZone)
		{
			if(waitZone.opposite)
			{
				if(waitZone.opposite == m_CurrentWaitZone)
					return true;
			}
			return false;
		}

		// -------------------------------------------------------------------
		// WaitZone

		WaitZone m_CurrentWaitZone;

		private bool m_IsWaiting;
		public bool isWaiting { get { return m_IsWaiting; }}

		private void CheckWaitZone()
		{
			if(m_IsWaiting)
			{
				if(m_CurrentWaitZone)
					m_IsWaiting = !m_CurrentWaitZone.canPass;
			}
		}

		public virtual bool CheckStop()
		{
			if(isWaiting)
				return true;
			return false;
		}

		// -------------------------------------------------------------------
		// Debug

		public virtual void OnDrawGizmos()
		{
			
		}
	}
}
