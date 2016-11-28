using Assets.Scripts.GameManager;
using System;
using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTSBiasedPlayout : MCTS
    {
        float[] weights;


        public MCTSBiasedPlayout(CurrentStateWorldModel currentStateWorldModel) : base(currentStateWorldModel)
        {
            weights = new float[] { 1, 3, 2, 4, 1};
        }

        protected override Reward Playout(WorldModel initialPlayoutState)
        {
            WorldModel state = initialPlayoutState.GenerateChildWorldModel();
            while (!state.IsTerminal())
            {
                getRandomAction(state).ApplyActionEffects(state);
            }
            Reward r = new Reward();
            r.Value = r.GetRewardForNode(new MCTSNode(state));
            return r;
        }

        protected MCTSNode Expand(WorldModel parentState, GOB.Action action)
        {
            //TODO: implement
            throw new NotImplementedException();
        }

        private GOB.Action getRandomAction(WorldModel state)
        {
            //TODO: fix
            throw new NotImplementedException();
            GOB.Action[] actions = state.GetExecutableActions();
            
            if (actions.Length > 0)
            {
                return actions[RandomGenerator.Next() % actions.Length];
            }
            return actions[0];
        } 
    }
}
