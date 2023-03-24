using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public enum Type { Fruit, Wood }

    public Type type;
    public int maxHealth;
    public int curHealth;
    public int fRangeFirst = -3;
    public int fRangeSec = 3;

    Rigidbody rigid; 
    BoxCollider hitArea;
    
    public Transform treePos;
    public GameObject tree;
    public Transform treeFruitPos;
    public GameObject fruit; 
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        hitArea = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Weapon.Type.Melee.ToString())
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth --;
            StartCoroutine(OnDemage());
        }
        else if (other.tag == Weapon.Type.Range.ToString())
        {
            // TODO 이 외의 공격 처리
            // StartCoroutine(OnDemage());
        }
    }

    IEnumerator OnDemage()
    {
        yield return new WaitForSeconds(0.1f); // 1 frame wait
        
        if (curHealth > 0)
        {
            GameObject intantTree = Instantiate(fruit, treeFruitPos.position, treeFruitPos.rotation);
            Rigidbody fruitRigid = intantTree.GetComponent<Rigidbody>();
            Vector3 fruitVec = treeFruitPos.up * Random.Range(-0.5f, -1) + Vector3.forward * Random.Range(fRangeFirst, fRangeSec) + Vector3.left * Random.Range(fRangeFirst, fRangeSec);
            fruitRigid.AddForce(fruitVec, ForceMode.Impulse);
            fruitRigid.AddTorque(Vector3.up * Random.Range(-4, 4), ForceMode.Impulse);
        }
        else
        {
            gameObject.layer = 16;
            Destroy(gameObject, 2);
        }
    }
}
