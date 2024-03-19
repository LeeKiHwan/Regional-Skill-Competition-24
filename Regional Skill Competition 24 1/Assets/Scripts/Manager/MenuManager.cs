using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Material[] carMaterials;
    public int carMaterialIndex;
    public static Material carMaterial;

    public MeshRenderer car;

    private void Update()
    {
        car.transform.Rotate(Vector3.up * 30 * Time.deltaTime);
    }

    public void ChangeCarMaterial(int index)
    {
        carMaterialIndex = Mathf.Clamp(carMaterialIndex + index, 0, carMaterials.Length-1);
        carMaterial = carMaterials[carMaterialIndex];

        Material[] m = new Material[car.materials.Length];

        for (int i = 0; i < car.materials.Length; i++)
        {
            m[i] = car.materials[i];
        }

        m[2] = carMaterial;
        car.materials = m;
    }

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
