
using UnityEngine;

namespace Sunbox.Avatars {

    public class Slot : MonoBehaviour {
        public SlotType SlotType;
        public AttachmentType AttachmentType;
        public Transform BoneTransform;

        void Awake() {
            BoneTransform = GetComponent<Transform>();
        }
        
    }

}