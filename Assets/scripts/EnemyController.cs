using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    public GameObject deathEffectPrefab;  // 죽을 때 나오는 이펙트 프리팹

    

    void Start()
    {
        Respawn();
    }

    public void Respawn()
    {
        // 지정된 영역 내에서 랜덤 위치 생성
        Vector3 newPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            transform.position.z
        );
        transform.position = newPosition;

    }
    public void Die()
    {
        // 죽을 때 이펙트 재생
        if (deathEffectPrefab != null)
        {
            // 새로운 파티클 효과 재생
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(deathEffect, 0.3f); // 파티클 효과를 2초 후에 제거
        }

        // 적을 새로운 위치로 리스폰
        Respawn();
    }
}

