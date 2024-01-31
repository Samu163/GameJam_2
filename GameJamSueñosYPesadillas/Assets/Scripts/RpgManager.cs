using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RpgManager : MonoBehaviour
{
    public UiController uiController;
    public BoardController board;
    public PlayerController playerPrefabRef;
    public EnemyController sombreritoPrefabRef;
    public List<EnemyController> enemies;
    public List<EnemyController> enemiesDay1;
    public List<EnemyController> enemiesDay2;
    public List<EnemyController> enemyPrefabs;
    public List<ItemConfig> allItemConfigsDay1;
    public List<ItemConfig> allItemConfigsDay2;
    public List<ItemConfig> allItemConfigs;
    public List<ItemController> inventory;
    public List<PlayerController> alliesInCombatDay1;
    public List<PlayerController> alliesInCombatDay2;
    public List<PlayerController> alliesInCombatPrefabs;
    public List<PlayerController> allies;
    public List<ActionButton> actionButtons;

    public ItemController itemPrefabRef;
    public state currentStep;
    public int activePlayer = 0;
    public int activeAction = 0;
    public int activeHability = 0;
    public int activeEnemy = 0;
    public int activeItem = 0;
    public int indexOfAlly = 0;
    public bool hasInitHabilities = false;
    public bool isPlayerTurn = true;
    public bool selectCharacter = false;
    public bool selectAction = false;
    public int maxItemsInARow = 4;
    public int turns;
    public string habilityName;

    bool hasChangeSide;

    public GameObject itemButton;
    public GameObject fleeButton;

    public enum state
    {
        SELECT_CHARACTER,
        SELECT_CHARACTER_FOR_CURE,
        SELECT_ACTION,
        SELECT_HABILITY,
        SELECT_ITEM,
        SELECT_CHARACTER_ITEM,
        SELECT_ENEMY_ITEM,
        SELECT_ENEMY,
        NONE
    };

    public void Awake()
    {
        if (GameManager.instance.day == 1)
        {
            alliesInCombatPrefabs = alliesInCombatDay1;
            enemyPrefabs = enemiesDay1;
            allItemConfigs = allItemConfigsDay1;
            if(GameManager.instance.decisions[1] == 1)
            {
                //La ex mujer y el collar tienen que estar los ultimos en la lista
                alliesInCombatPrefabs.RemoveAt(2);
                allItemConfigs.RemoveAt(allItemConfigs.Count-1);
            }
        }
        else if (GameManager.instance.day == 2)
        {
            alliesInCombatPrefabs = alliesInCombatDay2;
            enemyPrefabs = enemiesDay2;
            allItemConfigs = allItemConfigsDay2;
            //no te da la tarjeta (ultimo item)
            if (GameManager.instance.decisions[4] == 0)
            {
                RemoveActionButton(2);
            }

        }

    }




    // Start is called before the first frame update
    void Start()
    {
        itemButton.gameObject.SetActive(true);
        uiController.Init();
        //esto es para inciar el combate directamente, si hay un dialogo primero poner el dialogo
        currentStep = state.SELECT_CHARACTER;
        //Aqui es cuando pasamos de escena 
        for (int i = 0; i < alliesInCombatPrefabs.Count; i++)
        {

            var ally = Instantiate(alliesInCombatPrefabs[i], transform);
            ally.transform.position = new Vector3(ally.transform.position.x + 200 *(i%2) -400, ally.transform.position.y - 200 * i +150, ally.transform.position.z);
            ally.Init(CheckHabilityTarget);
            ally.InitHabilities();
            ally.ShowHabilities(false);
            allies.Add(ally);
        }
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            var enemy = Instantiate(enemyPrefabs[i], transform);
            enemy.transform.position = new Vector3(enemy.transform.position.x  - 200 * (i % 2) +400, enemy.transform.position.y - 200 * i +150, enemy.transform.position.z);
            enemy.Init();
            enemies.Add(enemy);
        }
        if (GameManager.instance.day == 2)
        {
            if (GameManager.instance.decisions[2] == 0 || GameManager.instance.decisions[2] == 1)
            {
                enemies[0].life *= 2;
                enemies[0].attack *= 2;
                enemies[0].defense *= 2;
            }
        }
            

        for (int i = 0; i < allItemConfigs.Count; i++)
        {
            var item = Instantiate(itemPrefabRef, uiController.itemsBg.transform);
            item.transform.position = new Vector3(item.transform.position.x + 100 * i + 50, item.transform.position.y + 50, item.transform.position.z);
            item.Init(allItemConfigs[i]);
            inventory.Add(item);
        }

        for (int i = 0; i < actionButtons.Count; i++)
        {
            actionButtons[i].ShowSelectedIcon(false);
        }

    }
    //true es arriba false es abajo
    public void FindNextPlayer(bool direction)
    {
        for (int i = 0; i < allies.Count; i++)
        {
            if (direction)
            {
                if (activePlayer <= 0)
                {
                    activePlayer = allies.Count - 1;
                }
                else
                {
                    activePlayer--;
                }
            }
            else
            {
                if (activePlayer >= allies.Count - 1)
                {
                    activePlayer = 0;
                }
                else
                {
                    activePlayer++;
                }
            }

            if (!allies[activePlayer].hasAttacked)
            {
                break;
            }
        }
    }
    public void FindNextPlayerForCure(bool direction)
    {
        if (direction)
        {
            if (activePlayer <= 0)
            {
                activePlayer = allies.Count - 1;
            }
            else
            {
                activePlayer--;
            }
        }
        else
        {
            if (activePlayer >= allies.Count - 1)
            {
                activePlayer = 0;
            }
            else
            {
                activePlayer++;
            }
        }
    }
    public void FindNextAction(bool direction)
    {
        if (direction)
        {
            if (activeAction <= 0)
            {
                activeAction = actionButtons.Count - 1;
            }
            else
            {
                activeAction--;
            }
        }
        else
        {
            if (activeAction >= actionButtons.Count - 1)
            {
                activeAction = 0;
            }
            else
            {
                activeAction++;
            }
        }
    }
    public void FindNextHability(bool direction)
    {
        if (direction)
        {
            if (activeHability <= 0)
            {
                activeHability = allies[activePlayer].buttonHabilities.Count - 1;
            }
            else
            {
                activeHability--;
            }
        }
        else
        {
            if (activeHability >= allies[activePlayer].buttonHabilities.Count - 1)
            {
                activeHability = 0;
            }
            else
            {
                activeHability++;
            }
        }
    }
    public void FindNextItem(bool direction)
    {
        if (direction)
        {
            if (activeItem <= 0)
            {
                activeItem = inventory.Count - 1;
            }
            else
            {
                activeItem--;
            }
        }
        else
        {
            if (activeItem >= inventory.Count - 1)
            {
                activeItem = 0;
            }
            else
            {
                activeItem++;
            }
        }
    }
    public void FindNextEnemy(bool direction)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (direction)
            {
                if (activeEnemy <= 0)
                {
                    activeEnemy = enemies.Count - 1;
                }
                else
                {
                    activeEnemy--;
                }
            }
            else
            {
                if (activeEnemy >= enemies.Count - 1)
                {
                    activeEnemy = 0;
                }
                else
                {
                    activeEnemy++;
                }
            }

            if (enemies[activeEnemy].life > 0)
            {
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        allies[activePlayer].ShowPlayerSelectedIcon(true);
        allies[activePlayer].AnimatorPlayer.SetTrigger("IdleTrigger");
        enemies[activeEnemy].enemyAnimator.SetTrigger("EIdleTrigger");
        switch (currentStep)
        {
            //Selecciona el personaje 
            case state.SELECT_CHARACTER:

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    allies[activePlayer].ShowPlayerSelectedIcon(false);
                    FindNextPlayer(true);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    allies[activePlayer].ShowPlayerSelectedIcon(false);

                    FindNextPlayer(false);
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    currentStep = state.SELECT_ACTION;
                }
                break;
            case state.SELECT_CHARACTER_FOR_CURE:
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    allies[activePlayer].ShowPlayerSelectedIcon(false);
                    FindNextPlayerForCure(true);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    allies[activePlayer].ShowPlayerSelectedIcon(false);

                    FindNextPlayerForCure(false);
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    CheckHability(habilityName);
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    activeHability = 0;
                    currentStep = state.SELECT_HABILITY;
                }
                break;
            case state.SELECT_CHARACTER_ITEM:
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    allies[activePlayer].ShowPlayerSelectedIcon(false);
                    FindNextPlayerForCure(true);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    allies[activePlayer].ShowPlayerSelectedIcon(false);

                    FindNextPlayerForCure(false);
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    allies[activePlayer].AnimatorPlayer.SetTrigger("ItemTrigger");
                    Invoke("CheckItem", 2);

                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    uiController.ShowItemsBg(false);
                    activeItem = 0;
                    currentStep = state.SELECT_ITEM;
                }
                break;
            //Selecciona la accion
            case state.SELECT_ACTION:
                actionButtons[activeAction].ShowSelectedIcon(true);

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    actionButtons[activeAction].ShowSelectedIcon(false);
                    FindNextAction(true);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    actionButtons[activeAction].ShowSelectedIcon(false);
                    FindNextAction(false);
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    actionButtons[activeAction].ShowSelectedIcon(false);
                    activeHability = 0;
                    actionButtons[activeAction].ActiveOnClick();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    activeHability = 0;

                    actionButtons[activeAction].ShowSelectedIcon(false);

                    currentStep = state.SELECT_CHARACTER;
                }
                break;
            case state.SELECT_ITEM:
                inventory[activeItem].ShowSelectedItem(true);
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    inventory[activeItem].ShowSelectedItem(false);
                    FindNextItem(true);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    inventory[activeItem].ShowSelectedItem(false);
                    FindNextItem(false);
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    inventory[activeItem].ShowSelectedItem(false);
                    uiController.ShowItemsBg(false);
                    CheckItemTarget();

                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    uiController.ShowItemsBg(false);
                    activeItem = 0;
                    currentStep = state.SELECT_ACTION;

                }
                break;
            case state.SELECT_HABILITY:
                allies[activePlayer].buttonHabilities[activeHability].ShowSelectedImage(true);
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    allies[activePlayer].buttonHabilities[activeHability].ShowSelectedImage(false);
                    FindNextHability(true);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    allies[activePlayer].buttonHabilities[activeHability].ShowSelectedImage(false);
                    FindNextHability(false);
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {

                    allies[activePlayer].buttonHabilities[activeHability].ShowSelectedImage(false);
                    allies[activePlayer].buttonHabilities[activeHability].OnButtonClick();
                    //Esto es provisional, en principio tendria que seleccionar al enemigo o al jugador 

                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    allies[activePlayer].buttonHabilities[activeHability].ShowSelectedImage(false);
                    allies[activePlayer].ShowHabilities(false);
                    activeHability = 0;

                    currentStep = state.SELECT_ACTION;
                }
                break;

            case state.SELECT_ENEMY:
                enemies[activeEnemy].ShowSelectedIcon(true);

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    enemies[activeEnemy].ShowSelectedIcon(false);
                    FindNextEnemy(true);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    enemies[activeEnemy].ShowSelectedIcon(false);

                    FindNextEnemy(false);
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    enemies[activeEnemy].ShowSelectedIcon(false);


                    CheckHability(habilityName);
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    activeHability = 0;
                    currentStep = state.SELECT_HABILITY;
                }

                break;
            case state.SELECT_ENEMY_ITEM:
                enemies[activeEnemy].ShowSelectedIcon(true);

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    enemies[activeEnemy].ShowSelectedIcon(false);
                    FindNextEnemy(true);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    enemies[activeEnemy].ShowSelectedIcon(false);

                    FindNextEnemy(false);
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    allies[activePlayer].AnimatorPlayer.SetTrigger("ItemTrigger");
                    Invoke("CheckItem", 2);
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    uiController.ShowItemsBg(true);
                    activeItem = 0;
                    currentStep = state.SELECT_ITEM;
                }

                break;
            default:
                break;
        }

        //Selecciona el objetivo 
    }
    public void AdjustItemsPositions()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            var newPositions = uiController.itemsBg.transform;
            inventory[i].transform.position = new Vector3(newPositions.position.x + 100 * i - 160, newPositions.position.y, newPositions.position.z);
        }
    }


    public void CheckItem()
    {
        switch (inventory[activeItem].nameObject.text)
        {
            case "Potion":

                Debug.Log("Heal Potion");
                allies[activePlayer].life += 15;
                break;
            case "Molotov":
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].life -= 10;
                }
                break;
            case "Colgante":
                Debug.Log("Sube el ataque de todos y baja el ataque de los enemigos");
                for(int i = 0; i < allies.Count; i++)
                {
                    allies[i].attackPower += 5;
                }
                for(int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].attack -= 5;
                }
                break;
            case "Card":
                // Cuando se use, el Desconocido se cambia al bando enemigo

                if (turns < 4 && !hasChangeSide)
                {
                    var player = allies.Find(p => p.config.name == "unknown");
                    if (player != null)
                    {
                        int num= allies.IndexOf(player);
                        RemoveAlly(num);
                        var sombreritoMalo = Instantiate(sombreritoPrefabRef, transform);
                        sombreritoMalo.transform.position = new Vector3(sombreritoMalo.transform.position.x + 200, sombreritoMalo.transform.position.y - 50, sombreritoMalo.transform.position.z);
                        sombreritoMalo.Init();
                        enemies.Add(sombreritoMalo);
                    }
                    hasChangeSide = true;
                }

                if (enemies[activeEnemy].idEnemy == 2)
                {
                    enemies[activeEnemy].life -= 40;
                    Debug.Log("Ataque a Madre");
                }
                else
                {
                    enemies[activeEnemy].life -= allies[activePlayer].attackPower * 2 - enemies[activeEnemy].defense;
                    Debug.Log("Ataque normal");
                }
                break;
            default:
                break;
        }
        CheckState();
        if (CheckBattleResults())
        {
            allies[activePlayer].hasAttacked = true;
            RemoveItem(activeItem);
            activeItem = 0;
            AdjustItemsPositions();
            uiController.ShowItemsBg(false);

            if (inventory.Count <= 0)
            {
                itemButton.gameObject.SetActive(false);
            }

            if (CheckAllPlayersHadAttacked())
            {
                allies[activePlayer].ShowPlayerSelectedIcon(false);
                Invoke("EnemyTurn", 2);
            }
            else
            {
                allies[activePlayer].ShowPlayerSelectedIcon(false);
                FindNextPlayer(true);
                currentStep = state.SELECT_CHARACTER;

            }
        }


    }

    public void CheckItemTarget()
    {
        if (inventory[activeItem].allyTarget)
        {
            currentStep = state.SELECT_CHARACTER_ITEM;
        }
        else if (inventory[activeItem].enemyTarget)
        {
            currentStep = state.SELECT_ENEMY_ITEM;
        }
        else
        {
            allies[activePlayer].AnimatorPlayer.SetTrigger("ItemTrigger");
            Invoke("CheckItem", 2);
         
        }
    }


    public bool CheckAllPlayersHadAttacked()
    {
        for (int i = 0; i < allies.Count; i++)
        {
            if (!allies[i].hasAttacked)
            {
                return false;
            }
        }
        return true;
    }

    public void CheckState()
    {
        for (int i = 0; i < allies.Count; i++)
        {
            if (allies[i].life <= 0)
            {
                RemoveAlly(i);
            }
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].life <= 0)
            {
                RemoveEnemy(i);
            }
        }
    }

    //Remove functions
    public void RemoveEnemy(int index)
    {
        var enemy = enemies[index];
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    public void RemoveActionButton(int index)
    {
        var button = actionButtons[index];
        actionButtons.Remove(button);
        Destroy(button.gameObject);
    }
    public void RemoveAlly(int index)
    {
        var ally = allies[index];
        allies.Remove(ally);
        Destroy(ally.gameObject);
    }

    public void RemoveItem(int index)
    {
        var item = inventory[index];
        inventory.Remove(item);
        Destroy(item.gameObject);
    }

    public void PlayerEndingTurn()
    {

    }

    public bool CheckBattleResults()
    {
        if (enemies.Count <= 0)
        {
            //Derrota enemigos 
            
            GameManager.instance.SaveRPGResult(1);

            if (GameManager.instance.day == 1)
            {
                SceneManager.LoadScene("Narrative");
            }
            else if (GameManager.instance.finalValue >= 0)
            {
                SceneManager.LoadScene("GoodEnding");
            }
            else if (GameManager.instance.finalValue < 0 && GameManager.instance.finalValue > -110)
            {
                SceneManager.LoadScene("BadEnding");
            }
            else if (GameManager.instance.finalValue <= -110)
            {
                SceneManager.LoadScene("VeryBadEnding");
            }



            return false;
        }

        if (allies.Count <= 0)
        {
            //Derrota aliados 
            GameManager.instance.SaveRPGResult(0);
            if(GameManager.instance.day == 1)
            {
                SceneManager.LoadScene("Narrative");
            }
            else if (GameManager.instance.finalValue >= 0)
            {
                SceneManager.LoadScene("GoodEnding");
            }
            else if (GameManager.instance.finalValue < 0 && GameManager.instance.finalValue > -110)
            {
                SceneManager.LoadScene("BadEnding");
            }
            else if (GameManager.instance.finalValue <= -110)
            {
                SceneManager.LoadScene("VeryBadEnding");
            }
            return false;
        }
        return true;

    }

    public void EnemyTurn()
    {
        if (allies[activePlayer] != null)
        {
            allies[activePlayer].ShowPlayerSelectedIcon(false);

        }
        if (enemies[activeEnemy] != null)
        {
            enemies[activeEnemy].ShowSelectedIcon(false);

        }
        for (int i = 0; i < enemies.Count; i++)
        {
            var j = enemies[i].config.habilities.Count;
            CheckEnemyHabilityResult(enemies[i].config.habilities[GetRandomIndex(j)].habilityName);
        }
        //El desconocido se tiene que llamar unknown
        if (GameManager.instance.day == 2 && !hasChangeSide)
        {
            if(GameManager.instance.decisions[3] == 0 && turns ==0)
            {
                var player = allies.Find(p => p.config.name == "unknown");
                if (player != null)
                {
                    int num = allies.IndexOf(player);
                    RemoveAlly(num);
                    var sombreritoMalo = Instantiate(sombreritoPrefabRef, transform);
                    sombreritoMalo.transform.position = new Vector3(sombreritoMalo.transform.position.x + 200, sombreritoMalo.transform.position.y - 50, sombreritoMalo.transform.position.z);
                    sombreritoMalo.Init();
                    enemies.Add(sombreritoMalo);
                }
                hasChangeSide = true;
            }
            else if(turns == 4 && GameManager.instance.decisions[3] == 1)
            {
                var player = allies.Find(p => p.config.name == "unknown");
                if (player != null)
                {
                    int num = allies.IndexOf(player);
                    RemoveAlly(num);
                    var sombreritoMalo = Instantiate(sombreritoPrefabRef, transform);
                    sombreritoMalo.transform.position = new Vector3(sombreritoMalo.transform.position.x + 200, sombreritoMalo.transform.position.y - 50, sombreritoMalo.transform.position.z);
                    sombreritoMalo.Init();
                    enemies.Add(sombreritoMalo);
                }
                hasChangeSide = true;
            }

        }
       
        SetPlayerTurn();
    }

    public void SetPlayerTurn()
    {
        turns++;
        for (int i = 0; i < allies.Count; i++)
        {
            allies[i].hasAttacked = false;
        }
        
        activePlayer = 0;
        activeHability = 0;
        currentStep = state.SELECT_CHARACTER;

    }

    // Bot�n para usar Items
    public void ItemButton()
    {
        currentStep = state.SELECT_ITEM;
        uiController.ShowItemsBg(true);
    }

    public int GetRandomIndex(int maxValue)
    {
        return Random.Range(0, maxValue);

    }

    public void CheckEnemyHabilityResult(string habilityName)
    {
        Debug.Log(habilityName);
        var j = GetRandomIndex(allies.Count);
  
        switch (habilityName)
        {
            case "Pu�etazo Chungo":
                enemies[activeEnemy].enemyAnimator.SetTrigger("EPunchTrigger");
      
                //A�adir animacion de ataque
                Debug.Log("vaya reventada, tenia " + allies[j].life);
                allies[j].life -= enemies[activeEnemy].attack * 2 - allies[j].defense;
                Debug.Log("Y le ha hecho 10 de da�o al player:" + j + "por lo que ahora tiene" + allies[j].life);
                break;
            case "Intimidar":
                enemies[activeEnemy].enemyAnimator.SetTrigger("EItemTrigger");
                allies[j].attackPower -= 5;
                Debug.Log(allies[j].attackPower);
                break;
            case "Criticas de Madre":
                for (int i = 0; i < allies.Count; i++)
                {
                    enemies[activeEnemy].enemyAnimator.SetTrigger("ECriticaTrigger");
                    allies[i].life -= 5;
                    Debug.Log(allies[i].life);
                }
                break;
            case "Blow Bottle":
                enemies[activeEnemy].enemyAnimator.SetTrigger("PunchTrigger");
                allies[j].life -= enemies[activeEnemy].attack * 3 - allies[j].defense;
                Debug.Log(allies[j].life);
                Debug.Log("Botellazo");
                break;
            case "Drink":
                enemies[activeEnemy].enemyAnimator.SetTrigger("EItemTrigger");
                enemies[activeEnemy].attack += 10;
                enemies[activeEnemy].life -= 10;
                Debug.Log(enemies[activeEnemy].attack);
                Debug.Log(enemies[activeEnemy].life);
                Debug.Log("Drink");
                break;
            default:
                break;
        }
        CheckState();
        CheckBattleResults();
    }
    //Si afecta a todos los enemigos/aliados no hace falta poner traget 
    public void CheckHabilityTarget(string habilityName, bool targetEnemy, bool targetPlayer)
    {
        this.habilityName = habilityName;
        if (targetEnemy)
        {
            currentStep = state.SELECT_ENEMY;
        }
        else if (targetPlayer)
        {
            currentStep = state.SELECT_CHARACTER_FOR_CURE;
        }
        else
        {
            CheckHability(habilityName);

        }

    }


    public void CheckHability(string habilityName)
    {
     
        //active enemy es el enemigo a atacar y active player es el player al que se le aplica la accion (puede ser a si mismo) 
        switch (habilityName)
        {
            case "Punch":
                //A�adir animacion de ataque
                allies[activePlayer].AnimatorPlayer.SetTrigger("AttackTriggerNormal");
              
             Debug.Log("Pu�etazo");
                enemies[activeEnemy].life -= allies[activePlayer].attackPower * 2 - enemies[activeEnemy].defense;
                Debug.Log(enemies[activeEnemy].life);
                break;
            case "Shot":
                allies[activePlayer].AnimatorPlayer.SetTrigger("AttackTrigerDisp");

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].life -= (allies[activePlayer].attackPower * 2 - 5) - enemies[activeEnemy].defense;
                    Debug.Log(enemies[i].life);
                }
                Debug.Log("Disparo");
                break;
            case "Help":
                allies[activePlayer].AnimatorPlayer.SetTrigger("HurtTrigger");

                allies[activePlayer].defense += 5;
                Debug.Log(allies[activePlayer].defense);
                Debug.Log("Sube defensa a aliado");
                break;
            case "Shout":
                //ProtaSegundaShoutTrigger
                allies[activePlayer].AnimatorPlayer.SetTrigger("ShoutTrigger");
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].defense -= 5;
                    Debug.Log(enemies[i].defense);
                }
                Debug.Log("Baja defensas enemigos");
                break;
            case "Hide Emotions":
                allies[activePlayer].AnimatorPlayer.SetTrigger("HideTrigger");
                // ProtaSegunda
                allies[activePlayer].defense += 10;
                Debug.Log(allies[activePlayer].defense);
                break;
            case "Let's Go":
                //EXRoarTrigger
                allies[activePlayer].AnimatorPlayer.SetTrigger("RoarTrigger");
                for (int i = 0; i < allies.Count; i++)
                {
                    allies[i].attackPower += 5;
                    Debug.Log(allies[i].attackPower);
                }
                Debug.Log("Suma ataque a Aliado");
                break;
            case "Run":
                //RUN

                allies[activePlayer].AnimatorPlayer.SetTrigger("RunningTrigger");
                allies[activePlayer].attackPower += 10;
                allies[activePlayer].defense -= 10;
                Debug.Log("Suma Ataque y baja defensa");

                break;
            case "Blow Bottle":
                enemies[activeEnemy].life -= allies[activePlayer].attackPower * 3 - enemies[activeEnemy].defense;
                Debug.Log(enemies[activeEnemy].life);
                Debug.Log("Botellazo");
                allies[activePlayer].AnimatorPlayer.SetTrigger("EPunchTrigger");
                break;
            case "Drink":

                allies[activePlayer].attackPower += 10;
                allies[activePlayer].life -= 10;
                Debug.Log(allies[activePlayer].attackPower);
                Debug.Log(allies[activePlayer].life);
                allies[activePlayer].AnimatorPlayer.SetTrigger("EItemTrigger");
                break;
            default:
                break;
        }
        //Checkear si se tiene que destruir algo 
        allies[activePlayer].hasAttacked = true;
        CheckState();

        if (CheckBattleResults())
        {
            if (CheckAllPlayersHadAttacked())
            {
                allies[activePlayer].ShowPlayerSelectedIcon(false);

                Invoke("EnemyTurn",2);
            }
            else
            {
                allies[activePlayer].ShowPlayerSelectedIcon(false);
                FindNextPlayer(true);
                currentStep = state.SELECT_CHARACTER;

            }
        }
    }

    public void FightButton()
    {

        allies[activePlayer].ShowHabilities(true);
        currentStep = state.SELECT_HABILITY;
    }


    // Bot�n para Huir del combate
    public void FleeButton()
    {
        //allies[indexOfAlly].DestroyHabilities();
    }
}

