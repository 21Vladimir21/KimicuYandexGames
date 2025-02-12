using System;

namespace Kimicu.YandexGames
{
    public static class WebApplication
    {
        public static event Action<bool> OnGameFocusChange;

        public static void Initialize(Action<bool> onGameFocusChange = null)
        {
            OnGameFocusChange = onGameFocusChange;

            #if !UNITY_EDITOR
            Agava.YandexGames.WebApplication.Initialize(OnAgavaGameFocusChange);
            #endif
        }

        private static void OnAgavaGameFocusChange(bool isFocused)
        {
	        OnGameFocusChange?.Invoke(isFocused);
        }
    }
}