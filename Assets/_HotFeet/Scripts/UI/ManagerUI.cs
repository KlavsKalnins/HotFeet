using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ManagerUI : MonoBehaviour
{
    public static ManagerUI instance;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text enemyCount;
    public GameObject gameEndStatusObj;
    [SerializeField] private TMP_Text gameEndText;
    [SerializeField] private GameObject[] gameEndButtons;
    public static Action OnNextLevel;
    

    private void OnEnable()
    {
        instance = this;
        Enemy.OnEnemyChange += UpdateEnemyCount;
        GameManager.OnNextLevel += UpdateLevel;
        Player.OnDeath += OnLevelEnd;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyChange -= UpdateEnemyCount;
        GameManager.OnNextLevel -= UpdateLevel;
        Player.OnDeath -= OnLevelEnd;
    }
    
    private void UpdateLevel(int value)
    {
        levelText.SetText("LEVEL: " + value);
    }

    private void UpdateEnemyCount(int value)
    {
        enemyCount.SetText(value.ToString());
        if (value == 0)
        {
            StartCoroutine(OnLevelEndState(true));
        }
    }

    IEnumerator OnLevelEndState(bool status)
    {
        Time.timeScale = 0.5f;
        OnNextLevel?.Invoke();
        yield return new WaitForSeconds(0.5f);
        IsContinuing(status);
        gameEndStatusObj.SetActive(true);
        if (status)
            gameEndText.SetText("level " + GameManager.instance.GetCurrentLevel() + " completed");
        else
            gameEndText.SetText("level " + GameManager.instance.GetCurrentLevel() + " failed");
        Time.timeScale = 0;
    }
    private void OnLevelEnd()
    {
        StartCoroutine(OnLevelEndState(false));
    }

    private void IsContinuing(bool status)
    {
        if (status)
        {
            gameEndButtons[0].SetActive(true);
            gameEndButtons[1].SetActive(false);
        }
        else
        {
            gameEndButtons[0].SetActive(false);
            gameEndButtons[1].SetActive(true);
        }
        Time.timeScale = 1;
    }
}
