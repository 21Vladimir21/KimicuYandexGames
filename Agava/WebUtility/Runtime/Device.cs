using System.Runtime.InteropServices;
using UnityEngine;

namespace Agava.WebUtility
{
    public static class Device
    {
        public static bool IsMobile
        {
            get
            {
                #if UNITY_WEBGL && !UNITY_EDITOR
                return GetDeviceIsMobile();
                #endif

                return SystemInfo.deviceType == DeviceType.Handheld;
            }
        }

        [DllImport("__Internal")]
        private static extern bool GetDeviceIsMobile();
    }
}
