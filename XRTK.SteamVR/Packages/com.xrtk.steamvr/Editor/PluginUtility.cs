// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using UnityEditor;
using XRTK.Utilities.Editor;

namespace XRTK.SteamVR.Editor
{
    [InitializeOnLoad]
    public static class PluginUtility
    {
        private const string OPEN_VR_API = "openvr_api.dll";
        private static readonly string SteamVRRootPath = PathFinderUtility.ResolvePath<IPathFinder>(typeof(SteamVRPathFinder));
        private static readonly string PluginPath = Path.GetFullPath($"{SteamVRRootPath}/Runtime/Plugins");
        private static readonly string NativeRoot = Path.GetFullPath($"{SteamVRRootPath}../../../../Submodules/steamvr_unity_plugin");
        private static readonly string NativeRuntime = Path.GetFullPath($"{NativeRoot}/Runtime");

        static PluginUtility()
        {
            if (!Directory.Exists(PluginPath))
            {
                UpdatePlugins();
            }
        }

        [MenuItem("Mixed Reality Toolkit/Tools/OpenVR/Update Plugin")]
        internal static void UpdatePlugins()
        {
            if (Directory.Exists(PluginPath))
            {
                Directory.Delete(PluginPath);
            }

            Directory.CreateDirectory($"{PluginPath}/x86");
            File.Copy($"{NativeRuntime}/x86/{OPEN_VR_API}", $"{PluginPath}/x86/{OPEN_VR_API}");
            File.Copy($"{NativeRuntime}/x86/{OPEN_VR_API}.meta", $"{PluginPath}/x86/{OPEN_VR_API}.meta");

            Directory.CreateDirectory($"{PluginPath}/x64");
            File.Copy($"{NativeRuntime}/x64/{OPEN_VR_API}", $"{PluginPath}/x64/{OPEN_VR_API}");
            File.Copy($"{NativeRuntime}/x64/{OPEN_VR_API}.meta", $"{PluginPath}/x64/{OPEN_VR_API}.meta");


            File.Copy($"{NativeRuntime}/openvr_api.cs", $"{PluginPath}/openvr_api.cs");
            File.Copy($"{NativeRoot}/License.md", $"{PluginPath}/Licnse.md");
            File.Copy($"{NativeRoot}/openvrLICENSE.md", $"{PluginPath}/openvrLICENSE.md");

            XRTK.Editor.Utilities.GuidRegenerator.RegenerateGuids($"{PluginPath}");
        }
    }
}
