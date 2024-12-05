﻿using Agava.YandexGames;
using Kimicu.YandexGames.Extension;
using KimicuYandexGames.Utils;

namespace Kimicu.YandexGames
{
    public static class AdBlock
    {
	    /// <returns> Status of an AdBlock addon in the browser. </returns>
	    public static bool Enabled =>
#if !UNITY_EDITOR && UNITY_WEBGL
	        Agava.WebUtility.AdBlock.Enabled;
#else
		    YandexEditorData.Instance.AdBlock;
#endif
    }
}