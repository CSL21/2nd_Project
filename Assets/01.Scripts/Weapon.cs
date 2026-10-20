using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    protected Camera camera;

    [SerializeField] protected float damage;

    [SerializeField] float delay = 1f;

    [SerializeField] protected bool canAttack;
    WaitForSeconds wait;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        canAttack = true;
        delay = 1f;
        wait = new WaitForSeconds(delay);
        StartCoroutine(AttackDelay());
    }

    protected abstract void Attack();

    protected void LookMouse()
    {
        // 화면 기준으로 마우스 위치 좌표(스크린 좌표)
        Vector2 mousePos = Mouse.current.position.ReadValue();

        // 월드 좌표
        Vector3 worldPos = camera.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        Vector2 dir = worldPos - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    IEnumerator AttackDelay()
    {
        while (true)
        {
            yield return new WaitWhile(() => canAttack);
            yield return wait;
            canAttack = true;
        }
    }

}


// 오일러 각 회전의 문제
// 
// 1. 회전 순서
// A각으로 회전 + B각으로 회전 != B각으로 회전 + A각으로 회전
// 
// 2. 짐벌락 발생 가능
// 
// 
// 쿼터니언
// 1. 값이 4개
// 2. 각 값을 직접 수정X -> 함수(ex. Quaternion.Euler)를 통해서 값 수정
// 
// 
// 자주 사용하는 기능
// Quaternion.Euler 오일러 각도를 쿼터니언으로 변환
// Quaternion.identity 회전이 없는 기본상태
// Quaternion.LookRotation <= 3D에서 특정 방향 바라보기