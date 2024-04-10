using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public bool isFree;
    public Text moneyText;
    public AudioClip bgm;

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

    public int handleGearPrice;
    public float handleGearSpeed;
    public GameObject handleGearSold;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Store") SoundManager.instance.PlayBGM(bgm);
    }

    private void Update()
    {
        moneyText.text = isFree ? "무료" : (GameManager.money / 10000) + "만원";

        stage1TireSold.SetActive(Player.stage1Tire);
        stage2TireSold.SetActive(Player.stage2Tire);
        stage3TireSold.SetActive(Player.stage3Tire);

        sixEngineSold.SetActive(Player.addSpeed >= sixEngineSpeed);
        eightEngineSold.SetActive(Player.addSpeed >= eightEngineSpeed);

        handleGearSold.SetActive(Player.addRotSpeed >= handleGearSpeed);

        if (Input.GetKeyDown(KeyCode.F2)) isFree = !isFree;
    }

    public void BuyTire(int tire)
    {
        switch (tire)
        {
            case 0:
                if (!Player.stage1Tire && (isFree || GameManager.money >= stage1TirePrice))
                {
                    Player.stage1Tire = true;
                    GameManager.money -= isFree ? 0 : stage1TirePrice;
                    SoundManager.instance.PlaySFX(MenuManager.staticStoreSFX);
                }
                break;
            case 1:
                if (!Player.stage2Tire && (isFree || GameManager.money >= stage2TirePrice))
                {
                    Player.stage2Tire = true;
                    GameManager.money -= isFree ? 0 : stage2TirePrice;
                    SoundManager.instance.PlaySFX(MenuManager.staticStoreSFX);
                }
                break;
            case 2:
                if (!Player.stage3Tire && (isFree || GameManager.money >= stage3TirePrice))
                {
                    Player.stage3Tire = true;
                    GameManager.money -= isFree ? 0 : stage3TirePrice;
                    SoundManager.instance.PlaySFX(MenuManager.staticStoreSFX);
                }
                break;
        }
    }

    public void BuyEngine(int engine)
    {
        switch (engine)
        {
            case 0:
                if (Player.addSpeed < sixEngineSpeed && (isFree || GameManager.money >= sixEnginePrice))
                {
                    Player.addSpeed = sixEngineSpeed;
                    GameManager.money -= isFree ? 0 : sixEnginePrice;
                    SoundManager.instance.PlaySFX(MenuManager.staticStoreSFX);
                }
                break;
            case 1:
                if (Player.addSpeed < eightEngineSpeed && (isFree || GameManager.money >= eightEnginePrice))
                {
                    Player.addSpeed = eightEngineSpeed;
                    GameManager.money -= isFree ? 0 : eightEnginePrice;
                    SoundManager.instance.PlaySFX(MenuManager.staticStoreSFX);
                }
                break;
        }
    }

    public void BuyHandle()
    {
        if (Player.addRotSpeed < handleGearSpeed && (isFree || GameManager.money >= handleGearPrice))
        {
            Player.addRotSpeed = handleGearSpeed;
            GameManager.money -= isFree ? 0 : handleGearPrice;
            SoundManager.instance.PlaySFX(MenuManager.staticStoreSFX);
        }
    }

    public void Next()
    {
        if (GameManager.curStage == 1)
        {
            SceneManager.LoadScene("Stage2");
            GameManager.curStage = 2;
        }
        else if (GameManager.curStage == 2)
        {
            SceneManager.LoadScene("Stage3");
            GameManager.curStage = 3;
        }
    }

    public void ContinueGame()
    {
        UIManager.instance.isStore = false;
        Time.timeScale = 1;
        UIManager.instance.inGameUI.SetActive(true);
        UIManager.instance.store.SetActive(false);
    }
}
