using System;
using UnityEngine;
public class Door : MonoBehaviour
{
    private GameObject doorway;
    [SerializeField] private GameObject key;
    private void Start()
    {
        doorway = GetComponentInChildren<MeshRenderer>().gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !key.activeSelf)
            doorway.transform.localRotation = Quaternion.Euler(0, doorway.transform.localRotation.eulerAngles.y + 90, 0);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !key.activeSelf)
            doorway.transform.localRotation = Quaternion.Euler(0, doorway.transform.localRotation.eulerAngles.y - 90, 0);
    }
}
