// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XRTK.Attributes;
using XRTK.Definitions.Platforms;
using XRTK.Interfaces.InputSystem;
using XRTK.SteamVR.Profiles;
using XRTK.Providers.Controllers;

namespace XRTK.SteamVR.Providers.Controllers
{
    [RuntimePlatform(typeof(SteamVRPlatform))]
    [System.Runtime.InteropServices.Guid("1586EF90-10B8-4EA5-A80A-5BD8E20DA47B")]
    public class SteamVRControllerDataProvider : BaseControllerDataProvider
    {
        /// <inheritdoc />
        public SteamVRControllerDataProvider(string name, uint priority, SteamVRControllerDataProviderProfile profile, IMixedRealityInputSystem parentService)
            : base(name, priority, profile, parentService)
        {
        }
    }
}

