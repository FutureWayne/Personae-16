using System;
using TMPro;
using UnityEditor;
using UnityEditor.Callbacks;

namespace DialogueSystem.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private static Dialogue _selectedDialogue = null;
        
        [MenuItem("Dialogue System/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow<DialogueEditor>("Dialogue Editor");
        }
        
        [OnOpenAsset(1)]
        public static bool OpenDialogue(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue != null)
            {
                ShowEditorWindow();
                _selectedDialogue = dialogue;
                return true;
            }
            return false;
        }

        private void OnGUI()
        {
            if (_selectedDialogue is null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
            }
            else
            {
                foreach (var node in _selectedDialogue.GetAllNodes())
                {
                    var newText = EditorGUILayout.TextField(node.text);
                    var newUniqueID = EditorGUILayout.TextField(node.uniqueID);
                    EditorGUI.BeginChangeCheck();
                    if (EditorGUI.EndChangeCheck()) continue;
                    
                    Undo.RecordObject(_selectedDialogue, "Update Dialogue Text");
                    node.text = newText;
                    node.uniqueID = newUniqueID;
                }
            }
        }

        private void OnSelectionChange()
        {
            Dialogue dialogue = Selection.activeObject as Dialogue;
            if (dialogue != null)
            {
                _selectedDialogue = dialogue;
            }
            else
            {
                _selectedDialogue = null;
            }
            Repaint();
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChange;
        }
    }
}
