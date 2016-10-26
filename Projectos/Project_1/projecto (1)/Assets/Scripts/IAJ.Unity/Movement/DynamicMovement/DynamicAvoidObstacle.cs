using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicAvoidObstacle : DynamicSeek
    {
        public DynamicAvoidObstacle(GameObject obstacle)
        {
            this.Target = new KinematicData(new StaticData(obstacle.transform.position));
            this._obstacle = obstacle;
        }
        public override string Name
        {
            get { return "AvoidObstacle"; }
        }
        
        public bool _avoiding { get; private set; }

        protected GameObject _obstacle { get; private set; }

        public float lookAhead { get; set; }

        public float avoidDistance { get; set; }

        public override MovementOutput GetMovement()
        {
            if (this.Character.velocity.magnitude != 0)
            {
                Ray rayVector = new Ray(this.Character.position, this.Character.velocity.normalized * lookAhead);

                RaycastHit hit;

                Collider collider = this._obstacle.GetComponent<Collider>();

                if (collider.Raycast(rayVector, out hit, lookAhead))
                {
                    this.Target.position = hit.point + hit.normal * avoidDistance;
                    this.Target.position.y = 0;
                    _avoiding = true;
                    //Debug.Log(_avoiding);
                    return base.GetMovement();
                }
            }
            _avoiding = false;
            return new MovementOutput();
        }

    }
}
