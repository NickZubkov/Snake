using System.Collections.Generic;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Components
{
    public struct DragonHead
    {
        public Data.DragonConfig Config;
        public List<Transform> BodyParts;
        public List<Vector3> PositionsHistory;
    }
}