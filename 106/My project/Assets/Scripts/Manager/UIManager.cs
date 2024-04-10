using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Space()]
    public TextMeshProUGUI countDownText;
    public CameraMove[] cameraMovePos;
    public AudioClip countDownClip;

    [Space()]
    public GameObject inGameUI;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI velocityText;
    public Image slowImage;
    public Image slowGauge;

    [Space()]
    public GameObject collisionText;
    public Transform collisionTextParent;

    [Space()]
    public GameObject[] itemUIs;
    public GameObject store;
    public bool isStore;

    [Space()]
    public GameObject endGameUI;
    public Text rankingText;
    public Text moneyText;
    public Text scoreText;

    [Space()]
    public ParticleSystem sand;
    public GameObject playerLight;
    public GameObject rain;
    public Transform rainParent;
    public float rainCool;

    private void Awake()
    {
        instance = this;

        StartCoroutine(CountDown());

        switch (GameManager.curStage)
        {
            case 1:
                StartCoroutine(SandStorm());    
                break;
            case 2:
                StartCoroutine(SunDown());
                break;
            case 3:
                StartCoroutine(Rain());
                break;
        }
    }

    public IEnumerator SandStorm()
    {
        yield return new WaitForSeconds(5);
        for (int i=0; i<10; i++)
        {
            sand.emissionRate += 2;
            yield return new WaitForSeconds(1);
        }
        for (int i = 0; i < 10; i++)
        {
            sand.emissionRate += 10;
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(5);
        for (int i=0;i<10;i++)
        {
            sand.emissionRate -= 12;
            yield return new WaitForSeconds(1);
        }

        yield break;
    }

    public IEnumerator SunDown()
    {
        Camera camera = Camera.main;
        Light light = GameObject.Find("Directional Light").GetComponent<Light>();

        while (camera.backgroundColor.r > 0)
        {
            camera.backgroundColor = new Color(camera.backgroundColor.r - Time.deltaTime / 40, camera.backgroundColor.g - Time.deltaTime / 50, camera.backgroundColor.b);
            light.intensity -= Time.deltaTime / 40;

            if (light.intensity < 0.2f)
            {
                playerLight.SetActive(true);
            }

            yield return null;
        }

        yield break;
    }

    public IEnumerator Rain()
    {
        while (true)
        {
            GameObject g = Instantiate(rain, rainParent);
            g.transform.position = g.transform.position + new Vector3(Random.Range(-860, 860), Random.Range(-540, 540));

            float color = Random.Range(0f, 1f);
            g.GetComponent<Image>().color = new Color(color, color, color); 

            float scale = Random.Range(0.1f, 1f);
            g.transform.localScale = new Vector3(scale, scale, scale);

            Destroy(g, 3);

            yield return new WaitForSeconds(rainCool);
        }
    }

    private void Update()
    {
        timeText.text = ((int)GameManager.instance.time / 60) + ":" + ((int)GameManager.instance.time % 60);
        velocityText.text = ((int)Player.instance.rb.velocity.magnitude * 3600 / 1000).ToString();

        slowImage.color = Player.instance.isSlowGauge ? Color.Lerp(slowImage.color, new Color(0f, 0f, 0f, 0.5f), Time.deltaTime * 10) : Color.Lerp(slowImage.color, new Color(0, 0, 0, 0), Time.deltaTime * 10);
        slowGauge.fillAmount = Player.instance.slowGauge / 100;
    }

    public IEnumerator CountDown()
    {
        PlayerCamera pc = Camera.main.GetComponent<PlayerCamera>();

        pc.followSpeed = 200;
        pc.zoomOut = 200;

        pc.lookTarget = cameraMovePos[0].lookTarget;
        pc.positionTarget = cameraMovePos[0].transform;
        cameraMovePos[0].isMove = true;
        SoundManager.instance.PlaySFX(countDownClip);
        countDownText.text = "3";
        yield return new WaitForSeconds(1.5f);

        pc.lookTarget = cameraMovePos[1].lookTarget;
        pc.positionTarget = cameraMovePos[1].transform;
        cameraMovePos[1].isMove = true;
        SoundManager.instance.PlaySFX(countDownClip);
        countDownText.text = "2";
        yield return new WaitForSeconds(1.5f);

        pc.lookTarget = cameraMovePos[2].lookTarget;
        pc.positionTarget = cameraMovePos[2].transform;
        cameraMovePos[2].isMove = true;
        SoundManager.instance.PlaySFX(countDownClip);
        countDownText.text = "1";
        yield return new WaitForSeconds(1.5f);

        pc.followSpeed = 50;
        pc.zoomOut = 50;
        pc.lookTarget = Player.instance.lookTarget;
        pc.positionTarget = Player.instance.positionTarget;
        SoundManager.instance.PlaySFX(countDownClip, 1, false, 1.5f);
        countDownText.text = "Go!";
        GameManager.instance.isStarted = true;
        yield return new WaitForSeconds(0.5f);
        countDownText.text = "";

        yield break;
    }

    public IEnumerator PlayerCollision(int col)
    {
        Text t = Instantiate(collisionText, collisionTextParent).GetComponentInChildren<Text>();

        switch (col)
        {
            case 0:
                t.color = new Color(1, 0, 0.2f);
                t.text = "전면충돌!";
                break;
            case 1:
                t.color = new Color(1, 0.5f, 0);
                t.text = "측면충돌!";
                break;
            case 2:
                t.color = new Color(0, 1, 0);
                t.text = "후면충돌!";
                break;
        }

        while (t.color.a > 0)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - Time.deltaTime);
            t.rectTransform.parent.Translate(Vector2.down * 150 * Time.deltaTime);
            yield return null;
        }

        Destroy(t.transform.parent.gameObject);

        yield break;
    }

    public IEnumerator PlayerFinish(int ranking)
    {
        yield return new WaitForSeconds(1.5f);

        inGameUI.SetActive(false);
        endGameUI.SetActive(true);

        rankingText.text = (ranking + 1) + "위";

        if (ranking == 0)
        {
            if (GameManager.curStage != 3) moneyText.text = "획득상금    " + ((int)(GameManager.instance.getMoney * GameManager.getMoneyRatio) / 10000) + "만원";
            else if (GameManager.curStage == 3) moneyText.text = "희망의 도시 티켓 획득!";

            GameManager.score += (GameManager.curStage * 10000) + Mathf.Max(0, 10000 - (int)GameManager.instance.time * 100);

            scoreText.text =
                "스테이지 점수    " + (GameManager.curStage * 10000) + "점" +
                "\n타이머 점수    " + Mathf.Max(0, 10000 - (int)GameManager.instance.time * 100) + "점" +
                "\n\n총 점수 : " + GameManager.score + "점";

            yield return new WaitForSeconds(3);

            if (GameManager.curStage != 3)
            {
                SceneManager.LoadScene("Store");
            }
            else if (GameManager.curStage == 3)
            {
                RankInfo rankInfo = new RankInfo() { name = MenuManager.inputName, score = GameManager.score };
                RankingManager.InsertRank(rankInfo);
                SceneManager.LoadScene("Ranking");
            }
        }
        else if (ranking == 1)
        {
            moneyText.text = "잠시 후 스테이지를 재시작 합니다...";

            yield return new WaitForSeconds(3);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        yield break;
    }

    public IEnumerator ShowItem(int item)
    {
        itemUIs[item].SetActive(true);
        yield return new WaitForSeconds(1);
        itemUIs[item].SetActive(false);

        if (item == 5)
        {
            isStore = true;
            inGameUI.SetActive(false);
            store.SetActive(true);
            Time.timeScale = 0;
        }

        yield break;
    }
}
