using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

namespace BaldiHurryUp.utils
{
    internal static class MTMModdingExtensions
    {
        public static SoundObject ToSoundObject(
            this AudioClip clip,
            Color color,
            string subtitle = "",
            SoundType type = SoundType.Effect,
            float sublength = -1f
        )
        {
            return ObjectCreators.CreateSoundObject(clip, subtitle, type, color, sublength);
        }
        public static Sprite ToSprite(
            this Texture2D tex
        )
        {
            return AssetLoader.SpriteFromTexture2D(tex);
        }
        public static Sprite ToSprite(this Texture2D tex, Vector2 center, float pixelsPerUnit = 1f)
        {
            return AssetLoader.SpriteFromTexture2D(tex, center, pixelsPerUnit);
        }
    }
}

// old show method
/*
obj = new GameObject();
img = obj.AddComponent<Image>();
trans = img.GetComponent<Transform>();
rectrans = img.GetComponent<RectTransform>();

img.sprite = pizzatime1;
trans.localScale = Vector3.one * 45;
trans.position = Vector3.zero + new Vector3(0, -7, 1);
rectrans.sizeDelta = new Vector2(0.12f, 0.12f);
obj.SetActive(true);

trans.SetParent(Singleton<CoreGameManager>.Instance.GetHud(0).Canvas().GetComponent<Transform>());
//pizzaTime.transform.SetParent(Singleton<CoreGameManager>.Instance.GetHud(0).Canvas().transform);
StartCoroutine(Animate());
*/