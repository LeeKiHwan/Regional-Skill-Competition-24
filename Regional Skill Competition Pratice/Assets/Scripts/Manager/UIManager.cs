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

    [Space()]
    public GameObject rain;
    public float rainCool;
    public Transform rainParent;
    public ParticleSystem sand;

    [Space()]
    public GameObject inGameUI;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI velocityText;
    public GameObject collisionText;
    public Transform collisionTextParent;

    [Space()]
    public GameObject[] itemUIs;
    public GameObject store;

    [Space()]
    public GameObject endGameUI;
    public Text rankingText;
    public Text moneyText;
    public Text scoreText;

    private void Awake()
    {
        instance = this;
        StartCoroutine(CountDown());

        if (GameManager.curStage == 1) StartCoroutine(SandStorm());
        if (GameManager.curStage == 2) StartCoroutine(SunDown());
        if (GameManager.curStage == 3) StartCoroutine(Rain());
    }

    private void Update()
    {
        timeText.text = ((int)GameManager.instance.time / 60) + ":" + ((int)GameManager.instance.time % 60);
        velocityText.text = ((int)Player.instance.rb.velocity.magnitude * 3600 / 1000).ToString();
    }

    public IEnumerator SandStorm()
    {
        for (int i=0; i<30; i++)
        {
            sand.emissionRate += 5;
            yield return new WaitForSeconds(1);
        }

        sand.gameObject.SetActive(false);

        yield break;
    }

    public IEnumerator SunDown()
    {
        Camera camera = Camera.main;
        Light light = GameObject.Find("Directional Light").GetComponent<Light>();

        while (camera.backgroundColor.r > 0)
        {
            camera.backgroundColor = new Color(camera.backgroundColor.r - Time.deltaTime / 60, camera.backgroundColor.g - Time.deltaTime / 60, camera.backgroundColor.b);
            light.intensity -= Time.deltaTime / 60;
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

            float scale = Random.Range(0f, 1f);
            g.transform.localScale = new Vector3(scale, scale, scale);

            Destroy(g, 2);

            yield return new WaitForSeconds(rainCool);
        }
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

    public IEnumerator PlayerCollision(int col)
    {
        Text t = Instantiate(collisionText, collisionTextParent).GetComponentInChildren<Text>();

        switch (col)
        {
            case 0:
                t.text = "전면충돌";
                t.color = new Color(1, 0, 0.2f);
                break;
            case 1:
                t.text = "측면충돌";
                t.color = new Color(1, 0.5f, 0);
                break;
            case 2:
                t.text = "후면충돌";
                t.color = new Color(0, 1, 0);
                break;
        }

        while (t.color.a > 0)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - Time.deltaTime);
            t.transform.parent.Translate(Vector3.down * 150 *  Time.deltaTime);
            yield return null;
        }

        Destroy(t.transform.parent.gameObject);

        yield break;
    }

    public IEnumerator PlayerFinish(int ranking)
    {
        Debug.Log("Player Finish");
        yield return new WaitForSeconds(1.5f);

        inGameUI.SetActive(false);
        endGameUI.SetActive(true);

        rankingText.text = (ranking+1) + "위";

        if (ranking == 0)
        {
            moneyText.text = GameManager.curStage == 3 ? "희망의 도시 티켓 획득!" : "획득상금    " + (GameManager.instance.getMoney / 10000) + "만원";

            GameManager.score += (GameManager.curStage * 10000) + Mathf.Max(0, 10000 - (int)GameManager.instance.time * 100);

            scoreText.text =
                "스테이지 점수    +" + (GameManager.curStage * 10000) +
                "\n보너스 점수    +" + Mathf.Max(0, 10000 - (int)GameManager.instance.time * 100) +
                "\n\n총 점수 : " + GameManager.score;

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
            moneyText.text = "잠시 후 재시작...";
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
            Time.timeScale = 0;
            inGameUI.SetActive(false);
            store.SetActive(true);
        }

        yield break;
    }
}
