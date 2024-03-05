using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isStarted;

    public float time;
    public int score;
    public int gold;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isStarted)
        {
            time += Time.deltaTime;
        }
    }

    public void GetGold(int getGold)
    {
        gold += getGold;
    }

    public void RestartCurrentStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
