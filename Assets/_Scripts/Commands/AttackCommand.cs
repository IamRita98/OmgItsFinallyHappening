using UnityEngine;

public class AttackCommand : ICommand
{
    private UnitStatSheet playerStats;
    private UnitStatSheet enemyStats;
    private int playerDamage;
    private int enemyDamage;

    public AttackCommand(UnitStatSheet playerStatsP, UnitStatSheet enemyStatsP, int pDam, int eDam)
    {
        playerStats = playerStatsP;
        enemyStats = enemyStatsP;
        playerDamage = pDam;
        enemyDamage = eDam;
    }
    public void Execute()
    {
        enemyStats.health -= playerDamage;
        playerStats.health -= enemyDamage;
        Debug.Log("Damage Dealt by player: " + playerDamage + "\n" + "Enemy HP: " + enemyStats.health);
    }
    public void Undo()
    {
        enemyStats.health += playerDamage;
        playerStats.health += enemyDamage;
    }
}
