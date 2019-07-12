using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Transform objectivesParent;
    public string mainMenuScene;
    public string[] mapList;

    private Scene currentScene;
    private Scene lastLoadedScene;
    
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == mainMenuScene)
            GetComponent<PlayerController>().enablePlayerControl = false;
    }

    private void FixedUpdate()
    {
        if (currentScene.name != mainMenuScene && EnemyCount() <= 0)
        {

        } else if (currentScene.name == mainMenuScene)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                LoadScene(mapList[0]);
            }
        }
    }

    public int EnemyCount()
    {
        return objectivesParent.childCount;
    }

    /* Loads the entered scene */
    public void LoadScene(string scene)
    {
        if (currentScene.buildIndex != lastLoadedScene.buildIndex)
            lastLoadedScene = SceneManager.GetSceneByName(scene);
        SceneManager.LoadScene(scene);
    }

    /* Loads last loaded scene again */
    public void LoadScene()
    {
        SceneManager.LoadScene(lastLoadedScene.buildIndex);
    }

    /* Returns whether current scene is main menu or not */
    public bool IsMainMenu()
    {
        return currentScene.name == mainMenuScene ? true : false;
    }
}
