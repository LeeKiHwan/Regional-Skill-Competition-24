using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Story : MonoBehaviour
{
    public Text text;
    public Transform position;
    public Transform look;


    private void Awake()
    {
        StartCoroutine(StoryCo());
    }

    public IEnumerator StoryCo()
    {
        text.text = "피폐 지역에서 카센터를 운영하며 열심히 살하가던 백승리 청년은..";

        yield return new WaitForSeconds(6);

        text.text = "스트리트 레이서 조직 왈패로부터 자동차 수리를 수리하게 된다.";

        yield return new WaitForSeconds(6);

        text.text = "그러나 왈패 조직은 수리 비용을 내지않고 폭행과 협박으로 돈을 요구하고..";

        yield return new WaitForSeconds(6);

        Transform camera = Camera.main.transform;

        camera.GetComponent<Animator>().enabled = false;

        camera.position = position.position;
        camera.LookAt(look);

        text.text = "참을 수 없던 백승리 군은 희망의 도시를 위해 험난하고 치열한 레이싱 대회에 도전장을 던진다...";

        yield return new WaitForSeconds(6);

        SceneManager.LoadScene("Stage1");

        yield break;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Stage1");
        }
    }
}
