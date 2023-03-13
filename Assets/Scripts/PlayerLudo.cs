using UnityEngine;

public class PlayerLudo : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;
    bool isDodge = false;
    bool isJump = false;
    int jumpPower = 15;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;
    
    private string TagFloor = "Floor";
    
    private string IsWalk = "isWalk";
    private string IsRun = "isRun";
    private string DoJump = "doJump";
    private string DoDodge = "doDodge";
    private string IsJump = "isJump";

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
    }

    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if (isDodge) moveVec = dodgeVec;

        transform.position += moveVec * (speed * (wDown ? 0.3f : 1) * Time.deltaTime);

        anim.SetBool(IsRun, moveVec != Vector3.zero);
        anim.SetBool(IsWalk, wDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJump = true;
            anim.SetBool(IsJump, isJump);        
            anim.SetTrigger(DoJump);
        }
       
    }
    
    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger(DoDodge);
            isDodge = true;

            Invoke("DoDgeOut", 0.4f);
        }
    }

    void DoDgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag(TagFloor)) return;
        isJump = false;
        anim.SetBool(IsJump, isJump);
    }
}
