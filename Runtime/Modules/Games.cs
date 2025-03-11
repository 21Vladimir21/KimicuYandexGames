using System;
using Agava.YandexGames;

namespace Kimicu.YandexGames
{
	public static class Games
	{
		public static void GetAllGames(Action<GameResponse[]> successCallback, Action<string> errorCallback)
		{
			#if !UNITY_EDITOR && UNITY_WEBGL
			Agava.YandexGames.Games.GetAll(successCallback, errorCallback);
			#else
			successCallback?.Invoke(new GameResponse[]
			{
				new GameResponse
				{
					appID = 100000,
					title = "Title1",
					url = "https://google.com",
					coverURL = "https://loremflickr.com/256/256",
					iconURL = "https://loremflickr.com/256/256"
				},
				new GameResponse
				{
					appID = 200000,
					title = "Title2",
					url = "https://google.com",
					coverURL = "https://loremflickr.com/256/256",
					iconURL = "https://loremflickr.com/256/256"
				},
				new GameResponse
				{
					appID = 300000,
					title = "Title3",
					url = "https://google.com",
					coverURL = "https://loremflickr.com/256/256",
					iconURL = "https://loremflickr.com/256/256"
				}
			});
			#endif
		}

		public static void GetGameById(int appId, Action<GameResponse> successCallback, Action<string> errorCallback)
		{
			#if !UNITY_EDITOR && UNITY_WEBGL
			Agava.YandexGames.Games.GetById(appId, successCallback, errorCallback);
			#else
			successCallback?.Invoke(new GameResponse
			{
				appID = appId,
				title = "Title",
				url = "https://google.com",
				coverURL = "https://loremflickr.com/256/256",
				iconURL = "https://loremflickr.com/256/256"
			});
			#endif
		}
	}
}