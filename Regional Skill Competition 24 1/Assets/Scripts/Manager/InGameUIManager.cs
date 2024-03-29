using System.Collections;
using TMPro;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager instance;

    [Space()]
    public TextMeshProUGUI countDownText;
    public CameraMove[] countDownCamera;
    public Transform lookTarget;
    public Transform positionTarget;

    [Space()]
    public GameObject inGameUILayer;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI playerVelocityText;
    public GameObject playerCollisionPrefab;
    public Transform playerCollsionPos;

    [Space()]
    public GameObject[] itemImages;
    public GameObject storeUILayer;

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
        PlayerCamera pc = Camera.main.GetComponent<PlayerCamera>();
        pc.followSpeed = 100;

        pc.positionTarget = countDownCamera[0].transform;
        pc.lookTarget = countDownCamera[0].lookTarget;

        yield return new WaitForSeconds(3);

        pc.positionTarget = countDownCamera[1].transform;
        pc.lookTarget = countDownCamera[1].lookTarget;
        countDownCamera[1].speed = 0.5f;

        countDownText.text = "3";
        yield return new WaitForSeconds(1.5f);

        pc.positionTarget = countDownCamera[2].transform;
        pc.lookTarget = countDownCamera[2].lookTarget;
        countDownCamera[2].speed = 0.5f;

        countDownText.text = "2";
        yield return new WaitForSeconds(1.5f);

        pc.positionTarget = countDownCamera[3].transform;
        pc.lookTarget = countDownCamera[3].lookTarget;
        countDownCamera[3].speed = 0.5f;

        countDownText.text = "1";
        yield return new WaitForSeconds(1.5f);

        pc.followSpeed = 1;
        pc.positionTarget = positionTarget;
        pc.lookTarget = lookTarget;

        countDownText.text = "GO!";
        pc.isStarted = true;
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
                t.text = "전면충돌!";
                t.color = new Color(1, 0, 0.2f);
                break;
            case 1:
                t.text = "측면충돌!";
                t.color = new Color(1, 0.5f, 0);
                break;
            case 2:
                t.text = "후면충돌!";
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

        rankingText.text = ranking + "위";

        if (ranking == 1)
        {
            getGoldText.text = "획득 상금\n" + GameManager.instance.getGold + "원";

            GameManager.score += (GameManager.curStage * 10000) + Mathf.Max(0, 10000 - (int)(GameManager.instance.time * 100));

            getScoreText.text =
                "스테이지 클리어    +" + GameManager.curStage * 10000 +
                "\n타임 보너스    +" + Mathf.Max(0, 10000 - (int)(GameManager.instance.time * 100)) +
                "\n총 점수 : " + GameManager.score;

            yield return new WaitForSeconds(3);

            GameManager.instance.Next();
        }
        else if (ranking == 2)
        {
            getGoldText.text = "잠시 후 스테이지를 재시작합니다...";

            yield return new WaitForSeconds(3);

            GameManager.instance.RestartStage();
        }

        yield break;
    }

    public IEnumerator ShowItem(int item)
    {
        itemImages[item].SetActive(true);

        yield return new WaitForSeconds(1.5f);

        itemImages[item].SetActive(false);

        if (item == 5)
        {
            Time.timeScale = 0;
            inGameUILayer.SetActive(false);
            storeUILayer.SetActive(true);
        }

        yield break;
    }
}
