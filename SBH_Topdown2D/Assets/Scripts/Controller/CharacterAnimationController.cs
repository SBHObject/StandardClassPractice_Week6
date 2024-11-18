using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CharacterAnimationController : AnimationController
{
    // Animator.StringToHash�� ���� Animator ���� ��ȯ�� Ȱ��Ǵ� �κп� ���� ����ȭ�� ������ �� �ֽ��ϴ�.
    // StringToHash�� IsWalking�̶�� ���ڿ��� �Ϲ��� �Լ��� �ؽ��Լ��� ���� Ư���� ������ ��ȯ�մϴ�.
    // �� �ñ��Ͻôٸ� C# GetHashCode�� �˻��غ�����!
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsHit = Animator.StringToHash("IsHit");

    private static readonly int Attack = Animator.StringToHash("Attack");

    private readonly float magnituteThreshold = 0.5f;

    private HealthSystem healthSystem;

    protected override void Awake()
    {
        base.Awake();
        healthSystem = GetComponent<HealthSystem>();
    }

    void Start()
    {
        // �����ϰų� ������ �� �ִϸ��̼��� ���� �����ϵ��� ����
        controller.OnAttackEvent += Attacking;
        controller.OnMoveEvent += Move;

        if (healthSystem != null)
        {
            healthSystem.OnDamage += Hit;
            healthSystem.OnInvincibilityEnd += InvincibilityEnd;
        }
    }

    private void Move(Vector2 obj)
    {
        animator.SetBool(IsWalking, obj.magnitude > magnituteThreshold);
    }

    // OnAttackEvent�� Action<AttackSO>�̱� ������ Attacking�� AttackSO�� ������� �ʾƵ� �Ű������� ������ �־�� �մϴ�.
    // �̷� �� �Լ�(�޼ҵ�) �ñ״�ó�� ����ٶ�� �մϴ�.
    private void Attacking(AttackSO obj)
    {
        animator.SetTrigger(Attack);
    }

    // ���� �ǰݺκ��� ������ �� �� ���̱� ������ �ϴ� �Ӵϴ�.
    private void Hit()
    {
        animator.SetBool(IsHit, true);
    }

    // ���� �ǰݺκ��� ������ �� �� ���̱� ������ �ϴ� �Ӵϴ�.
    private void InvincibilityEnd()
    {
        animator.SetBool(IsHit, false);
    }
}
