// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XRTK.Definitions.Controllers;
using XRTK.Definitions.Utilities;
using XRTK.SteamVR.Providers.Controllers;

namespace XRTK.SteamVR.Profiles
{
    public class SteamVRControllerDataProviderProfile : BaseMixedRealityControllerDataProviderProfile
    {
        public override ControllerDefinition[] GetDefaultControllerOptions()
        {
            return new[]
            {
                new ControllerDefinition(typeof(SteamVRController), Handedness.Left),
                new ControllerDefinition(typeof(SteamVRController), Handedness.Right)
            };
        }
    }
}

