

using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Sunbox.Avatars {

#if UNITY_EDITOR
    [CustomEditor(typeof(AvatarCustomization))]
    public class AvatarCustomizationEditor : Editor {

        public static AvatarCustomizationEditor CurrentEditor;

        AvatarCustomization _instance;

        private SerializedProperty _maleBaseGameObjectProperty;
        private SerializedProperty _femaleBaseGameObjectProperty;
        private SerializedProperty _maleBodyGeometryProperty;
        private SerializedProperty _femaleBodyGeometryProperty;
        private SerializedProperty _genderProperty;
        private SerializedProperty _nailStyleIndexProperty;
        
        private SerializedProperty _skinColorSerialized;
        private SerializedProperty _bodyHeightSerialized;
        private SerializedProperty _bodyFatSerialized;
        private SerializedProperty _bodyMuscleSerialized;
        private SerializedProperty _breastSizeSerialized;

        private bool _randomizeUnifiedHairColor = true;
        private bool _randomizeIgnoreHeight = true;

        private Texture2D _logo;

        private bool _alwaysUpdateCustomization = false;

        void OnEnable() {

            CurrentEditor = this;

            _instance = (AvatarCustomization) target;
            
            _maleBaseGameObjectProperty = serializedObject.FindProperty(nameof(_instance.MaleBase));
            _femaleBaseGameObjectProperty = serializedObject.FindProperty(nameof(_instance.FemaleBase));
            _maleBodyGeometryProperty = serializedObject.FindProperty(nameof(_instance.MaleBodyGEO));
            _femaleBodyGeometryProperty = serializedObject.FindProperty(nameof(_instance.FemaleBodyGEO));
            _genderProperty = serializedObject.FindProperty(nameof(_instance.CurrentGender));
            _nailStyleIndexProperty = serializedObject.FindProperty(nameof(_instance.NailsMaterialIndex));

            _bodyHeightSerialized = serializedObject.FindProperty(nameof(_instance.BodyHeight));
            _bodyFatSerialized = serializedObject.FindProperty(nameof(_instance.BodyFat));
            _bodyMuscleSerialized = serializedObject.FindProperty(nameof(_instance.BodyMuscle));
            _breastSizeSerialized = serializedObject.FindProperty(nameof(_instance.BreastSize));

        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            DrawHeader_Internal();
            DrawGeneralAndReferencesProperties_Internal();

            if (_instance.AvatarReferences != null) {
                DrawBodyProperties_Internal();
                DrawHairProperties_Internal();
                DrawFaceProperties_Internal();
                DrawClothingItemProperties_Internal();
                DrawOtherSettings_Internal();
                DrawRandomizeSettings_Internal();

                GUILayout.Space(20);
                AvatarEditorUtilities.LineSeperator();
                
                GUILayout.Space(10);
                _alwaysUpdateCustomization = EditorGUILayout.ToggleLeft("Update in Editor", _alwaysUpdateCustomization);

                GUILayout.Space(10);
                if (GUILayout.Button("Update Customization", GUILayout.Height(50)) || _alwaysUpdateCustomization) {
                    _instance.UpdateCustomization();
                }
                GUILayout.Space(20);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawHeader_Internal() {
            if (_logo == null) {
                _logo = Resources.Load<Texture2D>("logo");
            }

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(_logo, GUILayout.Width(100), GUILayout.Height(100));
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUIStyle style = new GUIStyle() {
                fontSize = 20,  
            };
            style.normal.textColor = Color.white;

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Avatar Customization Editor FREE", style);
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Check FULL version on Asset Store")) {
                    Application.OpenURL("https://assetstore.unity.com/packages/3d/characters/humanoids/stylized-customizable-avatars-226885");
                } 
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
        }

        private void DrawGeneralAndReferencesProperties_Internal() {
            GUILayout.Space(20);
            AvatarEditorUtilities.LineSeperator();
            EditorGUILayout.LabelField("General Properties", EditorStyles.whiteLargeLabel);

            // Gender
            AvatarCustomization.AvatarGender currentGender = _instance.CurrentGender;
            AvatarCustomization.AvatarGender newGender = (AvatarCustomization.AvatarGender) EditorGUILayout.EnumPopup("Gender: ", _instance.CurrentGender);
            if (currentGender != newGender) {
                _genderProperty.enumValueIndex = (int) newGender;
                _instance.SetGender(newGender, true);
                _instance.UpdateCustomization();
                _instance.UpdateClothing();     
            }

            // Animator controllers
            _instance.AnimatorController = (RuntimeAnimatorController) EditorGUILayout.ObjectField("Animator Controller", _instance.AnimatorController, typeof(RuntimeAnimatorController), allowSceneObjects: true);
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_instance.AvatarReferences)));
            EditorGUILayout.PropertyField(_maleBaseGameObjectProperty);
            EditorGUILayout.PropertyField(_femaleBaseGameObjectProperty);
            EditorGUILayout.PropertyField(_maleBodyGeometryProperty);
            EditorGUILayout.PropertyField(_femaleBodyGeometryProperty);
        }

        private void DrawBodyProperties_Internal() {
            GUILayout.Space(20);
            AvatarEditorUtilities.LineSeperator();
            EditorGUILayout.LabelField("Body Properties", EditorStyles.whiteLargeLabel);
            GUILayout.Space(10);

            AvatarEditorUtilities.AvatarProperty<float>(_instance, serializedObject, nameof(_instance.BodyHeight));
            AvatarEditorUtilities.AvatarProperty<float>(_instance, serializedObject, nameof(_instance.BodyFat));
            AvatarEditorUtilities.AvatarProperty<float>(_instance, serializedObject, nameof(_instance.BodyMuscle));

            if (_instance.CurrentGender == AvatarCustomization.AvatarGender.Female) {
                AvatarEditorUtilities.AvatarProperty<float>(_instance, serializedObject, nameof(_instance.BreastSize));
            }

            GUILayout.Space(10);

            // Skin
            if (_instance.CurrentGender == AvatarCustomization.AvatarGender.Male) {
                DrawMaterialSelection_Internal(nameof(_instance.SkinMaterialIndex), _instance.SkinMaterialIndex, _instance.AvatarReferences.MaleSkinMaterials);
            }

            if (_instance.CurrentGender == AvatarCustomization.AvatarGender.Female) {
                DrawMaterialSelection_Internal(nameof(_instance.SkinMaterialIndex), _instance.SkinMaterialIndex, _instance.AvatarReferences.FemaleSkinMaterials);
            }

            // Nails
            DrawMaterialSelection_Internal(nameof(_instance.NailsMaterialIndex), _instance.NailsMaterialIndex, _instance.AvatarReferences.NailMaterials);

            // Eyes
            DrawMaterialSelection_Internal(nameof(_instance.EyeMaterialIndex), _instance.EyeMaterialIndex, _instance.AvatarReferences.EyeMaterials);

            // Brows
            DrawMaterialSelection_Internal(nameof(_instance.BrowMaterialIndex), _instance.HairMaterialIndex, _instance.AvatarReferences.BrowMaterials);

            // Lashes
            DrawMaterialSelection_Internal(nameof(_instance.LashesMaterialIndex), _instance.LashesMaterialIndex, _instance.AvatarReferences.LashesMaterials);
        }

        private void DrawMaterialSelection_Internal(string fieldName, int currentIndex, Material[] materials) {
            if (materials == null || materials.Length == 0 || materials[0] == null) {
                GUILayout.Space(10);
                EditorGUILayout.HelpBox($"No materials found for index {fieldName}. Include them in References section", MessageType.Error);

                return;
            }

            if (currentIndex > materials.Length - 1) {
                currentIndex = 0;
            }

            AvatarEditorUtilities.AvatarProperty<int>(_instance, serializedObject, fieldName, 0, materials.Length - 1);

            if (materials == null || currentIndex > materials.Length - 1) {
                return;
            }
            
            EditorGUILayout.LabelField($"({materials[currentIndex].name})", EditorStyles.whiteMiniLabel);
        }
    
        private void DrawHairProperties_Internal() {
            GUILayout.Space(20);
            AvatarEditorUtilities.LineSeperator();
            EditorGUILayout.LabelField("Hair Properties", EditorStyles.whiteLargeLabel);
            GUILayout.Space(10);

            // Hair.
            if (_instance.AvatarReferences.HairItems.Length > 0) {
                if (_instance.HairStyleIndex > _instance.AvatarReferences.HairItems.Length - 1) {
                    _instance.HairStyleIndex = 0;
                    _instance.HairMaterialIndex = 0;
                }
                AvatarEditorUtilities.AvatarProperty<int>(_instance, serializedObject, nameof(_instance.HairStyleIndex), 0, _instance.AvatarReferences.HairItems.Length - 1);
                EditorGUILayout.LabelField($"({_instance.AvatarReferences.HairItems[_instance.HairStyleIndex].Name})", EditorStyles.whiteMiniLabel);

                if (_instance.AvatarReferences.HairItems[_instance.HairStyleIndex].HasVariations()) {
                    AvatarEditorUtilities.AvatarProperty<int>(_instance, serializedObject, nameof(_instance.HairMaterialIndex), 0, _instance.AvatarReferences.HairItems[_instance.HairStyleIndex].Variations.Length - 1);
                    EditorGUILayout.LabelField($"({_instance.AvatarReferences.HairItems[_instance.HairStyleIndex].Variations[_instance.HairMaterialIndex].name})", EditorStyles.whiteMiniLabel);
                }

            }
            else {
                EditorGUILayout.HelpBox($"No {nameof(_instance.AvatarReferences.HairItems)} added to References.", MessageType.Info);
            }

            // Facial hair
            if (_instance.CurrentGender == AvatarCustomization.AvatarGender.Male) {
                if (_instance.AvatarReferences.FacialHairItems.Length > 0) {
                    GUILayout.Space(10);

                    if (_instance.FacialHairStyleIndex > _instance.AvatarReferences.FacialHairItems.Length - 1) {
                        _instance.FacialHairStyleIndex = 0;
                        _instance.FacialHairMaterialIndex = 0;
                    }
                    AvatarEditorUtilities.AvatarProperty<int>(_instance, serializedObject, nameof(_instance.FacialHairStyleIndex), 0, _instance.AvatarReferences.FacialHairItems.Length - 1);
                    EditorGUILayout.LabelField($"({_instance.AvatarReferences.FacialHairItems[_instance.FacialHairStyleIndex].Name})", EditorStyles.whiteMiniLabel);

                    if (_instance.AvatarReferences.FacialHairItems[_instance.FacialHairStyleIndex].HasVariations()) {
                        AvatarEditorUtilities.AvatarProperty<int>(_instance, serializedObject, nameof(_instance.FacialHairMaterialIndex), 0, _instance.AvatarReferences.FacialHairItems[_instance.FacialHairStyleIndex].Variations.Length - 1);
                        EditorGUILayout.LabelField($"({_instance.AvatarReferences.FacialHairItems[_instance.FacialHairStyleIndex].Variations[_instance.FacialHairMaterialIndex].name})", EditorStyles.whiteMiniLabel);
                    }
                } 
                else {
                    EditorGUILayout.HelpBox($"No {nameof(_instance.AvatarReferences.FacialHairItems)} added to References.", MessageType.Info);
                }
            }
        }

        private void DrawFaceProperties_Internal() {
            GUILayout.Space(20);
            AvatarEditorUtilities.LineSeperator();
            EditorGUILayout.LabelField("Face Properties", EditorStyles.whiteLargeLabel);
            GUILayout.Space(10);

            AvatarEditorUtilities.AvatarProperty<float>(_instance, serializedObject, nameof(_instance.NoseLength));

            AvatarEditorUtilities.AvatarProperty<float>(_instance, serializedObject, nameof(_instance.LipsWidth));
            AvatarEditorUtilities.AvatarProperty<float>(_instance, serializedObject, nameof(_instance.JawWidth));

            AvatarEditorUtilities.AvatarProperty<float>(_instance, serializedObject, nameof(_instance.BrowWidth));
            AvatarEditorUtilities.AvatarProperty<float>(_instance, serializedObject, nameof(_instance.BrowHeight));

            AvatarEditorUtilities.AvatarProperty<float>(_instance, serializedObject, nameof(_instance.EyesSize));
            AvatarEditorUtilities.AvatarProperty<float>(_instance, serializedObject, nameof(_instance.EyesClosedDefault));

            GUILayout.Space(10);
        }
    
        private void DrawClothingItemProperties_Internal() {
            GUILayout.Space(20);
            AvatarEditorUtilities.LineSeperator();
            EditorGUILayout.LabelField("Clothing Items", EditorStyles.whiteLargeLabel);
            
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_instance.ClothingItemHat)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_instance.ClothingItemTop)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_instance.ClothingItemBottom)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_instance.ClothingItemGlasses)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_instance.ClothingItemShoes)));

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Clothing Item Variations", EditorStyles.whiteLargeLabel);
            GUILayout.Space(10);

            if (_instance.ClothingItemHat != null) {
                _instance.ClothingItemHatVariationIndex = EditorGUILayout.IntSlider($"{_instance.ClothingItemHat.Name} Variation", _instance.ClothingItemHatVariationIndex, 0, _instance.ClothingItemHat.Variations.Length - 1);
                EditorGUILayout.LabelField($"({_instance.ClothingItemHat.Variations[_instance.ClothingItemHatVariationIndex].name})", EditorStyles.whiteMiniLabel);
            }

            if (_instance.ClothingItemTop != null) {
                _instance.ClothingItemTopVariationIndex = EditorGUILayout.IntSlider($"{_instance.ClothingItemTop.Name} Variation", _instance.ClothingItemTopVariationIndex, 0, _instance.ClothingItemTop.Variations.Length - 1);
                EditorGUILayout.LabelField($"({_instance.ClothingItemTop.Variations[_instance.ClothingItemTopVariationIndex].name})", EditorStyles.whiteMiniLabel);
            }

            if (_instance.ClothingItemBottom != null) {
                _instance.ClothingItemBottomVariationIndex = EditorGUILayout.IntSlider($"{_instance.ClothingItemBottom.Name} Variation", _instance.ClothingItemBottomVariationIndex, 0, _instance.ClothingItemBottom.Variations.Length - 1);
                EditorGUILayout.LabelField($"({_instance.ClothingItemBottom.Variations[_instance.ClothingItemBottomVariationIndex].name})", EditorStyles.whiteMiniLabel);
            }

            if (_instance.ClothingItemGlasses != null) {
                _instance.ClothingItemGlassesVariationIndex = EditorGUILayout.IntSlider($"{_instance.ClothingItemGlasses.Name} Variation", _instance.ClothingItemGlassesVariationIndex, 0, _instance.ClothingItemGlasses.Variations.Length - 1);
                EditorGUILayout.LabelField($"({_instance.ClothingItemGlasses.Variations[_instance.ClothingItemGlassesVariationIndex].name})", EditorStyles.whiteMiniLabel);
            }

            if (_instance.ClothingItemShoes != null) {
                _instance.ClothingItemShoesVariationIndex = EditorGUILayout.IntSlider($"{_instance.ClothingItemShoes.Name} Variation", _instance.ClothingItemShoesVariationIndex, 0, _instance.ClothingItemShoes.Variations.Length - 1);
                EditorGUILayout.LabelField($"({_instance.ClothingItemShoes.Variations[_instance.ClothingItemShoesVariationIndex].name})", EditorStyles.whiteMiniLabel);
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Apply Clothing Items Changes", GUILayout.Height(50))) {
                _instance.UpdateClothing();
            }
        }

        private void DrawOtherSettings_Internal() {
            GUILayout.Space(20);
            AvatarEditorUtilities.LineSeperator();
            EditorGUILayout.LabelField("Other Settings", EditorStyles.whiteLargeLabel);
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_instance.ExpressionChangeSpeed)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_instance.BlinkingInterval)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_instance.BlinkSpeed)));
        }

        private void DrawRandomizeSettings_Internal() {
            GUILayout.Space(20);
            AvatarEditorUtilities.LineSeperator();
            EditorGUILayout.LabelField("Randomize", EditorStyles.whiteLargeLabel);
            GUILayout.Space(10);

            _randomizeIgnoreHeight = EditorGUILayout.Toggle("Ignore Height Randomization", _randomizeIgnoreHeight);
            _randomizeUnifiedHairColor = EditorGUILayout.Toggle("Unified Hair Colors", _randomizeUnifiedHairColor);
            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Randomize Body")) {
                _instance.RandomizeBodyParameters(
                    unifiedHairColors: _randomizeUnifiedHairColor,
                    ignoreHeight: _randomizeIgnoreHeight
                );
            }

            if (GUILayout.Button("Randomize Clothing")) {
                _instance.RandomizeClothing();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

#endif

    public static class AvatarEditorUtilities {

        private static FieldInfo[] _avatarCustomiaztionFields;

        public static void AvatarProperty<T>(AvatarCustomization instance, SerializedObject serializedObject, string variableName, T minValue = default, T maxValue = default) {
            if (_avatarCustomiaztionFields == null) {
                _avatarCustomiaztionFields = typeof(AvatarCustomization).GetFields();
            }

            SerializedProperty serializedProperty = serializedObject.FindProperty(variableName);

            foreach (FieldInfo fieldInfo in _avatarCustomiaztionFields) {
                if (fieldInfo.Name != variableName) {
                    continue;
                }

                AvatarFieldAttribute fieldAttribute = fieldInfo.GetCustomAttribute<AvatarFieldAttribute>();
                if (fieldAttribute != null && fieldAttribute is AvatarFloatFieldAttribute) {
                    AvatarFloatFieldAttribute floatFieldAttribute = (AvatarFloatFieldAttribute) fieldAttribute;
                    RemappedSlider(
                        serializedProperty, 
                        floatFieldAttribute.DisplayName, 
                        floatFieldAttribute.SourceMinValue, 
                        floatFieldAttribute.SourceMaxValue, 
                        floatFieldAttribute.DisplayMinValue, 
                        floatFieldAttribute.DisplayMaxValue
                    );
                }

                if (fieldAttribute != null && fieldAttribute is AvatarIntegerFieldAttribute) {
                    AvatarIntegerFieldAttribute intFieldAttribuet = (AvatarIntegerFieldAttribute) fieldAttribute;
                    if (typeof(T) == typeof(int)) {
                        serializedProperty.intValue = EditorGUILayout.IntSlider(fieldAttribute.DisplayName, serializedProperty.intValue,(int)(object) minValue,(int)(object) maxValue);
                    }
                    else {
                        serializedProperty.intValue = EditorGUILayout.IntField(fieldAttribute.DisplayName, serializedProperty.intValue);
                    }
                }

                if (fieldAttribute != null && fieldAttribute is AvatarColorFieldAttribute) {
                    serializedProperty.colorValue = EditorGUILayout.ColorField(fieldAttribute.DisplayName, serializedProperty.colorValue);
                }
            }
        }
        
        public static void RemappedSlider(SerializedProperty serializedProperty, string label, float sourceLeft, float sourceRight, float displayLeft, float displayRight) {
            float currentValue = serializedProperty.floatValue;
            float remappedValue = Remap(currentValue, sourceLeft, sourceRight, displayLeft, displayRight);
            remappedValue = EditorGUILayout.Slider(label, remappedValue, displayLeft, displayRight);
            serializedProperty.floatValue = Remap(remappedValue, displayLeft, displayRight, sourceLeft, sourceRight);
        }

        public static void LineSeperator(int height = 1) {
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            rect.height = height;
            EditorGUI.DrawRect(rect, new Color(0.4f, 0.4f, 0.4f, 1));
            GUILayout.Space(10);
        }

        public static float Remap(float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

    }
}

