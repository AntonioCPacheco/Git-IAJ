using UnityEngine;
using RAIN.Navigation.Graph;
using System;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics
{
    public class EuclideanDistanceHeuristic : IHeuristic
    {
        public float H(Vector3 current, Vector3 target)
        {
            return Mathf.Sqrt(Mathf.Pow(target.x - current.x, 2) + Mathf.Pow(target.y - current.y, 2));
        }

        public float H(NavigationGraphNode node, NavigationGraphNode goalNode)
        {
            return Mathf.Sqrt(Mathf.Pow(goalNode.Position.x - node.Position.x, 2) + Mathf.Pow(goalNode.Position.y - node.Position.y, 2));
        }
    }
}
