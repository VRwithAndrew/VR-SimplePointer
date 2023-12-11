
using UnityEngine;

namespace Sunbox.Avatars {

    public enum SlotType {
        Hair, 
        Hat, 
        Neck, 
        Glasses, 
        Top, 
        Bottom, 
        Shoes
    }

    public enum AttachmentType {
        ParentToBone,
        SkinnedToArmature
    }

    [CreateAssetMenu(fileName = "ClothingItem", menuName = "Sunbox Games/Avatars/Create Clothing Item", order = 1)]
    public class ClothingItem : ScriptableObject, IVariations {
        public string Name;
        public bool IsEmpty;
        public SlotType SlotType;
        public Mesh MaleMesh;
        public Mesh FemaleMesh;
        public Material[] Variations;
        public Material SecondaryMaterial;
        public AvatarCustomization.HideBlendShapeIndex HideShapeWeightIndex = AvatarCustomization.HideBlendShapeIndex.Nothing;
        public AvatarCustomization.HideBlendShapeIndex HideShapeWeightIndexSecondary = AvatarCustomization.HideBlendShapeIndex.Nothing;
        public SlotType[] HideSlots;

        public Material GetVariation(int i) {
            return Variations[i];
        }

        public string GetVariationName(int i) {
            return Variations[i].name;
        }

        public int GetVariationsCount() {
            return Variations.Length;
        }

        public override string ToString() => Name;
    }
}
