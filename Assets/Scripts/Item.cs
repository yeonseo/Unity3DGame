using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        Ammo, Coin, Grenade, Heart, Weapon
    }

    public Type type;
    public int value;

    readonly int itemRotateSpeet = 30;
    Rigidbody _rigid;
    SphereCollider _sphereCollider;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _sphereCollider = GetComponent<SphereCollider>(); // 여러개인 경우 첫번째를 가지고 옴. 그러므로 물리효과를 담당하는 SphereCollider를 위로 올리기.
    }

    void Update()
    {
        transform.Rotate(Vector3.up * (itemRotateSpeet * Time.deltaTime));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _rigid.isKinematic = true;
            _sphereCollider.enabled = false;
        }
    }
}
