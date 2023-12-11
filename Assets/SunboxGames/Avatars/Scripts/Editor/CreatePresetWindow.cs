
using System.IO;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

namespace Sunbox.Avatars {

    public class CreatePresetWindow : EditorWindow {
        
        const string PATH = "Assets/SunboxGames/Avatars/Resources/Presets/";

        public AvatarCustomization Instance;
        
        public string _fileName;
        private bool _fileExists = false;

        public static void ShowWindow(AvatarCustomization instance) {
            CreatePresetWindow window = (CreatePresetWindow) EditorWindow.GetWindow(typeof(CreatePresetWindow));
            window.maxSize = new Vector2(300, 200);
            window.titleContent.text = "Save Avatar Preset";
            window.titleContent.image = Resources.Load<Texture2D>("logo");
            window.Instance = instance;

            if (instance.Preset != null) {
                window._fileName = instance.Preset.name;
            }
            else {
                window._fileName = "avatar_preset_";
            }
            
        }
        
        void OnGUI() {
            GUILayout.Space(40);
            GUILayout.Label("Save Preset?", EditorStyles.whiteLargeLabel);
            
            GUILayout.Space(20);
            _fileName = EditorGUILayout.TextField("File Name", _fileName);

            GUILayout.Space(20);
            if (GUILayout.Button("Save") && !_fileExists) {
                CheckFileExists_Internal();
                if (!_fileExists) {
                    SafeToFile_Internal();
                }
            }

            if (_fileExists) {
                CheckFileExists_Internal();
                if (GUILayout.Button("Override Preset") && _fileExists) {
                    SafeToFile_Internal();
                    CheckFileExists_Internal();
                }
            }
        }

        string GetPath_Internal() => $"{PATH}{_fileName}.txt";

        void CheckFileExists_Internal() => _fileExists = File.Exists(GetPath_Internal());

        void SafeToFile_Internal() {
            StreamWriter writer = new StreamWriter(GetPath_Internal());
            writer.Write(AvatarCustomization.ToConfigString(Instance));
            writer.Close();
            AssetDatabase.ImportAsset(GetPath_Internal());

            this.Close();
        }
    }

}
