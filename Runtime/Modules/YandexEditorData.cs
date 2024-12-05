using System;
using System.Collections.Generic;
using Agava.YandexGames;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif
using UnityEngine;
// We cannot remove "private set;" as Rider suggests
// Because this will lead to unity not being able to recognize field as SerializeField
//
// ReSharper disable once UnusedAutoPropertyAccessor.Local

namespace KimicuYandexGames.Utils
{
    internal class YandexEditorData : ScriptableObject
    {
        [field: SerializeField, Header("YandexSDK")] public YandexGamesEnvironment Environment { get; private set; } =
            new()
            {
                app = new YandexGamesEnvironment.App()
                {
                    id = "123456"
                },
                i18n = new YandexGamesEnvironment.Internationalization()
                {
                    lang = "ru",
                    tld = "ru"
                },
                payload = ""
            };

        [Header("Cloud")]
        [SerializeField] public string SaveData;

        [field: SerializeField, Header("Billing")] public CatalogProduct[] CatalogProducts { get; private set; }
        [SerializeField] public List<PurchasedProduct> PurchasedProducts;

        [field: SerializeField, Header("AdBlock")] public bool AdBlock { get; private set; } = false;
        [field: SerializeField, Header("Device")] public bool IsMobile { get; private set; } = false;

        [field: SerializeField, Header("Flags")] public FlagKeyValuePair[] Flags { get; private set; }

        [MenuItem ("Window/Kimicu Yandex Games/Select Settings", false, 0)]
        private static void SelectInstance()
        {
            Selection.activeObject = Instance;
        }

        private static YandexEditorData cachedYandexEditorData;

        public static YandexEditorData Instance
        {
            get
            {
                cachedYandexEditorData ??= Resources.Load<YandexEditorData>("Kimicu/Yandex Editor Data");

                if (cachedYandexEditorData != null) return cachedYandexEditorData;
                
                #if UNITY_EDITOR
                YandexEditorData newYandexEditorData = CreateInstance<YandexEditorData>();

                if(!Directory.Exists(Application.dataPath + "/Resources"))
                    Directory.CreateDirectory(Application.dataPath + "/Resources");
                if(!Directory.Exists(Application.dataPath + "/Resources/Kimicu"))
                    Directory.CreateDirectory(Application.dataPath + "/Resources/Kimicu");

                AssetDatabase.CreateAsset(newYandexEditorData, "Assets/Resources/Kimicu/Yandex Editor Data.asset");
                AssetDatabase.Refresh();
                
                AssetDatabase.SaveAssets();

                cachedYandexEditorData = newYandexEditorData;
                
                Selection.activeObject = cachedYandexEditorData;
                
                return cachedYandexEditorData;
                #else
                throw new NullReferenceException($"Somehow {nameof(YandexEditorData)} was requested outside of Editor");
                #endif
            }
        }

        [Serializable]
        public struct FlagKeyValuePair
        {
            public string Key;
            public string Value;
        }
    }
}
