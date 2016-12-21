using UnityEngine;
using System.Collections;
namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{ 
public class FleeFromClick : DynamicFlee
    {
        public float fleeRadius { get; set; }
        public Camera camera { get; set; }
        public override string Name
        {
            get { return "FleeFromClick"; }
        }

        
         public override MovementOutput GetMovement()
         {
             var output = new MovementOutput();
             if (Input.GetMouseButton(0))
             {
                Vector3 mouseAux = Input.mousePosition;
                mouseAux.z = camera.transform.position.y;
                mouseAux = camera.ScreenToWorldPoint(mouseAux);
                output.linear = this.Character.position - mouseAux;
                if (output.linear.sqrMagnitude > 0 && output.linear.sqrMagnitude <= (fleeRadius * fleeRadius))
                {

                    output.linear.Normalize();
                    output.linear *= this.MaxAcceleration;
                    output.angular = 0;
                    return output;
                }
            }
            return new MovementOutput();
        }
    }

}
