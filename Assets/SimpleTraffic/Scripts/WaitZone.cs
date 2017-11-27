using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic
{
	public class WaitZone : MonoBehaviour 
	{
		// -------------------------------------------------------------------
		// Properties

		public TrafficType type;
		public WaitZone opposite;

		// -------------------------------------------------------------------
		// State
		
		private bool m_CanPass = false;

		public bool canPass 
		{ 
			get {return m_CanPass;} 
			set {m_CanPass = value;}
		}
	}
}