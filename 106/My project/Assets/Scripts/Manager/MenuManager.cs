using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static string inputName;
    public TMP_InputField nameInputField;

    public TextMeshProUGUI[] rankingText;

    public MeshRenderer carRenderer;
    public Material[] carMaterials;
    public int carMaterialIndex;
    public static Material carMaterial;

    public GameObject[] helpUIs;
    public int helpUIIndex;

    public GameObject title;
    public AudioClip bgm;
    public AudioClip storeSFX;
    public static AudioClip staticStoreSFX;

    private void Awake()
    {
        for (int i = 0; i < Mathf.Clamp(RankingManager.ranking.Count, 0, rankingText.Length); i++)
        {
            rankingText[i].text = $"{i + 1}    \"{RankingManager.ranking[i].name}\"    {RankingManager.ranking[i].score}p";
        }

        carMaterial = carMaterials[carMaterialIndex];
        Material[] m = carRenderer.materials;
        m[0] = carMaterial;
        carRenderer.materials = m;

        staticStoreSFX = storeSFX;

        StartCoroutine(TitleBig());
    }

    private void Start()
    {
        SoundManager.instance.PlayBGM(bgm);
    }

    public IEnumerator TitleBig()
    {
        while (title.transform.localScale.x < 1.1f)
        {
            title.transform.localScale = new Vector3(title.transform.localScale.x + Time.deltaTime / 2, title.transform.localScale.x + Time.deltaTime / 2, title.transform.localScale.x + Time.deltaTime / 2);
            yield return null;
        }

        StartCoroutine(TitleSmall());

        yield break;
    }

    public IEnumerator TitleSmall()
    {
        while (title.transform.localScale.x > 0.9f)
        {
            title.transform.localScale = new Vector3(title.transform.localScale.x - Time.deltaTime / 2, title.transform.localScale.x - Time.deltaTime / 2, title.transform.localScale.x - Time.deltaTime / 2);
            yield return null;
        }

        StartCoroutine (TitleBig());

        yield break;
    }

    private void Update()
    {
        carRenderer.transform.Rotate(Vector3.up, 30 * Time.deltaTime);
    }

    public void ChangeMaterial(int index)
    {
        carMaterialIndex = Mathf.Clamp(carMaterialIndex + index, 0, carMaterials.Length-1);
        carMaterial = carMaterials[carMaterialIndex];
        Material[] m = carRenderer.materials;
        m[0] = carMaterial;
        carRenderer.materials = m;
    }

    public void ChangeHelpUI(int index)
    {
        helpUIs[helpUIIndex].SetActive(false);
        helpUIIndex = Mathf.Clamp(helpUIIndex + index, 0, helpUIs.Length-1);
        helpUIs[helpUIIndex].SetActive(true);
    }

    public void StartGame(int diff)
    {
        Player.stage1Tire = false;
        Player.stage2Tire = false;
        Player.stage3Tire = false;
        Player.addSpeed = 0;
        Player.addRotSpeed = 0;

        GameManager.curStage = 1;
        GameManager.money = 0;
        GameManager.score = 0;

        inputName = nameInputField.text;

        switch (diff)
        {
            case 0:
                Enemy.addSpeed = -20f;
                GameManager.getMoneyRatio = 1.5f;
                break;
            case 1:
                Enemy.addSpeed = 0;
                GameManager.getMoneyRatio = 1;
                break;
            case 2:
                Enemy.addSpeed = 20;
                GameManager.getMoneyRatio = 0.75f;
                break;
        }

        SceneManager.LoadScene("Story");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
