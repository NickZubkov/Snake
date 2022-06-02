
using System.Collections.Generic;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Components
{
    public struct DragonBody
    {
        public int HeadID;
        public DragonHead Head;
        public List<MeshRenderer> ViewRenderers;
    }
}