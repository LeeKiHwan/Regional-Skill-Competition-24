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
    public int itemCheatIndex;
    public bool itemCheatOn;
    public GameObject[] itemCheat;

    public bool isEnemyFinished = false;
    public Transform endCameraPos;

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

        if (itemCheatOn)
        {
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                itemCheat[itemCheatIndex].SetActive(false);
                itemCheatIndex = Mathf.Clamp(itemCheatIndex + 1, 0, 5);
                itemCheat[itemCheatIndex].SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                itemCheat[itemCheatIndex].SetActive(false);
                itemCheatIndex = Mathf.Clamp(itemCheatIndex - 1, 0, 5);
                itemCheat[itemCheatIndex].SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Player.instance.GetItem(itemCheatIndex);
                itemCheat[itemCheatIndex].SetActive(false);
                itemCheatOn = false;
            }
        }
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

    public void Next()
    {
        if (curStage != 3)
        {
            SceneManager.LoadScene("Store");
        }
        else if (curStage == 3)
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            itemCheatOn = !itemCheatOn;
            itemCheat[itemCheatIndex].SetActive(itemCheatOn);
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
