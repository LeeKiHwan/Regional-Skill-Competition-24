using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager instance;

    [Space()]
    public Text countDownText;

    [Space()]
    public GameObject inGameUILayer;
    public Text timeText;

    [Space()]
    public GameObject[] itemImages;
    public Text itemName;

    [Space()]
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

            GameManager.instance.GoStore();
        }
        else if (ranking == 2)
        {
            yield return new WaitForSeconds(3);

            GameManager.instance.RestartStage();
        }

        yield break;
    }

    public IEnumerator ShowItem(int item)
    {
        itemImages[item].SetActive(true);

        switch (item)
        {
            case 0:
                itemName.text = "100만원";
                break;
            case 1:
                itemName.text = "500만원";
                break;
            case 2:
                itemName.text = "1000만원";
                break;
            case 3:
                itemName.text = "소폭 부스트";
                break;
            case 4:
                itemName.text = "대폭 부스트";
                break;
        }

        yield return new WaitForSeconds(1.5f);

        itemImages[item].SetActive(false);
        itemName.text = "";

        yield break;
    }
}
