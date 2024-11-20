using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStatHandler : MonoBehaviour
{
    // 기본 스탯과 버프 스탯들의 능력치를 종합해서 스탯을 계산하는 컴포넌트
    [SerializeField] private CharacterStat baseStats;
    public CharacterStat CurrentStat { get; private set; } = new();
    public List<CharacterStat> statsModifiers = new List<CharacterStat>();

    private readonly float minAttackDelay = 0.03f;
    private readonly float minAttackPower = 0.5f;
    private readonly float minAttackSize = 0.4f;
    private readonly float minAttackSpeed = 0.1f;

    private readonly float minSpeed = 0.8f;

    private readonly int minMaxHealth = 5;

    //코드를 분리한다는게... Instantiate 하는 부분을 메서드로 따로 빼달라는건가?
    private void Awake()
    {
        if(baseStats.attackSO != null)
        {
            baseStats.attackSO = Instantiate(baseStats.attackSO);
            CurrentStat.attackSO = Instantiate(baseStats.attackSO);
        }

        UpdateCharacterStat();
    }

    public void AddStatModifire(CharacterStat statModifier)
    {
        statsModifiers.Add(statModifier);
        UpdateCharacterStat();
    }

    public void RemoveStatModifier(CharacterStat statModifier)
    {
        statsModifiers.Remove(statModifier);
        UpdateCharacterStat();
    }

    private void UpdateCharacterStat()
    {
        ApplyStatModifiers(baseStats);

        foreach(var modifier in statsModifiers.OrderBy(o => o.statsChangeType))
        {
            ApplyStatModifiers(modifier);
        }
    }

    //분리...?
    private void ApplyStatModifiers(CharacterStat modifier)
    {
        Func<float, float, float> operation = modifier.statsChangeType switch
        {
            StatsChangeType.Add => (current, change) => current + change,
            StatsChangeType.Multiple => (current, change) => current * change,
            _ => (current, change) => change,
        };

        UpdateBasicStats(operation, modifier);
        UpdateAttackStats(operation, modifier);

        if(CurrentStat.attackSO is RangedAttackSO currentRanged && modifier.attackSO is RangedAttackSO newRanged)
        {
            UpdateRangedAttackStats(operation, currentRanged, newRanged);
        }
    }

    private void UpdateBasicStats(Func<float, float, float> operation, CharacterStat modifier)
    {
        CurrentStat.maxHealth = Mathf.Max((int)operation(CurrentStat.maxHealth, modifier.maxHealth), minMaxHealth);
        CurrentStat.speed = Mathf.Max(operation(CurrentStat.speed, modifier.speed), minSpeed);
    }

    private void UpdateAttackStats(Func<float, float, float> operation, CharacterStat modifier)
    {
        if(CurrentStat.attackSO == null || modifier.attackSO == null)
        {
            return;
        }

        var currentAttack = CurrentStat.attackSO;
        var newAttack = modifier.attackSO;

        currentAttack.delay = Mathf.Max(operation(currentAttack.delay, newAttack.delay), minAttackDelay);
        currentAttack.power = Mathf.Max(operation(currentAttack.power, newAttack.power), minAttackPower);
        currentAttack.size = Mathf.Max(operation(currentAttack.size, newAttack.size), minAttackSize);
        currentAttack.speed = Mathf.Max(operation(currentAttack.speed, newAttack.speed), minAttackSpeed);
    }

    private void UpdateRangedAttackStats(Func<float,float,float> operation, RangedAttackSO currentRanged, RangedAttackSO newRanged)
    {
        currentRanged.multipleProjectilesAngel = operation(currentRanged.multipleProjectilesAngel, newRanged.multipleProjectilesAngel);
        currentRanged.spread = operation(currentRanged.spread, newRanged.spread);
        currentRanged.duration = operation(currentRanged.duration, newRanged.duration);
        currentRanged.numberofProjectilesPerShot = Mathf.CeilToInt(operation(currentRanged.numberofProjectilesPerShot,
            newRanged.numberofProjectilesPerShot));
        currentRanged.projectileColor = UpdateColor(operation, currentRanged.projectileColor, newRanged.projectileColor);
    }

    private Color UpdateColor(Func<float,float,float> operation, Color current, Color modifier)
    {
        return new Color(
            operation(current.r, modifier.r),
            operation(current.g, modifier.g),
            operation(current.b, modifier.b),
            operation(current.a, modifier.a));
    }
}
