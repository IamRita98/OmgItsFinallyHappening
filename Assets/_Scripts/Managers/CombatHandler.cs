using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CombatHandler : MonoBehaviour
{
    ItemHandler itemHandler;
    public static CombatHandler Instance;
    float playerUnitRange;
    GameObject unitSelectorGO;
    UnitSelector unitSelector;
    GameObject playerObject;
    GameObject enemyObject;
    public GameObject AttackTiles;
    public List<GameObject> enemiesToAttack = new List<GameObject>();
    public List<GameObject> selectTargets=new List<GameObject>();
    List<Vector2> attackRange = new List<Vector2>();
    public List<GameObject> allEnemiesList = new List<GameObject>();
    public ItemTargets displayTargetUI;
    bool inCombat = false;
    int index = 0;
    DrawTiles drawTiles;
    UnitStatSheet unitStats;
    int critMulti = 2;
    bool noEnemies=false;
    public Item item;
    UnitStatSheet attackerStats;
    UnitStatSheet defenderStats;
    int attackerHp;
    int attackerDamage;
    int attackerHitChance;
    int attackerCritChance;
    int attackerAtkRange;

    int defenderHp;
    int defenderDamage;
    int defenderHitChance;
    int defenderCritChance;
    int defenderAtkRange;
    public static event Action<GameObject> UnitDied;/*when player attacks the checks are done 
                                          later, maybe setup a second signal or refactor?*/
    public static event Action UsedItem;
    private void Awake()
    {

        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        allEnemiesList = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        unitSelectorGO = GameObject.FindGameObjectWithTag("UnitSelector");
        unitSelector = unitSelectorGO.GetComponent<UnitSelector>();
        drawTiles = gameObject.GetComponent<DrawTiles>();
    }
    private void Start()
    {
        itemHandler = GameObject.FindGameObjectWithTag("UnitSelector").GetComponent<ItemHandler>();
    }

    public void SelectTarget(ItemTargets target=ItemTargets.Enemies)
    {
        //select target 
        //if target selected is enemy ->attack
        //if target selected is friendly-> positive effect
        //self->unitSelector.selectedGO
        displayTargetUI = target;//for controlling UI display in update
        if (target == ItemTargets.Enemies)
        {
            SelectEnemies();
        }
        else if (target == ItemTargets.Self)
        {
            SelectSelf();
        }else if (target == ItemTargets.Allys)
        {
            SelectAllies();
        }        
    }

    private void SelectEnemies()
    {
        playerObject = unitSelectorGO.GetComponent<UnitSelector>().PlayerGO;
        //enemyObject = GameObject.FindGameObjectWithTag("UnitSelector").GetComponent<UnitSelector>().GOHovered;

        unitStats = playerObject.GetComponent<UnitStatSheet>();
        unitStats.GetAttackRange();
        drawTiles.DrawTilesGO(AttackTiles, unitStats.attackTiles);
        attackRange = unitStats.attackTiles;
        foreach (var tile in attackRange)
        {
            foreach (var enemy in allEnemiesList)
            {
                Vector2 temp = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
                if (temp == tile) enemiesToAttack.Add(enemy);
            }
        }
        if (enemiesToAttack.Count > 0)
        {
            selectTargets = enemiesToAttack;
            inCombat = true;
            Vector2 tempPos = enemiesToAttack[index].transform.position;
            unitSelectorGO.transform.position = tempPos;
        }
        else
        {
            //do something else if no enemies
            //UIManager.Instance.DisableCombatUI();
            //unitSelectorGO.GetComponent<UnitSelector>().EndUnitTurn();
            noEnemies = true;
            unitSelectorGO.GetComponent<UnitSelector>().ResumeSelectorControl();
            //move freely
        }
    }

    private void SelectSelf()
    {
        //TODO: add in logic for self
    }

    private void SelectAllies()
    {
        
    }
    bool skipOnce = false;
    private void Update()
    {
        if (inCombat)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                index--;
                if (index < 0)
                {
                    index = selectTargets.Count - 1;
                }
                Vector2 tempPos = selectTargets[index].transform.position;
                unitSelectorGO.transform.position = tempPos;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                index++;
                if (index >= selectTargets.Count())
                {
                    index = 0;
                }
                Vector2 tempPos = selectTargets[index].transform.position;
                unitSelectorGO.transform.position = tempPos;
            }

            if (displayTargetUI == ItemTargets.Enemies)
            {
                UnitStatSheet enemyStats = selectTargets[index].GetComponent<UnitStatSheet>();
                CombatCalc(unitStats, enemyStats);//find a way to hide this when allies are being selected
            }


            if (Input.GetKeyDown(KeyCode.Z) && unitSelector.GOHovered.CompareTag("Player"))
            {
                
                if (item != null)
                {
                    UsedItem?.Invoke();
                    itemHandler.ItemUsed(unitSelector.GOHovered, item);
                    //CALL ITEMMANAGER OR WHATEVER SCRIPT TO HANDLE USAGE
                }
                else
                {
                    //spell used
                }
                //TODO: healing/buffing friendly ally or self
                //if item!=null ->call item manager else call 
                //if spell !=null->spell manager
            }
            if (Input.GetKeyDown(KeyCode.Z) && unitSelector.GOHovered.CompareTag("Enemy"))
            {
                RunCombatCalc();
                UIManager.Instance.ClearUI();
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<DrawTiles>().ClearTiles();
                unitSelector.EndUnitTurn();
                GameStateManager.Instance.state = State.Combat;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                inCombat = false;
                UnitSelector.Instance.GOHovered=null;
                UnitSelector.Instance.transform.position = UnitSelector.Instance.playerUnitSelected.transform.position;
                UIManager.Instance.SetCombatOptionsStates();
                ClearTiles();
            }
        }

        if (noEnemies)
        {

            /*
             aoe attack list to display while moving the selector around
             */
            if (Input.GetKeyDown(KeyCode.Z) && unitSelector.canMoveSelector)
            {
                if(skipOnce) unitSelector.InvalidMove();
                skipOnce = true;
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                CancelNoEnemiesAttack();
            }
        }
    }

    void CancelNoEnemiesAttack()
    {
        unitSelector.ReturnToPickLocationAndCancel();
        unitSelector.playerUnitSelected = null;
        skipOnce = false;
        noEnemies = false;
        ClearTiles();
    }

    public void ClearTiles()
    {
        enemiesToAttack.Clear();
        attackRange.Clear();
        drawTiles.ClearTiles();
        //UIManager.Instance.ClearUI();
    }
    public void CombatCalc(UnitStatSheet attackerStatsP, UnitStatSheet defenderStatsP)
    {
        attackerStats = attackerStatsP;
        defenderStats = defenderStatsP;

        attackerHp = (int)(attackerStats.Health.Value);
        attackerDamage = (int)Mathf.Clamp((attackerStats.Strength.Value - defenderStats.Defense.Value), 1, Mathf.Infinity);
        attackerHitChance = (int)(attackerStats.HitChance.Value + attackerStats.Skill.Value - defenderStats.Speed.Value);
        attackerCritChance = (int)(attackerStats.Skill.Value);
        attackerAtkRange = (int)(attackerStats.AttackRange.Value);

        defenderHp = (int)(defenderStats.Health.Value);
        defenderDamage = (int)Mathf.Clamp((defenderStats.Strength.Value - attackerStats.Defense.Value), 1, Mathf.Infinity);
        defenderHitChance = (int)(defenderStats.HitChance.Value + defenderStats.Skill.Value - attackerStats.Speed.Value);
        defenderCritChance = (int)(defenderStats.Skill.Value);
        defenderAtkRange = (int)(defenderStats.AttackRange.Value);

        if (GameStateManager.Instance.gameState == GameStateManager.TurnState.PlayerTurn)
        {
            UIManager.Instance.ShowCombatCalcs(attackerHp, defenderHp, attackerDamage, defenderDamage, attackerHitChance, defenderHitChance, attackerCritChance, defenderCritChance);
        }
    }

    public void RunCombatCalc()
    {
        //Attacker's attack
        attackerDamage = RollHitChance(attackerHitChance, attackerCritChance, attackerDamage);
        bool counter = DefenderCanCounterAttack();
        
        // Only run attack command if player turn
        if (GameStateManager.Instance.gameState == GameStateManager.TurnState.PlayerTurn)
        {
            if (counter)
            {
                defenderDamage=RollHitChance(defenderHitChance, defenderCritChance, defenderDamage);
            }
            else
            {
                defenderDamage=0;
            }
            AttackCommand attackerAttack = new AttackCommand(attackerStats, defenderStats, attackerDamage, defenderDamage);
            CommandManager.Instance.Execute(attackerAttack);
            Debug.Log($"Enemy counter attacked for: {defenderDamage} playerHP: {attackerStats.health}");
            inCombat = false;
            enemiesToAttack.Clear();
        }
        else
        {
            defenderStats.health -= attackerDamage;
            
            //playerStats.health -= enemyDamage; This is the counter attack I'm pretty sure, I stole this from AttackCommand but we don't want to rip it like this, there are cases where there will be no coutner attack
            Debug.Log("Damage Dealt by enemy: " + attackerDamage + "\n" + "Defender HP: " + defenderStats.health);
            if (counter)
            {
                defenderDamage=RollHitChance(defenderHitChance, defenderCritChance, defenderDamage);
                attackerStats.health -= defenderDamage;
                Debug.Log($"Player countered for: {defenderDamage} AttackerHP: {attackerStats.health}");
               
            }
            else
            {
                /*Debug.Log($"L BOZO, CAN'T COUNTER");*/
            }
            
            if (defenderStats.health <= 0)
            {
                UnitDied?.Invoke(defenderStats.gameObject);
            }
        }
    }
    
    public int RollHitChance(int attackerHit, int attackerCrit, int attackerDam)
    {
        int hitRoll = Random.Range(1, 101);
        if (hitRoll >= 100 - attackerHit)
        {
            int critRoll = Random.Range(1, 101);
            if (critRoll >= 100 - attackerCrit)
            {
                attackerDam *= critMulti;
                print("BIG CRIT!!!!");
            }
            else print("Normal Dmg");
            return attackerDam;
        }
        else
        {
            print("Miss");
            //signal for when attacks missed->sound fx, animation
            return 0;
        }
    }

    public bool DefenderCanCounterAttack()
    {
        if (defenderAtkRange >= attackerAtkRange)
        {
            Debug.Log($"Counter attack succeeded Defender Atk Range: {attackerAtkRange}");
            return true; //Made this a method because I assume more logic will go into it later
        }
        else
        {
            Debug.Log($"Failed to counter rolling 0-> Defender Atk Range: {attackerAtkRange}");
            return false;
        }
    }
}
