using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[System.Serializable]
	public struct GameNPCs
	{
		public GameObject m_healthyPrefab;
		public int m_healthiesStartCount;
		public GameObject m_zombiePrefab;
		public int m_zombiesStartCount;

		[HideInInspector]
		public GameObject[] m_healthiesNPCList;
		[HideInInspector]
		public GameObject[] m_healthiesActiveList;
		[HideInInspector]
		public GameObject[] m_healthiesInactiveList;
		[HideInInspector]
		public GameObject[] m_zombiesNPCList;
		[HideInInspector]
		public GameObject[] m_zombiesActiveList;
		[HideInInspector]
		public GameObject[] m_zombiesInactiveList;
	};


	public GameNPCs m_NPCsDefinition;

	private void Awake()
	{
		int NPCsCount = m_NPCsDefinition.m_healthiesStartCount + m_NPCsDefinition.m_zombiesStartCount;
		if ( NPCsCount > 0 && m_NPCsDefinition.m_healthyPrefab != null && m_NPCsDefinition.m_zombiePrefab != null )
		{
			m_NPCsDefinition.m_healthiesNPCList = new GameObject[NPCsCount];
			var healthiesArray = m_NPCsDefinition.m_healthiesNPCList;
			m_NPCsDefinition.m_zombiesNPCList = new GameObject[NPCsCount];
			var zombiesArray = m_NPCsDefinition.m_healthiesNPCList;

			for ( int i = 0 ; i < NPCsCount ; ++i )
			{
				healthiesArray[i] = Instantiate( m_NPCsDefinition.m_healthyPrefab );
				zombiesArray[i] = Instantiate( m_NPCsDefinition.m_zombiePrefab );
			}

			int activeHealthiesCount = m_NPCsDefinition.m_healthiesStartCount;
			int activeZombiesCount = m_NPCsDefinition.m_zombiesStartCount;

			m_NPCsDefinition.m_healthiesActiveList = new GameObject[NPCsCount];
			m_NPCsDefinition.m_healthiesInactiveList = new GameObject[NPCsCount];
			m_NPCsDefinition.m_zombiesActiveList = new GameObject[NPCsCount];
			m_NPCsDefinition.m_zombiesInactiveList = new GameObject[NPCsCount];

			// Healthy number of NPCs : active healthies and inactive zombies
			var activeArray = m_NPCsDefinition.m_healthiesActiveList;
			var inactiveArray = m_NPCsDefinition.m_zombiesInactiveList;
			for ( int i = 0 ; i < activeHealthiesCount ; ++i )
			{
				activeArray[i] = healthiesArray[i];
				inactiveArray[i] = zombiesArray[activeZombiesCount + i];
			}

			// Zombie number of NPCs : active zombie and inactive healthies
			activeArray = m_NPCsDefinition.m_zombiesActiveList;
			inactiveArray = m_NPCsDefinition.m_zombiesInactiveList;
			for ( int i = 0 ; i < activeZombiesCount ; ++i )
			{
				activeArray[i] = zombiesArray[i];
				inactiveArray[i] = healthiesArray[activeHealthiesCount + i];
			}
		}
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
