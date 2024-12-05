using System;
using System.Collections.Generic;
using System.Linq;
using Kimicu.YandexGames.Extension;
using Kimicu.YandexGames.Utils;
using KimicuYandexGames.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Kimicu.YandexGames
{
    public static class Flags
    {
	    public static void GetFlags(Action<Dictionary<string, string>> onSuccessCallback)
        {
            if (!YandexGamesSdk.IsInitialized) throw new Exception("YandexGamesSdk not initialized!");
#if !UNITY_EDITOR && UNITY_WEBGL // Yandex //
            Agava.YandexGames.Flags.Get(flagsJson =>
            {
                Debug.Log($"flags - {flagsJson}");
                onSuccessCallback?.Invoke(JsonConvert.DeserializeObject<Dictionary<string, string>>(flagsJson));
            });
#endif
#if UNITY_EDITOR && UNITY_WEBGL // Editor //
			onSuccessCallback?.Invoke(YandexEditorData.Instance.Flags
				.ToDictionary(flag => flag.Key, flag => flag.Value));
#endif
        }

        public static void GetFlag(string key, string defaultValue = default, Action<string> onSuccessCallback = null)
        {
            if (!YandexGamesSdk.IsInitialized) throw new Exception("YandexGamesSdk not initialized!");
#if !UNITY_EDITOR && UNITY_WEBGL // Yandex //
            Agava.YandexGames.Flags.Get(response =>
            {
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                onSuccessCallback?.Invoke(dictionary.GetValueOrDefault(key, defaultValue));
            });
#endif
#if UNITY_EDITOR && UNITY_WEBGL // Editor //
            GetFlags(response => onSuccessCallback?.Invoke(response.GetValueOrDefault(key, defaultValue)));
#endif
        }
    }
}