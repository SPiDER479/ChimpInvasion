using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyPlayerSpotted : MonoBehaviour
{
    private NavMeshAgent nva;
    [SerializeField] private float spottedMoveSpeed;
    private Animator anim;
    private EnemyDefault ed;
    private int rotateDirection;
    private bool turretingOn;
    private GameObject player;
    private void Awake()
    {
        player = GameObject.Find("Player");
        anim = GetComponentInChildren<Animator>();
        rotateDirection = 0;
        ed = GetComponent<EnemyDefault>();
    }
    private void OnEnable()
    {
        turretingOn = false;
        ed.enabled = false;
        anim.SetBool("Moving", true);
        nva = GetComponent<NavMeshAgent>();
        nva.stoppingDistance = 0;
        nva.speed = spottedMoveSpeed;
        nva.SetDestination(player.transform.position);
    }
    private void Update()
    {
        if (transform.position == nva.destination)
        {
            anim.SetBool("Moving", false);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 
                transform.rotation.eulerAngles.y + rotateDirection * Time.deltaTime, transform.rotation.eulerAngles.z);
            if (!turretingOn)
            {
                turretingOn = true;
                StartCoroutine(changeRotationDirection());
                StartCoroutine(wait());
            }
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(5);
        enabled = false;
        ed.enabled = true;
    }
    IEnumerator changeRotationDirection()
    {
        yield return new WaitForSeconds(2);
        switch (rotateDirection)
        {
            case 0:
                rotateDirection = 20;
                break;
            case 20:
                rotateDirection = -20;
                break;
            case -20:
                rotateDirection = 0;
                break;
        }
        StartCoroutine(changeRotationDirection());
    }
    private void OnDisable()
    {
        anim.SetBool("Moving", true);
        StopAllCoroutines();
    }
}
