using System.Collections;
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
    public static float getMoneyRatio;
    public AudioClip bgm;

    [Space()]
    public Transform endCameraPos;
    public bool isEnemyFinished;
    public GameObject[] firstEffects;
    public AudioClip winClip;
    public AudioClip defeatClip;

    [Space()]
    public bool itemCheatOn;
    public GameObject itemCheat;
    public bool isTime;

    public void Awake()
    {
        instance = this;
        SoundManager.instance.PlayBGM(bgm);
    }

    private void Update()
    {
        if (isStarted)
        {
            time += Time.deltaTime * 1.1f;
        }

        CheatKey();
    }

    public void PlayerFinish()
    {
        if (!isEnemyFinished)
        {
            money += (int)(getMoney * getMoneyRatio);

            SoundManager.instance.PlaySFX(winClip);

            StartCoroutine(UIManager.instance.PlayerFinish(0));
            StartCoroutine(FirstEffect());
        }
        else
        {
            SoundManager.instance.PlaySFX(defeatClip);
            StartCoroutine(UIManager.instance.PlayerFinish(1));
        }
        Camera.main.GetComponent<PlayerCamera>().positionTarget = endCameraPos;
    }

    public IEnumerator FirstEffect()
    {
        for (int i=0; i < firstEffects.Length; i++)
        {
            firstEffects[i].SetActive(true);
            yield return new WaitForSeconds(0.25f);
        }

        yield break;
    }

    public void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            itemCheatOn = !itemCheatOn;
            itemCheat.SetActive(itemCheatOn);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            switch (curStage)
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
            if (Time.timeScale > 0)
            {
                isTime = true;
                Time.timeScale = 0;
            }
            else if (Time.timeScale == 0)
            {
                isTime = false;
                Time.timeScale = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            SceneManager.LoadScene("Menu");
        }

        if (itemCheatOn)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) Player.instance.GetItem(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) Player.instance.GetItem(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) Player.instance.GetItem(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) Player.instance.GetItem(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) Player.instance.GetItem(4);
            if (Input.GetKeyDown(KeyCode.Alpha6)) Player.instance.GetItem(5);
        }
    }
}
