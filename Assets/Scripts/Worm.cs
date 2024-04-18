using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Enemy
{

    void Start()
    {

        CurHP = MaxHP;
        anim = GetComponent<Animator>();
    }

  
    void Update()
    {
        HpBar.value = CurHP / MaxHP;
        HpSlowBar.value = Mathf.Lerp(HpSlowBar.value, CurHP / MaxHP, Time.deltaTime * 2f); // õõ�� ����ϴ� hp
        StartCoroutine(Die());
    }
}
