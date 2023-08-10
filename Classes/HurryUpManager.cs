using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace Mod.Classes
{
	public class HurryUpManager : MonoBehaviour
	{
		private bool active;
		private bool lap2active;

		private float remainingTime;
		private EnvironmentController ec;

		public void Initialize(EnvironmentController _ec)
		{
			ec = _ec;
			if (active) { Stop(); }
		}

		public void Begin()
		{
			if (active) { return; }
			active = true;

			ec.audMan.PlaySingle(Mod.aud_hurryup);

			Mod.hurryupGui.popPizzaTime();

			IEnumerator co = Timer(70f);
			StartCoroutine(co);
		}

		public void Stop()
		{
			if (!active) { return; }
			active = false;
			Singleton<MusicManager>.Instance.StopFile();
			StopAllCoroutines();
		}

		public void Lap2()
		{
			if ((!active) && (!lap2active)) { return; }
			active = true;
			lap2active = true;
			Stop();

			Singleton<BaseGameManager>.Instance.AngerBaldi(-1f); // slower baldi
			Singleton<BaseGameManager>.Instance.AddNotebookTotal(ec.notebookTotal);

			float elevators = ec.elevators.Count;
			float notebooks = ec.notebookTotal;

			IEnumerator co = Timer(elevators * 20f + notebooks + 20f);
			StartCoroutine(co);
		}

		public void Expired()
		{
			Stop();
			Singleton<BaseGameManager>.Instance.AngerBaldi(30f);

			Baldi baldi = this.ec.GetBaldi();
			ec.audMan.PlaySingle(ec.audBell);
		}

		private IEnumerator Timer(float time)
		{
			yield return null;
			remainingTime = time;
			while (remainingTime > 0f)
			{
				remainingTime -= Time.deltaTime * ec.EnvironmentTimeScale;
				Singleton<CoreGameManager>.Instance.GetHud(0).UpdateText(0, string.Concat(new string[]
				{
										"ESCAPE! ", Mathf.RoundToInt(remainingTime).ToString(), "s"
				}));
				yield return null;
			}
			Expired();
			yield break;
		}

		private IEnumerator Timer2(float time)
		{
			yield return null;
			remainingTime = time;
			while (remainingTime > 0f)
			{
				remainingTime -= Time.deltaTime * ec.EnvironmentTimeScale;

				Singleton<CoreGameManager>.Instance.GetHud(0).UpdateText(0, string.Concat(new string[]
				{
										 Singleton<BaseGameManager>.Instance.foundNotebooks.ToString(),
										"/",
										Mathf.Max(ec.notebookTotal, Singleton<BaseGameManager>.Instance.foundNotebooks).ToString(),
										" (",
										Mathf.RoundToInt(remainingTime).ToString(),
										"s)"
				}));
				yield return null;
			}
			Expired();
			yield break;
		}


	}

	[HarmonyPatch(typeof(MainGameManager), nameof(MainGameManager.AllNotebooks))]
	class HurryUpStartPatch
	{
		static bool Prefix(MainGameManager __instance)
		{
			Mod.hurryupManager.Initialize(__instance.ec);
			Mod.hurryupManager.Begin();
			return true;
		}
	}

	[HarmonyPatch(typeof(BaseGameManager), nameof(BaseGameManager.PrepareToLoad))]
	class HurryUpCleanupPatch
	{

		static bool Prefix()
		{
			Mod.hurryupManager.Stop();
			return true;
		}
	}
}
