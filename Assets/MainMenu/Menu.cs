using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private GameObject[] levelDisplays;
    [SerializeField] private Material[] mats;
    private Image backgroundImage;
    private GameObject homePanel, levelsPanel, loadingPanel;
    private AudioSource clickSound;
    private TextMeshProUGUI titleText;
    private string[] titles;
    private int k, j;
    private void Start()
    {
        backgroundImage = GetComponentInChildren<Image>();
        clickSound = GetComponent<AudioSource>();
        titleText = GameObject.Find("PageName").GetComponent<TextMeshProUGUI>();
        homePanel = GameObject.Find("HomePanel");
        levelsPanel = GameObject.Find("LevelsPanel");
        loadingPanel = GameObject.Find("LoadingPanel");
        titles = new[] {"Forest", "Suburbs", "Warehouses", "Offices", "Space"};
        k = 0;
        backgroundImage.material = mats[0];
        if (!File.Exists("level.txt"))
            reset();
        else
            readFile();
        StreamWriter w = new StreamWriter("currentlevel.txt");
        w.Write(0);
        w.Close();
        levelsPanel.SetActive(false);
        homePanel.SetActive(false);
        loadingPanel.SetActive(true);
        StartCoroutine(loading());
    }
    IEnumerator loading()
    {
        yield return new WaitForSeconds(1);
        loadingPanel.SetActive(false);
        homePanel.SetActive(true);
    }
    public void changeDisplay(int i)
    {
        clickSound.Play();
        if (k + i < levelDisplays.Length && k + i >= 0)
        {
            levelDisplays[k].SetActive(false);
            k += i;
            backgroundImage.material = mats[k + 1];
            levelDisplays[k].SetActive(true);
            titleText.text = titles[k];
        }
    }
    public void openLevels()
    {
        homePanel.SetActive(false);
        levelsPanel.SetActive(true);
        backgroundImage.material = mats[k + 1];
        foreach (GameObject o in levelDisplays)
            o.SetActive(false);
        levelDisplays[k].SetActive(true);
        titleText.text = titles[k];
        clickSound.Play();
    }
    public void openHome()
    {
        backgroundImage.material = mats[0];
        homePanel.SetActive(true);
        levelsPanel.SetActive(false);
        clickSound.Play();
    }
    public void openLevel(GameObject o)
    {
        StreamWriter w = new StreamWriter("currentlevel.txt");
        w.Write(Int32.Parse(o.name.Substring(5)));
        w.Close();
        clickSound.Play();
        SceneManager.LoadScene("Game");
    }
    public void reset()
    {
        clickSound.Play();
        StreamWriter w = new StreamWriter("level.txt");
        w.Write(1);
        w.Close();
        readFile();
    }
    private void readFile()
    {
        StreamReader r = new StreamReader("level.txt");
        j = Int32.Parse(r.ReadLine());
        r.Close();
        for (int i = 1; i < levelButtons.Length; i++)
        {
            if (i < j)
            {
                levelButtons[i].interactable = true;
                if (i % 8 <= 4 && i % 8 >= 1)
                    levelButtons[i].GetComponent<Image>().color = Color.yellow;
                else
                    levelButtons[i].GetComponent<Image>().color = Color.cyan;
            }
            else if (i == j)
            {
                levelButtons[i].interactable = true;
                levelButtons[i].GetComponent<Image>().color = Color.green;
            }
            else
            {
                levelButtons[i].interactable = false;
                levelButtons[i].GetComponent<Image>().color = Color.grey;
            }
        }
    }
}
