using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // UI 출력을 위해 필요

public class TurnBasedGame : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float critChance = 0.2f;
    [SerializeField] float meanDamage = 20f;
    [SerializeField] float stdDevDamage = 5f;
    [SerializeField] float enemyHP = 100f;
    [SerializeField] float poissonLambda = 2f;
    [SerializeField] float hitRate = 0.6f;
    [SerializeField] float critDamageRate = 2f;
    [SerializeField] int maxHitsPerTurn = 5;

    [Header("UI Reference")]
    [SerializeField] TextMeshProUGUI resultText; // 화면에 결과를 표시할 텍스트

    // 데이터 집계용 변수
    int totalTurns = 0;
    int totalSpawnedEnemies = 0;
    int totalKilledEnemies = 0;
    float maxDamageDealt = 0f;
    float minDamageDealt = float.MaxValue;
    int totalHits = 0;
    int critHits = 0;

    // 아이템 집계
    int potionCount, goldCount, normalWeaponCount, rareWeaponCount, normalArmorCount, rareArmorCount;

    bool rareItemObtained = false;
    float rareChanceModifier = 0f; // 매 턴 5%씩 증가할 확률

    string[] rewards = { "Gold", "Weapon", "Armor", "Potion" };

    public void StartSimulation()
    {
        // 초기화
        rareItemObtained = false;
        totalTurns = 0;
        rareChanceModifier = 0f;
        ResetStats();

        // 1. 레어 아이템 나올 때까지 진행 (While 루프)
        while (!rareItemObtained)
        {
            totalTurns++;
            SimulateTurn();

            // 2. 턴마다 레어 아이템 획득 확률 5%씩 상승
            rareChanceModifier += 0.05f;
        }

        // 3. 결과 출력
        DisplayResults();
    }

    void ResetStats()
    {
        totalSpawnedEnemies = 0; totalKilledEnemies = 0;
        maxDamageDealt = 0f; minDamageDealt = float.MaxValue;
        totalHits = 0; critHits = 0;
        potionCount = 0; goldCount = 0;
        normalWeaponCount = 0; rareWeaponCount = 0;
        normalArmorCount = 0; rareArmorCount = 0;
    }

    void SimulateTurn()
    {
        int enemyCount = SamplePoisson(poissonLambda);
        totalSpawnedEnemies += enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            int hits = SampleBinomial(maxHitsPerTurn, hitRate);
            float totalDamage = 0f;

            for (int j = 0; j < hits; j++)
            {
                totalHits++;
                float damage = SampleNormal(meanDamage, stdDevDamage);

                if (Random.value < critChance)
                {
                    damage *= critDamageRate;
                    critHits++;
                }

                // 최대/최소 데미지 기록
                if (damage > maxDamageDealt) maxDamageDealt = damage;
                if (damage < minDamageDealt) minDamageDealt = damage;

                totalDamage += damage;
            }

            if (totalDamage >= enemyHP)
            {
                totalKilledEnemies++;
                ProcessReward();
            }
        }
    }

    void ProcessReward()
    {
        string reward = rewards[Random.Range(0, rewards.Length)];
        float currentRareProb = 0.2f + rareChanceModifier; // 기본 20% + 보정치

        switch (reward)
        {
            case "Potion": potionCount++; break;
            case "Gold": goldCount++; break;
            case "Weapon":
                if (Random.value < currentRareProb) { rareWeaponCount++; rareItemObtained = true; }
                else normalWeaponCount++;
                break;
            case "Armor":
                if (Random.value < currentRareProb) { rareArmorCount++; rareItemObtained = true; }
                else normalArmorCount++;
                break;
        }
    }

    void DisplayResults()
    {
        float hitAccuracy = (totalHits > 0) ? ((float)totalHits / (totalTurns * maxHitsPerTurn)) * 100f : 0;
        float critRate = (totalHits > 0) ? ((float)critHits / totalHits) * 100f : 0;

        // 화면용 텍스트 정렬 (이미지 구성 참고)
        string result = $"<color=yellow>전투 결과</color>\n" +
                        $"총 진행 턴 수 : {totalTurns}\n" +
                        $"발생한 적 : {totalSpawnedEnemies}\n" +
                        $"처치한 적 : {totalKilledEnemies}\n" +
                        $"공격 명중 결과 : {hitAccuracy:F2}%\n" +
                        $"발생한 치명타 결과 : {critRate:F2}%\n" +
                        $"최대 데미지 : {maxDamageDealt:F2}\n" +
                        $"최소 데미지 : {(minDamageDealt == float.MaxValue ? 0 : minDamageDealt):F2}\n\n" +
                        $"<color=yellow>획득한 아이템</color>\n" +
                        $"포션 : {potionCount}개\n" +
                        $"골드 : {goldCount}개\n" +
                        $"무기 - 일반 : {normalWeaponCount}개\n" +
                        $"무기 - <color=orange>레어</color> : {rareWeaponCount}개\n" +
                        $"방어구 - 일반 : {normalArmorCount}개\n" +
                        $"방어구 - <color=orange>레어</color> : {rareArmorCount}개";

        if (resultText != null) resultText.text = result;
        Debug.Log(result);
    }

    // --- 분포 샘플 함수들 (기존 유지) ---
    int SamplePoisson(float lambda)
    {
        int k = 0; float p = 1f; float L = Mathf.Exp(-lambda);
        while (p > L) { k++; p *= Random.value; }
        return k - 1;
    }

    int SampleBinomial(int n, float p)
    {
        int success = 0;
        for (int i = 0; i < n; i++) if (Random.value < p) success++;
        return success;
    }

    float SampleNormal(float mean, float stdDev)
    {
        float u1 = Random.value; float u2 = Random.value;
        float z = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
        return mean + stdDev * z;
    }
}