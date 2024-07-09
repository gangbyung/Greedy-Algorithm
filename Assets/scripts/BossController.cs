using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossController : MonoBehaviour
{
    private int hitsRequired = 20;
    public GameObject hitEffectPrefab;
    private int currentHits = 0;
    public Animator animator;

    public void SetHitsRequired(int hits)
    {
        hitsRequired = hits;
        
    }

    public void TakeHit()
    {
        currentHits++;
        if (animator != null)
        {
            animator.SetTrigger("hit");
        }
        
        if (hitEffectPrefab != null)
        {
            
            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(hitEffect, 0.2f); // 이펙트를 2초 후에 제거
        }
    }

    public bool IsDefeated()
    {
        return currentHits >= hitsRequired;
    }
}
