using UnityEngine.UI;

namespace Assets.Scripts
{
	public static class ImageExtension
	{
		public static void SetActive(this Image image)
		{
			image.gameObject.SetActive(true);
		}

		public static void SetInactive(this Image image)
		{
			image.gameObject.SetActive(false);
		}
	}
}
