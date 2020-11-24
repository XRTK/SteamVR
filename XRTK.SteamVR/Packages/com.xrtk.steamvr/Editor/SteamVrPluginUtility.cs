// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using XRTK.Utilities.Editor;

namespace XRTK.SteamVR.Editor
{
    [InitializeOnLoad]
    public static class SteamVRPluginUtility
    {
        private const string GIT_ROOT = "../../../../";
        private const string OPEN_VR_API = "openvr_api.dll";
        private const string NATIVE_ROOT_PATH = "Submodules/unity-xr-plugin";

        private static readonly string SteamVRRootPath = PathFinderUtility.ResolvePath<IPathFinder>(typeof(SteamVRPathFinder));
        private static readonly string PluginPath = Path.GetFullPath($"{SteamVRRootPath}/Runtime/Plugins");

        private static string NativeRootPath
        {
            get
            {
                var path = Path.GetFullPath($"{SteamVRRootPath}{GIT_ROOT}{NATIVE_ROOT_PATH}");

                if (!Directory.Exists(path))
                {
                    path = Path.GetFullPath($"{SteamVRRootPath}{GIT_ROOT}Submodules/SteamVR/{NATIVE_ROOT_PATH}");
                }

                return path;
            }
        }

        private static string PackageRoot => Path.GetFullPath($"{NativeRootPath}/com.valve.openvr");

        private static string NativeRuntime => Path.GetFullPath($"{PackageRoot}/Runtime");

        static SteamVRPluginUtility()
        {
            if (!Directory.Exists(PluginPath) ||
                EditorPreferences.Get($"Reimport_{nameof(SteamVRPluginUtility)}", true))
            {
                if (!Directory.Exists(NativeRootPath))
                {
                    throw new Exception($"Failed to find {NativeRootPath}");
                }

                if (Directory.Exists(PluginPath))
                {
                    var files = Directory.GetFiles(PluginPath, "*", SearchOption.AllDirectories);

                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }

                    var directories = Directory.GetDirectories(PluginPath, "*", SearchOption.AllDirectories);

                    foreach (var directory in directories)
                    {
                        Directory.Delete(directory);
                    }

                    Directory.Delete(PluginPath);
                }

                Directory.CreateDirectory($"{PluginPath}/x86");
                File.Copy($"{NativeRuntime}/x86/{OPEN_VR_API}", $"{PluginPath}/x86/{OPEN_VR_API}");

                Directory.CreateDirectory($"{PluginPath}/x64");
                File.Copy($"{NativeRuntime}/x64/{OPEN_VR_API}", $"{PluginPath}/x64/{OPEN_VR_API}");

                File.Copy($"{NativeRuntime}/openvr_api.cs", $"{PluginPath}/openvr_api.cs");
                File.Copy($"{PackageRoot}/LICENSE.md", $"{PluginPath}/LICENSE.md");

                EditorApplication.delayCall += () => AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }

            if (!Directory.Exists(PluginPath) &&
                EditorPreferences.Get($"Reimport_{nameof(SteamVRPluginUtility)}", false))
            {
                EditorPreferences.Set($"Reimport_{nameof(SteamVRPluginUtility)}", false);

                var rootPluginPath = $"{SteamVRRootPath}/Runtime/Plugins";

                var x86Path = $"{rootPluginPath}/x86/{OPEN_VR_API}";
                Debug.Assert(File.Exists(x86Path), $"Library not found at {x86Path}");
                var x86Importer = AssetImporter.GetAtPath(x86Path) as PluginImporter;
                Debug.Assert(x86Importer != null, $"Failed to load {x86Path}");
                x86Importer.ClearSettings();
                x86Importer.SetCompatibleWithAnyPlatform(false);
                x86Importer.SetCompatibleWithEditor(true);
                x86Importer.SetEditorData("CPU", "x86");
                x86Importer.SetPlatformData(BuildTarget.NoTarget, "CPU", "x86");
                x86Importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, true);
                x86Importer.SetPlatformData(BuildTarget.StandaloneWindows, "CPU", "x86");
                x86Importer.SaveAndReimport();

                var x64Path = $"{rootPluginPath}/x64/{OPEN_VR_API}";
                Debug.Assert(File.Exists(x64Path), $"Library not found at {x64Path}");
                var x64Importer = AssetImporter.GetAtPath(x64Path) as PluginImporter;
                Debug.Assert(x64Importer != null, $"Failed to load {x64Path}");
                x64Importer.ClearSettings();
                x64Importer.SetCompatibleWithAnyPlatform(false);
                x64Importer.SetCompatibleWithEditor(true);
                x64Importer.SetEditorData("CPU", "x64");
                x64Importer.SetPlatformData(BuildTarget.NoTarget, "CPU", "x64");
                x64Importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, true);
                x64Importer.SetPlatformData(BuildTarget.StandaloneWindows64, "CPU", "x64");
                x64Importer.SaveAndReimport();
            }
        }

        [MenuItem("Mixed Reality Toolkit/Tools/SteamVR/Reimport Plugins", true)]
        private static bool UpdatePluginValidation() => Directory.Exists(NativeRootPath);

        [MenuItem("Mixed Reality Toolkit/Tools/SteamVR/Reimport Plugins", false)]
        private static void UpdatePlugins()
        {
            if (EditorUtility.DisplayDialog("Attention!",
                "In order to reimport the SteamVR plugins, we'll need to restart the editor, is this ok?", "Restart", "Cancel"))
            {
                EditorPreferences.Set($"Reimport_{nameof(SteamVRPluginUtility)}", true);
                EditorApplication.OpenProject(Directory.GetParent(Application.dataPath).FullName);
            }
        }
    }
}
