using System;
using System.Collections;
using UnityEngine;
public class CameraRotate : MonoBehaviour
{
    [SerializeField] private int[] directions;
    [SerializeField] private int[] turnTimes;
    [SerializeField] private int[] waitTimes;
    [SerializeField] private float rotSpeed;
    private bool isWaiting;
    private int nodeNumber;
    private void Start()
    {
        isWaiting = false;
        nodeNumber = 0;
        if (directions.Length != 0)
            StartCoroutine(turnTime());
    }
    private void Update()
    {
        if (!isWaiting && directions.Length != 0)
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + directions[nodeNumber] * rotSpeed * Time.deltaTime, 0);
    }
    IEnumerator turnTime()
    {
        yield return new WaitForSeconds(turnTimes[nodeNumber]);
        isWaiting = true;
        StartCoroutine(waitTime());
    }
    IEnumerator waitTime()
    {
        yield return new WaitForSeconds(waitTimes[nodeNumber]);
        nodeNumber = ++nodeNumber == directions.Length ? 0 : nodeNumber;
        isWaiting = false;
        StartCoroutine(turnTime());
    }
}
