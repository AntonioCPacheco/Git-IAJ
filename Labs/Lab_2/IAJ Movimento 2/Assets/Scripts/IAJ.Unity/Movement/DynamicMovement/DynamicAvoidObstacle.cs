using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicAvoidObstacle : DynamicSeek
    {
        public DynamicAvoidObstacle(GameObject obstacle)
        {
            this.Target = new KinematicData(new StaticData(obstacle.transform.position));
            
        }
        public override string Name
        {
            get { return "AvoidObstacle"; }
        }
        
        public float lookAhead { get; set; }

        public float avoidDistance { get; set; }

        public override MovementOutput GetMovement()
        {
            Vector3 rayVector = this.Character.velocity.normalized * lookAhead;

            RaycastHit hit;

            if (!Physics.Raycast(this.Character.position, rayVector, out hit))
			    return null;

            this.Target.position = hit.point + hit.normal * avoidDistance;
            return base.GetMovement();
        }
    }
}
