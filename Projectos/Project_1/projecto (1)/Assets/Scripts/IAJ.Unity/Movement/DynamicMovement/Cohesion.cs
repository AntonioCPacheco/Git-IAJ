using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.Util;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{

    public class Cohesion : DynamicArrival
    {

        public List<DynamicCharacter> flock { get; set; }

        public float radius { get; set; }
        public float fanAngle { get; set; }


        public override string Name
        {
            get { return "Cohesion"; }
        }

        public override MovementOutput GetMovement()
        {
            Vector3 massCenter = new Vector3();
            float closeBoids = 0;
            foreach(DynamicCharacter boid in flock)
            {
                if(this.Character != boid.KinematicData)
                {
                    Vector3 direction = boid.KinematicData.position - this.Character.position;
                    if(direction.magnitude <= radius)
                    {
                        float angle = MathHelper.ConvertVectorToOrientation(direction);
                        float angleDifference = Mathf.DeltaAngle(this.Character.orientation, angle);

                        if(Mathf.Abs(angleDifference) <= fanAngle)
                        {
                            massCenter += boid.KinematicData.position;
                            closeBoids++;
                        }
                    }
                }
            }
            if (closeBoids == 0) return new MovementOutput();
            massCenter /= closeBoids;
            Target.position = massCenter;

            return base.GetMovement();
        }
    }
}