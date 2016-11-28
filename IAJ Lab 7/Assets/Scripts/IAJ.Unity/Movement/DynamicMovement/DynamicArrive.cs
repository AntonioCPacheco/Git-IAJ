using Assets.Scripts.IAJ.Unity.Utils;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicArrive : DynamicSeek
    {
        public DynamicArrive()
        {
            MaxSpeed = 20.0f; 
            StopRadius = 0.0f;
            SlowRadius = 5.0f;
        }

        public float MaxSpeed { get; set; }

        public float StopRadius { get; set; }

        public float SlowRadius { get; set; }

        public float TimeToTargetSpeed { get; set; }

        public float TargetRadius { get; set; }

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
