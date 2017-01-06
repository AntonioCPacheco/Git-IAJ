using UnityEngine;
using System.Collections;
using Assets.Scripts.IAJ.Unity.Movement;
using System.Collections.Generic;
using System;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;

public class VelocityObstacle
{

    public List<KinematicData> Obstacles { get; set; }
    public List<KinematicData> DynamicObstacles { get; set; }
    public KinematicData Character { get; set; }
    public Vector3 apex { get; private set; }
    public Vector3 sideL { get; private set; }
    public Vector3 sideR { get; private set; }
    public double detectRadius { get; set; }

    public string Name
    {
        get { return "VelocityObstacle"; }
    }

    public VelocityObstacle(List<KinematicData> Obstacles, List<KinematicData> DynamicObstacles)
    {
        this.Obstacles = Obstacles;
        this.DynamicObstacles = DynamicObstacles;
    }

    public List<VelocityObstacle> calculateVOs()
    {
        List<VelocityObstacle> result = new List<VelocityObstacle>();

        foreach (KinematicData obstacle in DynamicObstacles)
        {

            Vector3 out1, out2;
            FindTangents((obstacle.position - Character.position), (obstacle.radius + Character.radius), Vector3.zero, out out1, out out2);
            this.sideL = out1.normalized;
            this.sideR = out2.normalized;
            this.apex = (obstacle.velocity + Character.velocity) / 2;

            float distL = (sideL - (Character.velocity - this.apex)).sqrMagnitude;
            float distR = (sideR - (Character.velocity - this.apex)).sqrMagnitude;
            if (distL == distR || distL > distR)
            {
                float size = 1000;
                Vector3 vec1 = apex;
                Vector3 vec2 = apex + sideR * size;
                Vector3 vec3 = obstacle.velocity;
                Vector3 vec4 = obstacle.velocity + sideL * size;
                apex = new Vector3(((vec1.x * vec2.z - vec1.z * vec2.x) * (vec3.x - vec4.x) - (vec1.x - vec2.x) * (vec3.x * vec4.z - vec3.z * vec4.x)) / ((vec1.x - vec2.x) * (vec3.z - vec4.z) - (vec1.z - vec2.z) * (vec3.x - vec4.x)),
                                   ((vec1.x * vec2.z - vec1.z * vec2.x) * (vec3.z - vec4.z) - (vec1.z - vec2.z) * (vec3.x * vec4.z - vec3.z * vec4.x)) / ((vec1.x - vec2.x) * (vec3.z - vec4.z) - (vec1.z - vec2.z) * (vec3.x - vec4.x)));
            }
            else
            {
                float size = 1000;
                Vector3 vec1 = apex;
                Vector3 vec2 = apex + sideL * size;
                Vector3 vec3 = obstacle.velocity;
                Vector3 vec4 = obstacle.velocity + sideR * size;
                apex = new Vector3(((vec1.x * vec2.z - vec1.z * vec2.x) * (vec3.x - vec4.x) - (vec1.x - vec2.x) * (vec3.x * vec4.z - vec3.z * vec4.x)) / ((vec1.x - vec2.x) * (vec3.z - vec4.z) - (vec1.z - vec2.z) * (vec3.x - vec4.x)),
                                   ((vec1.x * vec2.z - vec1.z * vec2.x) * (vec3.z - vec4.z) - (vec1.z - vec2.z) * (vec3.x * vec4.z - vec3.z * vec4.x)) / ((vec1.x - vec2.x) * (vec3.z - vec4.z) - (vec1.z - vec2.z) * (vec3.x - vec4.x)));
            }
            result.Add(this);
        }

        foreach (KinematicData obstacle in Obstacles)
        {
            Vector3 out1, out2;
            FindTangents((obstacle.position - Character.position), (obstacle.radius + Character.radius), Vector3.zero, out out1, out out2);
            this.sideL = out1.normalized;
            this.sideR = out2.normalized;
            this.apex = obstacle.velocity;

            result.Add(this);
        }
        return result;
    }

    public void FindTangents(Vector3 center, float radius, Vector3 external_point, out Vector3 out1, out Vector3 out2)
    {
        float dSquared = (center - external_point).sqrMagnitude;
        if (dSquared < radius * radius)
        {
            out1 = new Vector3();
            out2 = new Vector3();
        }
        else
        {
            float L = Mathf.Sqrt(dSquared - radius * radius);

            FindCirclesIntersections(center, radius, external_point, L, out out1, out out2);
        }
    }

    public void FindCirclesIntersections(Vector3 center1, float radius1, Vector3 center2, float radius2, out Vector3 out1, out Vector3 out2)
    {
        Vector3 diff = (center1 - center2);
        float dist = diff.magnitude;
        if (dist > (radius1 + radius2) || dist < Mathf.Abs(radius1 - radius2) || ((dist == 0) && (radius1 == radius2)))
        {
            out1 = new Vector3();
            out2 = new Vector3();
            return;
        }
        else
        {
            float a = (radius1 * radius1 - radius2 * radius2 + dist * dist) / (2 * dist);
            float h = Mathf.Sqrt(radius1 * radius1 - a * a);

            float cx2 = center1.x + a * (center2.x - center1.x) / dist;
            float cy2 = center1.z + a * (center2.z - center1.z) / dist;

            out1 = new Vector3((cx2 + h * (center2.z - center1.z) / dist), 0, (cy2 - h * (center2.x - center1.x) / dist));
            out2 = new Vector3((cx2 + h * (center2.z - center1.z) / dist), 0, (cy2 + h * (center2.x - center1.x) / dist));
        }
    }
}
