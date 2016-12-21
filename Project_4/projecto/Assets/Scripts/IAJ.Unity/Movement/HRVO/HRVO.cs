using UnityEngine;
using System.Collections;
using Assets.Scripts.IAJ.Unity.Movement;

public class HRVO : MonoBehaviour {

    public KinematicData Target { get; set; }
    public KinematicData Character { get; set; }

    public void calculateVO()
    {
        RaycastHit rayHit = new RaycastHit();
        Vector3 position = this.Target.position - this.Character.position;
        float radius = 2.0f;
        Vector3 leftR = position;
        Vector3 rightR = position;
        Vector3 rvoVelocity = (this.Target.velocity + this.Character.velocity) * 0.5f;
        leftR.x -= radius;
        rightR.x += radius;

        if (Physics.Raycast(rvoVelocity, leftR, out rayHit))
        {
            this.Character.velocity -= leftR;
        }

        else if (Physics.Raycast(rvoVelocity, position, out rayHit))
        {
            this.Character.velocity -= leftR;
        }

        else if (Physics.Raycast(rvoVelocity, rightR, out rayHit))
        {
            this.Character.velocity += rightR;
        }

    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
