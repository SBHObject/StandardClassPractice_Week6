using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = .5f;

    private CharacterStatHandler statsHandler;
    private float timeSinceLastChange = float.MaxValue;
    private bool isAttacked = false;

    // 체력이 변했을 때 할 행동들을 정의하고 적용 가능
    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;

    public float CurrentHealth { get; private set; }

    // get만 구현된 것처럼 프로퍼티를 사용하는 것
    // 이렇게 하면 데이터의 복제본이 여기저기 돌아다니다가 싱크가 깨지는 문제를 막을 수 있어요!
    public float MaxHealth => statsHandler.CurrentStat.maxHealth;

    [SerializeField] private AudioClip damageClip;

    private void Awake()
    {
        statsHandler = GetComponent<CharacterStatHandler>();
    }

    private void Start()
    {
        CurrentHealth = statsHandler.CurrentStat.maxHealth;
    }

    private void Update()
    {
        if (isAttacked && timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay)
            {
                OnInvincibilityEnd?.Invoke();
                isAttacked = false;
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        // 무적 시간에는 체력이 달지 않음
        if (timeSinceLastChange < healthChangeDelay)
        {
            return false;
        }

        timeSinceLastChange = 0f;
        CurrentHealth += change;
        // [최솟값을 0, 최댓값을 MaxHealth로 하는 구문]
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        // CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        // CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth; 와 같아요!

        if (CurrentHealth <= 0f)
        {
            CallDeath();
            return true;
        }

        if (change >= 0)
        {
            OnHeal?.Invoke();
        }
        else
        {
            OnDamage?.Invoke();
            isAttacked = true;

            if(damageClip)
            {
                SoundManager.PlayClip(damageClip);
            }
        }


        return true;
    }

    public void TakeDamageNonInvincible(float change)
    {
        CurrentHealth -= change;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        if (CurrentHealth <= 0f)
        {
            CallDeath();
        }

        OnDamage?.Invoke();

        if (damageClip)
        {
            SoundManager.PlayClip(damageClip);
        }
    }

    private void CallDeath()
    {
        OnDeath?.Invoke();
    }
}
