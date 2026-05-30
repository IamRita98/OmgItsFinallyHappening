using UnityEngine;

public class PlayerExplorationController : MonoBehaviour
{
    SpriteRenderer playerSRend;
    public Sprite playerExplorationSprite;

    private void Start()
    {
        //playerSRend = gameObject.GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        playerSRend = gameObject.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        CheckRayCast();
    }
    public void SetExplorationPlayer()
    {
        playerSRend.sprite = playerExplorationSprite;
    }
    void CheckRayCast()
    {

    }

}
