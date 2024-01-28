using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpgManager : MonoBehaviour
{
    public BoardController board;
    public PlayerController playerPrefabRef;
    public EnemyController enemyPrefabsRef;
    public List<EnemyController> listOfEnemies;
    public List<EnemyConfig> enemyConfigs;
    public List<HabilityConfig> allHabilities;
    public List<PlayerConfig> alliesInCombatConfig;
    public List<PlayerController> allies;



    // Start is called before the first frame update
    void Start()
    {
        //Aqui es cuando pasamos de escena 
        for (int i = 0; i < alliesInCombatConfig.Count; i++)
        {
            var ally = Instantiate(playerPrefabRef, transform);
            ally.transform.position = new Vector3(ally.transform.position.x - 300, ally.transform.position.y - 50 * i, ally.transform.position.z);
            ally.Init(alliesInCombatConfig[i]);
            allies.Add(ally);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
