using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

    public GameObject spawnable;
    public GameObject fixate;
    public Material spawnMaterial;

    //This is the valuable which will drop from Spawned minions
    public Drop drop;

    public int team;

    //Global upgrade for all spawnables
    public int factor = 1;

    //Max number of spawns up at one time
    public int maxSpawn = 1;

    //The interval for each spawn
    public float spawnInterval = 3.0f;

    //Should all the max spawn spawn as a group?
    public bool spawnImmediate = true;

    private List<SpawnAI> spawns = new List<SpawnAI>();

    void Start () {
        SetUpgradeText();
        StartCoroutine(spawnSpawnable());
	}

    private IEnumerator spawnSpawnable()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (spawns.Count < maxSpawn)
            {
                int numSpawn = CalculateNumToSpawn();
                for (int x = 0; x < numSpawn; x++)
                {
                    Vector3 spawnLocation = this.gameObject.transform.position;
                    
                    //We need to lightly scatter spawnables which are all spawning at once
                    if (spawnImmediate)
                    {
                        spawnLocation.x += x;
                        spawnLocation.z += x;
                    }

                    GameObject go = Instantiate(spawnable, spawnLocation, Quaternion.identity) as GameObject;
                    
                    go.GetComponent<Renderer>().material = spawnMaterial;
                    
                    SpawnAI spawned = go.GetComponent<SpawnAI>();

                    //Some items assigned don't immediately fixate
                    if (fixate != null)
                    {
                        spawned.target = fixate;
                    }
                    spawned.health = factor;
                    spawned.team = this.team;
                    spawned.parent = this;
                    spawned.drop = drop;
                    go.transform.localScale += new Vector3(factor / 4, factor / 4, factor / 4);

                    spawns.Add(spawned);
                }
            }
        }
    }


    public void AssignNewTarget(GameObject go)
    {
        for(int x = 0; x < spawns.Count; x++)
        {
            spawns[x].target = go;
        }
    }

    //Eventually this should be better than constantly instancing stuff
    public void RemoveFromSpawnList(SpawnAI spawn)
    {
        spawns.Remove(spawn);
    }

    public void Upgrade()
    {
        Upgrade(1);
    }

    public void Upgrade(int amount)
    {
        this.factor += amount;
        SetUpgradeText();
    }

    private void SetUpgradeText()
    {
        TextMesh tm = this.GetComponentInChildren<TextMesh>();
        if (tm != null)
            tm.text = factor.ToString();
    }

    private int CalculateNumToSpawn()
    {
        if (spawnImmediate)
        {
            return maxSpawn - spawns.Count;
        }
        else
        {
            return 1;
        }
    }
}
