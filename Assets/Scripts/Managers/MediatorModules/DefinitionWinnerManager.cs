using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Assets.Scripts.Constants;

namespace Assets.Scripts.Managers
{
    public class DefinitionWinnerManager
    {
        public byte LastWinnerID = NOT_A_PLAYER_ID;

        private Dictionary<byte, byte> FortressRate_Owner_Pairs => FortressManager.instance.FortressOwnerPairs;

        public void DefineWinner()
        {
            var winnerID = GetWinnerID();
            if (winnerID != NOT_A_PLAYER_ID)
                UIManager.instance.ShowWinnerPanel(winnerID);
            else
                UIManager.instance.ShowDrawPanel();
            PlayerManager.instance.IncreaseWinNumber(winnerID);

            LastWinnerID = winnerID;
        }

        private byte GetWinnerID()
        {
            if (FortressRate_Owner_Pairs.Count == 0) return NOT_A_PLAYER_ID;

            var playerFortressesCount = new Dictionary<byte, byte>(4);
            for (byte i = 0; i <= MAX_PLAYER_ID; i++)
                playerFortressesCount[i] = 0;

            foreach (var pair in FortressRate_Owner_Pairs)
            {
                if (pair.Value != NOT_A_PLAYER_ID)
                    playerFortressesCount[pair.Value]++;

            }

            var max = playerFortressesCount.Values.Max();
            List<byte> plrsWithMostFortresses = playerFortressesCount
                                                .Where(KeyValue => KeyValue.Value == max)
                                                .Select(kv => kv.Key)
                                                .ToList();

            if (plrsWithMostFortresses.Count == 1)
                return plrsWithMostFortresses[0];

            var winnerID = GetWinnerBetweenCandidates(plrsWithMostFortresses);

            return winnerID;
        }

        private byte GetWinnerBetweenCandidates(List<byte> playersIDs)
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