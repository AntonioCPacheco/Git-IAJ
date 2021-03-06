﻿using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using Assets.Scripts.IAJ.Unity.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
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

        protected override MCTSNode BestUCTChild(MCTSNode node)
        {
            float MCTSValue;
            float RAVEValue;
            float UCTValue;
            float bestUCT = float.MinValue;
            MCTSNode bestNode = null;
            //step 1, calculate beta and 1-beta. beta does not change from child to child. So calculate this only once
            float Beta = node.NRAVE / (node.N + node.NRAVE + 4 * node.N * node.NRAVE * b * b);

            //step 2, calculate the MCTS value, the RAVE value, and the UCT for each child and determine the best one
            foreach (MCTSNode child in node.ChildNodes)
            {
                MCTSValue = child.Q;
                RAVEValue = ((1 - Beta) * child.Q / child.N + Beta * child.QRAVE / child.NRAVE) + (C * Mathf.Sqrt(Mathf.Log(node.N) / child.N));
                UCTValue = child.Q + C * Mathf.Sqrt(Mathf.Log(node.N) / child.N);
                if(RAVEValue > bestUCT)
                {
                    bestUCT = RAVEValue;
                    bestNode = child;
                }
            }
            return bestNode;
        }


        protected override Reward Playout(WorldModel initialPlayoutState)
        {
            ActionHistory = new List<Pair<int, GOB.Action>>();
            WorldModel state = initialPlayoutState.GenerateChildWorldModel();
            Action nextAction;
            while (!state.IsTerminal())
            {
                Action[] actions = state.GetExecutableActions();
                if (actions.Length > 0) {
                    nextAction = actions[RandomGenerator.Next() % actions.Length];
                    ActionHistory.Add(new Pair<int, GOB.Action>(state.GetNextPlayer(), nextAction));
                    nextAction.ApplyActionEffects(state);
                    state.CalculateNextPlayer();
                }
            }
            Reward r = new Reward();
            r.Value = state.GetScore();
            return r;
        }

        protected override void Backpropagate(MCTSNode node, Reward reward)
        {
            int player;
            while (node != null)
            {
                node.N++;
                node.Q += reward.Value;
                Action action = node.Action;
                node = node.Parent;
                if (node != null)
                {
                    player = node.PlayerID;
                    this.ActionHistory.Add(new Pair<int, GOB.Action>(player, action));
                    foreach (MCTSNode child in node.ChildNodes)
                    {
                        if (this.ActionHistory.Contains(new Pair<int, GOB.Action>(player, child.Action)))
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
