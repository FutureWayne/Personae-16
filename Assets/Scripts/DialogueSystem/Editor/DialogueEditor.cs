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
        
        [NonSerialized]
        private static GUIStyle _nodeStyle;
        [NonSerialized]
        private DialogueNode _draggingNode = null;
        [NonSerialized]
        private DialogueNode _creatingNode = null;
        [NonSerialized]
        private DialogueNode _deletingNode = null;
        [NonSerialized]
        private DialogueNode _linkingParentNode = null;
        
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
                // Process GUI events
                ProcessEvents(Event.current);
                
                // Draw the connections first so they appear behind the nodes
                foreach (var node in _selectedDialogue.GetAllNodes())
                {
                    DrawNodeConnection(node);
                }
                
                // Draw the nodes
                foreach (var node in _selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }

                // Create and delete nodes
                if (_creatingNode != null)
                {
                    Undo.RecordObject(_selectedDialogue, "Add Dialogue Node");
                    _selectedDialogue.CreateNode(_creatingNode);
                    _creatingNode = null;
                }
                
                if (_deletingNode != null)
                {
                    Undo.RecordObject(_selectedDialogue, "Delete Dialogue Node");
                    _selectedDialogue.DeleteNode(_deletingNode);
                    _deletingNode = null;
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

        private void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.rect, _nodeStyle);
            
            EditorGUI.BeginChangeCheck();
            var newText = EditorGUILayout.TextField(node.text);
            
            // if the text has changed, record the change
            if (!EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_selectedDialogue, "Update Dialogue Text");
                node.text = newText;
            };
            
            // add buttons to add and delete nodes
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("Add Child"))
            {
                _creatingNode = node;
            }

            if (_linkingParentNode == null)
            {
                if( GUILayout.Button("Link"))
                {
                    _linkingParentNode = node;
                }
            }
            else
            {
                if (GUILayout.Button("Child"))
                {
                    Undo.RecordObject(_selectedDialogue, "Add Dialogue Link");
                    _linkingParentNode.children.Add(node.uniqueID);
                    _linkingParentNode = null;
                }
            }
            
            if(GUILayout.Button("Delete Node"))
            {
                _deletingNode = node;
            }
            
            GUILayout.EndHorizontal();
            
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
            
            _nodeStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("node0") as Texture2D,
                    textColor = Color.white
                },
                padding = new RectOffset(20, 20, 20, 20),
                border = new RectOffset(12, 12, 12, 12)
            };
        }
    }
}
