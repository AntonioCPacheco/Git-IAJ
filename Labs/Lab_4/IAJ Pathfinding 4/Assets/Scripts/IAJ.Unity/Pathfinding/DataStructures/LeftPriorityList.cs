using System;
using System.Collections.Generic;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures
{
    public class LeftPriorityList : IOpenSet
    {
        private List<NodeRecord> Open { get; set; }

        public LeftPriorityList()
        {
            this.Open = new List<NodeRecord>();    
        }
        public void Initialize()
        {
            //TODO implement
            this.Open.Clear();
        }

        public void Replace(NodeRecord nodeToBeReplaced, NodeRecord nodeToReplace)
        {
            //TODO implement
            int index = this.Open.BinarySearch(nodeToBeReplaced);
            if (index < 0)
            {
                this.Open.Insert(~index, nodeToReplace);
            } else
            {
                this.Open.Remove(nodeToBeReplaced);
                this.Open.Insert(index, nodeToReplace);
            }
        }

        public NodeRecord GetBestAndRemove()
        {
            //TODO implement
            var best = this.PeekBest();
            this.Open.Remove(best);
            return best;
        }

        public NodeRecord PeekBest()
        {
            //TODO implement
            return this.Open[0];
        }

        public void AddToOpen(NodeRecord nodeRecord)
        {
            //a little help here
            //is very nice that the List<T> already implements a binary search method
            int index = this.Open.BinarySearch(nodeRecord);
            if (index < 0)
            {
                this.Open.Insert(~index, nodeRecord);
            }
        }

        public void RemoveFromOpen(NodeRecord nodeRecord)
        {
            //TODO implement
            this.Open.Remove(nodeRecord);
        }

        public NodeRecord SearchInOpen(NodeRecord nodeRecord)
        {
            //TODO implement
            int index = this.Open.BinarySearch(nodeRecord);
            return this.Open[index];

        }

        public ICollection<NodeRecord> All()
        {
            //TODO implement
            return this.Open;
        }

        public int CountOpen()
        {
            //TODO implement
            return this.Open.Count;
        }
    }
}
