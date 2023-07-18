using UnityEngine;
using MTM101BaldAPI.AssetManager;
using System.IO;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Mod.Classes
{
    class PizzaTimeImage : MonoBehaviour
    {
        
        private readonly Sprite pizzatime1 = AssetManager.SpriteFromTexture2D(AssetManager.TextureFromFile(Path.Combine(Mod.modPath, "Textures", "balditime1.png"))); 
        private readonly Sprite pizzatime2 = AssetManager.SpriteFromTexture2D(AssetManager.TextureFromFile(Path.Combine(Mod.modPath, "Textures", "balditime2.png")));
        
        private GameObject obj;
        private Image img;
        private Transform trans;
        private RectTransform rectrans;

        private float spriteTime = 0;

        public void Show()
        {
            obj = new GameObject();
            img = obj.AddComponent<Image>();
            trans = img.GetComponent<Transform>();
            rectrans = img.GetComponent<RectTransform>();

            img.sprite = pizzatime1;
            trans.localScale = Vector3.one*45;
            trans.position = Vector3.zero + new Vector3(0,-7,1);
            rectrans.sizeDelta = new Vector2(0.12f,0.12f);
            obj.SetActive(true);

            trans.SetParent(Singleton<CoreGameManager>.Instance.GetHud(0).Canvas().GetComponent<Transform>());
            //pizzaTime.transform.SetParent(Singleton<CoreGameManager>.Instance.GetHud(0).Canvas().transform);
            StartCoroutine(Animate());
        }

        public void SwitchSprite() 
        {
            spriteTime += Time.deltaTime * 10f;
            if (Mathf.RoundToInt(spriteTime) % 2 == 0) 
            {
                img.sprite = pizzatime2;
            } 
            else
            {
                img.sprite = pizzatime1;
            }
        }

        public IEnumerator Animate()
        {
            yield return null;
            float i = 0f;
            while (i < 14f)
            {
                i += Time.deltaTime * 5f;
                trans.position = new Vector3(0, i-7, 1);
                SwitchSprite();
                yield return null;
            }
            obj.SetActive(false);
            yield break;
        }
    }

    public class HurryUpGui : MonoBehaviour
    {
        PizzaTimeImage pizzaTime;

        public void Initialize()
        {
            pizzaTime = Mod.getGameObject().AddComponent<PizzaTimeImage>();
        }

        public void popPizzaTime()
        {
            Console.WriteLine("Popped your mom");
            pizzaTime.Show();
        }
    }
}
