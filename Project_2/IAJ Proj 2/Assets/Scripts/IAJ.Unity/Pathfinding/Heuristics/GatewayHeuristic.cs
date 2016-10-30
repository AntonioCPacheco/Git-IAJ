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

        public float H(NavigationGraphNode node, NavigationGraphNode goalNode)
        {
            var startCluster = this.ClusterGraph.Quantize(node);
            var goalCluster = this.ClusterGraph.Quantize(goalNode);

            //for now just returns the euclidean distance
            if (object.ReferenceEquals(startCluster, null) || object.ReferenceEquals(goalCluster, null) || startCluster == goalCluster)
                return EuclideanDistance(node.LocalPosition, goalNode.LocalPosition);
            //TODO implement this properly
            else
            {
                float shortestDistance = 10000000000000000000000000000000f;
                var gatewayDistanceTable = this.ClusterGraph.gatewayDistanceTable;
                for (int i = 0; i < gatewayDistanceTable.Length; i++)
                {
                    for (int j = 0; j < gatewayDistanceTable[i].entries.Length; j++)
                    { 
                        for (int k = 0; k < startCluster.gateways.Count; k++)
                        { 
                            for (int l = 0; l < goalCluster.gateways.Count; l++)
                            { 
                                if (startCluster.gateways[k].center == gatewayDistanceTable[i].entries[j].startGatewayPosition && goalCluster.gateways[l].center == gatewayDistanceTable[i].entries[j].endGatewayPosition)
                                    shortestDistance = Mathf.Min(shortestDistance, EuclideanDistance(node.LocalPosition, startCluster.gateways[k].center) + gatewayDistanceTable[i].entries[j].shortestDistance + EuclideanDistance(goalNode.LocalPosition, goalCluster.gateways[l].center));
                            }
                        }
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
