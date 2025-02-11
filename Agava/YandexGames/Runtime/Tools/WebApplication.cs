using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Agava.YandexGames
{
	public static class WebApplication
	{
		[DllImport("__Internal")]
		private static extern void WebApplicationInitialize(Action<bool> onGameFocusChangeCallback);
		
		private static Action<bool> s_onGameFocusChangeCallback;

		public static void Initialize(Action<bool> onGameFocusChangeCallback)
		{
			s_onGameFocusChangeCallback = onGameFocusChangeCallback;
			
			WebApplicationInitialize(OnGameFocusChange);
		}

		[MonoPInvokeCallback(typeof(Action<bool>))]
		private static void OnGameFocusChange(bool isFocused)
		{
			if (YandexGamesSdk.CallbackLogging)
				Debug.Log($"{nameof(WebApplication)}.{nameof(OnGameFocusChange)} called. {nameof(isFocused)}={isFocused}");
			
			s_onGameFocusChangeCallback?.Invoke(isFocused);
		}
	}
}