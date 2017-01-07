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
            float finalDistance = float.MaxValue;
            Vector3 finalVelocity = new Vector3();
            foreach (VelocityObstacle velocityObstacle in VOs)
            {
                lines.Add(new Line
                {
                    origin = velocityObstacle.apex + velocityObstacle.sideL * 1000,
                    end = velocityObstacle.apex + velocityObstacle.sideR * 1000
                });
            }

            foreach (Line line1 in lines)
            {
                foreach (Line line2 in lines)
                {
                    if (line1 != line2)
                    {
                        Vector3 intersection = line1.intersectWith(line2);
                        if (intersection != Vector3.zero)
                            intersections.Add(intersection);
                    }
                }
            }

            foreach (Vector3 intersection in intersections)
            {
                foreach (VelocityObstacle vobj in VOs)
                {
                    if (CheckInsideVO(intersection, vobj))
                    {
                        intersections.Remove(intersection);
                        break;
                    }
                }
            }
            foreach (Vector3 candidate in intersections)
            {
                float distance = GetDistance(i.prefVelocity.x, i.prefVelocity.z, candidate.x, candidate.z);
                if (distance < finalDistance)
                {
                    finalDistance = distance;
                    finalVelocity = candidate;
                }
            }
            i.velocity = finalVelocity;
            //Now that we have all the candidates, we need to check which of the candidates
            //is closer to the preferred velocity, and change the velocity of the Character
            //to the velocity
        }
    }

    private static float GetDistance(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt(Mathf.Pow((x2 - x1), 2) + Mathf.Pow((y2 - y1), 2));
    }

    bool CheckInsideVO(Vector3 intersection, VelocityObstacle vobj)
    {
        Line A = LineToClosestPoint(intersection, vobj.sideL, vobj.apex);
        Line B = LineToClosestPoint(intersection, vobj.sideR, vobj.apex);
        double angle = angleBetween2Lines(A, B) * (180.0 / Mathf.PI);
        if (angle > 90 && angle < 180)
        {
            return true;
        }
        else { return false; }

    }
    Line LineToClosestPoint(Vector3 intersection, Vector3 side, Vector3 apex)
    {
        Vector3 A = apex;
        Vector3 B = apex + side * 1000;
        Vector3 AP = intersection - A;
        Vector3 AB = B - A;
        float magnitudeAB = AB.sqrMagnitude;
        float ABAPproduct = Vector3.Dot(AP, AB);
        float distance = ABAPproduct / magnitudeAB;

        if (distance < 0)
        {
            return new Line { origin = intersection, end = A };
        }
        else if (distance > 1)
        {
            return new Line { origin = intersection, end = B };
        }
        else
        {
            return new Line { origin = intersection, end = A + AB * distance };
        }
    }
    float angleBetween2Lines(Line A, Line B)
    {
        float angle1 = Mathf.Atan2(A.origin.z - A.end.z, A.origin.x - A.end.x);
        float angle2 = Mathf.Atan2(B.origin.z - B.end.z, B.origin.x - B.end.x);
        return Mathf.Abs(angle1) - Mathf.Abs(angle2);
    }
}
