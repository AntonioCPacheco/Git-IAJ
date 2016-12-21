using System;
using Assets.Scripts.IAJ.Unity.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class Separation : DynamicMovement
    {

        public override string Name
        {
            get { return "Separation"; }
        }

        public override KinematicData Target { get; set; }

        public float radius { get; set; }

        public float separationFactor { get; set; }

        public List<DynamicCharacter> flock { get; set; }

        public override MovementOutput GetMovement()
        {
            var output = new MovementOutput();

            foreach(var boid in flock)
            {
                if(!boid.Equals(this.Character))
                {
                    Vector3 direction = this.Character.position - boid.KinematicData.position;
                    float distance = direction.magnitude;
                    if (distance < radius)
                    {
                        float separationStrength = Mathf.Min((separationFactor / (distance * distance)), MaxAcceleration);
                        Vector3 normal = direction.normalized;
                        output.linear += normal * separationStrength;
                    }
                }
            }

            if(output.linear.magnitude > MaxAcceleration)
            {
                output.linear = output.linear.normalized;
                output.linear *= MaxAcceleration;
            }

            return output;
        }
    }
}
