using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private static Dialogue _selectedDialogue;
        
        [NonSerialized]
        private static GUIStyle _nodeStyle;
        [NonSerialized]
        private static GUIStyle _playerNodeStyle;
        [NonSerialized]
        private DialogueNode _draggingNode;
        [NonSerialized]
        private DialogueNode _creatingNode;
        [NonSerialized]
        private DialogueNode _deletingNode;
        [NonSerialized]
        private DialogueNode _linkingParentNode;
        [NonSerialized]
        private Vector2 _scrollPosition = Vector2.zero;
        
        private const float CanvasSize = 4000;
        private const float BackgroundSize = 50;
        
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow<DialogueEditor>("Dialogue Editor");
        }
        
        [OnOpenAsset(1)]
        // Show up the editor window when a dialogue is selected
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

        // Update is called once per frame
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
                
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                var canvas = GUILayoutUtility.GetRect(CanvasSize, CanvasSize);
                Texture2D backgroundTex = Resources.Load("background") as Texture2D;
                Rect texCoords = new Rect(0, 0, CanvasSize / BackgroundSize, CanvasSize / BackgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);
                
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
                
                EditorGUILayout.EndScrollView();

                // Create and delete nodes
                if (_creatingNode != null)
                {
                    _selectedDialogue.CreateNode(_creatingNode);
                    _creatingNode = null;
                }
                
                if (_deletingNode != null)
                {
                    _selectedDialogue.DeleteNode(_deletingNode);
                    _deletingNode = null;
                }
            }
        }

        private void DrawNodeConnection(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
            foreach (DialogueNode childNode in _selectedDialogue.GetAllChildrenNodes(node))
            {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
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
                case EventType.MouseDown when _draggingNode is null:
                    _draggingNode = GetNodeAtPoint(e.mousePosition + _scrollPosition);
                    if (_draggingNode is not null)
                    {
                        Selection.activeObject = _draggingNode;
                    }
                    else
                    {
                        Selection.activeObject = _selectedDialogue;
                    }
                    break;
                case EventType.MouseDrag:
                    if (_draggingNode is not null && e.button == 0)
                    {
                        var rect = _draggingNode.GetRect();
                        rect.position += e.delta;
                        _draggingNode.SetPosition(rect.position);
                        Repaint();
                    }
                    
                    else if( e.isMouse && e.button == 1)
                    {
                        _scrollPosition -= e.delta;
                        Repaint();
                    }
                    break;
                case EventType.MouseUp when _draggingNode is not null:
                    _draggingNode = null;
                    break;
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 eMousePosition)
        {
            DialogueNode foundNode = null;
            foreach (var node in _selectedDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(eMousePosition))
                {
                    foundNode = node;
                }
            }

            return foundNode;
        }

        private void DrawNode(DialogueNode node)
        {
            var style = _nodeStyle;
            if (node.IsPlayerSpeaking())
            {
                style = _playerNodeStyle;
            }
            GUILayout.BeginArea(node.GetRect(), style);
            
            node.SetText(EditorGUILayout.TextField(node.GetText()));

            // add buttons
            GUILayout.BeginHorizontal();
            DrawButtons(node);
            GUILayout.EndHorizontal();
            
            GUILayout.EndArea();
        }

        private void DrawButtons(DialogueNode node)
        {
            // Add Button
            if (GUILayout.Button("+"))
            {
                _creatingNode = node;
            }

            
            // Link Button
            if (_linkingParentNode is null)
            {
                if (GUILayout.Button("Link"))
                {
                    _linkingParentNode = node;
                }
            }
            
            else if (_linkingParentNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    _linkingParentNode = null;
                }
            }
            
            else if (_linkingParentNode.GetChildren().Contains(node.name))
            {
                if(GUILayout.Button("Unlink"))
                {
                    _linkingParentNode.RemoveChild(node.name);
                }
            }
            
            else
            {
                if (GUILayout.Button("Child"))
                {
                    _linkingParentNode.AddChild(node.name);
                }
            }

            // Delete Button
            if (GUILayout.Button("-"))
            {
                _deletingNode = node;
            }
        }

        private void OnSelectionChange()
        {
            Dialogue dialogue = Selection.activeObject as Dialogue;
            if (dialogue == null) return;
            _selectedDialogue = dialogue;
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
            
            _playerNodeStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("node1") as Texture2D,
                    textColor = Color.white
                },
                padding = new RectOffset(20, 20, 20, 20),
                border = new RectOffset(12, 12, 12, 12)
            };
        }
    }
}
