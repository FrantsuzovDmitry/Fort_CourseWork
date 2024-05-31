using Assets.Scripts.AI.AINeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using static Assets.Scripts.Managers.GameState;

namespace Assets.Scripts.AI
{
    public struct GameInfo
    {
        public const int NumberOfNonCollectionParameters = 4;

        public readonly GameStage gameStage;
        public readonly byte numberOfPlayers;
        public readonly byte numberOfFortresses;
        public readonly byte cardsInDeck;
        public readonly byte numberOfSandglasses;
        public readonly List<Fortress> fortressesInGame;
        public readonly Dictionary<byte, byte> players_fortressNumber_pairs;
        public readonly Dictionary<byte, byte> players_cardNumber_pairs;
    }

    public class AIPlayer : Player
    {
        public int Score;

        private NeuralNetwork _neuralNetwork;


        public AIPlayer(byte ID) : base(ID)
        {
            Score = 0;
            _neuralNetwork = new NeuralNetwork(
                new Topology(GameInfo.NumberOfNonCollectionParameters, 2, 9));
        }

        public void IncreaseScore(int pointsCount) => Score += pointsCount;

        public void DecreaseScore(int pointsCount) => Score -= pointsCount;

        public AIAction MakeDecision(GameInfo gameInfo)
        {
            List<int> inputs = MakeInputs(gameInfo);

            if (gameInfo.gameStage == GameStage.CardExchanging)
                return SelectCardToGive();
            else
                return MakeMove(inputs);
        }

        private List<int> MakeInputs(GameInfo gameInfo)
        {
            List<int> inputs = new List<int>
            {
                (int)gameInfo.gameStage,
                gameInfo.numberOfPlayers,
                gameInfo.numberOfFortresses,
                gameInfo.cardsInDeck,
                gameInfo.numberOfSandglasses,
            };

            List<Fortress> fortressInGame = gameInfo.fortressesInGame;
            const int bigConst = 32000;

            // Adding FORTRESSES RATES or bigConst if fortress at this time out of game
            AddInputs(inputs, fortressInGame, f => f.Rate, bigConst);

            // Adding FORTRESSES DEFENDERS or bigConst if fortress at this time out of game
            AddInputs(inputs, fortressInGame, f => f.DefendersGroup.ToList().Count, bigConst);

            // Adding FORTRESS NUMBERS each of players in game
            AddInputsDict(inputs, gameInfo.players_fortressNumber_pairs, -1);

            // Adding CARD NUMBERS IN HAND each of players in game
            AddInputsDict(inputs, gameInfo.players_cardNumber_pairs, -1);

            return inputs;

            void AddInputs(List<int> inputs, in List<Fortress> fortressesInGame, Func<Fortress, int> GetValueToAddIfExist, int valueToAddIfDoesntExist)
            {
                for (int rate = 1; rate <= 8; rate++)
                {
                    Fortress fortress = fortressesInGame.FirstOrDefault(f => f.Rate == rate);
                    if (fortress == null)
                    {
                        inputs.Add(valueToAddIfDoesntExist);
                    }
                    else
                    {
                        inputs.Add(GetValueToAddIfExist(fortress));
                    }
                }
            }

            void AddInputsDict(List<int> inputs, in Dictionary<byte, byte> pairs, int valueToAddIfDoesntExist)
            {
                for (byte plrID = 0; plrID < 4; plrID++)
                {
                    bool found = gameInfo.players_cardNumber_pairs.TryGetValue(plrID, out var result);
                    if (!found)
                    {
                        inputs.Add(-1);
                    }
                    else
                    {
                        inputs.Add(result);
                    }
                }
            }

        }

        private AIAction MakeMove(List<int> inputs)
        {

            var result = _neuralNetwork.FeedForward(inputs);
            // AttackTHeFortress
            // TakeCard
            return null;
        }

        private AIAction SelectCardToGive()
        {
            // GiveCardToAnotherPlayer
            throw new NotImplementedException();
        }

        private float[] GetInput(GameInfo gameInfo)
        {
            var input = new float[GameInfo.NumberOfNonCollectionParameters];
            return input;
        }
    }
}
