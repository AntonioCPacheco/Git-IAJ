using UnityEngine;
using RAIN.Navigation.Graph;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics
{
    public class EuclideanDistanceHeuristic : IHeuristic
    {
        public float H(NavigationGraphNode node, NavigationGraphNode goalNode)
        {
            return Mathf.Sqrt(Mathf.Pow(goalNode.Position.x - node.Position.x, 2) + Mathf.Pow(goalNode.Position.y - node.Position.y, 2));
        }
    }
}
