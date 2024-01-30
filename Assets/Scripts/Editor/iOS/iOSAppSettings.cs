using System.Runtime.InteropServices;
using AOT;

namespace Pretia.RelocChecker.Editor.iOS
{

    public class iOSAppSettings
    {
        public enum CameraAuthorizationStatus
        {
            NotDetermined = 0,
            Restricted = 1,
            Denied = 2,
            Authorized = 3,
        }

        public delegate void PermissionEvent(bool value);

        private static event PermissionEvent _onPermissionRequested;

#if UNITY_IOS

        [DllImport("__Internal")]
        private static extern int showAlert(string title, string message);

        [DllImport("__Internal")]
        private static extern int cameraPermissionStatus();

        [DllImport("__Internal")]
        private static extern int requestCameraAccess(PermissionEvent callback);

        [MonoPInvokeCallback(typeof(PermissionEvent))]
        private static void cs_callback(bool value)
        {
            _onPermissionRequested?.Invoke(value);
        }
#endif

        public static void ShowAlert(string title, string message)
        {
#if UNITY_IOS
            showAlert(title, message);
#else
        throw new System.NotSupportedException(nameof(ShowAlert));
#endif
        }

        public static CameraAuthorizationStatus GetCameraAuthorizationStatus()
        {
#if UNITY_IOS
            return (CameraAuthorizationStatus)cameraPermissionStatus();
#else
        throw new System.NotSupportedException(nameof(GetCameraAuthorizationStatus));
#endif
        }

        public static void RequestCameraAccess(PermissionEvent callback)
        {
#if UNITY_IOS
            _onPermissionRequested += callback;
            requestCameraAccess(cs_callback);
#else
        throw new System.NotSupportedException(nameof(RequestCameraAccess));
#endif
        }
    }
}