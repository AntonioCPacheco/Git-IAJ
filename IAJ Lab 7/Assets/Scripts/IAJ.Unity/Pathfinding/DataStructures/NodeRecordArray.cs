using System;
using System.Collections.Generic;
using System.Linq;
using RAIN.Navigation.Graph;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures
{
    public class NodeRecordArray : IOpenSet, IClosedSet
    {
        private NodeRecord[] NodeRecords { get; set; }
        private List<NodeRecord> SpecialCaseNodes { get; set; } 
        private NodePriorityHeap Open { get; set; }

        public NodeRecordArray(List<NavigationGraphNode> nodes)
        {
            //this method creates and initializes the NodeRecordArray for all nodes in the Navigation Graph
            this.NodeRecords = new NodeRecord[nodes.Count];
            
            for(int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                node.NodeIndex = i; //we're setting the node Index because RAIN does not do this automatically
                this.NodeRecords[i] = new NodeRecord {node = node, status = NodeStatus.Unvisited};
            }

            this.SpecialCaseNodes = new List<NodeRecord>();

            this.Open = new NodePriorityHeap();
        }

        public NodeRecord GetNodeRecord(NavigationGraphNode node)
        {
            //do not change this method
            //here we have the "special case" node handling
            if (node.NodeIndex == -1)
            {
                for (int i = 0; i < this.SpecialCaseNodes.Count; i++)
                {
                    if (node == this.SpecialCaseNodes[i].node)
                    {
                        return this.SpecialCaseNodes[i];
                    }
                }
                return null;
            }
            else
            {
                return  this.NodeRecords[node.NodeIndex];
            }
        }

        public void AddSpecialCaseNode(NodeRecord node)
        {
            this.SpecialCaseNodes.Add(node);
        }

        void IOpenSet.Initialize()
        {
            this.Open.Initialize();
            //we want this to be very efficient (that's why we use for)
            for (int i = 0; i < this.NodeRecords.Length; i++)
            {
                this.NodeRecords[i].status = NodeStatus.Unvisited;
            }

            this.SpecialCaseNodes.Clear();
        }

        void IClosedSet.Initialize()
        {
            //TODO implement
            return;
        }

        public void AddToOpen(NodeRecord nodeRecord)
        {
            //TODO implement
            nodeRecord.status = NodeStatus.Open;
            this.Open.AddToOpen(nodeRecord);
        }

        public void AddToClosed(NodeRecord nodeRecord)
        {
            //TODO implement
            nodeRecord.status = NodeStatus.Closed;
        }

        public NodeRecord SearchInOpen(NodeRecord nodeRecord)
        {
            //TODO implement
            return this.Open.SearchInOpen(nodeRecord);
        }

        public NodeRecord SearchInClosed(NodeRecord nodeRecord)
        {
            //TODO implement
            if (nodeRecord.status != NodeStatus.Closed)
                return null;
            return nodeRecord;
        }

        public NodeRecord GetBestAndRemove()
        {
            //TODO implement
            return this.Open.GetBestAndRemove();
        }

        public NodeRecord PeekBest()
        {
            //TODO implement
            return this.Open.PeekBest();
        }

        public void Replace(NodeRecord nodeToBeReplaced, NodeRecord nodeToReplace)
        {
            //TODO implement
            if (nodeToBeReplaced.status == NodeStatus.Open)
                this.Open.Replace(nodeToBeReplaced, nodeToReplace);
            nodeToBeReplaced.gValue = nodeToReplace.gValue;
            nodeToBeReplaced.hValue = nodeToReplace.hValue;
            nodeToBeReplaced.fValue = nodeToReplace.fValue;
        }

        public void RemoveFromOpen(NodeRecord nodeRecord)
        {
            //TODO implement
            this.Open.RemoveFromOpen(nodeRecord);
        }

        public void RemoveFromClosed(NodeRecord nodeRecord)
        {
            //TODO implement
            nodeRecord.status = NodeStatus.Open;
        }

        ICollection<NodeRecord> IOpenSet.All()
        {
            //TODO implement
            return this.Open.All();
        }

        ICollection<NodeRecord> IClosedSet.All()
        {
            return this.NodeRecords.Where(node => node.status == NodeStatus.Closed).ToList();
        }

        public int CountOpen()
        {
            //TODO implement
            return this.Open.CountOpen();
        }
    }
}
