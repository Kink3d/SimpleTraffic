using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic
{
	public class Train : Vehicle 
	{
		// -------------------------------------------------------------------
		// Properties

		[Header("Train")]
		public int carriageCount = 2;

		// -------------------------------------------------------------------
		// Initialization

		public override void Initialize(NavSection navSection, NavConnection destination)
		{
			for(int i = 0; i < carriageCount; i++)
				SpawnCarriage(navSection, destination, i + 1);
			base.Initialize(navSection, destination);
		}

        public override void RegisterVehicle(NavSection section, bool isAdd)
        {
            if (isAdd && m_CarriageIndex == 0)
                base.RegisterVehicle(section, isAdd);
            else if (!isAdd && m_CarriageIndex == m_MaxCarriages)
                m_Engine.ForceUnregisterVehicle(section);      
        }

        public void ForceUnregisterVehicle(NavSection section)
        {
            base.RegisterVehicle(section, false);
        }

        // -------------------------------------------------------------------
        // Carriages

        private Train m_Engine;
        private int m_CarriageIndex = 0;
		private int m_MaxCarriages = 2;

		private void SpawnCarriage(NavSection navSection, NavConnection destination, int carriage)
		{
			Vector3 pos = transform.TransformPoint(0, 0, -carriage * 1.05f);
			Train newCarriage = Instantiate(TrafficSystem.Instance.trainCarriagePrefab, pos, transform.rotation, transform).GetComponent<Train>();
            newCarriage.transform.localScale = Vector3.one;
			newCarriage.InitializeCarriage(navSection, destination, this, carriage, carriageCount);
		}

		public void InitializeCarriage(NavSection navSection, NavConnection destination, Train engine, int carriage, int maxCarriages)
		{
            m_Engine = engine;
			m_CarriageIndex = carriage;
			m_MaxCarriages = maxCarriages;
			blockedDistance = 0.1f;
			Initialize(navSection, destination);
		}

		// -------------------------------------------------------------------
		// Collisions

		public override void OnTriggerEnter(Collider col)
		{
			if(col.tag == "TrainJunction")
			{
				JunctionTrigger trigger = col.GetComponent<JunctionTrigger>();
				if(m_CarriageIndex == 0 && trigger.type == JunctionTrigger.TriggerType.Enter)
					trigger.TriggerJunction();
				else if(m_CarriageIndex == m_MaxCarriages && trigger.type == JunctionTrigger.TriggerType.Exit)
					trigger.TriggerJunction();
			}
			base.OnTriggerEnter(col);
		}
	}
}
