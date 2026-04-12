using UnityEngine;

public class AttackCommand : MonoBehaviour,ICommand
{
    private float playerDamage;
    private float playerHealth;
    private float enemyDamage;
    private float enemyHealth;
    public AttackCommand(float pDam,float pHp,float eDam,float eHp)
    {
        playerDamage = pDam;
        playerHealth = pHp;
        enemyDamage = eDam;
        enemyHealth = eHp;
    }
    public void Execute()
    {
        //To be implemented
    }
    public void Undo()
    {
        //To be implemented
    }
}
