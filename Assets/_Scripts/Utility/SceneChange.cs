using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public SceneAsset newScene;
    public Vector2 posInNewScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadSceneAsync(newScene.name);
        DontDestroyOnLoad(collision);
        collision.gameObject.transform.position = posInNewScene;
    }
}
