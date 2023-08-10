using BepInEx;
using HarmonyLib;
using Mod.Classes;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Mod
{


	[BepInPlugin("sakyce.baldiplus.hurryup", "Hurry Up!", "1.0.0.0")]
	public class Mod : BaseUnityPlugin
	{
		public static Dictionary<string, LoopingSoundObject> loadedAudios = new Dictionary<string, LoopingSoundObject>();

		public static HurryUpManager hurryupManager;
		public static HurryUpGui hurryupGui;

		private static GameObject _gameObject;
		public static SoundObject aud_hurryup;

		public static void playMusic(LoopingSoundObject aud)
		{
			aud.mixer = Singleton<PlayerFileManager>.Instance.mixer[(int)SoundType.Music];
			Singleton<MusicManager>.Instance.QueueFile(aud, true);
			Singleton<MusicManager>.Instance.StartFileQueue();
		}
		private void loadAssets()
		{
			aud_hurryup = AssetManager.LoadSoundObject("escape.wav", AudioType.WAV, soundType: SoundType.Music);
		}

		public void Awake()
		{
			Mod._gameObject = gameObject;
			loadAssets();

			Mod.hurryupManager = gameObject.AddComponent<HurryUpManager>();
			Mod.hurryupGui = gameObject.AddComponent<HurryUpGui>();

			Harmony harmony = new Harmony("sakyce.baldiplus.hurryup");
			harmony.PatchAll();

			Mod.hurryupGui.Initialize();
		}

		public static GameObject getGameObject()
		{
			return Mod._gameObject;
		}
	}
}
