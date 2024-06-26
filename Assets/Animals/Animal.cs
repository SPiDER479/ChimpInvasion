using System.Collections;
using UnityEngine;
public class Animal : MonoBehaviour
{
    public void saved()
    {
        GetComponent<BoxCollider>().enabled = false;
        transform.position += new Vector3(0, 5, 0);
        StartCoroutine(spin(0, 0.01f));
    }
    IEnumerator spin(int temp, float y)
    {
        yield return new WaitForSeconds(0.01f);
        transform.position += new Vector3(0, y, 0);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 5, 0);
        if (++temp >= 200)
        {
            if (transform.position.y > 50)
                gameObject.SetActive(false);
            else
                StartCoroutine(spin(200, 1));
        }
        else
            StartCoroutine(spin(temp, 0.01f));
    }
}
