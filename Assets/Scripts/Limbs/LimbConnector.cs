using UnityEngine;
using System;


namespace FictionalOctoDoodle.Core
{
    public partial class LimbAssembly
    {
        [Serializable]
        public class LimbConnector
        {
            public LimbSlot slotId;
            public Vector2 position;
            public int orderInLayer;
            public LimbData limbData;
            public GameObject limbObj;

            public void ClearLoadedLimb()
            {
                limbData = null;
                Destroy(limbObj);
                limbObj = null;
            }
        }
    }
}
