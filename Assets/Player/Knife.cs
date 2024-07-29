using System;
using System.Collections;
using UnityEngine;
public class Knife : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private ParticleSystem bloodFX;
    private bool impactSoundPlayed;
    private void OnEnable()
    {
        GetComponent<Collider>().enabled = true;
        body.SetActive(true);
        StartCoroutine(destroy());
        impactSoundPlayed = false;
    }
    private void Update()
    {
        transform.position += 15 * Time.deltaTime * transform.forward;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy") || other.collider.CompareTag("Wall") || other.collider.CompareTag("Window"))
        {
            if (other.collider.CompareTag("Enemy"))
                bloodFX.Play();
            GetComponent<Collider>().enabled = false;
            body.SetActive(false);
            if (!impactSoundPlayed)
            {
                hitSound.Play();
                impactSoundPlayed = true;
            }
        }
    }
    IEnumerator destroy()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}