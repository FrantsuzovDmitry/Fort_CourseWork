using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public static class UnityExtentions
	{
		public static void SetActive(this Image image)
		{
			image.gameObject.SetActive(true);
		}

		public static void SetInactive(this Image image)
		{
			image.gameObject?.SetActive(false);
		}

		public static void Destroy(this GameObject gameObject)
		{
			Destroy(gameObject);
		}
	}
}
