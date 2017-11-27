using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Kawaiiju.Traffic
{
	public enum TrafficType { Pedestrian, Vehicle }

	public class TrafficSystem : MonoBehaviour 
	{
		// -------------------------------------------------------------------
		// Singleton
		private static TrafficSystem _Instance;
		public static TrafficSystem Instance
		{
			get
			{
				if(_Instance == null)
					_Instance = FindObjectOfType<TrafficSystem>();
				return _Instance;
			}
		}

		// -------------------------------------------------------------------
		// Properties

		public bool drawGizmos = false;
		public GameObject pedestrianPrefab;	// TODO - Get from object pool
		public GameObject vehiclePrefab; // TODO - Get from object pool
		public GameObject trainPrefab; // TODO - Get from object pool
		public GameObject trainCarriagePrefab; // TODO - Get from object pool
		public Transform pool;	// TODO - Get from object pool
		public bool spawnOnStart = true;
		public int maxRoadVehicles = 100;
		public int maxTrains = 5;
		public int maxPedestrians = 100;

		// -------------------------------------------------------------------
		// Initialization

		private List<Road> m_Roads = new List<Road>();
		private List<Track> m_Tracks = new List<Track>();

		private void Start () 
		{
			Road[] roadsFound = FindObjectsOfType<Road>();
			foreach(Road r in roadsFound)
				m_Roads.Add(r);
			Track[] tracksFound = FindObjectsOfType<Track>();
			foreach(Track t in tracksFound)
				m_Tracks.Add(t);

			if(spawnOnStart)
			{
				for(int i = 0; i < maxRoadVehicles; i++)
					SpawnRoadVehicle(true);
				for(int i = 0; i < maxTrains; i++)
					SpawnTrain(true);
				for(int i = 0; i < maxPedestrians; i++)
					SpawnPedestrian(true);
			}
		}

		// -------------------------------------------------------------------
		// Update

		private void Update()
		{
			if(Input.GetKeyUp(KeyCode.Backspace))
				SpawnPedestrian(true);
			if(Input.GetKeyUp(KeyCode.Return))
				SpawnRoadVehicle(true);
			if(Input.GetKeyUp(KeyCode.RightShift))
				SpawnTrain(true);
		}

		// -------------------------------------------------------------------
		// Spawn

		private int m_RoadVehicleSpawnAttempts;
		private int m_TrainSpawnAttempts;
		private int m_PedestrianSpawnAttempts;

		private void SpawnRoadVehicle(bool reset)
		{
			if(reset)
				m_RoadVehicleSpawnAttempts = 0;
			int index = UnityEngine.Random.Range(0, m_Roads.Count);
			Road road = m_Roads[index];
			VehicleSpawn spawn;
			if(!road.TryGetVehicleSpawn(out spawn))
			{
				m_RoadVehicleSpawnAttempts++;
				if(m_RoadVehicleSpawnAttempts < m_Roads.Count)
					SpawnRoadVehicle(false);
				return;
			}
			Vehicle newVehicle = Instantiate(vehiclePrefab, spawn.spawn.position, spawn.spawn.rotation, pool.transform).GetComponent<Vehicle>();
			newVehicle.Initialize(road, spawn.destination);
		}

		private void SpawnTrain(bool reset)
		{
			if(reset)
				m_TrainSpawnAttempts = 0;
			int index = UnityEngine.Random.Range(0, m_Tracks.Count);
			Track track = m_Tracks[index];
			VehicleSpawn spawn;
			if(!track.TryGetVehicleSpawn(out spawn))
			{
				m_TrainSpawnAttempts++;
				if(m_TrainSpawnAttempts < m_Tracks.Count)
					SpawnRoadVehicle(false);
				return;
			}
			Train newTrain = Instantiate(trainPrefab, spawn.spawn.position, spawn.spawn.rotation, pool.transform).GetComponent<Train>();
			newTrain.Initialize(track, spawn.destination);
		}

		private void SpawnPedestrian(bool reset)
		{
			if(reset)
				m_PedestrianSpawnAttempts = 0;
			int index = UnityEngine.Random.Range(0, m_Roads.Count);
			Road road = m_Roads[index];
			Transform spawn;
			if(!road.TryGetPedestrianSpawn(out spawn))
			{
				m_PedestrianSpawnAttempts++;
				if(m_PedestrianSpawnAttempts < m_Roads.Count)
					SpawnPedestrian(false);
				return;
			}
			Agent newAgent = Instantiate(pedestrianPrefab, spawn.position, spawn.rotation, pool.transform).GetComponent<Agent>();
			newAgent.Initialize();
		}

		// -------------------------------------------------------------------
		// Navigation

		public Transform GetPedestrianDestination()
		{
			int index = UnityEngine.Random.Range(0, m_Roads.Count);
			Road road = m_Roads[index];
			Transform destination;
			if(!road.TryGetPedestrianSpawn(out destination))
			{
				return GetPedestrianDestination();
			}
			return destination;
		}

		// -------------------------------------------------------------------
		// Static

		public float GetAgentSpeedFromKPH(int kph)
		{
			return kph * .02f;
		}
	}
}

