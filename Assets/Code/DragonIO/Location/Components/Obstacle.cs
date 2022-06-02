
using System.Collections.Generic;
using UnityEngine;

namespace Modules.DragonIO.Location.Components
{
    public struct Obstacle
    {
        public int DestroyThreshold;
        public List<MeshRenderer> ViewMeshRenderers;
    }
}