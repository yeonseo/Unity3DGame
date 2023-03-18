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

    void Update()
    {
        transform.Rotate(Vector3.up * (itemRotateSpeet * Time.deltaTime));
    }
}
