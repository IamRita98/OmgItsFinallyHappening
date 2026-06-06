using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public SceneAsset newScene;
    public Vector2 posInNewScene;
    GameObject player;
    UnitSelector unitSelector;
    PlayerExplorationController explorationController;
    public string currentSceneName;


    public enum NewSceneType
    {
        Exploration,
        Combat
    }
    public NewSceneType newSceneType;

    private void Start()
    {
        unitSelector = GameObject.FindGameObjectWithTag("UnitSelector").GetComponent<UnitSelector>();
        explorationController = GameObject.FindGameObjectWithTag("UnitSelector").GetComponent<PlayerExplorationController>();
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadSceneAsync(newScene.name);
        DontDestroyOnLoad(collision);
        collision.gameObject.transform.position = posInNewScene;
        //MakeChangesBasedOnSceneType();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += MakeChangesBasedOnSceneType;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= MakeChangesBasedOnSceneType;
    }

    void MakeChangesBasedOnSceneType(Scene loadedScene, LoadSceneMode sceneMode)
    {
        currentSceneName = loadedScene.name;
        //Change this switch statement from using the pre-set Enum to automatically detect scene type based on scene name using truncated strings:
        //RPGTestSceneExploration > "...Exploration" changes GSM into Exploration State
        switch (newSceneType)
        {
            case NewSceneType.Exploration:
                GameStateManager.Instance.state = State.Exploration;
                explorationController.enabled = true;
                unitSelector.enabled = false;
                break;
            case NewSceneType.Combat:
                GameStateManager.Instance.state = State.Combat;
                unitSelector.enabled = true;
                explorationController.enabled = false;
                unitSelector.SetCombatPlayer();
                break;
        }
    }
}
