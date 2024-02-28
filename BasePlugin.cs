using BaldiHurryUp.classes;
using BaldiHurryUp.utils;
using BepInEx;
using HarmonyLib;
using MTM101BaldAPI.AssetTools;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace BaldiHurryUp
{
	// --> major.baldiversion.baldiversion2
	[BepInPlugin("sakyce.baldiplus.hurryup", "Hurry Up!", "1.4.0")]
	public unsafe class BasePlugin : BaseUnityPlugin
	{
		public static SoundObject lap1music = AudioClipFromMod("audio", "escape.wav").ToSoundObject(Color.white);
		private static new GameObject gameObject;
		public static HurryUpManager hurryUpManager;
		public static PizzaTimeImage pizzaTimeImage;
		public void Awake()
		{
			Harmony harmony = new Harmony("sakyce.baldiplus.hurryup");
			harmony.PatchAll();

			BasePlugin.gameObject = base.gameObject;
			BasePlugin.hurryUpManager = base.gameObject.AddComponent<HurryUpManager>();
			BasePlugin.pizzaTimeImage = base.gameObject.AddComponent<PizzaTimeImage>();
		}

		// Theses doesn't works in a static context, i think ?
		public static AudioClip AudioClipFromMod(params string[] paths)
		{

			List<string> list = Enumerable.ToList(paths);
			list.Insert(0, Path.Combine(Application.streamingAssetsPath, "Modded", "sakyce.baldiplus.hurryup"));
			return AssetLoader.AudioClipFromFile(Path.Combine(list.ToArray()));
		}
		// Same
		public static Texture2D TextureFromMod(params string[] paths)
		{
			List<string> list = Enumerable.ToList(paths);
			list.Insert(0, Path.Combine(Application.streamingAssetsPath, "Modded", "sakyce.baldiplus.hurryup"));
			return AssetLoader.TextureFromFile(Path.Combine(list.ToArray()));
		}
		public static GameObject getGameObject()
		{
			return BasePlugin.gameObject;
		}

		[HarmonyPatch(typeof(MainGameManager), nameof(MainGameManager.AllNotebooks))]
		class HurryUpStartPatch
		{
			static bool Prefix(MainGameManager __instance)
			{
				BasePlugin.hurryUpManager.Initialize(__instance.ec);
				BasePlugin.hurryUpManager.NextLap();
				return true;
			}
		}
		[HarmonyPatch(typeof(BaseGameManager), nameof(BaseGameManager.PrepareToLoad))]
		class HurryUpCleanupPatch
		{

			static bool Prefix()
			{
				BasePlugin.hurryUpManager.Stop();
				return true;
			}
		}
	}
}

 

 
 
 
 