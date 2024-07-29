using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;
public class MainCamera : MonoBehaviour
{
    private GameObject player;
    private CinemachineVirtualCamera cvc;
    private CinemachineCameraOffset cco;
    private void Start()
    {
        player = GameObject.Find("Player");
        cvc = GetComponent<CinemachineVirtualCamera>();
        cco = GetComponent<CinemachineCameraOffset>();
        setup();
    }
    public void setup()
    {
        cvc.LookAt = player.transform;
        cvc.Follow = player.transform;
        cco.m_Offset = new Vector3(0, 100, 0);
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(setCam());
    }
    IEnumerator setCam()
    {
        yield return new WaitForSeconds(0.01f);
        if (cco.m_Offset.y > 70)
        {
            cco.m_Offset.y -= 0.5f;
            StartCoroutine(setCam());
        }
    }
    public void enemyKill(Transform enemy)
    {
        Time.timeScale = 0.1f;
        cvc.LookAt = enemy;
        cvc.Follow = enemy;
        cco.m_Offset.y = 20;
        StartCoroutine(slowMotion());
        StartCoroutine(spin(Random.Range(0, 2)));
    }
    IEnumerator slowMotion()
    {
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1;
        cvc.LookAt = player.transform;
        cvc.Follow = player.transform;
        cco.m_Offset = new Vector3(0, 70, 0);
        StopAllCoroutines();
    }
    IEnumerator spin(int direction)
    {
        yield return new WaitForSeconds(0.001f);
        if (direction == 0)
            cco.m_Offset += new Vector3(-0.1f, 0, 0);
        else
            cco.m_Offset += new Vector3(0.1f, 0, 0);
        StartCoroutine(spin(direction));
    }
}
