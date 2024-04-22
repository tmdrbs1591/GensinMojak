using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSC : MonoBehaviour
{
    [SerializeField] private float speed;


    [SerializeField] float AttackcoolTime = 0.5f;
    private float AttackcurTime;
    void Start()
    {
    }

  
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            for (int i = 0; i < 10; i++)
                {
                if (AttackcurTime <= 0)
                {
                    Enemy monster = other.gameObject.GetComponent<Enemy>();
                    monster.TakeDamage(12);
                    AttackcurTime = AttackcoolTime;
                }
                else
                {
                    AttackcurTime -= Time.deltaTime;
                }
            }

        }
    }
  
}
