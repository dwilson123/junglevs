using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public abstract class Drop : MonoBehaviour {

    public abstract void AddValue(PCInputManager gameObject);

    void OnTriggerEnter(Collider other)
    {
        PCInputManager player = other.gameObject.GetComponent<PCInputManager>();

        if (player != null)
        {
            AddValue(player);

            Destroy(this.gameObject);
        }
    }


    public void Spawn(GameObject spawningObject)
    {
        Instantiate(this.gameObject, spawningObject.gameObject.transform.position, Quaternion.identity);
    }

}
