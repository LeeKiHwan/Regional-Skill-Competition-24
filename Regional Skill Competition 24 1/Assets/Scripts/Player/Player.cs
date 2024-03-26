using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Space()]
    public Rigidbody rb;
    public static Player instance;

    [Space()]
    public float speed;
    public float maxSpeed;
    public float slowSpeed;
    public static float addSpeed;
    public float rotSpeed;

    [Space()]
    public GameObject driftEffect;
    public float itemGetTime;
    public GameObject itemGetEffect;
    public MeshRenderer carRenderer;

    [Space()]
    public float boostTime;
    public GameObject miniBoost;
    public GameObject megaBoost;

    public static bool stage1Tire;
    public static bool stage2Tire;
    public static bool stage3Tire;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();

        Material[] m = carRenderer.materials;
        m[2] = MenuManager.carMaterial;
        carRenderer.materials = m;
    }

    private void Update()
    {
        if (GameManager.instance.isStarted)
        {
            Move();
        }
    }
    public void Move()
    {
        float z = Input.GetAxis("Vertical") * speed * Time.deltaTime * 350;
        float yRot = Input.GetAxis("Horizontal") * rotSpeed * Input.GetAxis("Vertical") * Time.deltaTime;

        rb.AddForce(transform.forward * z, ForceMode.Acceleration);
        transform.Rotate(transform.up, yRot);

        driftEffect.SetActive(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.975f && rb.velocity.y > -1f);

        Vector3 vel = Vector3.ClampMagnitude(rb.velocity, maxSpeed + addSpeed - slowSpeed);
        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(vel.x, rb.velocity.y, vel.z), Time.deltaTime * 10);

        rb.angularVelocity = Vector3.zero;

        if ((transform.rotation.eulerAngles.x > 30 && transform.rotation.eulerAngles.x < 330) ||
            (transform.rotation.eulerAngles.z > 30 && transform.rotation.eulerAngles.z < 330))                                              
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 10);
        }
    }

    public IEnumerator SpeedBuff(float speed, float time)
    {
        maxSpeed += speed;
        yield return new WaitForSeconds(time);
        maxSpeed -= speed;

        yield break;
    }

    public void GetItem(int? index = null)
    {
        int rand = index.HasValue ? index.Value : Random.Range(0, 6);

        switch (rand)
        {
            case 0:
                GameManager.gold += 1000000;
                break;
            case 1:
                GameManager.gold += 5000000;
                break;
            case 2:
                GameManager.gold += 10000000;
                break;
            case 3:
                StartCoroutine(SpeedBuff(7.5f, 3));
                StartCoroutine(BoostEffect(0, 3));
                break;
            case 4:
                StartCoroutine(SpeedBuff(15, 3));
                StartCoroutine(BoostEffect(1, 3));
                break;
        }

        StartCoroutine(InGameUIManager.instance.ShowItem(rand));
    }

    public IEnumerator BoostEffect(int boost, float time)
    {
        if (boost == 0)
        {
            miniBoost.SetActive(true);
            yield return new WaitForSeconds(time);
            miniBoost.SetActive(false);
        }
        else if (boost == 1)
        {
            megaBoost.SetActive(true);
            yield return new WaitForSeconds(time);
            megaBoost.SetActive(false);
        }

        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            GameManager.instance.PlayerFinish();
        }

        if (other.CompareTag("Item") && itemGetTime < Time.time)
        {
            GameObject g = Instantiate(itemGetEffect, other.transform.position, Quaternion.identity);
            Destroy(g, g.GetComponent<ParticleSystem>().main.duration);
            GetItem();
            Destroy(other.gameObject);

            itemGetTime = Time.time + 2;
        }

        if (other.CompareTag("Boost") && boostTime < Time.time)
        {
            StartCoroutine(SpeedBuff(10, 1.5f));
            StartCoroutine(BoostEffect(0, 1.5f));
            boostTime = Time.time + 2;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Slow"))
        {
            if (GameManager.curStage == 1 && !stage1Tire)
            {
                slowSpeed = 15;
            }
            if (GameManager.curStage == 2 && !stage2Tire)
            {
                slowSpeed = 15;
            }
            if (GameManager.curStage == 3 && !stage3Tire)
            {
                slowSpeed = 15;
            }
        }
        else
        {
            slowSpeed = 0;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<NPC>())
            {
                Vector3 dir = (transform.position - collision.transform.position).normalized;
                rb.AddForce(dir * 9f, ForceMode.VelocityChange);
            }
            else
            {
                Vector3 dir = (transform.position - collision.transform.position).normalized;
                rb.AddForce(dir * 3f, ForceMode.VelocityChange);
            }
        }
    }
}
