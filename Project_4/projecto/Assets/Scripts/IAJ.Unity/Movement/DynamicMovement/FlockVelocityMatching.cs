using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.Util;
namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{ 
    public class FlockVelocityMatching : DynamicVelocityMatch {

        

        public override string Name
        {
            get { return "FlockVelocityMatch"; }
        }

        public List<DynamicCharacter> flock { get; set; }

        public float radius { get; set; }

        public float fanAngle { get; set; }


        public override MovementOutput GetMovement()
        {
            Vector3 averageVelocity = new Vector3();
            int closeBoids = 0;

            foreach(DynamicCharacter boid in flock) {
                if(this.Character != boid.KinematicData)
                {
                    Vector3 direction = boid.KinematicData.position - this.Character.position;

                    if(direction.magnitude <= radius)
                    {
                        float angle = MathHelper.ConvertVectorToOrientation(direction);
                        float angleDifference = Mathf.DeltaAngle(this.Character.orientation, angle);

                        if(Mathf.Abs(angleDifference) <= fanAngle)
                        {
                            averageVelocity += boid.KinematicData.velocity;
                            closeBoids++;
                        }
                    }
                }



            }

            if (closeBoids == 0)
                return new MovementOutput();

            averageVelocity /= closeBoids;
            Target.velocity = averageVelocity;

            return base.GetMovement();

            /*
            var output = new MovementOutput();
            output.linear = (this.MovingTarget.velocity - this.Character.velocity) / this.TimeToTargetSpeed;

            if (output.linear.sqrMagnitude > this.MaxAcceleration * this.MaxAcceleration)
            {
                output.linear = output.linear.normalized * this.MaxAcceleration;
            }
            output.angular = 0;
            return output;
            */

        }
    }
}