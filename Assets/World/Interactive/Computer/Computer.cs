using UnityEngine;
public class Computer : MonoBehaviour
{
    private AudioSource keyboardClicks;
    [SerializeField] private GameObject[] cams;
    private void Start()
    {
        keyboardClicks = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player") && enabled)
        {
            keyboardClicks.Play();
            keyboardClicks.time = 14;
            foreach (GameObject cam in cams)
                cam.SetActive(false);
            enabled = false;
        }
    }
}
