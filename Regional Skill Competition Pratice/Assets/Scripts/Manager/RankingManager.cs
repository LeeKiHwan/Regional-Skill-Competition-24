using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour
{
    public static List<RankInfo> ranking = new List<RankInfo>();
    public TextMeshProUGUI[] rankingText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        for (int i=0;i<rankingText.Length;i++)
        {
            rankingText[i].text = "";
        }

        for (int i=0; i < Mathf.Clamp(ranking.Count, 0, rankingText.Length);i++)
        {
            rankingText[i].text = $"{i+1}    \"{ranking[i].name}\"    {ranking[i].score}p";
        }

        if (SceneManager.GetActiveScene().name == "Ranking")
        {
            nameText.text = "NAME : " + MenuManager.inputName;
            scoreText.text = "SCORE : " + GameManager.score + "p";
        }
    }

    public static void InsertRank(RankInfo rankInfo)
    {
        ranking.Add(rankInfo);
        ranking = ranking.OrderByDescending(i => i.score).ToList();
    }
    
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}

public struct RankInfo
{
    public string name;
    public int score;
}
