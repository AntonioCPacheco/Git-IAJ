using Assets.Scripts.IAJ.Unity.Utils;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicArrive : DynamicVelocityMatch
    {
        public DynamicArrive()
        {
            MaxSpeed = 20.0f; 
            StopRadius = 1.0f;
            SlowRadius = 5.0f;
        }

        public float MaxSpeed { get; private set; }

        public float StopRadius { get; private set; }

        public float SlowRadius { get; private set; }

        public override MovementOutput GetMovement()
        {
            Vector3 direction = this.Target.position - this.Character.position;
            float distance = direction.magnitude;

            if (distance < StopRadius)
                return null;

            float targetSpeed;
            if (distance > SlowRadius)
                targetSpeed = MaxSpeed;
            else
                targetSpeed = MaxSpeed * (distance / SlowRadius);

            this.Target.velocity = direction.normalized * targetSpeed;

            return base.GetMovement();
        }
    }
}
