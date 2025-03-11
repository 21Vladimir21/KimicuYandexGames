using System;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace Agava.YandexGames
{
	public static class Games
	{
#region GetAll
		private static Action<GameResponse[]> s_onGetAllGamesSuccessCallback;
		private static Action<string> s_onGetAllGamesErrorCallback;

		public static void GetAll(Action<GameResponse[]> onSuccessCallback, Action<string> onErrorCallback)
		{
			s_onGetAllGamesSuccessCallback = onSuccessCallback;
			s_onGetAllGamesErrorCallback = onErrorCallback;

			GetAllGames(OnGetAllSuccessCallback, OnGetAllErrorCallback);
		}

		[DllImport("__Internal")]
		private static extern void GetAllGames(Action<string> onSuccessCallback, Action<string> onErrorCallback);

		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnGetAllSuccessCallback(string json)
		{
			if (YandexGamesSdk.CallbackLogging)
				Debug.Log($"{nameof(Games)}.{nameof(OnGetAllSuccessCallback)} called. {nameof(json)}={json}");
			
			Debug.LogWarning(json);
			Debug.LogWarning(JsonConvert.DeserializeObject<GameResponse[]>(json));
			
			s_onGetAllGamesSuccessCallback?.Invoke(JsonConvert.DeserializeObject<GameResponse[]>(json));
		}
		
		
		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnGetAllErrorCallback(string errorMessage)
		{
			if (YandexGamesSdk.CallbackLogging)
				Debug.Log($"{nameof(Games)}.{nameof(OnGetAllErrorCallback)} called. {nameof(errorMessage)}={errorMessage}");
			
			s_onGetAllGamesErrorCallback?.Invoke(errorMessage);
		}
#endregion

#region GetById
		private static Action<GameResponse> s_onGetGameByIdSuccessCallback;
		private static Action<string> s_onGetGameByIdErrorCallback;

		public static void GetById(int appId, Action<GameResponse> onSuccessCallback, Action<string> onErrorCallback)
		{
			s_onGetGameByIdSuccessCallback = onSuccessCallback;
			s_onGetGameByIdErrorCallback = onErrorCallback;

			GetGameById(appId, OnGetByIdSuccessCallback, OnGetByIdErrorCallback);
		}

		[DllImport("__Internal")]
		private static extern void GetGameById(int appId, Action<string> onSuccessCallback, Action<string> onErrorCallback);

		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnGetByIdSuccessCallback(string json)
		{
			if (YandexGamesSdk.CallbackLogging)
				Debug.Log($"{nameof(Games)}.{nameof(OnGetByIdSuccessCallback)} called. {nameof(json)}={json}");
			
			s_onGetGameByIdSuccessCallback?.Invoke(JsonConvert.DeserializeObject<GameResponse>(json));
		}
		
		
		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnGetByIdErrorCallback(string errorMessage)
		{
			if (YandexGamesSdk.CallbackLogging)
				Debug.Log($"{nameof(Games)}.{nameof(OnGetByIdErrorCallback)} called. {nameof(errorMessage)}={errorMessage}");
			
			s_onGetGameByIdErrorCallback?.Invoke(errorMessage);
		}

#endregion
	}
	
	[Serializable]
	public class GameResponse
	{
		[field: Preserve]
		public int appID;
		[field: Preserve]
		public string title;
		[field: Preserve]
		public string url;
		[field: Preserve]
		public string coverURL;
		[field: Preserve]
		public string iconURL;
	}
}