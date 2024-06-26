using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;
public class MainCamera : MonoBehaviour
{
    private GameObject player;
    private CinemachineFreeLook cfl;
    private CinemachineCameraOffset cco;
    private void Start()
    {
        player = GameObject.Find("Player");
        cfl = GetComponent<CinemachineFreeLook>();
        cco = GetComponent<CinemachineCameraOffset>();
        setup();
    }
    public void setup()
    {
        cfl.LookAt = player.transform;
        cfl.Follow = player.transform;
        cfl.m_Orbits[1].m_Radius = 20;
        cfl.m_Orbits[1].m_Height = 150;
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
        if (cfl.m_Orbits[1].m_Height > 100)
        {
            cfl.m_Orbits[1].m_Height -= 0.5f;
            StartCoroutine(setCam());
        }
    }
    public void enemyKill(Transform enemy)
    {
        Time.timeScale = 0.1f;
        cfl.LookAt = enemy;
        cfl.Follow = enemy;
        cfl.m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
        cfl.m_Orbits[1].m_Radius = 10;
        cfl.m_Orbits[1].m_Height = 20;
        StartCoroutine(slowMotion());
        StartCoroutine(spin(Random.Range(0, 2)));
    }
    IEnumerator slowMotion()
    {
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1;
        cfl.LookAt = player.transform;
        cfl.Follow = player.transform;
        cfl.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
        cfl.m_Orbits[1].m_Radius = 20;
        cfl.m_Orbits[1].m_Height = 100;
        cco.m_Offset = Vector3.zero;
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
