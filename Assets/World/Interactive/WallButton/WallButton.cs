using System;
using UnityEngine;
public class WallButton : MonoBehaviour
{
    private AudioSource buttonPress;
    [SerializeField] private GameObject[] walls;
    private void Start()
    {
        buttonPress = GetComponent<AudioSource>();
    }
    public void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            buttonPress.Play();
            foreach (GameObject wall in walls)
                wall.SetActive(!wall.activeSelf);
        }
    }
}
