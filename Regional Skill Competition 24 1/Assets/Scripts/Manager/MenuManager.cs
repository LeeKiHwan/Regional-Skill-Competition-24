using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        Player.addSpeed = 0;
        Player.stage1Tire = false;
        Player.stage2Tire = false;
        Player.stage3Tire = false;

        GameManager.curStage = 1;
        GameManager.score = 0;
        GameManager.gold = 0;

        SceneManager.LoadScene("Stage1");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
