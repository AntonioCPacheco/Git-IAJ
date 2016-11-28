using RAIN.Navigation.Graph;
using Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures.HPStructures;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics
{
    public class GatewayHeuristic : IHeuristic
    {
        private ClusterGraph ClusterGraph { get; set; }

        public GatewayHeuristic(ClusterGraph clusterGraph)
        {
            this.ClusterGraph = clusterGraph;
        }

        public float H(Vector3 current, Vector3 end)
        {
            var startCluster = this.ClusterGraph.Quantize(current);
            var goalCluster = this.ClusterGraph.Quantize(end);
            float h;
            float shortestDistance = float.MaxValue;

            //for now just returns the euclidean distance
            if (object.ReferenceEquals(startCluster, null) || object.ReferenceEquals(goalCluster, null) || object.ReferenceEquals(startCluster, goalCluster))
                return EuclideanDistance(current, end);
            //TODO implement this properly
            else
            {
                var gatewayDistanceTable = this.ClusterGraph.gatewayDistanceTable;
                for (int k = 0; k < startCluster.gateways.Count; k++)
                {
                    var startGateway = startCluster.gateways[k];
                    for (int l = 0; l < goalCluster.gateways.Count; l++)
                    {
                        var goalGateway = goalCluster.gateways[l];

                        h = EuclideanDistance(current, startGateway.center) + EuclideanDistance(end, goalGateway.center) + gatewayDistanceTable[startGateway.id].entries[goalGateway.id].shortestDistance;
                        shortestDistance = Mathf.Min(shortestDistance, h);
                    }

                }
                return shortestDistance;
            }
        }

        public float H(NavigationGraphNode node, NavigationGraphNode goalNode)
        {
            var startCluster = this.ClusterGraph.Quantize(node);
            var goalCluster = this.ClusterGraph.Quantize(goalNode);
            float h;
            float shortestDistance = float.MaxValue;

            //for now just returns the euclidean distance
			if (object.ReferenceEquals(startCluster, null) || object.ReferenceEquals(goalCluster, null) || object.ReferenceEquals(startCluster, goalCluster))
                return EuclideanDistance(node.LocalPosition, goalNode.LocalPosition);
            //TODO implement this properly
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
