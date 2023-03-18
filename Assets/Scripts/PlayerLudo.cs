using UnityEngine;

public class PlayerLudo : MonoBehaviour
{
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public int hasGrenades;
    public Camera followCamera;

    public int ammo;
    public int coin;
    public int heart;


    public int maxAmmo;
    public int maxCoin;
    public int maxHeart;
    public int maxHasGrenades;

    readonly int jumpPower = 15;

    float hAxis;
    float vAxis;

    /*********************************************
     * Input Control
     *********************************************/
    bool wDown;
    bool jDown;

    bool isJump;
    bool isDodge;
    bool isSwap;

    bool iDown;

    bool fDown;
    bool rDown;
    bool isFireReady = true;
    bool isReload;
    bool isBoarder;

    /*********************************************
     * Input Item
     *********************************************/
    bool sDown1;
    bool sDown2;
    bool sDown3;


    /*********************************************
     * Object
     *********************************************/
    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;

    GameObject nearObject;
    Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;


    /*********************************************
     * TAG
     *********************************************/
    readonly string TagFloor = "Floor";
    readonly string TagWeapon = "Weapon";
    readonly string TagItem = "Item";

    /*********************************************
     * Anim Parameter
     *********************************************/
    readonly string IsWalk = "isWalk";
    readonly string IsRun = "isRun";
    readonly string IsJump = "isJump";
    readonly string DoJump = "doJump";
    readonly string DoDodge = "doDodge";
    readonly string DoSwap = "doSwap";
    readonly string DoSwing = "doSwing";
    readonly string DoShot = "doShot";
    readonly string DoReload = "doReload";



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
        Attack();
        Reload();
    }

    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButton("Fire1");
        rDown = Input.GetButton("Reload");
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
        if (isSwap || !isFireReady || isReload) moveVec = Vector3.zero;

        if (!isBoarder)
        {
            transform.position += moveVec * (speed * (wDown ? 0.3f : 1) * Time.deltaTime);
        }

        anim.SetBool(IsRun, moveVec != Vector3.zero);
        anim.SetBool(IsWalk, wDown);
    }

    void Turn()
    {
        // keyboard
        transform.LookAt(transform.position + moveVec);

        // mouse
        if (fDown)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 100))
            {
                Vector3 nextVec = raycastHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
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
                equipWeapon.gameObject.SetActive(false);
            }
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger(DoSwap);

            isSwap = true;
            Invoke("SwapOut", 0.4f);
        }
    }

    void Attack()
    {
        if (equipWeapon == null)
        {
            return;
        }

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !isDodge && !isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? DoSwing : DoShot);
            fireDelay = 0;
        }
    }

    void Reload()
    {
        if (equipWeapon == null || equipWeapon.type == Weapon.Type.Melee || ammo == 0)
        {
            return;
        }

        if (rDown && !isJump && !isDodge && !isSwap && isFireReady)
        {
            anim.SetTrigger(DoReload);
            isReload = true;
            Invoke("ReloadOut", 1.5f);
        }
    }


    void FreezeRotaion()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        FreezeRotaion();
        StopToWall();
    }
    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBoarder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }


    void ReloadOut()
    {
        int reAmmo = ammo > equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
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




    void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag(TagFloor)) return;
        isJump = false;
        anim.SetBool(IsJump, isJump);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagItem)
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                    {
                        ammo = maxAmmo;
                    }
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                    {
                        coin = maxCoin;
                    }
                    break;
                case Item.Type.Heart:
                    heart += item.value;
                    if (heart > maxHeart)
                    {
                        heart = maxHeart;
                    }
                    break;
                case Item.Type.Grenade:
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if (hasGrenades > maxHasGrenades)
                    {
                        hasGrenades = maxHasGrenades;
                    }
                    break;
            }

            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == TagWeapon)
        {
            nearObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (nearObject.tag == TagWeapon)
        {
            nearObject = null;
        }
    }
}
