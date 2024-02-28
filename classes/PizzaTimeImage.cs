using BaldiHurryUp.utils;
using MTM101BaldAPI.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BaldiHurryUp.classes
{
	public class PizzaTimeImage : MonoBehaviour
	{
		private static Sprite pizzatime1 = BasePlugin.TextureFromMod("textures", "balditime1.png").ToSprite();
		private static Sprite pizzatime2 = BasePlugin.TextureFromMod("textures", "balditime2.png").ToSprite();

		private Image image;
		private float spriteTime = 0;

		public void Awake()
		{
		}
		public void Show()
		{
			Transform parent = Singleton<CoreGameManager>.Instance.GetHud(0).Canvas().GetComponent<Transform>();
			if (!image) image = UIHelpers.CreateImage(pizzatime1, parent, Vector3.zero + new Vector3(0, -7, 1));
			StartCoroutine(Animate());
		}
		public IEnumerator Animate()
		{
			yield return null;
			float i = 0f;
			image.gameObject.SetActive(true);
			while (i < 14f)
			{
				i += Time.deltaTime * 5f;
				image.transform.position = new Vector3(0, i - 7, 1);
				SwitchSprite();
				yield return null;
			}
			image.gameObject.SetActive(false);
			yield break;
		}
		public void SwitchSprite()
		{
			spriteTime += Time.deltaTime * 10f;
			image.sprite = Mathf.RoundToInt(spriteTime) % 2 == 0 ? pizzatime2 : pizzatime1;
		}
	}
}