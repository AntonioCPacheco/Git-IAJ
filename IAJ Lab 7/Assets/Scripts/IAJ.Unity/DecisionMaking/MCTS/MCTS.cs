using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTS
    {
        public const float C = 1.4f;
        public bool InProgress { get; private set; }
        public int MaxIterations { get; set; }
        public int MaxIterationsProcessedPerFrame { get; set; }
        public int MaxPlayoutDepthReached { get; private set; }
        public int MaxSelectionDepthReached { get; private set; }
        public float TotalProcessingTime { get; private set; }
        public MCTSNode BestFirstChild { get; set; }
        public List<GOB.Action> BestActionSequence { get; private set; }


        private int CurrentIterations { get; set; }
        private int CurrentIterationsInFrame { get; set; }
        private int CurrentDepth { get; set; }

        private CurrentStateWorldModel CurrentStateWorldModel { get; set; }
        private MCTSNode InitialNode { get; set; }
        private System.Random RandomGenerator { get; set; }
        
        

        public MCTS(CurrentStateWorldModel currentStateWorldModel)
        {
            this.InProgress = false;
            this.CurrentStateWorldModel = currentStateWorldModel;
            this.MaxIterations = 100;
            this.MaxIterationsProcessedPerFrame = 10;
            this.RandomGenerator = new System.Random();
        }


        public void InitializeMCTSearch()
        {
            this.MaxPlayoutDepthReached = 0;
            this.MaxSelectionDepthReached = 0;
            this.CurrentIterations = 0;
            this.CurrentIterationsInFrame = 0;
            this.TotalProcessingTime = 0.0f;
            this.CurrentStateWorldModel.Initialize();
            this.InitialNode = new MCTSNode(this.CurrentStateWorldModel)
            {
                Action = null,
                Parent = null,
                PlayerID = 0
            };
            this.InProgress = true;
            this.BestFirstChild = null;
            this.BestActionSequence = new List<GOB.Action>();
        }

        public GOB.Action Run()
        {
            MCTSNode selectedNode = null;
            Reward reward;

            var startTime = Time.realtimeSinceStartup;
            this.CurrentIterationsInFrame = 0;
            if (this.CurrentIterations < this.MaxIterations)
            {
                for (; this.CurrentIterationsInFrame < this.MaxIterationsProcessedPerFrame; this.CurrentIterationsInFrame++)
                {
                    selectedNode = this.Selection(this.InitialNode);
                    reward = this.Playout(this.InitialNode.State);
                    this.Backpropagate(selectedNode, reward);
                }
            }
            var stopTime = Time.realtimeSinceStartup;
            this.CurrentIterations += this.CurrentIterationsInFrame;
            this.TotalProcessingTime += startTime - stopTime;
            return this.BestChild(selectedNode).Action;
        }

        private MCTSNode Selection(MCTSNode initialNode)
        {
            GOB.Action nextAction;
            MCTSNode currentNode = initialNode;
            MCTSNode bestChild = null;

            while (!currentNode.State.IsTerminal())
            {
                nextAction = currentNode.State.GetNextAction();
                if (nextAction != null)
                {
                    bestChild = this.Expand(currentNode, nextAction);
                    break;
                }
                else
                {
                    bestChild = this.BestChild(currentNode);
                }
            }
            return bestChild;
        }

        private Reward Playout(WorldModel initialPlayoutState)
        {
            while (!initialPlayoutState.IsTerminal())
            {
                initialPlayoutState.GetNextAction().ApplyActionEffects(initialPlayoutState);
            }
            Reward reward = new Reward();
            reward.Value = initialPlayoutState.GetScore();
            return reward;
        }

        private void Backpropagate(MCTSNode node, Reward reward)
        {
            while (node != null)
            {
                node.N++;
                node.Q += reward.Value;
                node = node.Parent;
            }
        }

        private MCTSNode Expand(MCTSNode parent, GOB.Action action)
        {
            MCTSNode childNode = new MCTSNode(parent.State);
            childNode.Parent = parent;
            childNode.Action = action;
            action.ApplyActionEffects(childNode.State);
            parent.ChildNodes.Add(childNode);
            return childNode;
        }

        //gets the best child of a node, using the UCT formula
        private MCTSNode BestUCTChild(MCTSNode node)
        {
            float max = float.MinValue;
            float uct;
            int iMax = 0;
            MCTSNode childNode;
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                childNode = node.ChildNodes[i];
                uct = childNode.Q + C * Mathf.Sqrt(Mathf.Log(childNode.Parent.N) / childNode.N);
                if (uct > max)
                {
                    iMax = i;
                    max = uct;
                }
            }
            return node.ChildNodes[iMax];
        }

        //this method is very similar to the bestUCTChild, but it is used to return the final action of the MCTS search, and so we do not care about
        //the exploration factor
        private MCTSNode BestChild(MCTSNode node)
        {
            float max = float.MinValue;
            float uct;
            int iMax = 0;
            MCTSNode childNode;
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                childNode = node.ChildNodes[i];
                uct = childNode.Q + Mathf.Sqrt(Mathf.Log(childNode.Parent.N) / childNode.N);
                if (uct > max)
                {
                    iMax = i;
                    max = uct;
                }
            }
            return node.ChildNodes[iMax];
        }
    }
}

