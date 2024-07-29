using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public class Powerups : MonoBehaviour
{
    [SerializeField] private PowerupData pud;
    [SerializeField] private GameObject powerPanel, microPanel;
    [SerializeField] private TextMeshProUGUI bananaCount;
    private const int powerupCost = 50;
    private void Awake()
    {
        powerPanel.SetActive(false);
        microPanel.SetActive(false);
        setBananaCount(0);
    }
    public void openInventory(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Time.timeScale = Time.timeScale > 0 ? 0 : 1;
            powerPanel.SetActive(!powerPanel.activeSelf);
        }
    }
    public void radiusIncrease(InputAction.CallbackContext ctx)
    {
        if (ctx.started && powerPanel.activeSelf && pud.bananaCount >= powerupCost)
        {
            pud.radius *= 2;
            setBananaCount(-powerupCost);
        }
    }
    public void angleIncrease(InputAction.CallbackContext ctx)
    {
        if (ctx.started && powerPanel.activeSelf && pud.bananaCount >= powerupCost)
        {
            if ((pud.angle *= 2) > 360)
                pud.angle = 360;
            setBananaCount(-powerupCost);
        }
    }
    public void enemyRadiusDecrease(InputAction.CallbackContext ctx)
    {
        if (ctx.started && powerPanel.activeSelf && pud.bananaCount >= powerupCost)
        {
            pud.enemyRadiusDecrease *= 2;
            setBananaCount(-powerupCost);
        }
    }
    public void knifeDamageIncrease(InputAction.CallbackContext ctx)
    {
        if (ctx.started && powerPanel.activeSelf && pud.bananaCount >= powerupCost)
        {
            pud.knifeDamageMultiplier *= 2;
            setBananaCount(-powerupCost);
        }
    }
    public void setBananaCount(int changeAmount)
    {
        pud.bananaCount += changeAmount;
        bananaCount.text = pud.bananaCount.ToString();
    }
    public void openMicrotransactions()
    {
        powerPanel.SetActive(!powerPanel.activeSelf);
        microPanel.SetActive(!microPanel.activeSelf);
    }
    public void videoBananas(int bananaAmount)
    {
        setBananaCount(bananaAmount); //watch video for *bananaAmount* bananas
    }
    public void buyBananas(int bananaAmount)
    {
        int money = 0;
        switch (bananaAmount)
        {
            case 270:
                money = 50;
                break;
            case 560:
                money = 100;
                break;
            case 1180:
                money = 200;
                break;
        }
        setBananaCount(bananaAmount); //buy *bananaAmount* bananas for *money*
    }
}