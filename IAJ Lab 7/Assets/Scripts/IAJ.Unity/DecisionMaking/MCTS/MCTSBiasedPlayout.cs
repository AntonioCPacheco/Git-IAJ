using Assets.Scripts.GameManager;
using System;
using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTSBiasedPlayout : MCTS
    {
        public MCTSBiasedPlayout(CurrentStateWorldModel currentStateWorldModel) : base(currentStateWorldModel)
        {
        }

        protected Reward Playout(WorldModel initialPlayoutState)
        {
            //TODO: implement
            //throw new NotImplementedException();
            while (!initialPlayoutState.IsTerminal())
            {
                initialPlayoutState.GetNextAction().ApplyActionEffects(initialPlayoutState);
            }
            Reward reward = new Reward();
            reward.Value = initialPlayoutState.GetScore();
            return reward;
        }

        protected MCTSNode Expand(WorldModel parentState, GOB.Action action)
        {
            //TODO: implement
            throw new NotImplementedException();
        }
    }
}
