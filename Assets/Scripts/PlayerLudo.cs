using UnityEngine;

public class PlayerLudo : MonoBehaviour
{
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    
    int jumpPower = 15;
    
    float hAxis;
    float vAxis;
    
    /*********************************************
     * Control
     *********************************************/
    bool wDown;
    bool jDown;
    
    bool isJump = false;
    bool isDodge = false;
    bool isSwap = false;

    bool iDown = false;



    /*********************************************
     * Item
     *********************************************/
    bool sDown1 = false;
    bool sDown2 = false;
    bool sDown3 = false;
    
    
    /*********************************************
     * Object
     *********************************************/
    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;
    
    GameObject nearObject = null;
    GameObject equipWeapon = null;
    int equipWeaponIndex = -1;
    
    /*********************************************
     * TAG
     *********************************************/
    private string TagFloor = "Floor";
    private string TagWeapon = "Weapon";
    private string TagItem = "Item";

    /*********************************************
     * Anim Parameter
     *********************************************/
    private string IsWalk = "isWalk";
    private string IsRun = "isRun";
    private string IsJump = "isJump";
    private string DoJump = "doJump";
    private string DoDodge = "doDodge";
    private string DoSwap = "doSwap";


    
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
        Interaction();
        Swap();
    }


    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interaction");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    
    
    /*********************************************
     * Method Control
     *********************************************/
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if (isDodge) moveVec = dodgeVec;
        if (isSwap) moveVec = Vector3.zero;

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
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge && !isSwap)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJump = true;
            anim.SetBool(IsJump, isJump);        
            anim.SetTrigger(DoJump);
        }
       
    }
    
    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge && !isSwap)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger(DoDodge);
            isDodge = true;

            Invoke("DodgeOut", 0.4f);
        }
    }
    
    void Interaction()
    {
        if (iDown && nearObject is not null && !isJump && !isDodge && !isSwap)
        {
            if (nearObject.tag == TagWeapon)
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;
                
                Destroy(nearObject);
            }
        }
    }
    
    void Swap()
    {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0)) return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1)) return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2)) return;

        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;
        
        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge && !isSwap)
        {
            if (equipWeapon != null)
            {
                equipWeapon.SetActive(false);
            }
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);
            
            anim.SetTrigger(DoSwap);

            isSwap = true;
            Invoke("SwapOut", 0.4f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }
    
    void SwapOut()
    {
        isSwap = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag(TagFloor)) return;
        isJump = false;
        anim.SetBool(IsJump, isJump);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == TagWeapon)
        {
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (nearObject.tag == TagWeapon)
        {
            nearObject = null;
        }
    }
}
