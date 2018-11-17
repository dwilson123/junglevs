using UnityEngine;
using System.Collections;

public class NPCSpawnAI : MonoBehaviour {


    public int health;
    public int team = 0; //No team
    public float speed = 0.1f;


    private Vector3 startingSpot;
    private GameObject target;
        	
	void Update () {
	
	}


    //Our spawner has been aggro'd by something I should follow
    //If a target is already assigned it will be overwritten
    void AssignTarget(GameObject target)
    {

    }

}
