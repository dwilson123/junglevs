using UnityEngine;
using System.Collections;

public class AggroRangeSensor : MonoBehaviour {


    //Spawner will assign targets
    public Spawner spawner;

    private PCInputManager collidedWith;
    private bool pulse = false;

    void Start()
    {
        StartCoroutine(DoPulse());
    }

    IEnumerator DoPulse()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            if (pulse)
            {
                this.gameObject.GetComponent<SphereCollider>().enabled = !this.gameObject.GetComponent<SphereCollider>().enabled;
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (collidedWith != null)
        {
            return;
        }

        PCInputManager pc = other.gameObject.GetComponent<PCInputManager>();

        if (pc == null)
        {
            return;
        }
        
        spawner.AssignNewTarget(pc.gameObject);
        pulse = true;
    }

    void OnTriggerExit(Collider other)
    {
        PCInputManager pc = other.gameObject.GetComponent<PCInputManager>();

        if (collidedWith != null && collidedWith.GetInstanceID().Equals(pc.GetInstanceID()))
        {
            collidedWith = null;
            spawner.AssignNewTarget(null);
            pulse = false;
        }
    }

}
