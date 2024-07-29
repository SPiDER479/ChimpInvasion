using System;
using UnityEngine;
public class Manager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    public GameObject[] levels;
    public int level;
    public int[] animalCounts;
    void Start()
    {
        level = levelData.currentLevel;
        Instantiate(levels[level], transform.position, transform.rotation);
    }
}
