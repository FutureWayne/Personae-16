using System;
using TMPro;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private static Dialogue _selectedDialogue = null;
        
        private static GUIStyle _nodeStyle;
        
        private DialogueNode _draggingNode = null;
        
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
                ProcessEvents(Event.current);
                foreach (var node in _selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                    DrawNodeConnection(node);
                }
            }
        }

        private void DrawNodeConnection(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.rect.xMax, node.rect.center.y);
            foreach (DialogueNode childNode in _selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.rect.xMin, childNode.rect.center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(startPosition, endPosition, startPosition + controlPointOffset,
                    endPosition - controlPointOffset, Color.white, null, 4f);
            }
        }

        private void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown when _draggingNode == null:
                    _draggingNode = GetNodeAtPoint(e.mousePosition);
                    break;
                case EventType.MouseDrag when _draggingNode != null:
                    Undo.RecordObject(_selectedDialogue, "Move Dialogue Node");
                    _draggingNode.rect.position += e.delta;
                    Repaint();
                    break;
                case EventType.MouseUp when _draggingNode != null:
                    _draggingNode = null;
                    break;
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 eMousePosition)
        {
            DialogueNode foundNode = null;
            foreach (var node in _selectedDialogue.GetAllNodes())
            {
                if (node.rect.Contains(eMousePosition))
                {
                    foundNode = node;
                }
            }

            return foundNode;
        }

        private static void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.rect, _nodeStyle);
            EditorGUILayout.LabelField("Node ID: " + node.uniqueID, EditorStyles.whiteLargeLabel);
            var newText = EditorGUILayout.TextField(node.text);
            var newUniqueID = EditorGUILayout.TextField(node.uniqueID);
            EditorGUI.BeginChangeCheck();
            if (EditorGUI.EndChangeCheck()) return;

            Undo.RecordObject(_selectedDialogue, "Update Dialogue Text");
            node.text = newText;
            node.uniqueID = newUniqueID;
            
            GUILayout.EndArea();
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
            
            _nodeStyle = new GUIStyle();
            _nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            _nodeStyle.normal.textColor = Color.white;
            _nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            _nodeStyle.border = new RectOffset(12, 12, 12, 12);
        }
    }
}
