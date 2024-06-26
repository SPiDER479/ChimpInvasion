using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI : MonoBehaviour
{
    public void nextScene()
    {
        GameObject o = GameObject.FindGameObjectWithTag("Level");
        string s = o.name;
        int k = int.Parse(s.Substring(5, s.Length - 12));
        StreamReader r = new StreamReader("level.txt");
        if (Int32.Parse(r.ReadLine()) <= k)
        {
            r.Close();
            StreamWriter w = new StreamWriter("level.txt");
            w.Write(k + 1);
            w.Close();
        }
        else
            r.Close();
        StreamWriter ww = new StreamWriter("currentlevel.txt");
        ww.Write(k + 1);
        ww.Close();
        if (k + 1 == GameObject.Find("Manager").GetComponent<Manager>().levels.Length)
            SceneManager.LoadScene("MainMenu");
        else
        {
            GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
            GameObject.Find("Player").GetComponent<PlayerMovement>().setup(1);
            Instantiate(GameObject.Find("Manager").GetComponent<Manager>().levels[k + 1], Vector3.zero,
                Quaternion.identity);
            Destroy(o);
            
            GameObject.Find("FreeLook Camera").GetComponent<MainCamera>().setup();
        }
    }
    public void reloadScene()
    {
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
        GameObject.Find("Player").GetComponent<PlayerMovement>().setup(0);
        GameObject o = GameObject.FindGameObjectWithTag("Level");
        string s = o.name;
        int k = int.Parse(s.Substring(5, s.Length - 12));
        Instantiate(GameObject.Find("Manager").GetComponent<Manager>().levels[k], Vector3.zero,
            Quaternion.identity);
        Destroy(o);
        
        GameObject.Find("FreeLook Camera").GetComponent<MainCamera>().setup();
    }
    public void sucessMainMenu()
    {
        GameObject o = GameObject.FindGameObjectWithTag("Level");
        string s = o.name;
        int k = int.Parse(s.Substring(5, s.Length - 12));
        StreamReader r = new StreamReader("level.txt");
        if (Int32.Parse(r.ReadLine()) <= k)
        {
            r.Close();
            StreamWriter w = new StreamWriter("level.txt");
            w.Write(k + 1);
            w.Close();
        }
        else
            r.Close();
        SceneManager.LoadScene("MainMenu");
    }
    public void failMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
