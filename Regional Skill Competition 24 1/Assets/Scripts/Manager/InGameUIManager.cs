using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager instance;

    public Text countDownText;

    public GameObject inGameUILayer;
    public Text timeText;

    public GameObject finishLayer;
    public Text rankingText;
    public Text getGoldText;
    public Text getScoreText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(CountDown());
    }

    public IEnumerator CountDown()
    {
        countDownText.text = "3";
        yield return new WaitForSeconds(1);
        countDownText.text = "2";
        yield return new WaitForSeconds(1);
        countDownText.text = "1";
        yield return new WaitForSeconds(1);
        countDownText.text = "출발!";
        yield return new WaitForSeconds(0.5f);
        countDownText.text = "";

        GameManager.instance.isStarted = true;

        yield break;
    }

    private void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        timeText.text = (int)(GameManager.instance.time / 60) + " : " + (int)(GameManager.instance.time % 60);
    }

    public IEnumerator PlayerFinish(int ranking)
    {
        inGameUILayer.SetActive(false);
        finishLayer.SetActive(true);

        rankingText.text = "순위 : " + ranking;

        if (ranking == 1)
        {
            getGoldText.text = "획득 상금 : " + GameManager.instance.getGold + "원";

            getScoreText.text =
                "\n스테이지 클리어 +" + GameManager.instance.curStage * 10000 +
                "\n타임 보너스 +" + Mathf.Max(0, 10000 - (int)(GameManager.instance.time * 100)) +
                "\n총 점수 : " + GameManager.score;

            yield return new WaitForSeconds(3);

            GameManager.instance.NextStage();
        }
        else if (ranking == 2)
        {
            yield return new WaitForSeconds(3);

            GameManager.instance.RestartStage();
        }

        yield break;
    }
}
