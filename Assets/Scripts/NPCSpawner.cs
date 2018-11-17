using UnityEngine;
using System.Collections;

public class NPCSpawner : MonoBehaviour {

    public GameObject spawnable;
    public GameObject fixate;

    public int team;
    public int maxSpawn = 1;

    //The interval for each spawn
    public float spawnInterval = 3.0f;

    //Should all the max spawn spawn as a group?
    public bool spawnImmediate = true;

    //Upgrade count
    public int factor = 1;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(spawnSpawnable());
    }

    private IEnumerator spawnSpawnable()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            GameObject go = Instantiate(spawnable, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            Material m = GetComponent<Renderer>().material;
            go.GetComponent<Renderer>().material = m;
            go.GetComponent<SpawnAI>().target = fixate;
            go.GetComponent<SpawnAI>().health = factor;
            go.GetComponent<SpawnAI>().team = this.team;
            go.transform.localScale += new Vector3(factor / 4, factor / 4, factor / 4);
        }
    }
}
