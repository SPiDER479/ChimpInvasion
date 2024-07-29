using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    public void nextScene()
    {
        GameObject o = GameObject.FindGameObjectWithTag("Level");
        string s = o.name;
        int k = int.Parse(s.Substring(5, s.Length - 12));
        if (levelData.level <= k)
            levelData.level = k + 1;
        levelData.currentLevel = k + 1;
        if (k + 1 == GameObject.Find("Manager").GetComponent<Manager>().levels.Length)
            SceneManager.LoadScene("MainMenu");
        else
        {
            GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
            GameObject.Find("Player").GetComponent<PlayerMovement>().setup();
            Instantiate(GameObject.Find("Manager").GetComponent<Manager>().levels[k + 1], Vector3.zero,
                Quaternion.identity);
            Destroy(o);
            GameObject.Find("Virtual Camera").GetComponent<MainCamera>().setup();
        }
    }
    public void reloadScene()
    {
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
        GameObject.Find("Player").GetComponent<PlayerMovement>().setup();
        GameObject o = GameObject.FindGameObjectWithTag("Level");
        string s = o.name;
        int k = int.Parse(s.Substring(5, s.Length - 12));
        Instantiate(GameObject.Find("Manager").GetComponent<Manager>().levels[k], Vector3.zero,
            Quaternion.identity);
        Destroy(o);
        GameObject.Find("Virtual Camera").GetComponent<MainCamera>().setup();
    }
    public void sucessMainMenu()
    {
        GameObject o = GameObject.FindGameObjectWithTag("Level");
        string s = o.name;
        int k = int.Parse(s.Substring(5, s.Length - 12));
        if (levelData.level <= k)
            levelData.level = k + 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void failMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
