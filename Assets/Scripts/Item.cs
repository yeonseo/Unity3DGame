using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        Ammo, Coin, Grenade, Heart, Weapon
    }

    public Type type;
    public int value;

    private readonly int itemRotateSpeet = 10;

    private void Update()
    {
        transform.Rotate(Vector3.up * (itemRotateSpeet * Time.deltaTime));
    }
}
