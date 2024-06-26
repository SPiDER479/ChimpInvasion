using System;
using System.IO;
using UnityEngine;
public class Manager : MonoBehaviour
{
    public GameObject[] levels;
    public int level;
    public int[] animalCounts;
    void Start()
    {
        StreamReader r = new StreamReader("currentlevel.txt");
        level = int.Parse(r.ReadLine());
        r.Close();
        Instantiate(levels[level], transform.position, transform.rotation);
    }
}
