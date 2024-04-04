using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Material[] carMaterials;
    public static Material carMaterial;
    public int carMaterialIndex;
    public MeshRenderer carModel;

    public GameObject title;

    public static string inputName;
    public TMP_InputField nameInputField;

    private void Awake()
    {
        Material[] m = carModel.materials;
        carMaterial = carMaterials[carMaterialIndex];
        m[0] = carMaterial;
        carModel.materials = m;

        StartCoroutine(TitleAnimation());
    }

    public IEnumerator TitleAnimation()
    {
        while (title.transform.localScale.x > 0.75f)
        {
            title.transform.localScale = new Vector3(title.transform.localScale.x - Time.deltaTime, title.transform.localScale.x - Time.deltaTime, title.transform.localScale.x - Time.deltaTime);
            yield return null;
        }

        StartCoroutine(TitleAnimation2());

        yield break;
    }

    public IEnumerator TitleAnimation2()
    {
        while (title.transform.localScale.x < 1.25f)
        {
            title.transform.localScale = new Vector3(title.transform.localScale.x + Time.deltaTime, title.transform.localScale.x + Time.deltaTime, title.transform.localScale.x + Time.deltaTime);
            yield return null;
        }

        StartCoroutine(TitleAnimation());

        yield break;
    }

    private void Update()
    {
        carModel.transform.Rotate(Vector3.up, Time.deltaTime * 30);
    }

    public void ChangeMaterial(int index)
    {
        carMaterialIndex = Mathf.Clamp(carMaterialIndex + index, 0, carMaterials.Length-1);

        Material[] m = carModel.materials;
        carMaterial = carMaterials[carMaterialIndex];
        m[0] = carMaterial;
        carModel.materials = m;
    }

    public void StartGame()
    {
        Player.stage1Tire = false;
        Player.stage2Tire = false;
        Player.stage3Tire = false;
        Player.addSpeed = 0;

        GameManager.curStage = 1;
        GameManager.money = 0;
        GameManager.score = 0;

        inputName = nameInputField.text;

        SceneManager.LoadScene("Stage1");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
