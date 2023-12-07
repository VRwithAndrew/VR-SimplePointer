
using UnityEngine;

namespace Sunbox.Avatars {
    
    [CreateAssetMenu(fileName = "HairItem", menuName = "Sunbox Games/Avatars/Create Hair Item", order = 1)]
    public class HairItem : HairItemBase {
        public MeshRenderer HairMesh;
        public Vector3 HatOffset = Vector3.zero;
        public bool HideHairWhenHatEquipped = false;
    }

}
