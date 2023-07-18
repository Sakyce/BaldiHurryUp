using System;
using System.IO;
using BepInEx;
using HarmonyLib;
using MTM101BaldAPI.AssetManager;
using MTM101BaldAPI;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.Audio;
using Mod.Classes;
using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Mod
{


    [BepInPlugin("sakyce.baldiplus.hurryup", "Hurry Up!", "1.0.0.0")]
    public class Mod : BaseUnityPlugin
    {

        public static LoopingSoundObject aud_hurryup;
        public static Dictionary<string, LoopingSoundObject> loadedAudios = new Dictionary<string, LoopingSoundObject>();

        public static string modPath;

        public static HurryUpManager hurryupManager;
        public static HurryUpGui hurryupGui;

        private static GameObject _gameObject;

        public static void playMusic(LoopingSoundObject aud)
        {
            aud.mixer = Singleton<PlayerFileManager>.Instance.mixer[(int)SoundType.Music];
            Singleton<MusicManager>.Instance.QueueFile(aud, true);
            Singleton<MusicManager>.Instance.StartFileQueue();
        }
        public static LoopingSoundObject loadLoopedMusic(string filename)
        {
            if (loadedAudios.ContainsKey(filename))
            {
                return loadedAudios[filename];
            }

            string path = Path.Combine("File:///", modPath, "Musics", filename);
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV);
            request.SendWebRequest();
            while (!request.isDone) {}
            AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                
            //AudioClip clip = AssetManager.AudioClipFromFile(path);
            LoopingSoundObject obj = (LoopingSoundObject)ScriptableObject.CreateInstance("LoopingSoundObject");
            obj.clips = new AudioClip[] { clip };
            loadedAudios.Add(filename, obj);
            return obj;
        }
        private void loadAssets()
        {
            // ONLY SUPPORTS WAV
            modPath = AssetManager.GetModPath(this);
            aud_hurryup = loadLoopedMusic("escape.wav");
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
