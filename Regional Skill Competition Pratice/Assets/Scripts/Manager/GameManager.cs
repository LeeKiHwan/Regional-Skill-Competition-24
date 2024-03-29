using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int curStage;
    public static int money;
    public static int score;

    [Space()]
    public bool isStarted;
    public float time;
    public int getMoney;

    [Space()]
    public bool isEnemyFinished;
    public Transform endCameraPos;
    public GameObject[] firstEffect;

    [Space()]
    public bool itemCheatOn;
    public int itemCheatIndex;
    public GameObject[] itemCheat;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (isStarted)
        {
            time += Time.deltaTime * 1.15f;
        }

        CheatKey();
    }

    public void PlayerFinish()
    {
        if (!isEnemyFinished)
        {
            money += getMoney;
            StartCoroutine(UIManager.instance.PlayerFinish(0));

            StartCoroutine(FirstEffect());
        }
        else
        {
            StartCoroutine(UIManager.instance.PlayerFinish(1));
        }

        Camera.main.GetComponent<PlayerCamera>().positionTarget = endCameraPos;
    }

    public IEnumerator FirstEffect()
    {
        for (int i = 0; i < firstEffect.Length; i++)
        {
            firstEffect[i].SetActive(true);
            yield return new WaitForSeconds(0.25f);
        }

        yield break;
    }

    public void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            itemCheatOn = !itemCheatOn;
            itemCheat[itemCheatIndex].SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            switch(curStage)
            {
                case 1:
                    SceneManager.LoadScene("Stage2");
                    curStage = 2;
                    break;
                case 2:
                    SceneManager.LoadScene("Stage3");
                    curStage = 3;
                    break;
                case 3:
                    SceneManager.LoadScene("Stage1");
                    curStage = 1;
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (Time.timeScale > 0) Time.timeScale = 0;
            else if (Time.timeScale == 0) Time.timeScale = 1;
        }

        if (itemCheatOn)
        {
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                itemCheat[itemCheatIndex].SetActive(false);
                itemCheatIndex = Mathf.Clamp(itemCheatIndex - 1, 0, 5);
                itemCheat[itemCheatIndex].SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                itemCheat[itemCheatIndex].SetActive(false);
                itemCheatIndex = Mathf.Clamp(itemCheatIndex + 1, 0, 5);
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
}
