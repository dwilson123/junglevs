using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    private bool isAttacking = false;
    PCInputManager parent;

    const float rotateSpeed = 5.0f;
    int team;
    Vector3 mask = new Vector3(0, 1, 0);
	// Use this for initialization
	void Start () {
        parent = this.gameObject.GetComponentInParent<PCInputManager>();
        team = parent.team;
	}
	
	// Update is called once per frame
	void Update () {
	    if (isAttacking)
        {
            this.gameObject.transform.RotateAround(parent.gameObject.transform.position, mask, rotateSpeed);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SpawnAI>() != null)
        {
            SpawnAI ai = other.GetComponent<SpawnAI>();
            if (ai.team != this.team)
            {
                ai.TakeDamage(20);
            }
        }
    }

    public void Attack()
    {
        this.gameObject.SetActive(true);
        //It is vertical, we want horizontal
        if(this.gameObject.transform.rotation.z == 0)
        {
            Vector3 eulers = this.gameObject.transform.eulerAngles;
            eulers.z = 90;
            transform.eulerAngles = eulers;
        }

        StartCoroutine(attackCoroutine());
    }


    IEnumerator attackCoroutine()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1);
        isAttacking = false;
        this.gameObject.SetActive(false);
    }
}
