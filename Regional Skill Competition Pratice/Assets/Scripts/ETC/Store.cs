using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public Text moneyText;
    public bool isFree;

    public int stage1TirePrice;
    public GameObject stage1TireSold;

    public int stage2TirePrice;
    public GameObject stage2TireSold;

    public int stage3TirePrice;
    public GameObject stage3TireSold;

    public int sixEnginePrice;
    public float sixEngineSpeed;
    public GameObject sixEngineSold;

    public int eightEnginePrice;
    public float eightEngineSpeed;
    public GameObject eightEngineSold;

    private void Update()
    {
        moneyText.text = isFree ? "무료" : (GameManager.money / 10000) + "만원";

        if (Input.GetKeyDown(KeyCode.F2))
        {
            isFree = !isFree;
        }

        stage1TireSold.SetActive(Player.stage1Tire);
        stage2TireSold.SetActive(Player.stage2Tire);
        stage3TireSold.SetActive(Player.stage3Tire);

        sixEngineSold.SetActive(Player.addSpeed >= sixEngineSpeed);
        eightEngineSold.SetActive(Player.addSpeed >= eightEngineSpeed);
    }

    public void BuyTire(int tire)
    {
        switch (tire)
        {
            case 0:
                if (!Player.stage1Tire && (GameManager.money >= stage1TirePrice || isFree))
                {
                    Player.stage1Tire = true;
                    GameManager.money -= isFree ? 0 : stage1TirePrice;
                }
                break;
            case 1:
                if (!Player.stage2Tire && (GameManager.money >= stage2TirePrice || isFree))
                {
                    Player.stage2Tire = true;
                    GameManager.money -= isFree ? 0 : stage2TirePrice;
                }
                break;
            case 2:
                if (!Player.stage3Tire && (GameManager.money >= stage3TirePrice || isFree))
                {
                    Player.stage3Tire = true;
                    GameManager.money -= isFree ? 0 : stage3TirePrice;
                }
                break;
        }
    }

    public void BuyEngine(int engine)
    {
        switch (engine)
        {
            case 0:
                if (Player.addSpeed < sixEngineSpeed && (GameManager.money >= sixEnginePrice || isFree))
                {
                    Player.addSpeed = sixEngineSpeed;
                    GameManager.money -= isFree ? 0 : sixEnginePrice;
                }
                break;
            case 1:
                if (Player.addSpeed < eightEngineSpeed && (GameManager.money >= eightEnginePrice || isFree))
                {
                    Player.addSpeed = eightEngineSpeed;
                    GameManager.money -= isFree ? 0 : eightEnginePrice;
                }
                break;
        }
    }

    public void NextStage()
    {
        if (GameManager.curStage == 1)
        {
            SceneManager.LoadScene("Stage2");
        }
        else if (GameManager.curStage == 2)
        {
            SceneManager.LoadScene("Stage3");
        }
        GameManager.curStage++;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        UIManager.instance.store.SetActive(false);
        UIManager.instance.inGameUI.SetActive(true);
    }
}
