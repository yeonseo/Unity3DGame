using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range }

    public Type type;
    public int damage;
    public float rate;

    public int maxAmmo;
    public int curAmmo;

    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;
    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");

            StartCoroutine("Swing");
        }

        else if (type == Type.Range && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {
        // 1.
        yield return new WaitForSeconds(0.3f); // 0.1 sec wait
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        // 2.
        yield return new WaitForSeconds(0.3f); // 0.1 sec wait
        meleeArea.enabled = false;
        // 3.
        yield return new WaitForSeconds(0.3f); // 0.1 sec wait
        trailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;

        yield return null; // 1 frame wait

        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }

}
