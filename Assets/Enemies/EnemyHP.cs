using UnityEngine;
public class EnemyHP : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private PowerupData pud;
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Knife"))
        {
            health -= 10 * pud.knifeDamageMultiplier;
            if (health <= 0)
            {
                GameObject.Find("Virtual Camera").GetComponent<MainCamera>().enemyKill(transform);
                Invoke(nameof(disable), 0.11f);
            }
        }
    }
    private void disable()
    {
        gameObject.SetActive(false);
    }
}