﻿// Copyright (c) XRTK. All rights reserved.
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
        private const string NATIVE_ROOT_PATH = "Submodules/steamvr_unity_plugin";

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

        private static string NativeRuntime => Path.GetFullPath($"{NativeRootPath}/Runtime");

        static SteamVRPluginUtility()
        {
            if (!Directory.Exists(PluginPath) || EditorPreferences.Get($"Reimport_{nameof(SteamVRPluginUtility)}", false))
            {
                EditorPreferences.Set($"Reimport_{nameof(SteamVRPluginUtility)}", false);

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
                File.Copy($"{NativeRootPath}/License.md", $"{PluginPath}/Licnse.md");
                File.Copy($"{NativeRootPath}/openvrLICENSE.md", $"{PluginPath}/openvrLICENSE.md");
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