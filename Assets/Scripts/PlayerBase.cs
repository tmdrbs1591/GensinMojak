using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [Header("플레이어 스텟")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float runSpeed;
    [SerializeField] protected float curSpeed;
    [SerializeField] public float AttackDamage;
    [SerializeField] protected float JumpPower;

    [Header("쿨타임")]
    [SerializeField] protected float AttackcoolTime = 0.5f;
    protected float AttackcurTime;


    [Space]
    [SerializeField] protected Transform characterBody;
    [SerializeField] public Transform cameraArm;
    [SerializeField] protected ObjectPool objectpool;


    [Header("스킬")]
    [SerializeField] protected GameObject Skill;

    protected int attackIndex;
    protected float attackBuffer;


    protected bool isAttack = false;
    protected bool isGround = false;
    protected bool isUltimate = false;
    protected bool isUltimateReady = false;


    // TPSCharController.cs

    [SerializeField] public Vector3 moveDir;

    protected Animator anim;
    protected Rigidbody rb;
    void Start()
    {
        
    }

  
    void Update()
    {
        
    }
    protected void Move()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
        anim.SetBool("isMove", isMove);
        if (isMove && !isAttack)
        {

            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            characterBody.forward = moveDir;

            transform.position += moveDir * Time.deltaTime * curSpeed;
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Run(true, runSpeed);
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                Run(false, moveSpeed);
            }

        }
        Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);
    }

    protected void Run(bool isRun, float speed)
    {
        anim.SetBool("isRun", isRun);
        curSpeed = speed;
    }


    protected void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.velocity = Vector3.up * JumpPower;
            anim.SetBool("isJump", true);
            isGround = false;

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            anim.SetBool("isJump", false);

        }
    }
    protected void LookAround()
    {
        if (!isUltimateReady)
        {

            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
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
    }
    protected void Damage(Transform boxPos, Vector3 boxSize)
    {


        Collider[] colliders = Physics.OverlapBox(boxPos.position, boxSize / 2f);

        foreach (Collider collider in colliders)
        {
            if (collider != null && collider.CompareTag("Enemy"))
            {
                Enemy monster = collider.gameObject.GetComponent<Enemy>();


                if (monster != null && !monster.isDie)
                {
                    GameObject attackEffect = GameManager.instance.objectpool.MakeObj("damageEffect");
                    attackEffect.transform.position = collider.transform.position;



                    monster.TakeDamage(AttackDamage);
                    CameraShake.instance.Shake();
                }
            }
        }
    }
}
