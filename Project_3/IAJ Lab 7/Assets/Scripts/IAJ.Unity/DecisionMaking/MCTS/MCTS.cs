using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using Assets.Scripts.DecisionMakingActions;
using System;
using Assets.Scripts.GameManager;
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


        protected int CurrentIterations { get; set; }
        protected int CurrentIterationsInFrame { get; set; }
        protected int CurrentDepth { get; set; }

        protected CurrentStateWorldModel CurrentStateWorldModel { get; set; }
        protected MCTSNode InitialNode { get; set; }
        protected System.Random RandomGenerator { get; set; }



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

            while (this.CurrentIterations <= this.MaxIterations && this.CurrentIterationsInFrame <= this.MaxIterationsProcessedPerFrame)
            {
                selectedNode = Selection(this.InitialNode);
                reward = Playout(selectedNode.State);
                Backpropagate(selectedNode, reward);

                this.CurrentIterationsInFrame++;
            }

            /*MCTSNode node = BestUCTChild(selectedNode);
            while(node != null)
            {
                this.BestActionSequence.Add(node.Action);
                node = BestUCTChild(node);
            }*/
            this.CurrentIterations += this.CurrentIterationsInFrame;
            this.InProgress = false;
            this.TotalProcessingTime += Time.realtimeSinceStartup - startTime;
            return BestChild(this.InitialNode).Action;
        }

        private MCTSNode Selection(MCTSNode initialNode)
        {
            GOB.Action nextAction;
            MCTSNode currentNode = initialNode;
            MCTSNode bestChild = currentNode;

            while(!currentNode.State.IsTerminal())
            {
                nextAction = currentNode.State.GetNextAction();
                if (nextAction != null)
                {
                    return Expand(currentNode, nextAction);
                }
                else
                {
                    currentNode = BestUCTChild(currentNode);
                }
                this.MaxSelectionDepthReached++;
            }
            return bestChild;
        }

        protected virtual Reward Playout(WorldModel initialPlayoutState)
        {
            FutureStateWorldModel state = (FutureStateWorldModel)initialPlayoutState.GenerateChildWorldModel();
            while (!state.IsTerminal())
            {
                GOB.Action[] actions = state.GetExecutableActions();
                actions[RandomGenerator.Next() % actions.Length].ApplyActionEffects(state);
                state.CalculateNextPlayer();
                this.MaxPlayoutDepthReached++;
            }
            return new Reward();
        }

        protected virtual void Backpropagate(MCTSNode node, Reward reward)
        {
            while(node != null)
            {
                node.N = node.N + 1;
                node = node.Parent;
            }
        }

        private MCTSNode Expand(MCTSNode parent, GOB.Action action)
        {
            WorldModel worldmodel = CurrentStateWorldModel.GenerateChildWorldModel();
            action.ApplyActionEffects(worldmodel);
            worldmodel.CalculateNextPlayer();
            MCTSNode n = new MCTSNode(worldmodel)
            {
                Action = action,
                Parent = parent,
                N = 0,
                Q = 0
            };
            parent.ChildNodes.Add(n);
            return n;
        }

        //gets the best child of a node, using the UCT formula
        protected virtual MCTSNode BestUCTChild(MCTSNode node)
        {
            MCTSNode bestChild = new MCTSNode(node.State);
            float bestChildValue = float.MinValue;
            for(int i=0; i<node.ChildNodes.Count; i++)
            {
                if((node.ChildNodes[i].Q + 2 * (Mathf.Log(node.N)/node.ChildNodes[i].N)) > bestChildValue)
                {
                    bestChildValue = node.ChildNodes[i].Q + C * Mathf.Sqrt(Mathf.Log(node.N) / node.ChildNodes[i].N);
                    bestChild = node.ChildNodes[i];
                }
                    
            }
            return bestChild;
        }

        //this method is very similar to the bestUCTChild, but it is used to return the final action of the MCTS search, and so we do not care about
        //the exploration factor
        private MCTSNode BestChild(MCTSNode node)
        {
            MCTSNode bestChild = new MCTSNode(node.State);
            float bestChildValue = float.MinValue;
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (C*C * (Mathf.Log(node.N) / (node.ChildNodes[i].N == 0 ? 1 : node.ChildNodes[i].N)) > bestChildValue)
                {
                    bestChildValue = node.ChildNodes[i].Q;
                    bestChild = node.ChildNodes[i];
                }

            }
            return bestChild;
        }
    }
}

