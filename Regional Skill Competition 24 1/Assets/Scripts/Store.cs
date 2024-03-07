using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public Text CurGold;

    public int stage1TirePrice;

    [Space()]
    public int stage2TirePrice;
    
    [Space()]
    public int stage3TirePrice;

    [Space()]
    public int sixEnginePrice;
    public float sixEngineSpeed;

    [Space()]
    public int eightEnginePrice;
    public float eightEngineSpeed;

    private void Update()
    {
        CurGold.text = "현재 자금 : " + GameManager.gold + "원";
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
}
