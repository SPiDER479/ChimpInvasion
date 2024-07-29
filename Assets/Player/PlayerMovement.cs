using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private int playerSpeed;
    public string gameMode;
    public int animalCount;
    private float horizontalMovement, verticalMovement;
    private Animator anim;
    private LayerMask enemyMask;
    private AudioSource breakLock, running;
    [SerializeField] private Image UIBG;
    [SerializeField] private Sprite[] UIBGSprites;
    private GameObject successPanel, failPanel, loadingPanel, joystick;
    private int levelNumber;
    [SerializeField] private PowerupData pud;
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
        setup();
    }
    public void setup()
    {
        pud.radius = 10;
        pud.angle = 45;
        pud.enemyRadiusDecrease = 1;
        pud.knifeDamageMultiplier = 1;
        levelNumber = levelData.currentLevel;
        if (levelNumber % 8 <= 4 && levelNumber % 8 >= 1)
            gameMode = "Berserk";
        else
            gameMode = "Stealth";
        animalCount = GameObject.Find("Manager").GetComponent<Manager>().animalCounts[levelNumber];
        transform.position = new Vector3(0, 0.5f, -15);
        transform.rotation = Quaternion.identity;
        loadingPanel.SetActive(true);
        UIBG.gameObject.SetActive(false);
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
                other.gameObject.GetComponent<Animal>().saved();
                breakLock.Play();
                animalCount--;
                break;
            case "Enemy":
                levelEnd(false);
                break;
            case "Exit":
                if (animalCount == 0)
                    levelEnd(true);
                break;
            case "Key":
                other.gameObject.SetActive(false);
                break;
            case "BronzeBanana":
                other.gameObject.SetActive(false);
                GetComponent<Powerups>().setBananaCount(1);
                break;
            case "SilverBanana":
                other.gameObject.SetActive(false);
                GetComponent<Powerups>().setBananaCount(2);
                break;
            case "GoldBanana":
                other.gameObject.SetActive(false);
                GetComponent<Powerups>().setBananaCount(3);
                break;
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
        UIBG.sprite = UIBGSprites[levelNumber / 16];
        UIBG.gameObject.SetActive(true);
        successPanel.SetActive(success);
        failPanel.SetActive(!success);
        joystick.SetActive(false);
        enabled = false;
    }
}