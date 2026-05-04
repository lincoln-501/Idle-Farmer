using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Producer : MonoBehaviour
{
    public double rewardPerCycle = 10;
    public float cycleTime = 3f;
    public double unitLevel = 1;

    [Header("Unit Settings")]
    public string unitName = "Cows";

    [Header("Costs")]
    public double unlockCost = 100;
    public double rewardUpgradeCost = 50;
    public double speedUpgradeCost = 75;
    public double farmerCost = 500;

    [Header("Upgrades")]
    public double rewardIncrease = 5;
    public float speedMultiplier = 0.9f;

    [Header("UI Texts")]
    public TextMeshProUGUI rewardUpgradeCostText;
    public TextMeshProUGUI speedUpgradeCostText;
    public TextMeshProUGUI farmerCostText;
    public TextMeshProUGUI rewardPerCycleText;
    public  TextMeshProUGUI unitLevelText;

    private float timer = 0f;
    private bool isRunning = false;
    private bool unlocked = false;
    private bool autoMode = false;

    public Image progressBar;
    public GameObject unlockButton;
    public GameObject buyFarmerButton;

    private GameManager gameManager;



    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (!unlocked) return;

        if (isRunning)
        {
            timer += Time.deltaTime;

            if (cycleTime > 0.2)
            {
                progressBar.fillAmount = Mathf.Clamp01(timer / cycleTime);
            } else
            {
                progressBar.fillAmount = 1;
            }
                

            if (timer >= cycleTime)
            {
                CompleteCycle();
            }
        }
        UpdateUI();
    }

    void CompleteCycle()
    {
        gameManager.AddMoney(rewardPerCycle);
        timer = 0f;

        if (cycleTime > 0.2)
        {
        progressBar.fillAmount = 0f;
        }

        if (autoMode)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        UpdateUI();
    }

    public void StartCycle()
    {
        if (!isRunning)
        {
            timer = 0f;
            isRunning = true;
        }
    }

    // UNLOCK
    public void Unlock()
    {
        if (gameManager.CanAfford(unlockCost))
        {
            gameManager.SpendMoney(unlockCost);

            unlocked = true;
            unlockButton.SetActive(false);
        }
    }

    // BUY REWARD UPGRADE
    public void BuyRewardUpgrade()
    {
        if (gameManager.CanAfford(rewardUpgradeCost))
        {
            gameManager.SpendMoney(rewardUpgradeCost);

            rewardPerCycle += rewardIncrease;
            rewardUpgradeCost *= 1.2;
            unitLevel++;
            CheckMilestone();
        }
        
        UpdateUI();
    }

    // BUY SPEED UPGRADE
    public void BuySpeedUpgrade()
    {
        if (gameManager.CanAfford(speedUpgradeCost))
        {
            gameManager.SpendMoney(speedUpgradeCost);

            cycleTime *= speedMultiplier;
            speedUpgradeCost *= 1.3;
        }
        UpdateUI();
    }

    // BUY FARMER 
    public void BuyFarmer()
    {
        if (gameManager.CanAfford(farmerCost))
        {
            gameManager.SpendMoney(farmerCost);

            autoMode = true;
            StartCycle();
            buyFarmerButton.SetActive(false);
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        
        rewardUpgradeCostText.text = "Buy:  $" + FormatNumber(rewardUpgradeCost);    
        speedUpgradeCostText.text = "Upgrade Speed : $" + FormatNumber(speedUpgradeCost);
        farmerCostText.text = " Buy Farmer: $" + FormatNumber(farmerCost);
        unitLevelText.text = unitName + ": " + FormatNumber(unitLevel);
        if (cycleTime > 0.2)
        {
            rewardPerCycleText.text = FormatNumber(rewardPerCycle);
        } else
        {
            rewardPerCycleText.text = FormatNumber(rewardPerCycle/cycleTime) + "/sec";
        }
        

    }

    void CheckMilestone()
    {
        if (unitLevel == 10 || unitLevel == 25 || unitLevel == 50 || unitLevel%50 == 0)
        {
            rewardIncrease *= 2;
            rewardPerCycle *= 2;
        }
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