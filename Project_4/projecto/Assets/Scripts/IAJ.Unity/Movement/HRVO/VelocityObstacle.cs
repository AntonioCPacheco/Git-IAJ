using UnityEngine;
using System.Collections;
using Assets.Scripts.IAJ.Unity.Movement;
using System.Collections.Generic;
using System;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;

public class VelocityObstacle : DynamicStraightAhead {

    public List<KinematicData> Obstacles { get; set; }
    public new KinematicData Character { get; set; }

    public override string Name
    {
        get { return "VelocityObstacle"; }
    }

    public override KinematicData Target
    { get; set; }

    public VelocityObstacle(GameObject[] objects)
    {
        this.MovementDebugColor = Color.red;
        this.Obstacles = new List<KinematicData>();
        for (int i = 0; i < objects.Length; i++)
            this.Obstacles.Add(new KinematicData(new StaticData(objects[i].transform.position)));
    }

    public Vector3 calculateVO (KinematicData target)
    {
        RaycastHit rayHit = new RaycastHit();
        Vector3 position = target.position - this.Character.position;
        Vector3 velocity = Vector3.zero;
        float radius = 2.0f;
        Vector3 leftR = position + target.velocity;
        Vector3 rightR = position + target.velocity;
        leftR.x -= radius;
        rightR.x += radius;

        /*if (Physics.Raycast(target.velocity, leftR, out rayHit, 5f))
        {
            velocity = this.Character.velocity + rightR;
        }

        else if (Physics.Raycast(target.velocity, position, out rayHit, 5f))
        {
            leftR.x += radius * 0.5f;
            rightR.x -= radius * 0.5f;
            if (Physics.Raycast(target.velocity, rightR, out rayHit, 5f)) {
                leftR.x -= radius * 2f;
                rightR.x += radius * 2f;
                velocity = this.Character.velocity - leftR;
            }
            else if (Physics.Raycast(target.velocity, leftR, out rayHit, 5f)) {
                leftR.x -= radius * 2f;
                rightR.x += radius * 2f;
                velocity = this.Character.velocity + rightR;
            }
        }

        else if (Physics.Raycast(target.velocity, rightR, out rayHit, 5f))
        {
            velocity = this.Character.velocity - leftR;
        }*/
        if (Physics.Raycast(this.Character.position, this.Character.velocity, out rayHit, 5f))
            velocity = leftR;
        return velocity;
    }

    public override MovementOutput GetMovement()
    {
        var output = new MovementOutput();
        
        for (int i = 0; i < this.Obstacles.Count; i++)
        {
            output.linear += this.calculateVO(this.Obstacles[i]);
            
        }
        if (output.linear.Equals(Vector3.zero))
            return base.GetMovement();
        else
            return output;
    }
}
