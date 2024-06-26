using UnityEngine;
public class EnemyHP : MonoBehaviour
{
    [SerializeField] private int health;
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Knife"))
        {
            health -= 10;
            if (health <= 0)
            {
                GameObject.Find("FreeLook Camera").GetComponent<MainCamera>().enemyKill(transform);
                gameObject.SetActive(false);
            }
        }
    }
}
