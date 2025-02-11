using System;

namespace Kimicu.YandexGames
{
    public static class WebApplication
    {
        public static event Action<bool> OnGameFocusChange;

        public static void Initialize(Action<bool> onGameFocusChange = null)
        {
            OnGameFocusChange = onGameFocusChange;

            Agava.YandexGames.WebApplication.Initialize(OnAgavaGameFocusChange);
        }

        private static void OnAgavaGameFocusChange(bool isFocused)
        {
	        OnGameFocusChange?.Invoke(isFocused);
        }
    }
}