using MTM101BaldAPI;
using System.Collections;
using UnityEngine;

namespace BaldiHurryUp.classes
{

	public class HurryUpManager : MonoBehaviour
	{
		private int currentLap = 0;
		private float remainingTime = 120f;

		private EnvironmentController ec;

		public void Initialize(EnvironmentController _ec)
		{
			ec = _ec;
		}
		private IEnumerator Timer(float time)
		{
			yield return null;
			remainingTime = time;
			while (remainingTime > 0f)
			{
				remainingTime -= Time.deltaTime * ec.EnvironmentTimeScale;
				Singleton<CoreGameManager>.Instance.GetHud(0).UpdateText(0,
						string.Concat(new string[] { "ESCAPE! ", Mathf.RoundToInt(remainingTime).ToString(), "s" }));
				yield return null;
			}
			Expired();
			yield break;
		}
		public void Expired()
		{
			Stop();
			ec.audMan.PlaySingle(ec.audBell);
			MakeBaldiFaster();
		}
		public void Stop()
		{
			if (currentLap <= 0) return;
			currentLap = 0;
			Singleton<MusicManager>.Instance.StopFile();
			StopAllCoroutines();
		}
		public void SpawnNull()
		{

		}
		// This seems to spam the console with exceptions
		public void SpawnPrincipals()
		{
			Principal principal = (Principal)ObjectFinders.GetFirstInstance(Character.Principal);
			if (this.ec.offices.Count > 0)
			{
				for (int i = 0; i < 3; i++)
				{
					int index = UnityEngine.Random.Range(0, this.ec.offices.Count);
					ec.SpawnNPC(principal, IntVector2.GetGridPosition(ec.RealRoomMid(this.ec.offices[index])));
				}

				foreach (var player in ec.players)
					player.RuleBreak("AfterHours", 1000000f);
			}
		}
		public void MakeBaldiFaster()
		{
			Singleton<BaseGameManager>.Instance.AngerBaldi(10f);
		}
		public void NextLap()
		{
			if (currentLap >= 2) return;
			currentLap++;
			Singleton<BaseGameManager>.Instance.AngerBaldi(-1f);
			if (currentLap == 1)
			{
				ec.audMan.PlaySingle(BasePlugin.lap1music);
				BasePlugin.pizzaTimeImage.Show();

				IEnumerator co = Timer(70f);
				StartCoroutine(co);
			}
		}
	}
}
