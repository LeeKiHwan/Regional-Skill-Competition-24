using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour
{
    public static List<RankInfo> rankings = new List<RankInfo>();
    public TextMeshProUGUI[] rankingTexts;

    private void Awake()
    {
        ShowRanking();
    }

    public static void InsertRank(RankInfo rankInfo)
    {
        rankings.Add(rankInfo);
        rankings = rankings.OrderByDescending(i => i.score).ToList();
    }

    public void ShowRanking()
    {
        for (int i = 0; i<rankingTexts.Length; i++)
        {
            rankingTexts[i].text = "";
        }

        for (int i = 0; i<Mathf.Clamp(rankings.Count, 0, rankingTexts.Length); i++)
        {
            rankingTexts[i].text = $"{i+1}    \"{rankings[i].name}\"    {rankings[i].score}p";
        }
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