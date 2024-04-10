using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player instance;
    public Rigidbody rb;

    [Space()]
    public float speed;
    public float maxSpeed;
    public float slowSpeed;
    public static float addSpeed;
    public float rotSpeed;
    public static float addRotSpeed;

    [Space()]
    public PlayerCamera playerCamera;
    public Transform lookTarget;
    public Transform positionTarget;
    public float itemGetTime;
    public float slowGauge;
    public bool isSlowGauge;
    public GameObject slowTrail;

    [Space()]
    public MeshRenderer carRenderer;
    public TrailRenderer[] driftTrail;
    public bool isSlow;
    public GameObject smoke;
    public GameObject[] smokeEffect;
    public GameObject dust;
    public GameObject[] dustEffect;

    [Space()]
    public GameObject baseBoost;
    public GameObject miniBoost;
    public GameObject megaBoost;
    public GameObject boostBoost;
    public float boostTime;
    public AudioClip boostClip;

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
        if (Input.GetKey(KeyCode.Space) && slowGauge > 0 && !UIManager.instance.isStore && !GameManager.instance.isTime)
        {
            Time.timeScale = 0.5f;
            slowGauge -= Time.deltaTime * 20;
            slowTrail.SetActive(true);
            isSlowGauge = true;
        }
        else if (!UIManager.instance.isStore && !GameManager.instance.isTime)
        {
            Time.timeScale = 1;
            slowGauge += Time.deltaTime * 10;
            slowTrail.SetActive(false); 
            isSlowGauge = false;
        }

        slowGauge = Mathf.Clamp(slowGauge, 0, 100);

        float z = Input.GetAxis("Vertical") * speed * Time.deltaTime * 350;
        float yRot = Input.GetAxis("Horizontal") * Input.GetAxis("Vertical") * (rotSpeed + addRotSpeed) * Time.deltaTime;

        if (Physics.Raycast(transform.position, Vector3.down, 1, 1 << LayerMask.NameToLayer("Ground")))
        {
            rb.AddForce(transform.forward * z, ForceMode.Acceleration);
        }
        transform.Rotate(transform.up, yRot);

        driftTrail[0].emitting = (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.85f && rb.velocity.y > -5);
        driftTrail[1].emitting = (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.85f && rb.velocity.y > -5);

        smoke.SetActive(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.85f && !isSlow);
        for (int i=0; i<smokeEffect.Length; i++)
        {
            smokeEffect[i].transform.localEulerAngles = new Vector3(0, Input.GetAxis("Horizontal") > 0 ? 360 - Input.GetAxis("Horizontal") * 20 : Input.GetAxis("Horizontal") * -20);
        }

        dust.SetActive(isSlow);
        for (int i=0;i<dustEffect.Length;i++)
        {
            dustEffect[i].transform.localEulerAngles = new Vector3(0, Input.GetAxis("Horizontal") > 0 ? 360 - Input.GetAxis("Horizontal") * 20 : Input.GetAxis("Horizontal") * -20);
        }

        lookTarget.localPosition = Vector3.Lerp(lookTarget.localPosition, new Vector3(Input.GetAxis("Horizontal") * 2, lookTarget.localPosition.y, lookTarget.localPosition.z), Time.deltaTime * 2);
        positionTarget.localPosition = Vector3.Lerp(positionTarget.localPosition, new Vector3(Input.GetAxis("Horizontal") * 4, positionTarget.localPosition.y, positionTarget.localPosition.z), Time.deltaTime * 2);

        Vector3 vel = Vector3.ClampMagnitude(rb.velocity, maxSpeed + addSpeed - slowSpeed);
        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(vel.x, rb.velocity.y, vel.z), Time.deltaTime * 10);

        rb.angularVelocity = Vector3.zero;

        if ((transform.rotation.eulerAngles.x > 30 && transform.rotation.eulerAngles.x < 330) ||
            (transform.rotation.eulerAngles.z > 30 && transform.rotation.eulerAngles.z < 330))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 10);
        }
    }

    public void GetItem(int? index = null)
    {
        int rand = index.HasValue ? index.Value : Random.Range(0,6);

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
                StartCoroutine(SpeedBuff(10, 2));
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

        SoundManager.instance.PlaySFX(boostClip, 1, false, 1.25f);

        yield return new WaitForSeconds(time);

        if (boost == 0) miniBoost.SetActive(false);
        else if (boost == 1) megaBoost.SetActive(false);
        else if (boost == 2) boostBoost.SetActive(false);

        baseBoost.SetActive(true);

        yield break;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Slow"))
        {
            if (GameManager.curStage == 1 && !stage1Tire) slowSpeed = 20;
            if (GameManager.curStage == 2 && !stage2Tire) slowSpeed = 20;
            if (GameManager.curStage == 3 && !stage3Tire) slowSpeed = 20;

            PlayerCamera.staticCameraShake = 0.025f;
            isSlow = true;
        }
        else
        {
            PlayerCamera.staticCameraShake = 0;
            isSlow = false;
            slowSpeed = 0;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 dir = (transform.position  - collision.transform.position).normalized;
            if (collision.gameObject.GetComponent<NPC>()) rb.AddForce(dir * 9, ForceMode.VelocityChange);
            else rb.AddForce(dir * 5, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item") && itemGetTime < Time.time)
        {
            GetItem();
            itemGetTime = Time.time + 2;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Finish") && !isFinish)
        {
            GameManager.instance.PlayerFinish();
            isFinish = true;
        }

        if (other.CompareTag("Boost") && boostTime < Time.time)
        {
            StartCoroutine(SpeedBuff(15, 2));
            StartCoroutine(BoostEffect(2, 2));
            boostTime = Time.time + 2;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.down * 1);
    }
}
