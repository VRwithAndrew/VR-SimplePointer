
using UnityEngine;

namespace Sunbox.Avatars {

    /// <summary>
    /// Instantiated clothing item behaviour
    /// </summary>
    public class UClothingItem : MonoBehaviour {
        public ClothingItem ClothingItem;
        public int VariationIndex;
        public bool IsEquipped;

        /// <summary>
        /// Sets clothing item variation. Returns true if the variation has been set
        /// or false if not (index out of range).
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool SetVariation(int index) {
            if (index < ClothingItem.Variations.Length && index >= 0) {

                Renderer renderer = GetComponentInChildren<Renderer>();

                if (renderer == null) {
                    Debug.LogError($"{nameof(UClothingItem)} Renderer is null for {ClothingItem.Name}");
                }

                if (ClothingItem.SecondaryMaterial != null) {
                    Material[] materials = new Material[2];
                    materials[0] = ClothingItem.Variations[index];
                    materials[1] = ClothingItem.SecondaryMaterial;
                    renderer.sharedMaterials = materials;
                }

                if (ClothingItem.SecondaryMaterial == null) {
                    renderer.sharedMaterial = ClothingItem.Variations[index];
                }
                
                VariationIndex = index;

                return true;            
            }

            return false;
        }
    }

}
