using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    private HealthSystem healthSystem;
    private Rigidbody2D rb;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody2D>();
        // ���� ���� ��ü�� healthSystem��
        healthSystem.OnDeath += OnDeath;
    }

    void OnDeath()
    {
        // ���ߵ��� ����
        rb.velocity = Vector3.zero;

        // �ణ �������� �������� ����
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // ��ũ��Ʈ ���̻� �۵� ���ϵ��� ��
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        // 2�ʵڿ� �ı�
        Destroy(gameObject, 2f);
    }
}
