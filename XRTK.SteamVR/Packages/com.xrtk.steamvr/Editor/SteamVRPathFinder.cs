// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using XRTK.Editor.Utilities;

namespace XRTK.SteamVR.Editor
{
    /// <summary>
    /// Dummy scriptable object used to find the relative path of the com.xrtk.steamvr.
    /// </summary>
    /// <inheritdoc cref="IPathFinder" />
    public class SteamVRPathFinder : ScriptableObject, IPathFinder
    {
        ///// <inheritdoc />
        public string Location => $"/Editor/{nameof(SteamVRPathFinder)}.cs";
    }
}
