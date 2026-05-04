using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public double money = 100;

    [Header("UI")]
    public TextMeshProUGUI moneyText;

    void Update()
    {
        moneyText.text = "Money: " + FormatNumber(money);
    }

    public bool CanAfford(double cost)
    {
        return money >= cost;
    }

    public void SpendMoney(double amount)
    {
        money -= amount;
    }

    public void AddMoney(double amount)
    {
        money += amount;
    }

    string FormatNumber(double value)
    {
        if (value >= 1_000_000)
            return (value / 1_000_000).ToString("F1") + "M";
        if (value >= 1_000)
            return (value / 1_000).ToString("F1") + "K";

        return value.ToString("F0");
    }
}