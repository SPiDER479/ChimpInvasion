using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class KnifePool : MonoBehaviour
{
    private TextMeshProUGUI knifeCountText;
    public static KnifePool SharedInstance;
    [SerializeField] private List<GameObject> knifePool;
    [SerializeField] private GameObject knife;
    [SerializeField] private int count;
    void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {
        GetComponentInChildren<Canvas>().worldCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        GetComponentInChildren<Canvas>().planeDistance = 1;
        if (GameObject.Find("Player").GetComponent<PlayerMovement>().gameMode == "Berserk")
        {
            if (count == 0)
                GetComponentInChildren<Canvas>().enabled = false;
            else
            {
                knifeCountText = GetComponentInChildren<TextMeshProUGUI>();
                knifeCountText.text = count.ToString();
                for (int i = 0; i < count; i++)
                    knifePool.Add(Instantiate(knife, transform));
                foreach (GameObject knife in knifePool)
                    knife.SetActive(false);
            }
        }
        else if (GameObject.Find("Player").GetComponent<PlayerMovement>().gameMode == "Stealth")
        {
            count = 0;
            GetComponentInChildren<Canvas>().enabled = false;
        }
    }
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < count; i++)
        {
            if (!knifePool[i].activeInHierarchy)
            {
                knifeCountText.text = (Int32.Parse(knifeCountText.text) - 1).ToString();
                StartCoroutine(knifeCount());
                return knifePool[i];
            }
        }
        return null;
    }
    IEnumerator knifeCount()
    {
        yield return new WaitForSeconds(5);
        knifeCountText.text = (Int32.Parse(knifeCountText.text) + 1).ToString();
    }
}
