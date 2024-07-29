using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    [SerializeField] private Image[] bgEnvironments;
    [SerializeField] private Image levelsPanelBackground;
    [SerializeField] private Slider slider;
    [SerializeField] private Image[] environments;
    [SerializeField] private LevelData levelData;
    [SerializeField] private PowerupData powerupData;
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private GameObject[] levelDisplays;
    private GameObject homePanel, levelsPanel, loadingPanel, environmentsPanel;
    private AudioSource clickSound;
    [SerializeField] private Sprite[] titleSprites;
    [SerializeField] private Image titleSprite;
    private int k, j;
    private void Start()
    {
        slider.minValue = (environments.Length - 1) * -400;
        clickSound = GetComponent<AudioSource>();
        homePanel = GameObject.Find("HomePanel");
        levelsPanel = GameObject.Find("LevelsPanel");
        loadingPanel = GameObject.Find("LoadingPanel");
        environmentsPanel = GameObject.Find("EnvironmentsPanel");
        k = 0;
        readFile();
        levelData.currentLevel = 0;
        environmentsPanel.SetActive(false);
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
    public void changeEnvironment()
    {
        for (int i = 0; i < environments.Length; i++)
        {
            float x = slider.value + i * 400;
            environments[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
            if (Mathf.Abs(400 - x) > 0)
            {
                environments[i].GetComponent<RectTransform>().localScale = new Vector3((400 - Mathf.Abs(x)) / 400 * 4,
                    (400 - Mathf.Abs(x)) / 400 * 4, 1);
                bgEnvironments[i].color = new Color(1, 1, 1, (400 - Mathf.Abs(x)) / 400);
            }
        }
    }
    public void openHome()
    {
        environmentsPanel.SetActive(false);
        homePanel.SetActive(true);
        clickSound.Play();
    }
    public void openEnvironments()
    {
        homePanel.SetActive(false);
        levelsPanel.SetActive(false);
        environmentsPanel.SetActive(true);
        clickSound.Play();
    }
    public void openLevels(int i)
    {
        k = i;
        environmentsPanel.SetActive(false);
        levelsPanelBackground.sprite = bgEnvironments[k].sprite;
        levelsPanel.SetActive(true);
        foreach (GameObject o in levelDisplays)
            o.SetActive(false);
        levelDisplays[k].SetActive(true);
        titleSprite.sprite = titleSprites[k];
        clickSound.Play();
    }
    public void openLevel(GameObject o)
    {
        levelData.currentLevel = Int32.Parse(o.name.Substring(5));
        clickSound.Play();
        SceneManager.LoadScene("Game");
    }
    public void reset()
    {
        clickSound.Play();
        levelData.level = 1;
        powerupData.bananaCount = 0;
        readFile();
    }
    private void readFile()
    {
        j = levelData.level;
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
                levelButtons[i].interactable = false;
        }
    }
    public void moveSlider(int i)
    {
        StopAllCoroutines();
        float targetValue = slider.value + 400 * i;
        if (targetValue >= slider.minValue && targetValue <= 0)
            StartCoroutine(sliderMove(i, targetValue));
    }
    IEnumerator sliderMove(int i, float target)
    {
        yield return new WaitForSeconds(0.001f);
        slider.value += i * 10;
        if (slider.value < target && i > 0)
            StartCoroutine(sliderMove(i, target));
        else if (slider.value > target && i < 0)
            StartCoroutine(sliderMove(i, target));
        else
            StopAllCoroutines();
    }
}