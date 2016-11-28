using RAIN.Navigation.Graph;
using Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures.HPStructures;
using UnityEngine;
using System;
using RAIN.Navigation.NavMesh;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics
{
    public class GatewayHeuristic : IHeuristic
    {
        private ClusterGraph ClusterGraph { get; set; }

        public GatewayHeuristic(ClusterGraph clusterGraph)
        {
            this.ClusterGraph = clusterGraph;
        }

        public float H(NavigationGraphNode node, NavigationGraphNode goalNode)
        {
            var startCluster = this.ClusterGraph.Quantize(node);
            var goalCluster = this.ClusterGraph.Quantize(goalNode);
            float h;
            float shortestDistance = float.MaxValue;

            //for now just returns the euclidean distance
            if (object.ReferenceEquals(startCluster, null) || object.ReferenceEquals(goalCluster, null) || startCluster == goalCluster)
                return -EuclideanDistance(node.LocalPosition, goalNode.LocalPosition);
            else
            {
                var gatewayDistanceTable = this.ClusterGraph.gatewayDistanceTable;
                for (int k = 0; k < startCluster.gateways.Count; k++)
                {
                    var startGateway = startCluster.gateways[k];
                    for (int l = 0; l < goalCluster.gateways.Count; l++)
                    {
                        var goalGateway = goalCluster.gateways[l];

                        h = EuclideanDistance(node.LocalPosition, startGateway.center) + EuclideanDistance(goalNode.LocalPosition, goalGateway.center) + gatewayDistanceTable[startGateway.id].entries[goalGateway.id].shortestDistance;
                        shortestDistance = Mathf.Min(shortestDistance,  h);
                    }
                    
                }
                return shortestDistance;
            }
        }

        public float EuclideanDistance(Vector3 startPosition, Vector3 endPosition)
        {
            return (endPosition - startPosition).magnitude;
        }
    }
}
