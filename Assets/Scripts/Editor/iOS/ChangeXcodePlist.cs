#if UNITY_EDITOR_OSX
using System.IO;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;

namespace Pretia.RelocChecker.Editor.iOS
{
    public class ChangeXcodePlist
    {
        /// <summary>
        /// Change values in Xcode plist
        /// </summary>
        /// <param name="buildTarget"></param>
        /// <param name="pathToBuiltProject"></param>
        [PostProcessBuild]
        public static void ChangeXcodeProject(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (buildTarget != BuildTarget.iOS){ return;}

            // Get plist
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
           
            // Get root
            PlistElementDict rootDict = plist.root;

            // File App support
            // Set `Supports Document Browser`to `YES`
            // https://developer.apple.com/documentation/bundleresources/information_property_list/uisupportsdocumentbrowser
            rootDict.SetBoolean("UISupportsDocumentBrowser", true);

            // App submission declaration, since we dont use any encryption method
            // Set `App Uses Non-Exempt Encryption` to `NO`
            // https://developer.apple.com/documentation/bundleresources/information_property_list/itsappusesnonexemptencryption/
            rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
#endif