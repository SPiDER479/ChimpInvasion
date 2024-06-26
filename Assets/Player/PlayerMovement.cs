using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int playerSpeed;
    public string gameMode;
    public int animalCount;
    private float horizontalMovement, verticalMovement;
    private Animator anim;
    private LayerMask enemyMask;
    private AudioSource breakLock, running;
    private GameObject successPanel, failPanel, loadingPanel, joystick;
    private int levelNumber;
    private void Start()
    {
        joystick = GameObject.FindGameObjectWithTag("Joystick");
        loadingPanel = GameObject.FindGameObjectWithTag("LoadingPanel");
        successPanel = GameObject.FindGameObjectWithTag("SuccessPanel");
        failPanel = GameObject.FindGameObjectWithTag("FailPanel");
        breakLock = GetComponents<AudioSource>()[0];
        running = GetComponents<AudioSource>()[1];
        enemyMask = LayerMask.GetMask("Enemy");
        horizontalMovement = 0;
        verticalMovement = 0;
        anim = GetComponentInChildren<Animator>();
        StreamReader r = new StreamReader("currentlevel.txt");
        levelNumber = int.Parse(r.ReadLine());
        r.Close();
        setup(0);
    }
    public void setup(int change)
    {
        levelNumber += change;
        if (levelNumber % 8 <= 4 && levelNumber % 8 >= 1)
            gameMode = "Berserk";
        else
            gameMode = "Stealth";
        animalCount = GameObject.Find("Manager").GetComponent<Manager>().animalCounts[levelNumber];
        transform.position = new Vector3(0, 0.5f, -15);
        transform.rotation = Quaternion.identity;
        loadingPanel.SetActive(true);
        successPanel.SetActive(false);
        failPanel.SetActive(false);
        joystick.SetActive(false);
        StartCoroutine(loading());
    }
    void Update()
    {
        Vector3 direction = new Vector3(horizontalMovement, 0, verticalMovement);
        transform.LookAt((transform.position += playerSpeed * Time.deltaTime * direction) + direction);
    }
    public void movement(InputAction.CallbackContext ctx)
    {
        if (!loadingPanel.activeSelf)
        {
            Vector2 moveInput = ctx.ReadValue<Vector2>();
            horizontalMovement = moveInput.x;
            verticalMovement = moveInput.y;
            if (horizontalMovement == 0 && verticalMovement == 0)
                anim.SetBool("Moving", false);
            else
                anim.SetBool("Moving", true);
            if (horizontalMovement != 0 || verticalMovement != 0)
            {
                if (!running.isPlaying)
                {
                    running.Play();
                    running.time = 2;
                }
            }
            else
                running.Stop();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        switch (other.collider.tag)
        {
            case "Animal":
            {
                other.gameObject.GetComponent<Animal>().saved();
                breakLock.Play();
                animalCount--;
                break;
            }
            case "Enemy":
            {
                levelEnd(false);
                break;
            }
            case "Exit":
            {
                if (animalCount == 0)
                    levelEnd(true);
                break;
            }
            case "Key":
            {
                other.gameObject.SetActive(false);
                break;
            }
        }
    }
    IEnumerator loading()
    {
        yield return new WaitForSeconds(1);
        loadingPanel.SetActive(false);
        joystick.SetActive(true);
    }
    public void levelEnd(bool success)
    {
        foreach (Collider enemy in Physics.OverlapSphere(transform.position, 1000, enemyMask))
            enemy.gameObject.SetActive(false);
        successPanel.SetActive(success);
        failPanel.SetActive(!success);
        joystick.SetActive(false);
        enabled = false;
    }
}
