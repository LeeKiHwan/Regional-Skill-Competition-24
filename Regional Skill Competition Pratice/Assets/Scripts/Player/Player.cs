using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Player : MonoBehaviour
{
    public static Player instance;

    [Space()]
    public Rigidbody rb;

    [Space()]
    public float speed;
    public float maxSpeed;
    public float slowSpeed;
    public static float addSpeed;
    public float rotSpeed;

    [Space()]
    public Transform lookTarget;
    public Transform positionTarget;
    public PlayerCamera playerCamera;

    [Space()]
    public bool isSlow;
    public GameObject smoke;
    public GameObject[] smokeEffects;
    public TrailRenderer[] driftTrail;
    public GameObject dustEffect;
    public GameObject[] dustEffects;
    public float itemGetTime;
    public GameObject itemGetEffect;
    public MeshRenderer carRenderer;

    [Space()]
    public float boostTime;
    public GameObject baseBoost;
    public GameObject miniBoost;
    public GameObject megaBoost;
    public GameObject boostBoost;

    [Space()]
    public static bool stage1Tire;
    public static bool stage2Tire;
    public static bool stage3Tire;

    public bool isFinish;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main.GetComponent<PlayerCamera>();    

        Material[] m = carRenderer.materials;
        m[0] = MenuManager.carMaterial;
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
        float yRot = Input.GetAxis("Horizontal") * Input.GetAxis("Vertical") * rotSpeed * Time.deltaTime;

        rb.AddForce(transform.forward * z, ForceMode.Acceleration);
        transform.Rotate(transform.up, yRot);

        lookTarget.localPosition = Vector3.Lerp(lookTarget.localPosition, new Vector3(Input.GetAxis("Horizontal") * 2f, 0.5f, 3), Time.deltaTime * 3);
        positionTarget.localPosition = Vector3.Lerp(positionTarget.localPosition, new Vector3(Input.GetAxis("Horizontal") * 4f , 1.75f, -4), Time.deltaTime * 3);

        driftTrail[0].emitting = (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.9f && rb.velocity.y > -5);
        driftTrail[1].emitting = (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.9f && rb.velocity.y > -5);

        smoke.SetActive(Input.GetAxis("Vertical") > 0.6f && !isSlow && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.9f);
        for (int i=0; i<4; i++)
        {
            smokeEffects[i].transform.localEulerAngles = new Vector3(0, Input.GetAxis("Horizontal") > 0 ? 360 - Input.GetAxis("Horizontal") * 20 : Input.GetAxis("Horizontal") * -20, 0);
        }

        dustEffect.SetActive(Input.GetAxis("Vertical") > 0.5f && isSlow);
        for (int i=0; i<4; i++)
        {
            dustEffects[i].transform.localEulerAngles = new Vector3(0, Input.GetAxis("Horizontal") > 0 ? 360 - Input.GetAxis("Horizontal") * 20 : Input.GetAxis("Horizontal") * -20, 0);
        }

        Vector3 vel = Vector3.ClampMagnitude(rb.velocity, maxSpeed + addSpeed - slowSpeed);
        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(vel.x, rb.velocity.y, vel.z), Time.deltaTime * 10);
        //rb.AddForce(Vector3.down * Time.deltaTime * 1750, ForceMode.Acceleration);

        rb.angularVelocity = Vector3.zero;
        
        if ((transform.rotation.eulerAngles.x > 30 && transform.rotation.eulerAngles.x < 330) ||
            (transform.rotation.eulerAngles.z > 30 && transform.rotation.eulerAngles.z < 30))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 10);
        }
    }

    public void GetItem(int? index = null)
    {
        int rand = index.HasValue ? index.Value : Random.Range(0, 6);

        switch (rand)
        {
            case 0:
                GameManager.money += 1_000_000;
                break;
            case 1:
                GameManager.money += 5_000_000;
                break;
            case 2:
                GameManager.money += 10_000_000;
                break;
            case 3:
                StartCoroutine(SpeedBuff(15, 2));
                StartCoroutine(BoostEffect(0, 2));
                break;
            case 4:
                StartCoroutine(SpeedBuff(20, 2));
                StartCoroutine(BoostEffect(1, 2));
                break;
        }

        StartCoroutine(UIManager.instance.ShowItem(rand));
    }

    public IEnumerator SpeedBuff(float speed, float time)
    {
        maxSpeed += speed;

        if (speed > 0) StartCoroutine(playerCamera.ZoomOut(time));

        yield return new WaitForSeconds(time);
        maxSpeed -= speed;

        yield break;
    }

    public IEnumerator BoostEffect(int boost, float time)
    {
        baseBoost.SetActive(false);

        if (boost == 0) miniBoost.SetActive(true);
        else if (boost == 1) megaBoost.SetActive(true);
        else if (boost == 2) boostBoost.SetActive(true);

        yield return new WaitForSeconds(time);

        baseBoost.SetActive(true);

        if (boost == 0) miniBoost.SetActive(false);
        else if (boost == 1) megaBoost.SetActive(false);
        else if (boost == 2) boostBoost.SetActive(false);

        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish") && !isFinish)
        {
            GameManager.instance.PlayerFinish();
            isFinish = true;
        }

        if (other.CompareTag("Item") && itemGetTime < Time.time)
        {
            GetItem();

            GameObject g = Instantiate(itemGetEffect, other.transform.position, Quaternion.identity);
            Destroy(g, g.GetComponent<ParticleSystem>().main.duration);

            itemGetTime = Time.time + 2;

            Destroy(other.gameObject);
        }

        if (other.CompareTag("Boost") && boostTime < Time.time)
        {
            StartCoroutine(SpeedBuff(15, 1.5f));
            StartCoroutine(BoostEffect(2, 1.5f));

            boostTime = Time.time + 2;
        }

        if (other.CompareTag("SuperBoost") && boostTime < Time.time)
        {
            float speed = other.GetComponent<SuperBoost>().addSpeed;
            float time = other.GetComponent<SuperBoost>().time;

            StartCoroutine(SpeedBuff(speed, time));

            boostTime = Time.time + 2;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SuperBoost") && boostTime < Time.time)
        {
            float speed = collision.gameObject.GetComponent<SuperBoost>().addSpeed;
            float time = collision.gameObject.GetComponent<SuperBoost>().time;

            StartCoroutine(SpeedBuff(speed, time));

            boostTime = Time.time + 2;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Slow"))
        {
            if (GameManager.curStage == 1 && !stage1Tire) slowSpeed = 15;
            if (GameManager.curStage == 2 && !stage2Tire) slowSpeed = 15;
            if (GameManager.curStage == 3 && !stage3Tire) slowSpeed = 15;

            PlayerCamera.staticCameraShake = 0.015f;
            isSlow = true;
        }
        else
        {
            slowSpeed = 0;
            PlayerCamera.staticCameraShake = 0f;
            isSlow = false;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 dir = (transform.position - collision.transform.position).normalized;
            if (collision.gameObject.GetComponent<NPC>()) rb.AddForce(dir * 8, ForceMode.VelocityChange);
            else rb.AddForce(dir * 3, ForceMode.VelocityChange);
        }
    }
}
