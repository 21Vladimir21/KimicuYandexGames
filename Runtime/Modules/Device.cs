using Kimicu.YandexGames.Extension;
using KimicuYandexGames.Utils;

namespace Kimicu.YandexGames
{
    public static class Device
    {
	    public static bool IsMobile =>
#if !UNITY_EDITOR && UNITY_WEBGL
            Agava.WebUtility.Device.IsMobile;
#else
		    YandexEditorData.Instance.IsMobile;
#endif
    }
}