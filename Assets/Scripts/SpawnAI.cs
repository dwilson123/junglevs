using UnityEngine;
using System.Collections;

public class SpawnAI : MonoBehaviour {


    //This is the fixate target the AI will move towards if not engaged
    //in some other activity [in general a base object]
    public GameObject target;


    //Variables that can be augmented through upgrades
    public int health;
    public int team;
    public int damage = 1;
    public float speed = .01f;
    public float attackCooldown = 0.5f;

    //This is the Drop item which will drop on death
    public Drop drop;

    //Public for the sake of setting
    public Spawner parent;

    //Private stuff
    private bool inCombat = false;
    private GameObject combatWith;
    private bool canAttack = true;

	// Update is called once per frame
	void Update () {
        //If this is not engaged in combat then move to fixate position
		if (!inCombat && target != null && !Vector3.zero.Equals (target))
			this.transform.position = Vector3.MoveTowards (transform.position, target.transform.position, speed);
		//If this is engaged in combat and can attack then attack
		else if (combatWith != null && canAttack) {
			combatWith.GetComponent<SpawnAI> ().TakeDamage ();
			canAttack = false;
			StartCoroutine (WaitForCooldown ());
		}
		//Most likely the target died
		else if(combatWith == null)
        {
            inCombat = false;
        }
		//implicit else -- do nothing

        if (this.transform.position.Equals(target))
        {
            Destroy(this.gameObject);
        }

	}
    
    void OnTriggerEnter(Collider other)
    {

        //If I'm already in combat with someone don't get a new target
        if (inCombat)
            return;

        SpawnAI ai = other.gameObject.GetComponent<SpawnAI>();
		if (ai != null && ai.team != this.team)
        {
            canAttack = false;
            inCombat = true;
            combatWith = other.gameObject;
            ai.TakeDamage();
            StartCoroutine(WaitForCooldown());
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Our combatant has left us
        if (combatWith == null)
        {
            inCombat = false;
            return;
        }

        if (other.GetInstanceID() == combatWith.gameObject.GetInstanceID())
        {
            inCombat = false;
            combatWith = other.gameObject;
        }
    }

    IEnumerator WaitForCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void TakeDamage()
    {
        TakeDamage(1);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
		Debug.Log ("Taking " + amount + " points of damage.  Remaining health is " + health);
        if (health <= 0)
        {
            drop.Spawn(this.gameObject);
            Destroy(this.gameObject);
            parent.RemoveFromSpawnList(this);
        }
    }
}
