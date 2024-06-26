using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyChase : MonoBehaviour
{
    private NavMeshAgent nva;
    private GameObject player;
    [SerializeField] private float chaseMoveSpeed;
    private Animator anim;
    private AudioSource spottedSound;
    private EnemyDefault ed;
    private EnemyPlayerSpotted eps;
    private void Awake()
    {
        spottedSound = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        anim = GetComponentInChildren<Animator>();
        ed = GetComponent<EnemyDefault>();
        eps = GetComponent<EnemyPlayerSpotted>();
    }
    void OnEnable()
    {
        ed.enabled = false;
        eps.enabled = false;
        anim.SetBool("Moving", true);
        nva = GetComponent<NavMeshAgent>();
        nva.speed = chaseMoveSpeed;
        nva.stoppingDistance = 1.5f;
        StartCoroutine(speedIncrease());
        spottedSound.Play();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        eps.enabled = true;
    }
    private void Update()
    {
        nva.SetDestination(player.transform.position);
    }
    IEnumerator speedIncrease()
    {
        yield return new WaitForSeconds(30);
        nva.speed += 2;
        if (nva.speed < chaseMoveSpeed + 4)
            StartCoroutine(speedIncrease());
    }
}