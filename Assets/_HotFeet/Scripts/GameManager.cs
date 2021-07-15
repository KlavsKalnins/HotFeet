using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject gameplayCamera;
    public static Action<bool> OnGameState;
    public static Action<int> OnNextLevel;
    [SerializeField] private int level;
    public GameObject player;
    [SerializeField] private Player playerScript;
    AsyncOperation loadedLevel;
    public Vector3 playerStart = new Vector3(0, 0.83f, 3);

    private void OnEnable()
    {
        instance = this;
        Player.OnDeath += GameOver;
    }

    private void OnDisable()
    {
        Player.OnDeath -= GameOver;
    }
    
    public void StartGame()
    {
        gameplayCamera.SetActive(true);
        OnGameState?.Invoke(true);
        NextLevel();
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        StopAllCoroutines();
        MeshGeneration.instance.ClearMeshes();
        ManagerUI.instance.gameEndStatusObj.SetActive(false);
        player.SetActive(false);
        player.transform.position = playerStart;
        
        playerScript.prevPos = playerStart;
        if (loadedLevel != null)
            SceneManager.UnloadSceneAsync(level);
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        level++;
        ManagerUI.instance.gameEndStatusObj.SetActive(false);
        if(level < SceneManager.sceneCountInBuildSettings)
        {
            OnNextLevel?.Invoke(level);
            gameplayCamera.SetActive(true);
            loadedLevel = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
            player.transform.position = playerStart;
            
            while (!loadedLevel.isDone)
                yield return null;

            player.SetActive(true);
            playerScript.Clear();
        }
        else
        {
            gameplayCamera.SetActive(false);
            OnGameState?.Invoke(false);
            level = 0;
            player.SetActive(false);
        }
        yield return loadedLevel;
    }

    public void GameOver()
    {
        OnGameState?.Invoke(false);
    }

    public int GetCurrentLevel() => level;
}
