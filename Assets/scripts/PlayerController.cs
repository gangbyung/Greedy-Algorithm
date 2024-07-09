using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public LayerMask enemyLayerMask;
    public Text scoreText;
    public GameObject bossPrefab;  // ���� ������
    public Transform bossSpawnPoint; // ���� ���� ��ġ
    public GameObject currentParticleEffect; // ���� ��� ���� ��ƼŬ ����Ʈ�� �����ϱ� ���� ����
    public GameObject bossHitEffectPrefab; // ������ ������ �� ������ ����Ʈ ������

    private int score = 0;
    private GameObject currentBoss;
    

    void Start()
    {
        UpdateScoreText();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (currentBoss == null)
                {
                    TeleportToClosestEnemy();
                }
                else
                {
                    HitBoss();
                }
                IncreaseScore();
            }
        }
    }

    void TeleportToClosestEnemy()
    {
        // ���� �ִ� ��� ���� ã��
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        if (enemies.Length == 0) return;

        // ���� ����� �� ã��
        EnemyController closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        // ���� ����� ������ �����̵�
        if (closestEnemy != null)
        {
            // ���� ��ƼŬ ����
            if (currentParticleEffect != null)
            {
                Destroy(currentParticleEffect);
            }

            transform.position = closestEnemy.transform.position;

            // ���� ����� ���� �����ϰ� ���ο� ���� ��ġ�� ��ȯ
            closestEnemy.Die();
        }
    }

    void IncreaseScore()
    {
        score++;
        UpdateScoreText();

        if (score % 100 == 0)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        if (currentBoss == null && bossPrefab != null && bossSpawnPoint != null)
        {
            currentBoss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            currentBoss.GetComponent<BossController>().SetHitsRequired(20); // 20�� ������ �״� ����
            BossController bossController = currentBoss.GetComponent<BossController>();

            // Animator ������Ʈ�� ������ ��, null ���� ó�� �߰�
            Animator bossAnimator = currentBoss.GetComponent<Animator>();
            if (bossAnimator != null)
            {
                bossController.animator = bossAnimator;
            }
            else
            {
                Debug.LogError("Animator component not found on boss prefab.");
            }
        }
    }

    void HitBoss()
    {
        if (currentBoss == null)
        {
            return;
        }

        BossController bossController = currentBoss.GetComponent<BossController>();
        bossController.TakeHit();

        // ������ ���� ������ ����Ʈ ���
        if (bossHitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(bossHitEffectPrefab, currentBoss.transform.position, Quaternion.identity);
            Destroy(hitEffect, 2f); // ����Ʈ�� 2�� �Ŀ� ����
        }

        if (bossController.IsDefeated())
        {
            Destroy(currentBoss);
            currentBoss = null;
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
