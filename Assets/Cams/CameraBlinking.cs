using System;
using System.Collections;
using UnityEngine;
public class CameraBlinking : MonoBehaviour
{
    [SerializeField] private int blinkRate;
    [SerializeField] private GameObject sight;
    private void Awake()
    {
        if (blinkRate > 0)
            StartCoroutine(blinking());
    }
    IEnumerator blinking()
    {
        yield return new WaitForSeconds(blinkRate);
        sight.SetActive(!sight.activeSelf);
        GetComponent<CameraSight>().enabled = !GetComponent<CameraSight>().enabled;
        StartCoroutine(blinking());
    }
}