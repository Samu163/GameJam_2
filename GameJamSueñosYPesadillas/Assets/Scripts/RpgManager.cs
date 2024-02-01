using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RpgManager : MonoBehaviour
{
    public UiController uiController;
    public PlayerController playerPrefabRef;
    public EnemyController sombreritoPrefabRef;
    public RectTransform canvasRect;
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

    public Animator EnemyText;
    public Animator PlayerText;


    public List<string> listOfDialogesDay1Español;
    public List<string> listOfDialogesDay1English;
    public List<string> listOfDialogesDay2Español;
    public List<string> listOfDialogesDay2English;
    List<string> _listOfDialoges;

    public int indexText = 0;

    public cameraScript zoomCamera;


    public ItemController itemPrefabRef;
    public state currentStep;
    public int activePlayer = 0;
    public int activeAction = 0;
    public int activeHability = 0;
    public int activeEnemy = 0;
    public int activeItem = 0;
    public int turns;
    public string habilityNameRPGManager;
    string enemyHabilityName;

    bool hasChangeSide;

    public GameObject itemButton;
    public GameObject fleeButton;

    bool hasFinishedAttacking;

    int _indexForCountingEnemies;

    bool hasInitText = false;

    public int indexDmg = 0;

    public GameObject bg1;
    public GameObject bg2;

    public AudioSource soundEffectPlayer;
    public AudioClip punchFx;
    public AudioClip shoutFx;
    public AudioClip shotFx;
    public AudioClip letsgoFx;
    public AudioClip drinkFx;
    public AudioClip bottleFx;
    public AudioClip runFx;
    public AudioClip helpFx;

    public Animator blackPanel;

    //la logica tiene que ser, ejecutar uno y cuando pase volver a ejecutar otro si sigue el count 



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
        blackPanel.Play("FadeOut");
        if (GameManager.instance.day == 1)
        {
            bg1.SetActive(true);
            bg2.SetActive(false);
            if(GameManager.instance.language == "Español")
            {
                _listOfDialoges = listOfDialogesDay1Español;
            }
            else
            {
                _listOfDialoges = listOfDialogesDay1English;
            }
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
            bg1.SetActive(false);
            bg2.SetActive(true);
            if (GameManager.instance.language == "Español")
            {
                _listOfDialoges = listOfDialogesDay2Español;
            }
            else
            {
                _listOfDialoges = listOfDialogesDay2English;
            }
            alliesInCombatPrefabs = alliesInCombatDay2;
            enemyPrefabs = enemiesDay2;
            allItemConfigs = allItemConfigsDay2;
            //no te da la tarjeta (ultimo item)
            if(GameManager.instance.decisions[3] == 1)
            {
                if (GameManager.instance.decisions[4] == 0)
                {
                    RemoveActionButton(2);
                }
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
            ally.transform.position = new Vector3(ally.transform.position.x -500 + 200*i, ally.transform.position.y -50 -125 * (i%2), ally.transform.position.z);
            ally.Init(CheckHabilityTarget);
            ally.InitHabilities();
            ally.ShowHabilities(false);
            uiController.InitBarraVida(ally.namePlayer, i);
            allies.Add(ally);
        }
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            var enemy = Instantiate(enemyPrefabs[i], transform);
            enemy.transform.position = new Vector3(enemy.transform.position.x + 300 + 200*i, enemy.transform.position.y - 50 - 125 * (i % 2), enemy.transform.position.z);
            enemy.Init();
            enemies.Add(enemy);
        }
        if (GameManager.instance.day == 2)
        {
            if (GameManager.instance.decisions[2] == 0 || GameManager.instance.decisions[2] == 1)
            {
                enemies[0].life += 20;
                enemies[0].attack += 5;
                enemies[0].defense += 5;
            }
        }          

        for (int i = 0; i < allItemConfigs.Count; i++)
        {
            var item = Instantiate(itemPrefabRef, uiController.itemsBg.transform);
            item.transform.position = new Vector3(item.transform.position.x + 140 * i + 120, item.transform.position.y +75, item.transform.position.z);
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
        
        allies[activePlayer].AnimatorPlayer.SetTrigger("IdleTrigger");
        enemies[activeEnemy].enemyAnimator.SetTrigger("EIdleTrigger");
        switch (currentStep)
        {
            //Selecciona el personaje 
            case state.SELECT_CHARACTER:
                allies[activePlayer].ShowPlayerSelectedIcon(true);
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
                    var hability = allies[activePlayer].buttonHabilities.Find(p => p.habilityName == habilityNameRPGManager);
                    uiController.ShowTextDisplay(true);
                    uiController.textDisplay.Init(hability.description);
                    uiController.textDisplay.SetFalse();
                    currentStep = state.NONE;
                    Invoke("CheckHability", 4);
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
                    currentStep = state.NONE;
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
                if (!hasInitText)
                {
                    uiController.textDisplay.Init(inventory[activeItem].description);
                    hasInitText = true;
                }
                uiController.textDisplay.SetFalse();
                inventory[activeItem].ShowSelectedItem(true);
                uiController.ShowTextDisplay(true);
                
                
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    hasInitText = false;
                    inventory[activeItem].ShowSelectedItem(false);
                 

                    FindNextItem(true);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    hasInitText = false;

                    inventory[activeItem].ShowSelectedItem(false);
                    FindNextItem(false);
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    hasInitText = false;
                    inventory[activeItem].ShowSelectedItem(false);
                    uiController.ShowItemsBg(false);
                    uiController.ShowTextDisplay(false);
                    uiController.textDisplay.Init(inventory[activeItem].description);
                    uiController.textDisplay.SetFalse();
                    currentStep = state.NONE;
                    CheckItemTarget();

                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    hasInitText = false;
                    inventory[activeItem].ShowSelectedItem(false);
                    uiController.ShowItemsBg(false);
                    uiController.ShowTextDisplay(false);
                    uiController.textDisplay.Init(inventory[activeItem].description);
                    uiController.textDisplay.SetFalse();
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
                    currentStep = state.NONE;
                    allies[activePlayer].buttonHabilities[activeHability].ShowSelectedImage(false);
                    allies[activePlayer].buttonHabilities[activeHability].OnButtonClick();

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

                    var hability = allies[activePlayer].buttonHabilities.Find(p => p.habilityName == habilityNameRPGManager);
                    uiController.ShowTextDisplay(true);
                    uiController.textDisplay.Init(hability.description);
                    uiController.textDisplay.SetFalse();
                    currentStep = state.NONE;
                    Invoke("CheckHability", 4);
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
                    currentStep = state.NONE;
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
                indexDmg = activeEnemy;
                Invoke("showDmg", 4);
                Invoke("deactivateDmg", 8);
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
                indexDmg = activeEnemy;
                Invoke("showDmg", 4);
                Invoke("deactivateDmg", 8);
                if (turns < 4 && !hasChangeSide)
                {
                    var player = allies.Find(p => p.config.name == "Unknown");
                    if (player != null)
                    {
                        int num= allies.IndexOf(player);
                        RemoveAlly(num);
                        var sombreritoMalo = Instantiate(sombreritoPrefabRef, transform);
                        sombreritoMalo.transform.position = new Vector3(sombreritoMalo.transform.position.x + 200, sombreritoMalo.transform.position.y - 50, -100);
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
                currentStep = state.NONE;
                activeEnemy = 0;
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
            currentStep = state.NONE;
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
            uiController.barraVidas[i].loadLife(allies[i].life, allies[i].maxLife);
            
            if (allies[i].life <= 0)
            {
                uiController.RemoveBarraVida(i);
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

    public bool CheckBattleResults()
    {
        if (enemies.Count <= 0)
        {
            //Derrota enemigos 
            
            GameManager.instance.SaveRPGResult(1);

            blackPanel.Play("FadeIn");
            Invoke("LoadNewScene", 4);



            return false;
        }

        if (allies.Count <= 0)
        {
            //Derrota aliados 
            GameManager.instance.SaveRPGResult(0);
            blackPanel.Play("FadeIn");
            Invoke("LoadNewScene", 4);
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
        var j = enemies[_indexForCountingEnemies].config.habilities.Count;
        enemyHabilityName = enemies[_indexForCountingEnemies].config.habilities[GetRandomIndex(j)].habilityName;            
        uiController.ShowTextDisplay(true);
        if(GameManager.instance.language == "Español")
        {
            uiController.textDisplay.Init(enemies[_indexForCountingEnemies].config.habilities[GetRandomIndex(j)].descriptionEspañol);
        }
        else
        {
            uiController.textDisplay.Init(enemies[_indexForCountingEnemies].config.habilities[GetRandomIndex(j)].descriptionIngles);
        }
        uiController.textDisplay.SetFalse();
        currentStep = state.NONE;
        Invoke("CheckEnemyHabilityResult", 4);
          
    }

    public void SetPlayerTurn()
    {
        //  aqui
        StartCoroutine("PlayerMessage");
        uiController.ShowTextDisplay(false);

        turns++;
        for (int i = 0; i < allies.Count; i++)
        {
            allies[i].hasAttacked = false;
        }
        
        activePlayer = 0;
        activeHability = 0;
        currentStep = state.SELECT_CHARACTER;

    }

    // Botón para usar Items
    public void ItemButton()
    {
        uiController.textDisplay.Init(inventory[activeItem].description);
        uiController.ShowTextDisplay(true);
        inventory[activeItem].ShowSelectedItem(false);
        uiController.textDisplay.SetFalse();

        currentStep = state.SELECT_ITEM;
        uiController.ShowItemsBg(true);
    }

    public int GetRandomIndex(int maxValue)
    {
        return Random.Range(0, maxValue);

    }

    public void CheckEnemyHabilityResult()
    {
        uiController.ShowTextDisplay(false);
        Debug.Log(enemyHabilityName);
        var j = GetRandomIndex(allies.Count);
  
        switch (enemyHabilityName)
        {
            case "Puñetazo Chungo":
                enemies[activeEnemy].enemyAnimator.SetTrigger("EPunchTrigger");
                soundEffectPlayer.clip = punchFx;
                soundEffectPlayer.Play();
                zoomCamera.ZoomIn(enemies[activeEnemy].transform);
      
                //Añadir animacion de ataque
                Debug.Log("vaya reventada, tenia " + allies[j].life);
                allies[j].life -= enemies[activeEnemy].attack * 2 - allies[j].defense;
                Debug.Log("Y le ha hecho 10 de daño al player:" + j + "por lo que ahora tiene" + allies[j].life);
                break;
            case "Intimidar":
                enemies[activeEnemy].enemyAnimator.SetTrigger("EItemTrigger");
                zoomCamera.ZoomIn(enemies[activeEnemy].transform);
                soundEffectPlayer.clip = shoutFx;
                soundEffectPlayer.PlayDelayed(1);
                allies[j].attackPower -= 5;
                Debug.Log(allies[j].attackPower);
                break;
            case "Criticas de Madre":
                soundEffectPlayer.clip = shoutFx;
                soundEffectPlayer.PlayDelayed(1);
                for (int i = 0; i < allies.Count; i++)
                {
                    zoomCamera.ZoomIn(enemies[activeEnemy].transform);
                    enemies[activeEnemy].enemyAnimator.SetTrigger("ECriticaTrigger");
                    allies[i].life -= 5;
                    Debug.Log(allies[i].life);
                }
                break;
            case "Blow Bottle":
                soundEffectPlayer.clip = bottleFx;
                soundEffectPlayer.PlayDelayed(1);
                zoomCamera.ZoomIn(enemies[activeEnemy].transform);
                enemies[activeEnemy].enemyAnimator.SetTrigger("PunchTrigger");
                allies[j].life -= enemies[activeEnemy].attack * 3 - allies[j].defense;
                Debug.Log(allies[j].life);
                Debug.Log("Botellazo");
                break;
            case "Drink":
                soundEffectPlayer.clip = drinkFx;
                soundEffectPlayer.PlayDelayed(1);
                zoomCamera.ZoomIn(enemies[activeEnemy].transform);
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
        if (_indexForCountingEnemies >= enemies.Count-1)
        {
            if (GameManager.instance.day == 2 && !hasChangeSide)
            {
                if (GameManager.instance.decisions[3] == 0 && turns == 0)
                {
                    var player = allies.Find(p => p.config.name == "Unknown");
                    if (player != null)
                    {
                        int num = allies.IndexOf(player);
                        RemoveAlly(num);
                        var sombreritoMalo = Instantiate(sombreritoPrefabRef, transform);
                        sombreritoMalo.transform.position = new Vector3(sombreritoMalo.transform.position.x + 200, sombreritoMalo.transform.position.y - 50, -100);
                        sombreritoMalo.Init();
                        enemies.Add(sombreritoMalo);
                    }
                    hasChangeSide = true;
                }
                else if (turns == 4 && GameManager.instance.decisions[3] == 1)
                {
                    var player = allies.Find(p => p.config.name == "Unknown");
                    if (player != null)
                    {
                        int num = allies.IndexOf(player);
                        RemoveAlly(num);
                        var sombreritoMalo = Instantiate(sombreritoPrefabRef, transform);
                        sombreritoMalo.transform.position = new Vector3(sombreritoMalo.transform.position.x + 200, sombreritoMalo.transform.position.y - 50, -100);
                        sombreritoMalo.Init();
                        enemies.Add(sombreritoMalo);
                    }
                    hasChangeSide = true;
                }

            }

            uiController.ShowTextDisplay(true);
            uiController.textDisplay.Init(_listOfDialoges[indexText]);
            uiController.textDisplay.SetFalse();
            activeEnemy = 0;
            _indexForCountingEnemies = 0;
            Invoke("SetPlayerTurn", 4);
        }
        else
        {
            _indexForCountingEnemies++;
            activeEnemy++;
            Invoke("EnemyTurn", 4);
        }
    }
    //Si afecta a todos los enemigos/aliados no hace falta poner traget 
    public void CheckHabilityTarget(string habilityName, bool targetEnemy, bool targetPlayer)
    {
        habilityNameRPGManager = habilityName;
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
            var hability = allies[activePlayer].buttonHabilities.Find(p => p.habilityName == habilityName);
            uiController.ShowTextDisplay(true);
            uiController.textDisplay.Init(hability.description);
            uiController.textDisplay.SetFalse();
            currentStep = state.NONE;
            Invoke("CheckHability" , 4);
        }

    }




    public void CheckHability()
    {
        uiController.ShowTextDisplay(false);
        zoomCamera.ZoomIn(allies[activePlayer].transform);
        //active enemy es el enemigo a atacar y active player es el player al que se le aplica la accion (puede ser a si mismo) 
        switch (habilityNameRPGManager)
        {
            case "Punch":
                //Añadir animacion de ataque
                allies[activePlayer].AnimatorPlayer.SetTrigger("AttackTriggerNormal");
                indexDmg = activeEnemy;
                soundEffectPlayer.clip = punchFx;
                soundEffectPlayer.Play(); 
                Invoke("showDmg", 4);
                Invoke("deactivateDmg", 8);
                Debug.Log("Puñetazo");
                enemies[activeEnemy].life -= allies[activePlayer].attackPower * 2 - enemies[activeEnemy].defense;
                Debug.Log(enemies[activeEnemy].life);
                break;
            case "Shot":
                allies[activePlayer].AnimatorPlayer.SetTrigger("AttackTrigerDisp");
                indexDmg = activeEnemy;
                soundEffectPlayer.clip = shotFx;
                soundEffectPlayer.Play();
                Invoke("showDmg", 4);
                Invoke("deactivateDmg", 8);

                for (int i = 0; i < enemies.Count; i++)
                {
                    
                    enemies[i].life -= (allies[activePlayer].attackPower * 2 - 5) - enemies[activeEnemy].defense;
                    Debug.Log(enemies[i].life);
                }
                Debug.Log("Disparo");
                break;
            case "Help":
                allies[activePlayer].AnimatorPlayer.SetTrigger("HurtTrigger");
                soundEffectPlayer.clip = helpFx;
                soundEffectPlayer.PlayDelayed(1);
                allies[activePlayer].defense += 5;
                Debug.Log(allies[activePlayer].defense);
                Debug.Log("Sube defensa a aliado");
                break;
            case "Shout":
                //ProtaSegundaShoutTrigger
                allies[activePlayer].AnimatorPlayer.SetTrigger("ShoutTrigger");
                soundEffectPlayer.clip = shoutFx;
                soundEffectPlayer.PlayDelayed(1);
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
                soundEffectPlayer.clip = letsgoFx;
                soundEffectPlayer.PlayDelayed(2);
                for (int i = 0; i < allies.Count; i++)
                {
                    allies[i].attackPower += 5;
                    Debug.Log(allies[i].attackPower);
                }
                Debug.Log("Suma ataque a Aliado");
                break;
            case "Run":
                //RUN
                soundEffectPlayer.clip = runFx;
                soundEffectPlayer.PlayDelayed(1);
                allies[activePlayer].AnimatorPlayer.SetTrigger("RunningTrigger");
                allies[activePlayer].attackPower += 10;
                allies[activePlayer].defense -= 10;
                Debug.Log("Suma Ataque y baja defensa");

                break;
            case "Blow Bottle":
                indexDmg = activeEnemy;
                soundEffectPlayer.clip = bottleFx;
                soundEffectPlayer.PlayDelayed(1);
                Invoke("showDmg", 4);
                Invoke("deactivateDmg", 8);
                enemies[activeEnemy].life -= allies[activePlayer].attackPower * 3 - enemies[activeEnemy].defense;
                Debug.Log(enemies[activeEnemy].life);
                Debug.Log("Botellazo");
                allies[activePlayer].AnimatorPlayer.SetTrigger("EPunchTrigger");
                break;
            case "Drink":
                soundEffectPlayer.clip = drinkFx;
                soundEffectPlayer.PlayDelayed(1);
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
                currentStep = state.NONE;
                activeEnemy = 0;
                // Poner aqui lo de enemy turn
                StartCoroutine("EnemyMessage");
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

    public void deactivateDmg()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].dmg.IsActive())
            {
                enemies[i].dmg.gameObject.SetActive(false);
            }
        }
    }

    public void showDmg()
    {
        enemies[indexDmg].dmgReceived = Random.Range(10000, 99999);
        enemies[indexDmg].dmg.gameObject.SetActive(true);
        enemies[indexDmg].dmg.text = enemies[indexDmg].dmgReceived.ToString();
    }
    public void FightButton()
    {

        allies[activePlayer].ShowHabilities(true);
        currentStep = state.SELECT_HABILITY;
    }

    private IEnumerator EnemyMessage()
    {
        bool o = false;
        while (!o)
        {

            yield return new WaitForSecondsRealtime(2.0f);
            EnemyText.SetTrigger("enemyTrigger");
            o = true;
        }
    }

    private IEnumerator PlayerMessage()
    {
        bool o = false;
        while (!o)
        {

            yield return new WaitForSecondsRealtime(2.0f);
            PlayerText.SetTrigger("enemyTrigger");
            o = true;
        }
    }

    // Botón para Huir del combate
    public void FleeButton()
    {
        //Derrota aliados 
        GameManager.instance.SaveRPGResult(0);
        blackPanel.Play("FadeIn");
        Invoke("LoadNewScene", 4);
        
    }

    public void LoadNewScene()
    {
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
    }
}

