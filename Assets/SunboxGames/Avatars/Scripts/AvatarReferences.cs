using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunbox.Avatars {

    [CreateAssetMenu(fileName = "AvatarReferences", menuName = "Sunbox Games/Avatars/Create Avatar References", order = 0)]
    public class AvatarReferences : ScriptableObject {
        
        public Material[] MaleSkinMaterials;
        public Material[] FemaleSkinMaterials;
        public Material[] EyeMaterials;
        public Material[] BrowMaterials;
        public Material[] NailMaterials;
        public Material[] LashesMaterials;
        public HairItem[] HairItems;
        public FacialHairItem[] FacialHairItems;
        public ClothingItem[] AvailableClothingItems;

    }

}
