
using UnityEngine;

namespace Sunbox.Avatars {

    public interface IVariations {
        public Material GetVariation(int i);
        public int GetVariationsCount();
        public string GetVariationName(int i);
    }

    public class HairItemBase : ScriptableObject, IVariations {
        public string Name;
        public Material[] Variations;

        public Material GetVariation(int i) {
            return Variations[i];
        }

        public string GetVariationName(int i) {
            return Variations[i].name;
        }

        public int GetVariationsCount() {
            return Variations.Length;
        }

        public bool HasVariations() {
            return Variations != null && Variations.Length > 0;
        }

        public override string ToString() => Name;
    }

}
