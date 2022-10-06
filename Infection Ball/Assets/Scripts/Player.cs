using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Audio;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LayerMask obstacles;
    public Path _path;
    public Bullet bullet;
    public Transform bulletSpawner;
    public Transform path;
    public AudioSource gateSound;

    public GameObject bulletPrefab;
    public GameObject gateTrigger;
    public GameObject restartMenu;
    public GameObject winMenu;

    public float bulletSpeed = 10;
    public bool freeWay = false;

    private GameObject currentBullet;
    private float bulletElapsedTime;
    private float playerElapsedTime;
    private float bulletDesiredDuration = 3f;
    private float playerDesiredDuration = 3f;
    private float bulletPercentageComplete;
    private float playerPercentageComplete;

    private bool isAlive = true;

    private Vector3 startBulletSize = new Vector3(0.1f, 0.1f, 0.1f);
    private Vector3 endBulletSize;

    private Vector3 startPlayerSize;
    private Vector3 endPlayerSize = new Vector3(0.1f, 0.1f, 0.1f);

    public Animator gateAnimator;
    public Animator playerAnimator;


    private void Start()
    {
        startPlayerSize = transform.localScale;
        endBulletSize = transform.localScale;
    }

    void Update()
    {
        path.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0.2f, 1), path.localScale.y, path.localScale.z);

        bulletPercentageComplete = bulletElapsedTime / bulletDesiredDuration;
        playerPercentageComplete = playerElapsedTime / playerDesiredDuration;

        if (isAlive == false || freeWay == true)
        {
            if(currentBullet != null)
            {
                Instantiate(bullet.hitParticle, currentBullet.transform.position, currentBullet.transform.rotation);
                Destroy(currentBullet);
            }
            
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            currentBullet = Instantiate(bulletPrefab, bulletSpawner.position, bulletSpawner.rotation);
        }

        if (Input.GetMouseButton(0))
        {
            GrowBullet();
            freeWay = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            currentBullet.GetComponent<Rigidbody>().velocity = bulletSpawner.forward * bulletSpeed;
            bulletDesiredDuration -= bulletElapsedTime;
            bulletElapsedTime = 0;
            endBulletSize = transform.localScale;
        }

        if (playerPercentageComplete >= 1)
        {
            Death();
            currentBullet.GetComponent<Rigidbody>().velocity = bulletSpawner.forward * bulletSpeed;
        }
    }

    private void FixedUpdate()
    {
        if(isAlive == true)
        {
            if (Physics.BoxCast(transform.position, Mathf.Clamp(transform.localScale.x, 0.1f, 1) * Vector3.one / 2, Vector3.forward, Quaternion.identity, 20, obstacles))
            {
                freeWay = false;
            }
            else
            {
                MovingForward();
            }
        }
    }

    public void GrowBullet()
    {
        if(isAlive == false)
        {
            return;
        }

        bulletElapsedTime += Time.deltaTime;

        if (playerPercentageComplete > 1)
        {
            return;
        }

        playerElapsedTime += Time.deltaTime;

        currentBullet.transform.localScale = Vector3.Lerp(startBulletSize, endBulletSize, bulletPercentageComplete);
        transform.localScale = Vector3.Lerp(startPlayerSize, endPlayerSize, playerPercentageComplete);
    }

    public void Death()
    {
        isAlive = false;
        restartMenu.SetActive(true);
        restartMenu.transform.DOScale(new Vector3(1, 1, 1), 1);
    }

    public void MovingForward()
    {
        if(freeWay == false)
        {
            if(isAlive == false)
            {
                transform.position = transform.localPosition;
            }
            else
            {
                freeWay = true;
                transform.DOMove(gateTrigger.transform.position, 5);
                playerAnimator.SetBool("Jumping", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Trigger") && isAlive == true)
        {
            gateAnimator.SetBool("isOpened", true);
            gateSound.Play();
            playerAnimator.SetBool("Jumping", false);

            winMenu.SetActive(true);
            winMenu.transform.DOScale(new Vector3(1, 1, 1), 1);
        }
    }
}
