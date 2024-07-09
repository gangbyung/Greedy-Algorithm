using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public LayerMask enemyLayerMask;
    public Text scoreText;
    public GameObject bossPrefab;  // 보스 프리팹
    public Transform bossSpawnPoint; // 보스 스폰 위치
    public GameObject currentParticleEffect; // 현재 재생 중인 파티클 이펙트를 추적하기 위한 변수
    public GameObject bossHitEffectPrefab; // 보스를 때렸을 때 나오는 이펙트 프리팹

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
        // 씬에 있는 모든 적을 찾음
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        if (enemies.Length == 0) return;

        // 가장 가까운 적 찾기
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

        // 가장 가까운 적에게 순간이동
        if (closestEnemy != null)
        {
            // 이전 파티클 제거
            if (currentParticleEffect != null)
            {
                Destroy(currentParticleEffect);
            }

            transform.position = closestEnemy.transform.position;

            // 가장 가까운 적을 제거하고 새로운 랜덤 위치에 소환
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
            currentBoss.GetComponent<BossController>().SetHitsRequired(20); // 20번 때려야 죽는 보스
            BossController bossController = currentBoss.GetComponent<BossController>();

            // Animator 컴포넌트를 설정할 때, null 예외 처리 추가
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

        // 보스를 때릴 때마다 이펙트 재생
        if (bossHitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(bossHitEffectPrefab, currentBoss.transform.position, Quaternion.identity);
            Destroy(hitEffect, 2f); // 이펙트를 2초 후에 제거
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
