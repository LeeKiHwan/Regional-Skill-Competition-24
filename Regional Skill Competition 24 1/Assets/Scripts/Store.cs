using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public TextMeshProUGUI CurGold;

    public int stage1TirePrice;
    public GameObject stage1TireSoldOut;

    [Space()]
    public int stage2TirePrice;
    public GameObject stage2TireSoldOut;

    [Space()]
    public int stage3TirePrice;
    public GameObject stage3TireSoldOut;

    [Space()]
    public int sixEnginePrice;
    public float sixEngineSpeed;
    public GameObject sixEngineSoldOut;

    [Space()]
    public int eightEnginePrice;
    public float eightEngineSpeed;
    public GameObject eightEngineSoldOut;

    private void Update()
    {
        CurGold.text = GameManager.gold + " ¿ø";

        stage1TireSoldOut.SetActive(Player.stage1Tire);
        stage2TireSoldOut.SetActive(Player.stage2Tire);
        stage3TireSoldOut.SetActive(Player.stage3Tire);

        sixEngineSoldOut.SetActive(Player.addSpeed >= sixEngineSpeed);
        eightEngineSoldOut.SetActive(Player.addSpeed >= eightEngineSpeed);
    }

    public void BuyTire(int stage)
    {
        switch (stage)
        {
            case 1:
                if (!Player.stage1Tire && GameManager.gold >= stage1TirePrice)
                {
                    Player.stage1Tire = true;
                    GameManager.gold -= stage1TirePrice;
                }
                break;
            case 2:
                if (!Player.stage2Tire && GameManager.gold >= stage2TirePrice)
                {
                    Player.stage2Tire = true;
                    GameManager.gold -= stage2TirePrice;
                }
                break;
            case 3:
                if (!Player.stage3Tire && GameManager.gold >= stage3TirePrice)
                {
                    Player.stage3Tire = true;
                    GameManager.gold -= stage3TirePrice;
                }
                break;
        }
    }

    public void BuyEngine(int engine)
    {
        switch (engine)
        {
            case 0:
                if (Player.addSpeed < sixEngineSpeed && GameManager.gold >= sixEnginePrice)
                {
                    Player.addSpeed = sixEngineSpeed;
                    GameManager.gold -= sixEnginePrice;
                }
                break;
            case 1:
                if (Player.addSpeed < eightEngineSpeed && GameManager.gold >= eightEnginePrice)
                {
                    Player.addSpeed = eightEngineSpeed;
                    GameManager.gold -= eightEnginePrice;
                }
                break;
        }
    }

    public void NextStage()
    {
        switch (GameManager.curStage)
        {
            case 1:
                SceneManager.LoadScene("Stage2");
                break;
            case 2:
                SceneManager.LoadScene("Stage3");
                break;
        }
    }
}
