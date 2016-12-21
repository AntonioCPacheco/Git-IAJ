
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Util;
namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicArrival : DynamicSeek
    {
        public float maxSpeed { get; set; }
        public float stopRadius { get; set; }
        public float slowRadius { get; set; }
        public float targetSpeed { get; set; }

        public override string Name
        {
            get { return "Arrival"; }
        }

        public DynamicArrival()
        {
            this.Target = new KinematicData();
            /*maxSpeed = 4f;
            stopRadius = 15f;
            slowRadius = 35f;
            */
        }

        public override MovementOutput GetMovement()
        {
            Vector3 direction = this.Target.position - this.Character.position;
            float distance = direction.magnitude;
            if (distance < stopRadius)
            {
                targetSpeed = 0;
            }
            else if (distance > slowRadius)
            {
                targetSpeed = maxSpeed;
            }
            else
            {
                targetSpeed = maxSpeed * (distance / slowRadius);
            }
            this.Target.velocity = direction.normalized * targetSpeed ;
            return base.GetMovement();
        }
    }
}
