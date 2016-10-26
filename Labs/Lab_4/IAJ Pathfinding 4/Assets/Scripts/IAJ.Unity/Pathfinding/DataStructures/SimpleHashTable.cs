using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures
{
    //very simple (and unefficient) implementation of the open/closed sets
    public class SimpleHashTable : IClosedSet
    {
        private uint addToClosedCalls { get; set; }
        private uint searchInClosedCalls { get; set; }
        private uint removeFromClosedCalls { get; set; }

        private Dictionary<NodeRecord,NodeRecord> NodeRecords { get; set; }

        public SimpleHashTable()
        {
            this.NodeRecords = new Dictionary<NodeRecord,NodeRecord>();
            this.addToClosedCalls = 0;
            this.searchInClosedCalls = 0;
            this.removeFromClosedCalls = 0;
        }

        public void Initialize()
        {
            this.NodeRecords.Clear();
        }

        public int Count()
        {
            return this.NodeRecords.Count;
        }

        public void AddToClosed(NodeRecord nodeRecord)
        {
            this.NodeRecords.Add(nodeRecord, nodeRecord);
        }

        public void RemoveFromClosed(NodeRecord nodeRecord)
        {
            this.NodeRecords.Remove(nodeRecord);
        }

        public NodeRecord SearchInClosed(NodeRecord nodeRecord)
        {
            if (this.NodeRecords.ContainsKey(nodeRecord))
                return this.NodeRecords[nodeRecord];
            else
                return null;
        }

        public ICollection<NodeRecord> All()
        {
            return this.NodeRecords.Values;
        }

        public void Replace(NodeRecord nodeToBeReplaced, NodeRecord nodeToReplace)
        {
            this.NodeRecords.Remove(nodeToBeReplaced);
            this.NodeRecords.Add(nodeToReplace, nodeToReplace);
        }

        public NodeRecord GetBestAndRemove()
        {
            var best = this.PeekBest();
            this.NodeRecords.Remove(best);
            return best;
        }

        public NodeRecord PeekBest()
        {
            var result = this.NodeRecords.Aggregate((nodeRecord1, nodeRecord2) => nodeRecord1.Value.fValue <= nodeRecord2.Value.fValue ? nodeRecord1 : nodeRecord2);
            return result.Value;
        }
        
    }
}
