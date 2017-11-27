using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic
{
	public class Road : NavSection 
	{
		// -------------------------------------------------------------------
		// Properties

		[Header("Road")]
		public Transform[] pedestrianSpawns;

		// -------------------------------------------------------------------
		// Get Data

		public bool TryGetPedestrianSpawn(out Transform spawn)
		{
			if(pedestrianSpawns.Length > 0)
			{
				int index = UnityEngine.Random.Range(0, pedestrianSpawns.Length);
				spawn = pedestrianSpawns[index];
				return true;
			}
			spawn = null;
			return false;
		}
	}
}
