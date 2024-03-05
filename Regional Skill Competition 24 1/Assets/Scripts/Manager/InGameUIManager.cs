using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public Text scoreText;
    public Text timeText;

    private void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        scoreText.text = "SCORE : " + GameManager.instance.score;
        timeText.text = (int)(GameManager.instance.time / 60) + " : " + (int)(GameManager.instance.time % 60);
    }
}
