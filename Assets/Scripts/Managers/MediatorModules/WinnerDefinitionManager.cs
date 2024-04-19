using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Assets.Scripts.Constants;

namespace Assets.Scripts.Managers
{
    public class WinnerDefinitionManager
    {
        public byte LastWinnerID = NOT_A_PLAYER_ID;

        private Dictionary<byte, byte> FortressRate_Owner_Pairs { get; set; }

        public void DefineWinner(Dictionary<byte, byte> fortressRate_Owner_Pairs)
        {
            FortressRate_Owner_Pairs = fortressRate_Owner_Pairs;
            var winnerID = GetWinnerID();
            LastWinnerID = winnerID;
        }

        private byte GetWinnerID()
        {
            if (FortressRate_Owner_Pairs.Count == 0) return NOT_A_PLAYER_ID;

            var playerFortressesCounts = new Dictionary<byte, byte>(4);
            for (byte i = 0; i <= MAX_PLAYER_ID; i++)
                playerFortressesCounts[i] = 0;
            CalculateFortressCounts(playerFortressesCounts);

            var max = playerFortressesCounts.Values.Max();
            List<byte> plrsWithMostFortresses = playerFortressesCounts
                                                .Where(fortsCount => fortsCount.Value == max)
                                                .Select(fortsCount => fortsCount.Key)
                                                .ToList();

            if (plrsWithMostFortresses.Count == 1)
                return plrsWithMostFortresses[0];

            var winnerID = GetWinnerIDBetweenCandidates(plrsWithMostFortresses);
            return winnerID;

            void CalculateFortressCounts(Dictionary<byte, byte> playerFortressesCounts)
            {
                foreach (var pair in FortressRate_Owner_Pairs)
                {
                    if (pair.Value != NOT_A_PLAYER_ID)
                        playerFortressesCounts[pair.Value]++;
                }
            }
        }

        private byte GetWinnerIDBetweenCandidates(List<byte> playersIDs)
        {
            byte minFortressRate = byte.MaxValue;
            byte winnerID = 0;

            foreach (var FortOwnerPair in FortressRate_Owner_Pairs)
            {
                var playerID = FortOwnerPair.Value;

                if (playersIDs.Contains(playerID)
                    && minFortressRate > FortOwnerPair.Key)
                {
                    minFortressRate = FortOwnerPair.Key;
                    winnerID = playerID;
                }
            }

            return winnerID;
        }
    }
}