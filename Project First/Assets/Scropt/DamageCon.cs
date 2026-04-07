using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class DamageCon : MonoBehaviour
{
    public TextMeshProUGUI statusDisplay;
    public TextMeshProUGUI logDisplay;
    public TextMeshProUGUI resultDisplay;
    public TextMeshProUGUI rangeDisplay;

    private int level = 1;
    private float totalDamage = 0, baseDamage = 20f;
    private int attackCount = 0;

    // 추가된 통계 변수들
    private int weakPointCount = 0;
    private int missCount = 0;
    private int critCount = 0;
    private float maxDamage = 0;

    private string weaponName;
    private float stdDevMult, critRate, critMult;

    void Start()
    {
        SetWeapon(0); 
    }



    public void LevelUp()
    {
        ResetStats(); 
        level++;
        baseDamage = level * 20f;
        logDisplay.text = string.Format("레벨업! 현재 레벨: {0}", level);
        UpdateUI();
    }

    // 단일 공격 버튼 연동
    public void OnAttack()
    {
        ProcessAttack();
        UpdateUI();
    }

    // [추가] 1000회 공격 버튼 연동
    public void OnAttackThousand()
    {
        for (int i = 0; i < 1000; i++)
        {
            ProcessAttack();
        }
        logDisplay.text = "<colored> 1000회 연속 공격</color>";
        UpdateUI();
    }

    // 실제 공격 로직 분리
    private void ProcessAttack()
    {
        float sd = baseDamage * stdDevMult;
        float normalDamage = GetNormalStdDevDamage(baseDamage, sd);
        float finalDamage = 0;

        bool isMiss = false;
        bool isWeakPoint = false;
        bool isCrit = false;

        // 1. 명중 실패 판정 (-2시그마 미만)
        if (normalDamage < baseDamage - 2 * sd)
        {
            finalDamage = 0;
            isMiss = true;
            missCount++;
        }
        else
        {
            // 2. 약점 공격 판정 (+2시그마 초과)
            float multiplier = 1f;
            if (normalDamage > baseDamage + 2 * sd)
            {
                multiplier = 2f;
                isWeakPoint = true;
                weakPointCount++;
            }

            // 3. 치명타 판정 (크리티컬과 약점은 별개 계산)
            isCrit = Random.value < critRate;
            float critBonus = isCrit ? critMult : 1f;
            if (isCrit) critCount++;

            finalDamage = normalDamage * multiplier * critBonus;
        }

        // 통계 누적
        attackCount++;
        totalDamage += finalDamage;
        if (finalDamage > maxDamage) maxDamage = finalDamage;

        // 모든 색상 태그를 제거하여 기본 색상(하얀색)으로 출력
        string msg = isMiss ? "[명중 실패] " : "";
        if (isWeakPoint) msg += "[약점 공격!] ";
        if (isCrit) msg += "[치명타!] ";

        logDisplay.text = string.Format("{0}데미지: {1:F1}", msg, finalDamage);
    }

    private void ResetData()
    {
        level = 1;
        baseDamage = 20f;
        ResetStats();
    }

    private void ResetStats()
    {
        totalDamage = 0;
        attackCount = 0;
        weakPointCount = 0;
        missCount = 0;
        critCount = 0;
        maxDamage = 0;
    }

    public void SetWeapon(int id)
    {
        ResetData();
        if (id == 0) SetStats("단검", 0.1f, 0.4f, 1.5f);
        else if (id == 1) SetStats("장검", 0.2f, 0.3f, 2.0f);
        else SetStats("도끼", 0.3f, 0.2f, 3.0f);

        logDisplay.text = string.Format("{0} 장착!", weaponName);
        UpdateUI();
    }

    private void SetStats(string _name, float _stdDev, float _critRate, float _critMult)
    {
        weaponName = _name;
        stdDevMult = _stdDev;
        critRate = _critRate;
        critMult = _critMult;
    }

    private void UpdateUI()
    {
        statusDisplay.text = string.Format("Level: {0} / 무기: {1}\n기본 데미지: {2} / 치명타: {3}% (x{4})",
            level, weaponName, baseDamage, critRate * 100, critMult);

        rangeDisplay.text = string.Format("일반 범위: [{0:F1} ~ {1:F1}]\n<color=blue>약점(+2σ): {2:F1} 초과</color> / <color=gray>실패(-2σ): {3:F1} 미만</color>",
            baseDamage - (3 * baseDamage * stdDevMult),
            baseDamage + (3 * baseDamage * stdDevMult),
            baseDamage + (2 * baseDamage * stdDevMult),
            baseDamage - (2 * baseDamage * stdDevMult));

        float dpa = attackCount > 0 ? totalDamage / attackCount : 0;
        resultDisplay.text = string.Format(
            "누적 데미지: {0:F1}\n" +
            "공격 횟수: {1}회\n" +
            "평균 DPA: {2:F2}\n" +
            "--------------------------\n" +
            "약점 공격: {3}회 / 명중 실패: {4}회\n" +
            "치명타 횟수: {5}회\n" +
            "<colored>최대 데미지: {6:F1}</color>",
            totalDamage, attackCount, dpa, weakPointCount, missCount, critCount, maxDamage);
    }

    private float GetNormalStdDevDamage(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }

}