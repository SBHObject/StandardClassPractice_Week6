using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : TopDownController
{
    private Camera _camera;
    protected override void Awake()
    {
        // 부모의 Awake도 빼먹지 말고 실행하라는 의미
        base.Awake();
        _camera = Camera.main;
    }

    public void OnMove(InputValue value)
    {
        if (GameManager.Instance.IsPaused) return;
        // Debug.Log("OnMove" + value.ToString());
        Vector2 moveInput = value.Get<Vector2>().normalized;
        CallMoveEvent(moveInput);
    }

    public void OnLook(InputValue value)
    {
        if (GameManager.Instance.IsPaused) return;
        // Debug.Log("OnLook" + value.ToString());
        Vector2 newAim = value.Get<Vector2>();
        Vector2 worldPos = _camera.ScreenToWorldPoint(newAim);
        newAim = (worldPos - (Vector2)transform.position).normalized;

        if (newAim.magnitude >= .9f)
        // Vector 값을 실수로 변환
        {
            CallLookEvent(newAim);
        }
    }

    public void OnFire(InputValue value)
    {
        if (GameManager.Instance.IsPaused) return;
        isAttacking = value.isPressed;
    }

    public void OnPause(InputValue value)
    {
        if(value.isPressed)
        {
            GameManager.Instance.PauseGame();
        }
    }
}
