using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyDefault : MonoBehaviour
{
    private NavMeshAgent nva;
    private int nodeNumber;
    [SerializeField] private Vector3[] defaultPath;
    [SerializeField] private int[] delays;
    [SerializeField] private float defaultMoveSpeed;
    private Animator anim;
    private EnemyPlayerSpotted eps;
    private EnemyChase ec;
    private bool delayStarted;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        eps = GetComponent<EnemyPlayerSpotted>();
        ec = GetComponent<EnemyChase>();
        nodeNumber = 0;
    }
    private void OnEnable()
    {
        delayStarted = false;
        ec.enabled = false;
        eps.enabled = false;
        nva = GetComponent<NavMeshAgent>();
        nva.speed = defaultMoveSpeed;
        nva.stoppingDistance = 0;
        nva.SetDestination(defaultPath[nodeNumber]);
    }
    private void Update()
    {
        if (!nva.hasPath && !delayStarted)
        {
            delayStarted = true;
            anim.SetBool("Moving", false);
            StartCoroutine(delay(delays[nodeNumber]));
        }
    }
    IEnumerator delay(int time)
    {
        yield return new WaitForSeconds(time);
        nodeNumber = ++nodeNumber == defaultPath.Length ? 0 : nodeNumber;
        if (defaultPath.Length != 1)
            anim.SetBool("Moving", true);
        nva.SetDestination(defaultPath[nodeNumber]);
        delayStarted = false;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}