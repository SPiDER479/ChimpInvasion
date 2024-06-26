using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Keypad : MonoBehaviour
{
    private Canvas canvas;
    private AudioSource keySound, correctCode;
    private TextMeshProUGUI display;
    [SerializeField] private string code;
    [SerializeField] private GameObject[] walls;
    private void Start()
    {
        correctCode = GetComponent<AudioSource>();
        display = GetComponentInChildren<TextMeshProUGUI>();
        keySound = GetComponentInChildren<AudioSource>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.worldCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        canvas.planeDistance = 2;
        canvas.enabled = false;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player") && enabled)
            canvas.enabled = true;
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Player") && enabled)
            canvas.enabled = false;
    }
    public void buttonPress(Button button)
    {
        string s = button.name.Substring(6);
        if (s == "Enter")
        {
            if (display.text == code)
            {
                correctCode.Play();
                foreach (GameObject wall in walls)
                    wall.SetActive(!wall.activeSelf);
                canvas.enabled = false;
                enabled = false;
            }
            else
            {
                keySound.Play();
                display.text = "";
            }
        }
        else
        {
            keySound.Play();
            if (s == "Back" && display.text.Length > 0)
                display.text = display.text.Substring(0, display.text.Length - 1);
            else if (s != "Back" && display.text.Length < code.Length)
                display.text += s;
        }
        
    }
}
