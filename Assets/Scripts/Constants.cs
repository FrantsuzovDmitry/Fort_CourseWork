using System.Linq;

namespace Assets.Scripts
{
	public static class Constants
	{
		public const byte MAX_FORT_RATE = 8;
		public const byte MIN_FORT_RATE = 1;
		public const byte MIN_PLAYER_ID = 0;
		public const byte MAX_PLAYER_ID = 4;
		public const byte NOT_A_PLAYER_ID = 100;
		public static byte LAST_PLAYER_ID = PlayerManager.instance.players.Last().ID;
	}
}
