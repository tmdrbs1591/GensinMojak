using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TPSCharController : PlayerBase
{

    [SerializeField] protected Vector3 AttackboxSize;
    [SerializeField] protected Transform AttackboxPos;
    [SerializeField] protected Vector3 UltimateboxSize;
    [SerializeField] protected Transform UltimateboxPos;
    [SerializeField] protected Camera UltimateCamera;
    [SerializeField] protected Camera mainCamera;

    [Header("����Ʈ")]
    [SerializeField] protected GameObject[] Slash;
    [SerializeField] protected GameObject DamageEffect;
    [SerializeField] protected GameObject SkillEffect;
    [SerializeField] protected GameObject UltimateReadyEffect;
    [SerializeField] protected GameObject UltimateEffect;
    [SerializeField] protected GameObject SkinMesh;


    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        anim = characterBody.GetComponent<Animator>();

    }

  
    void Update()
    {
     
        if (!CameraShake.instance.isShake)//ī�޶� ��鸮���߿��� ȭ�� ȸ�� �Ұ���
                LookAround();
        if (!isUltimate) { 
        Attack();
        Move();
        Jump();
        Skills();
        Ultimate();
        }
    }
  

    void Attack()
    {
        attackBuffer -= Time.deltaTime;
        if (attackBuffer <= 0) attackIndex = 1;
        if (Input.GetMouseButtonDown(0))
            attackBuffer = 0.319f;
        if (AttackcurTime <= 0)
        {
            if (attackBuffer > 0)
            {
                Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                moveDir = lookForward;

                characterBody.forward = moveDir;
                switch (attackIndex)
                {
                    case 1:
                        if (Slash[0].activeSelf)
                        {
                            AudioManager.instance.PlaySound(transform.position, 0, UnityEngine.Random.Range(1.1f, 1.1f), 1);

                            StartCoroutine(SlashPrcCor(3));
                        }
                        else
                        {
                            AudioManager.instance.PlaySound(transform.position, 0, UnityEngine.Random.Range(1.1f, 1.1f), 1);
                            StartCoroutine(SlashPrcCor(0));
                        }
                        break;
                    case 2:
                        AudioManager.instance.PlaySound(transform.position, 0, UnityEngine.Random.Range(1.1f, 1.1f), 1);

                        StartCoroutine(SlashPrcCor(1));
                        break;
                    case 3:
                        AudioManager.instance.PlaySound(transform.position, 0,  UnityEngine.Random.Range(0.8f, 0.8f), 1);

                        StartCoroutine(SlashPrcCor(2));
                        break;
                }
                anim.SetTrigger("isAttack" + attackIndex.ToString());

                StartCoroutine(DamageCor());
                AttackcurTime = AttackcoolTime; // ��Ÿ��
                StartCoroutine(isAttackCor(AttackcoolTime));

                Run(false, moveSpeed);
                attackIndex++;
                if (attackIndex >= 4) attackIndex = 1; //4�̻�Ǹ� 1�� ���ư�
            }
        }
        else
        {
            AttackcurTime -= Time.deltaTime;
        }
    }

    IEnumerator SlashPrcCor(int index)
    {
        Slash[index].SetActive(true);

        yield return new WaitForSeconds(0.5f);

        Slash[index].SetActive(false);

    }
  
    IEnumerator DamageCor()
    {
        yield return new WaitForSeconds(0.1f);
        Damage(AttackboxPos, AttackboxSize);
    }
    IEnumerator UltimateDamageCor()
    {
        for (int i = 0; i < 20; i++)
        {
            Damage(UltimateboxPos, UltimateboxSize);
            AudioManager.instance.PlaySound(transform.position, 0, UnityEngine.Random.Range(1.1f, 1.3f), 1);
            yield return new WaitForSeconds(0.1f);
        }
      
        
    }
   
    void Skills()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //CameraShake.instance.Shake();

            StartCoroutine(SkilleffectCor());
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f,cameraArm.forward.z).normalized;
            moveDir = lookForward;

            anim.SetTrigger("isSkill");


            Instantiate(Skill, transform.position + new Vector3(0, 1, 0), Quaternion.LookRotation(moveDir));

        }
    }
    void Ultimate()
    {
        StartCoroutine(UltimateCor());

    }
    IEnumerator UltimateCor()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioManager.instance.PlaySound(transform.position, 1, UnityEngine.Random.Range(1f, 1f), 1);

            //��ų ����Ʈ ���
            anim.SetTrigger("isUltimate");
            isUltimate = true;
            isUltimateReady = true; //�غ��Ҷ�
            mainCamera.gameObject.SetActive(false);
            UltimateCamera.gameObject.SetActive(true); // ��ųī�޶� Ȱ��ȭ
            UltimateReadyEffect.SetActive(true) ;
            StartCoroutine(isAttackCor(3f));
            yield return new WaitForSeconds(1f);
            mainCamera.gameObject.SetActive(true);  // ����ī�޶� Ȱ��ȭ
            UltimateCamera.gameObject.SetActive(false);
            isUltimateReady = false;
            StartCoroutine(UltimateDamageCor());
            StartCoroutine(UltimatEffectCor());
            SkinMesh.SetActive(false);
            UltimateReadyEffect.SetActive(false);
         

        }
    }
    IEnumerator UltimatEffectCor()
    {
        UltimateEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        SkinMesh.SetActive(true );
        UltimateEffect.SetActive(false);
        isUltimate = false;


    }
    IEnumerator isAttackCor(float Time) //���ݵ��� �������� �ְ� 
    {
        isAttack = true;
        yield return new WaitForSeconds(Time);
        isAttack = false;

    }  
    IEnumerator SkilleffectCor() //���ݵ��� �������� �ְ� 
    {
        SkillEffect.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        SkillEffect.SetActive(false);


    }

    private void OnDrawGizmos()//�ڽ� �׷��ֱ�
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(AttackboxPos.position, AttackboxSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(UltimateboxPos.position, UltimateboxSize);
   
    }
  

}
