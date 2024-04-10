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
        text.text = "���� �������� ī���͸� ��ϸ� ������ ���ϰ��� ��¸� û����..";

        yield return new WaitForSeconds(6);

        text.text = "��Ʈ��Ʈ ���̼� ���� ���зκ��� �ڵ��� ������ �����ϰ� �ȴ�.";

        yield return new WaitForSeconds(6);

        text.text = "�׷��� ���� ������ ���� ����� �����ʰ� ����� �������� ���� �䱸�ϰ�..";

        yield return new WaitForSeconds(6);

        Transform camera = Camera.main.transform;

        camera.GetComponent<Animator>().enabled = false;

        camera.position = position.position;
        camera.LookAt(look);

        text.text = "���� �� ���� ��¸� ���� ����� ���ø� ���� �賭�ϰ� ġ���� ���̽� ��ȸ�� �������� ������...";

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
