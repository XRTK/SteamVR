// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XRTK.Attributes;
using XRTK.Definitions.CameraSystem;
using XRTK.Definitions.Platforms;
using XRTK.Interfaces.CameraSystem;
using XRTK.Providers.CameraSystem;

namespace XRTK.SteamVR.Providers.CameraSystem
{
    [RuntimePlatform(typeof(SteamVRPlatform))]
    [System.Runtime.InteropServices.Guid("7314EC7D-E99B-42C9-B445-5AC0D89D083A")]
    public class SteamVRCameraDataProvider : BaseCameraDataProvider
    {
        /// <inheritdoc />
        public SteamVRCameraDataProvider(string name, uint priority, BaseMixedRealityCameraDataProviderProfile profile, IMixedRealityCameraSystem parentService)
            : base(name, priority, profile, parentService)
        {
        }
    }
}
