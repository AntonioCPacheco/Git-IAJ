using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicWander : DynamicSeek
    {
        public DynamicWander()
        {
            this.Target = new KinematicData();
            TurnAngle = 0.1f;
            WanderOffset = 5.0f;
            WanderRadius = 2.0f;
            WanderOrientation = 0.0f;
        }
        public override string Name
        {
            get { return "Wander"; }
        }
        public float TurnAngle { get; private set; }

        public float WanderOffset { get; private set; }
        public float WanderRadius { get; private set; }

        protected float WanderOrientation { get; set; }

        public override MovementOutput GetMovement()
        {
            WanderOrientation += RandomHelper.RandomBinomial() * TurnAngle;
            float nOrientation = WanderOrientation + Character.orientation;
            UnityEngine.Vector3 circleCenter = Character.position + WanderOffset * Character.GetOrientationAsVector();
            this.Target.position = circleCenter + WanderRadius * MathHelper.ConvertOrientationToVector(nOrientation);
            return base.GetMovement();
            //return new MovementOutput();
        }
    }
}
