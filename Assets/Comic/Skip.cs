using UnityEngine;
using UnityEngine.SceneManagement;
public class Skip : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(skip), 100);
    }
    public void skip()
    {
        SceneManager.LoadScene("MainMenu");
    }
}