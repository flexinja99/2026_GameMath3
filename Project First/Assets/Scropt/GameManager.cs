using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class GameManager : MonoBehaviour
{
    [Header("Player & Enemy Stats")]
    public float enemyMaxHP = 300f;
    private float currentEnemyHP;
    public int playerDamage = 30;

    [Header("Critical System (Practice 3)")]
    public int totalHits = 0;
    public int critHits = 0;
    public float targetCritRate = 0.3f; 

    [Header("Loot System (Assignment)")]
    public float[] baseLootRates = { 50f, 30f, 15f, 5f }; 
    private float[] currentLootRates = new float[4];
    public int[] inventory = new int[4];

    [Header("UI References")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI lootRateText;
    public TextMeshProUGUI inventoryText;

    void Start()
    {
        currentEnemyHP = enemyMaxHP;
        System.Array.Copy(baseLootRates, currentLootRates, 4);
        UpdateUI();
    }

    // 공격 버튼에 연결할 함수
    public void OnAttackButton()
    {
        totalHits++;
        bool isCrit = CheckCritical();

        int damageDealt = isCrit ? playerDamage * 2 : playerDamage;
        currentEnemyHP -= damageDealt;

        if (currentEnemyHP <= 0)
        {
            currentEnemyHP = 0;
            UpdateUI();
            OnEnemyDeath();
        }

        UpdateUI();
    }

    // 실습 3: 확률 보정 치명타 로직
    bool CheckCritical()
    {
        float currentRate = (totalHits > 1) ? (float)critHits / (totalHits - 1) : 0f;

        
        if (currentRate < targetCritRate && (float)(critHits + 1) / totalHits <= targetCritRate)
        {
            critHits++;
            Debug.Log("Critical Hit! (Forced)");
            return true;
        }
      
        if (currentRate > targetCritRate && (float)critHits / totalHits >= targetCritRate)
        {
            Debug.Log("Normal Hit! (Forced)");
            return false;
        }
       
        if (Random.value < targetCritRate)
        {
            critHits++;
            Debug.Log("Critical Hit! (Base)");
            return true;
        }

        return false;
    }

    void OnEnemyDeath()
    {
        
        float roll = Random.Range(0f, 100f);
        float cumulative = 0f;
        int selectedIndex = -1;

        for (int i = 0; i < currentLootRates.Length; i++)
        {
            cumulative += currentLootRates[i];
            if (roll <= cumulative)
            {
                selectedIndex = i;
                break;
            }
        }

        
        if (selectedIndex == 3) 
        {
            System.Array.Copy(baseLootRates, currentLootRates, 4);
            inventory[3]++;
            Debug.Log("전설 아이템 획득! 확률 초기화");
        }
        else 
        {
            if (selectedIndex != -1) inventory[selectedIndex]++;

            
            currentLootRates[3] += 1.5f;
            for (int i = 0; i < 3; i++)
            {
                currentLootRates[i] -= 0.5f;
            }
        }

        // 적 부활
        currentEnemyHP = enemyMaxHP;
        Debug.Log("새로운 적 등장!");
    }

    void UpdateUI()
    {
       
        statusText.text = $"전체 공격 횟수: {totalHits}\n" +
                          $"발생한 치명타: {critHits}\n" +
                          $"실제 치명타 확률: {(totalHits > 0 ? (float)critHits / totalHits * 100 : 0):F1}%";

        
        hpText.text = $"적 HP: {currentEnemyHP} / {enemyMaxHP}";

        
        lootRateText.text = $"현재 아이템 확률\n일반: {currentLootRates[0]:F1}% | 고급: {currentLootRates[1]:F1}%\n" +
                            $"희귀: {currentLootRates[2]:F1}% | 전설: {currentLootRates[3]:F1}%";

       
        inventoryText.text = $"획득 아이템\n일반:{inventory[0]} 고급:{inventory[1]} 희귀:{inventory[2]} 전설:{inventory[3]}";
    }
}