using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float CurHP;
    [SerializeField] protected float MaxHP;

    [SerializeField] protected Slider HpBar;
    [SerializeField] protected Slider HpSlowBar;

    [SerializeField] protected TMP_Text DamageText;


    public bool isDie = false;

    protected Animator anim;


    public void TakeDamage(float damage)
    {
        if (!isDie)
        {
            anim.SetTrigger("TakeDamage");
            CurHP -= damage;


            DamageText.text = damage.ToString();

            Vector3 midPoint = (gameObject.transform.position + Camera.main.transform.position) / 2f;


            float randomZ = UnityEngine.Random.Range(1f, 2f);
            float randomY = UnityEngine.Random.Range(1f, 3f);

            // ������ ������ ������ Vector3�� �����մϴ�.
            Vector3 randomVector = new Vector3(0f, randomY, randomZ); // z���� 0���� 

            Destroy(Instantiate(DamageText.gameObject, midPoint + (gameObject.transform.position - midPoint).normalized * 2 + randomVector, Quaternion.identity),2f);


            // GameObject attackText = GameManager.instance.objectpool.MakeObj("damageText");
            //attackText.transform.position = midPoint + (gameObject.transform.position - midPoint).normalized * 2 + randomVector;
        } }
        public IEnumerator Die()
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
