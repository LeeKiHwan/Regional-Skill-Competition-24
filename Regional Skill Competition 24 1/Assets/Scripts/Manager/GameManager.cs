using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isStarted;
    public static int curStage;

    public float time;
    public static int score;
    public static int gold;
    public int getGold;

    public bool isEnemyFinished = false;
    public Transform endCameraPos;

    private void Awake()
    {
        instance = this;

        curStage = 1;
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

            StartCoroutine(InGameUIManager.instance.PlayerFinish(1));
        }
        else
        {
            StartCoroutine(InGameUIManager.instance.PlayerFinish(2));
        }

        Camera.main.GetComponent<PlayerCamera>().positionTarget = endCameraPos;
    }

    public void GoStore()
    {
        SceneManager.LoadScene("Store");
    }

    public void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {

        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            RestartStage();
        }
        if (Input.GetKeyDown(KeyCode.F4))
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
                    SceneManager.LoadScene("Stage1");
                    break;
            }
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
