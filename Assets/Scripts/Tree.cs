using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Tree : MonoBehaviour
{
    public enum Type { Fruit, Leaf }

    public Type type;
    public int damage;
    public float rate;

    public int maxFruit;
    public int curFruit;

    public BoxCollider hitArea;
    public TrailRenderer trailEffect;

    public Transform treePos;
    public GameObject tree;
    public Transform treeFruitPos;
    public GameObject fruit;
    public void Use()
    {
        if (type == Type.Fruit)
        {
            // TODO 과일이 떨어지고, 값이 감소 한다.
            curFruit--;
            StartCoroutine("Shot");
        }

        else if (type == Type.Leaf && curFruit > 0)
        {
            // TODO 캐릭터의 움직임은 계속,
            // TODO 나뭇잎이나 나뭇가지가 떨어지는 모션
        }
    }

    IEnumerator Swing()
    {
        // 1.
        yield return new WaitForSeconds(0.3f); // 0.1 sec wait
        hitArea.enabled = true;
        trailEffect.enabled = true;

        // 2.
        yield return new WaitForSeconds(0.3f); // 0.1 sec wait
        hitArea.enabled = false;
        // 3.
        yield return new WaitForSeconds(0.3f); // 0.1 sec wait
        trailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        // GameObject intantFruit = Instantiate(tree, treePos.position, treePos.rotation);
        // Rigidbody bulletRigid = intantFruit.GetComponent<Rigidbody>();
        // bulletRigid.velocity = treePos.up * -9;

        yield return null; // 1 frame wait

        GameObject intantTree = Instantiate(fruit, treeFruitPos.position, treeFruitPos.rotation);
        Rigidbody fruitRigid = intantTree.GetComponent<Rigidbody>();
        Vector3 fruitVec = treeFruitPos.up * Random.Range(-3, -2) + Vector3.forward * Random.Range(-3, 3);
        fruitRigid.AddForce(fruitVec, ForceMode.Impulse);
        fruitRigid.AddTorque(Vector3.up * Random.Range(-4, 4), ForceMode.Impulse);
    }

}
