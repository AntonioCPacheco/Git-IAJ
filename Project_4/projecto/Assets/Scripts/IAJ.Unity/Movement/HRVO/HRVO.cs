using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.Movement;

public class HRVO : MonoBehaviour {

    public KinematicData Target { get; set; }
    public KinematicData Character { get; set; }
    public List<KinematicData> Obstacles { get; set; }
    public List<KinematicData> DynamicObstacles { get; set; }

    public class Line
    {
        public Vector3 origin { get; set; }
        public Vector3 end { get; set; }

        public Vector3 intersectWith(Line line)
        {
            try
            {
                return new Vector3(((this.origin.x * this.end.z - this.origin.z * this.end.x) * (line.origin.x - line.end.x) - (this.origin.x - this.end.x) * (line.origin.x * line.end.z - line.origin.z * line.end.x)) / ((this.origin.x - this.end.x) * (line.origin.z - line.end.z) - (this.origin.z - this.end.z) * (line.origin.x - line.end.x)),
                             ((this.origin.x * this.end.z - this.origin.z * this.end.x) * (line.origin.z - line.end.z) - (this.origin.z - this.end.z) * (line.origin.x * line.end.z - line.origin.z * line.end.x)) / ((this.origin.x - this.end.x) * (line.origin.z - line.end.z) - (this.origin.z - this.end.z) * (line.origin.x - line.end.x)));
            }
            catch (System.Exception e)
            {
                return Vector3.zero;
            }
        }
    }

    // Use this for initialization
    void Start () {
        GameObject[] objArray = GameObject.FindGameObjectsWithTag("Obstacle");
        this.Obstacles = new List<KinematicData>();
        for(int i=0; i<objArray.Length; i++)
        {
            this.Obstacles.Add(new KinematicData(new StaticData(objArray[i].transform.position)));
        }
        objArray = GameObject.FindGameObjectsWithTag("DynamicObstacle");
        this.DynamicObstacles = new List<KinematicData>();
        for (int i = 0; i < objArray.Length; i++)
        {
            this.DynamicObstacles.Add(new KinematicData(new StaticData(objArray[i].transform.position)));
        }
    }
	
	// Update is called once per frame
	void Update () {
        foreach (KinematicData i in DynamicObstacles)
        {
            VelocityObstacle VO = new VelocityObstacle(Obstacles, DynamicObstacles)
            {
                Character = i,
                detectRadius = 20f
            };

            List<VelocityObstacle> VOs = VO.calculateVOs();

            List<Vector3> intersections = new List<Vector3>();
            List<Line> lines = new List<Line>();
            foreach (VelocityObstacle velocityObstacle in VOs)
            {
                lines.Add(new Line
                {
                    origin = velocityObstacle.apex + velocityObstacle.sideL * 1000,
                    end = velocityObstacle.apex + velocityObstacle.sideR * 1000
                });
            }

            foreach(Line line1 in lines)
            {
                foreach(Line line2 in lines)
                {
                    if(line1 != line2)
                    {
                        Vector3 intersection = line1.intersectWith(line2);
                        if(intersection != Vector3.zero)
                            intersections.Add(intersection);
                    }
                }
            }

            foreach(Vector3 intersection in intersections)
            {
                ///CONTINUE
                //foreach VO in VOs
                //  if intersection is inside VO
                //      remove intersection from intersections 
            }

            //Now that we have all the candidates, we need to check which of the candidates
            //is closer to the preferred velocity, and change the velocity of the Character
            //to the velocity
        }
	}
}
