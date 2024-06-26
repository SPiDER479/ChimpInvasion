using System;
using UnityEngine;
public class Roof : MonoBehaviour
{
    [SerializeField] private Material opaqueMat, transparentMat;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GetComponent<MeshRenderer>().material = transparentMat;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            GetComponent<MeshRenderer>().material = opaqueMat;
    }
}
