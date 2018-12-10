using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PCInputManager : MonoBehaviour {

    Vector3 target;
    public const float speed = .03f;
    float y;
    Camera cam;
    Weapon weapon;

	//Starting coins
    public int value = 0;
    private int upgradeCost = 10;

    //Set these in editor
    public int team;
    public Text valueText;

	void Start () {
        valueText.text = value.ToString();
        y = this.gameObject.transform.position.y;
        cam = this.GetComponent<Camera>();
        
        weapon = this.GetComponentInChildren<Weapon>();

        //This puts it in the middle
        Vector3 weaponPos = this.gameObject.transform.position;
        weaponPos.x += weaponPos.x + (weapon.transform.localScale.x / 2);
        weapon.transform.position = weaponPos;
        weapon.gameObject.SetActive(false);
    }
	
	void Update () {

        // Check for mouse input to update position
	    if (Input.GetMouseButtonDown(1)) //RMB
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                target = new Vector3(hit.point.x, y, hit.point.z);
            }
        }

        // If we've arrived stop moving
        if (target.Equals(this.gameObject.transform.position))
        {
            target = Vector3.zero;
        }


        //If we have a target move towards it
        if (target != Vector3.zero)
        {
			this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, target, speed);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Attack();
        }

        if (Input.GetKeyUp(KeyCode.U))
        {
            Upgrade();
        }
    }

    public void Upgrade()
    {
        if (this.value < upgradeCost)
        {
            //Do some alert
            return;
        }

        //Ensure we actually upgraded something in case
        //all spawners are destroyed
        bool upgraded = false;
        Spawner[] spawners = FindObjectsOfType<Spawner>();

        foreach(Spawner spawner in FindObjectsOfType<Spawner>())
        {
            if (spawner.team == this.team)
            {
                spawner.Upgrade();
                upgraded = true;
            }
        }

        if (upgraded)
        {
            AddValue(-1);
        }
    }

    public void Attack()
    {
        weapon.Attack();
    }

    public void AddValue(int value)
    {
        this.value += value;
        valueText.text = "Coins: " + this.value.ToString();
    }

    public void SubtractValue(int value)
    {
        this.value -= value;
        valueText.text = this.value.ToString();
    }

}
