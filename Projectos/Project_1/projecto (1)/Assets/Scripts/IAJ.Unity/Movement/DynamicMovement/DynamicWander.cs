using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicWander : DynamicSeek
    {
        public DynamicWander()
        {
            this.Target = new KinematicData();
            TurnAngle = .000001f;
            WanderOffset = 2.0f;
            WanderRadius = 1f;
            WanderOrientation = 0f;
        }
        public override string Name
        {
            get { return "Wander"; }
        }
        public float TurnAngle { get; set; }

        public float WanderOffset { get; set; }
        public float WanderRadius { get; set; }

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
