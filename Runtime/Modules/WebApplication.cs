using System;

namespace Kimicu.YandexGames
{
    public static class WebApplication
    {
        private static bool customValue;
        public static bool CustomValue
        {
            get => customValue;
            set
            {
                customValue = value;
                CheckFocus();
            }
        }

        private static bool inFocus;
        
        public static event Action<bool> OnGameFocusChange;

        /// <summary>
        /// <b>False</b> - Game is in background, should be paused
        /// <b>True</b> - Game is active, should be unpaused
        /// </summary>
        public static void Initialize(Action<bool> onGameFocusChange = null)
        {
            OnGameFocusChange = onGameFocusChange;

            #if !UNITY_EDITOR
            Agava.YandexGames.WebApplication.Initialize(OnAgavaGameFocusChange);
            #endif
        }

        private static void OnAgavaGameFocusChange(bool isFocused)
        {
            inFocus = isFocused;
            CheckFocus();
        }

        private static void CheckFocus()
        {
            OnGameFocusChange?.Invoke(inFocus && !customValue);
        }
    }
}