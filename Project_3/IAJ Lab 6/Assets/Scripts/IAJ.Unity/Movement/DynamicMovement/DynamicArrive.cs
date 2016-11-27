using Assets.Scripts.IAJ.Unity.Utils;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicArrive : DynamicVelocityMatch
    {
        public DynamicArrive()
        {
            MaxSpeed = 20.0f; 
            StopRadius = .2f;
            SlowRadius = 6.0f;
        }

        public float MaxSpeed { get; set; }

        public float StopRadius { get; private set; }

        public float SlowRadius { get; set; }

        public float TargetRadius { get; set; }

        public override MovementOutput GetMovement()
        {
            Vector3 direction = this.Target.position - this.Character.position;
            float distance = direction.magnitude;
            float targetSpeed;

            if (distance < StopRadius)
                targetSpeed = 0;

            if (distance > SlowRadius)
                targetSpeed = MaxSpeed;
            else
                targetSpeed = MaxSpeed * (distance / SlowRadius);

            this.Target.velocity = direction.normalized * targetSpeed;

            return base.GetMovement();
        }
    }
}
