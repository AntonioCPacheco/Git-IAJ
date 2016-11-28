using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using Assets.Scripts.IAJ.Unity.Utils;
using System;
using System.Collections.Generic;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.GOB.Action;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTSRAVE : MCTS
    {
        protected const float b = 1;
        protected List<Pair<int,Action>> ActionHistory { get; set; }
        public MCTSRAVE(CurrentStateWorldModel worldModel) :base(worldModel)
        {
        }

        protected MCTSNode BestUCTChild(MCTSNode node)
        {
            float MCTSValue;
            float RAVEValue;
            float UCTValue;
            float bestUCT = float.MinValue;
            MCTSNode bestNode = null;

            float beta;
            //step 1, calculate beta and 1-beta. beta does not change from child to child. So calculate this only once
            //TODO: implement
            beta = node.NRAVE / (node.N + node.NRAVE + 4 * node.N * node.NRAVE * b * b);

            //step 2, calculate the MCTS value, the RAVE value, and the UCT for each child and determine the best one
            foreach(MCTSNode child in node.ChildNodes)
            {
                RAVEValue = (((1 - beta) * child.Q / child.N) + (beta * child.QRAVE / child.NRAVE)) + C * (float) Math.Log(node.N / child.N);
                UCTValue = child.Q + C * (float) Math.Sqrt(Math.Log(node.N) / child.N);
                MCTSValue = child.Q + (float) Math.Sqrt(Math.Log(node.N) / child.N);
            }
        }


        protected Reward Playout(WorldModel initialPlayoutState)
        {
            //Action[] actionHistory;
            Action a;
            Reward reward = new Reward();
            System.Random random = new System.Random();
            while(!initialPlayoutState.IsTerminal())
            {
                a = initialPlayoutState.GetExecutableActions()[random.Next()];
                this.ActionHistory.Add(new Utils.Pair<int, GOB.Action>(initialPlayoutState.GetNextPlayer(), a));
                a.ApplyActionEffects(initialPlayoutState);
            }
            reward.Value = initialPlayoutState.GetScore();
            return reward;
        }

        protected void Backpropagate(MCTSNode node, Reward reward)
        {
            int player;
            while(node != null)
            {
                node.N++;
                node.Q += reward.Value;
                this.ActionHistory.Add(new Pair<int, GOB.Action>(node.Parent.PlayerID, node.Action));
                node = node.Parent;

                if(node != null)
                {
                    player = node.PlayerID;

                    foreach(MCTSNode child in node.ChildNodes)
                    {
                        if(this.ActionHistory.Contains(new Pair<int, GOB.Action>(player, child.Action)))
                        {
                            child.NRAVE++;
                            child.QRAVE += reward.Value;
                        }
                    }
                }
            }
        }
    }
}
