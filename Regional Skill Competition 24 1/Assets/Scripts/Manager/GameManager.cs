using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isStarted;
    public int curStage;

    public float time;
    public static int score;
    public static int gold;
    public int getGold;

    public bool isEnemyFinished = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (isStarted)
        {
            time += Time.deltaTime;
        }

        CheatKey();
    }

    public void RestartStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayerFinish()
    {
        if (!isEnemyFinished)
        {
            gold += getGold;
            score += (curStage * 10000) + Mathf.Max(0, 10000 - (int)(time * 1000));

            StartCoroutine(InGameUIManager.instance.PlayerFinish(1));
        }
        else
        {
            StartCoroutine(InGameUIManager.instance.PlayerFinish(2));
        }

        isStarted = false;
    }

    public void NextStage()
    {
        switch (curStage)
        {
            case 1:
                SceneManager.LoadScene("Stage2");
                break;
            case 2:
                SceneManager.LoadScene("Stage3");
                break;
            case 3:
                SceneManager.LoadScene("Menu");
                break;
        }
    }

    public void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {

        }
        if (Input.GetKeyDown(KeyCode.F2))
        {

        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            RestartStage();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            NextStage();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0;
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
        }
    }
}
