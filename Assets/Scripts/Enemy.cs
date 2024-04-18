using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float CurHP;
    [SerializeField] protected float MaxHP;

    [SerializeField] protected Slider HpBar;
    [SerializeField] protected Slider HpSlowBar;

    public bool isDie = false;

    protected Animator anim;
   

    public void TakeDamage(float damage)
    {   if (!isDie) { 
        anim.SetTrigger("TakeDamage");
        CurHP -= damage;
        }
    }
    protected IEnumerator Die()
    {
        if (CurHP <= 0 && !isDie)
        {
            HpBar.gameObject.SetActive(false);
            HpSlowBar.gameObject.SetActive(false);

            isDie = true;

            anim.SetTrigger("Die");
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
    }
}
