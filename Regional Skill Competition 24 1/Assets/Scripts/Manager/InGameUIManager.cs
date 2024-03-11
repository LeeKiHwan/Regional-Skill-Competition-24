using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager instance;

    [Space()]
    public TextMeshProUGUI countDownText;

    [Space()]
    public GameObject inGameUILayer;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI playerVelocityText;
    public GameObject playerCollisionPrefab;
    public Transform playerCollsionPos;

    [Space()]
    public GameObject[] itemImages;
    public TextMeshProUGUI itemName;

    [Space()]
    public GameObject finishLayer;
    public TextMeshProUGUI rankingText;
    public TextMeshProUGUI getGoldText;
    public TextMeshProUGUI getScoreText;

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
        countDownText.text = "GO!";
        GameManager.instance.isStarted = true;
        yield return new WaitForSeconds(0.5f);
        countDownText.text = "";

        yield break;
    }

    private void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        timeText.text = (int)(GameManager.instance.time / 60) + " : " + (int)(GameManager.instance.time % 60);

        playerVelocityText.text = ((int)(Player.instance.rb.velocity.magnitude * 3600f / 1000f)).ToString();
    }

    public IEnumerator PlayerCollsiion(int col)
    {
        TextMeshProUGUI t = Instantiate(playerCollisionPrefab, playerCollsionPos).GetComponentInChildren<TextMeshProUGUI>();

        switch (col)
        {
            case 0:
                t.text = "�����浹!";
                t.color = new Color(1, 0, 0.2f);
                break;
            case 1:
                t.text = "�����浹!";
                t.color = new Color(1, 0.5f, 0);
                break;
            case 2:
                t.text = "�ĸ��浹!";
                t.color = new Color(0, 1, 0);
                break;
        }

        while (t.color.a > 0)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - Time.deltaTime);

            t.transform.parent.transform.Translate(Vector3.down * Time.deltaTime * 200);

            yield return null;
        }

        Destroy(t.transform.parent.gameObject);

        yield break;
    }

    public IEnumerator PlayerFinish(int ranking)
    {
        yield return new WaitForSeconds(1.5f);

        inGameUILayer.SetActive(false);
        finishLayer.SetActive(true);

        rankingText.text = ranking + "��";

        if (ranking == 1)
        {
            getGoldText.text = "ȹ�� ���\n" + GameManager.instance.getGold + "��";

            GameManager.score += (GameManager.curStage * 10000) + Mathf.Max(0, 10000 - (int)(GameManager.instance.time * 100));

            getScoreText.text =
                "�������� Ŭ����    +" + GameManager.curStage * 10000 +
                "\nŸ�� ���ʽ�    +" + Mathf.Max(0, 10000 - (int)(GameManager.instance.time * 100)) +
                "\n�� ���� : " + GameManager.score;

            yield return new WaitForSeconds(3);

            GameManager.instance.GoStore();
        }
        else if (ranking == 2)
        {
            getGoldText.text = "��� �� ���������� ������մϴ�...";

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
                itemName.text = "100����";
                break;
            case 1:
                itemName.text = "500����";
                break;
            case 2:
                itemName.text = "1000����";
                break;
            case 3:
                itemName.text = "���� �ν�Ʈ";
                break;
            case 4:
                itemName.text = "���� �ν�Ʈ";
                break;
        }

        yield return new WaitForSeconds(1.5f);

        itemImages[item].SetActive(false);
        itemName.text = "";

        yield break;
    }
}
