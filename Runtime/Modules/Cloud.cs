﻿using System;
using System.Collections;
using System.Collections.Generic;
using Kimicu.YandexGames.Extension;
using KimicuYandexGames.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Kimicu.YandexGames
{
    public static class Cloud
    {
        private static string _json = "{}";
        private static Dictionary<string, object> _jsonDictionary = new Dictionary<string, object>();

        private const string SAVE_NAME = "Save";

        public static bool Initialized { get; private set; }

        #region Initialization

        /// <summary> Initializes the cloud save module. </summary>
        public static IEnumerator Initialize(Action onSuccessCallback = null)
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
            Agava.YandexGames.PlayerAccount.GetCloudSaveData(OnGetCloudSuccessCallback, OnGetCloudErrorCallback);
            #elif UNITY_EDITOR
            OnGetCloudSuccessCallback(YandexEditorData.Instance.SaveData);
            #endif
            yield return new WaitUntil(() => Initialized);
            onSuccessCallback?.Invoke();
        }

        private static void OnGetCloudSuccessCallback(string json)
        {
            var dictionary = json.JsonToDictionary();
            if (dictionary.TryGetValue(SAVE_NAME, out object hex))
            {
                _json = ((string)hex).HexToString();
                _jsonDictionary = _json.JsonToDictionary();
            }
            else
            {
                _json = "{}";
                _jsonDictionary = new Dictionary<string, object>();
            }

            Initialized = true;
        }

        #if UNITY_WEBGL && !UNITY_EDITOR
        private static void OnGetCloudErrorCallback(string error) => Debug.LogError($"Error get cloud save data:\n{error}");
        #endif

        #endregion

        /// <summary> Get value from the cloud. </summary>
        public static T GetValue<T>(string key, T defaultValue = default)
        {
            if (!Initialized) throw new Exception($"{nameof(Cloud)}. Not Initialized!");
            if (_jsonDictionary.TryGetValue(key, out object value))
            {
                try
                {
                    if (value is T convertedValue) return convertedValue;
                    if (value != null && typeof(T).IsAssignableFrom(value.GetType())) return (T)value;
                    return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(value)); 
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception);
                }
            }

            Debug.Log($"Value not found, return default value.");
            return defaultValue;
        }

        /// <summary> Sets the value to the cloud and local or only local. </summary>
        public static void SetValue(string key, object value, bool saveToCloud = false, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            if (!Initialized) throw new Exception($"{nameof(Cloud)}. Not Initialized!");

            if (_jsonDictionary.TryGetValue(key, out object _)) _jsonDictionary[key] = value;
            else _jsonDictionary.Add(key, value);

            if (saveToCloud) SaveInCloud(onSuccessCallback, onErrorCallback);
            else onSuccessCallback?.Invoke();
        }

        public static bool HasKey(string key)
        {
            if (!Initialized) throw new Exception($"{nameof(Cloud)}. Not Initialized!");
            return _jsonDictionary.ContainsKey(key);
        }
        
        /// <summary> Saves all local data to the cloud. </summary>
        /// <param name="flush">
        /// Determines the order in which data is sent. If set to "true", the data will be sent to the server immediately;
        /// “false” (default value) - the request to send data will be queued.
        /// </param>
        public static void SaveInCloud(Action onSuccessCallback = null, Action<string> onErrorCallback = null, bool flush = false)
        {
            if (!Initialized) throw new Exception($"{nameof(Cloud)}. Not Initialized!");
            _json = _jsonDictionary.DictionaryToJson();
            string jsonToYandex = $"{{ \"{SAVE_NAME}\": \"{_json.StringToHex()}\"}}";

            if (!jsonToYandex.IsValidJson())
            {
                Debug.LogError($"Json is not valid.\nJson:\n{_json.HexToString()}\n\nJsonHex:\n{jsonToYandex}");
            }

            #if UNITY_WEBGL && !UNITY_EDITOR
            Agava.YandexGames.PlayerAccount.SetCloudSaveData(jsonToYandex, flush, onSuccessCallback, onErrorCallback);
            #elif UNITY_EDITOR
            YandexEditorData.Instance.SaveData = jsonToYandex;
            onSuccessCallback?.Invoke();
            #endif
        }

        /// <summary> Removes all data from the cloud. </summary>
        public static void ClearCloudData(Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            if (!Initialized) throw new Exception($"{nameof(Cloud)}. Not Initialized!");
            _json = "{}";
            _jsonDictionary = new Dictionary<string, object>();
            #if UNITY_WEBGL && !UNITY_EDITOR
            Agava.YandexGames.PlayerAccount.SetCloudSaveData("{}", false, onSuccessCallback, onErrorCallback);
            #elif UNITY_EDITOR
            YandexEditorData.Instance.SaveData = "";
            onSuccessCallback?.Invoke();
            #endif
        }
    }
}