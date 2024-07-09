using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    public GameObject deathEffectPrefab;  // ���� �� ������ ����Ʈ ������

    

    void Start()
    {
        Respawn();
    }

    public void Respawn()
    {
        // ������ ���� ������ ���� ��ġ ����
        Vector3 newPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            transform.position.z
        );
        transform.position = newPosition;

    }
    public void Die()
    {
        // ���� �� ����Ʈ ���
        if (deathEffectPrefab != null)
        {
            // ���ο� ��ƼŬ ȿ�� ���
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(deathEffect, 0.3f); // ��ƼŬ ȿ���� 2�� �Ŀ� ����
        }

        // ���� ���ο� ��ġ�� ������
        Respawn();
    }
}

