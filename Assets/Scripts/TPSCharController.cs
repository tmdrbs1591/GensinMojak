using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization;

public class TPSCharController : MonoBehaviour
{
    [Header("플레이어 스텟")]
    [SerializeField] float moveSpeed;
    [SerializeField] float runSpeed; 
    [SerializeField] float curSpeed;
    [SerializeField] float AttackDamage;
    [SerializeField] float JumpPower;

    [Header("쿨타임")]
    [SerializeField] float AttackcoolTime = 0.5f;
    private float AttackcurTime;

 
    [Space]
    [SerializeField] private Transform characterBody;
    [SerializeField] private Transform cameraArm;
    [SerializeField] private ObjectPool objectpool;
    [Space]

    [SerializeField] private Vector3 AttackboxSize;
    [SerializeField] private Transform AttackboxPos;

    [Header("이펙트")]
    [SerializeField] private GameObject[] Slash;
    [SerializeField] private GameObject DamageEffect;
    [SerializeField] private TMP_Text DamageText;

    int attackIndex;
    float attackBuffer;


    bool isAttack = false;
    bool isGround = false;
        
   

    Animator anim;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        anim = characterBody.GetComponent<Animator>();


    }

  
    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (!CameraShake.instance.isShake)//카메라 흔들리는중에는 화면 회전 불가능
                LookAround();
        Attack();
        Move();
        Jump();
    }
    void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
        anim.SetBool("isMove", isMove);
        if (isMove && !isAttack)
        {

            Vector3 lookForward = new Vector3(cameraArm.forward.x,0f,cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x,0f,cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

                characterBody.forward = moveDir;

            transform.position += moveDir * Time.deltaTime * curSpeed;
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Run(true, runSpeed);
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                Run(false, moveSpeed);
            }

        }
        
        Debug.DrawRay(cameraArm.position,new Vector3(cameraArm.forward.x,0f,cameraArm.forward.z).normalized,Color.red);
    }

    void Run(bool isRun, float speed)
    {
        anim.SetBool("isRun", isRun);
        curSpeed = speed;
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
                switch(attackIndex)
                {
                    case 1:
                        if (Slash[0].activeSelf)
                        {
                            StartCoroutine(SlashPrcCor(3));
                        }
                        else
                        {
                            StartCoroutine(SlashPrcCor(0));
                        }
                        break;
                    case 2:
                        StartCoroutine(SlashPrcCor(1));
                        break;
                    case 3:
                        StartCoroutine(SlashPrcCor(2));
                        break;
                }
                anim.SetTrigger("isAttack" + attackIndex.ToString());

                Invoke("Damage",0.1f);
                AttackcurTime = AttackcoolTime; // 쿨타임
                StartCoroutine(isAttackCor());

                Run(false, moveSpeed);
                attackIndex++;
                if (attackIndex >= 4) attackIndex = 1;
            }
        }
        else
        {
            AttackcurTime -= Time.deltaTime;
        }
       
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.velocity = Vector3.up * JumpPower;
            anim.SetBool("isJump", true);
            isGround = false;

        }
    }
    IEnumerator SlashPrcCor(int index)
    {
        Slash[index].SetActive(true);

        yield return new WaitForSeconds(0.5f);

        Slash[index].SetActive(false);

    }
    void LookAround()
    {
        Vector2 mouseDelta = new Vector2 (Input.GetAxis("Mouse X") , Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {

            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x - mouseDelta.y, camAngle.y + mouseDelta.x, camAngle.z);
    }
    void Damage()
    {
        DamageText.text = AttackDamage.ToString();

        Collider[] colliders = Physics.OverlapBox(AttackboxPos.position, AttackboxSize / 2f);

        foreach (Collider collider in colliders)
        {
            if (collider != null && collider.CompareTag("Enemy"))
            {
                Enemy monster = collider.gameObject.GetComponent<Enemy>();
                Vector3 midPoint = (collider.transform.position + Camera.main.transform.position) / 2f;


                if (monster != null && !monster.isDie)
                {

                    float randomZ = UnityEngine.Random.Range(1f, 2f); 
                    float randomY = UnityEngine.Random.Range(1f, 3f); 

                    // 생성된 무작위 값으로 Vector3를 생성합니다.
                    Vector3 randomVector = new Vector3(0f, randomY, randomZ); // z값은 0으로 


                    GameObject attackEffect = objectpool.MakeObj("damageEffect");
                    attackEffect.transform.position = collider.transform.position;

                    GameObject attackText = objectpool.MakeObj("damageText");
                    attackText.transform.position = midPoint + (collider.transform.position - midPoint).normalized * 2 + randomVector;

                    monster.TakeDamage(AttackDamage);
                    CameraShake.instance.Shake();
                }
            }
        }
    }
    IEnumerator isAttackCor() //공격도중 가마ㄴ히 있게 
    {
        isAttack = true;
        yield return new WaitForSeconds(AttackcoolTime);
        isAttack = false;

    }

    private void OnDrawGizmos()//박스 그려주기
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(AttackboxPos.position, AttackboxSize);
   
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            anim.SetBool("isJump", false);

        }
    }

}
