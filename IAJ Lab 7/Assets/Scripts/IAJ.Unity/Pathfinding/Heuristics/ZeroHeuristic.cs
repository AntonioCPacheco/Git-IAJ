using System;
using RAIN.Navigation.Graph;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics
{
    public class ZeroHeuristic : IHeuristic
    {
        public float H(Vector3 current, Vector3 target)
        {
            return 0;
        }

        public float H(NavigationGraphNode node, NavigationGraphNode goalNode)
        {
            return 0;
        }
    }
}
