﻿using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEditor;
using UnityEngine;

namespace Framework.AI
{
    public class BehaviourTreeGraphNode : GraphNode
    {
        public BehaviourTreeNode TreeNode { get; private set; }

        public delegate void NewChildNodeCallback(BehaviourTreeGraphNode node, Vector2 offset);

        public static NewChildNodeCallback OnNewChildNode;

        private Color TargetColor;
        private Color CurrentColor;
        
        public override Vector2 GetChildConnectPosition(GraphNode child)
        {
            float offset = 0;

            if (child is BehaviourTreeGraphNode)
            {
                var childGUINode = (BehaviourTreeGraphNode) child;

                var childNodes = TreeNode.AsParentNode().GetChildNodes();
                var childCount = childNodes.Count;

                offset -= ConnectorSize.x * childCount * 0.5f;
                offset += ConnectorSize.x * childNodes.IndexOf(childGUINode.TreeNode);
            }
            
            return new Vector2 (
                drawRect.center.x + offset,
                drawRect.yMax  + ConnectorSize.y * 0.1f
            );
        }

        public override Vector2 GetParentConnectPosition(GraphNode parent)
        {
            return new Vector2 (
                drawRect.center.x,
                drawRect.yMin // - ConnectorSize.y
            );
        }

        public override Color GetParentConnectColor(GraphNode childNode)
        {
            if (BehaviourTreeEditor.GetInstance().ExecuteInRuntime())
                return CurrentColor;

            return Color.white;
        }

        public float GetDesiredWidth()
        {
            return EditorStyles.largeLabel.CalcSize(new GUIContent(Name)).x;
            //    var textRect = GUILayoutUtility.GetRect(new GUIContent(TreeNode.Name), GUI.skin.label);
            //    var childCount = TreeNode.IsParentNode() ? TreeNode.AsParentNode().GetChildNodes().Count : 0;

            //    return Mathf.Max(100, textRect.width, childCount * ConnectorSize.x);
        }

        public BehaviourTreeGraphNode(BehaviourTreeNode node)
        {
            TreeNode = node;
            Size = new Vector2(120, 40);

            Position = TreeNode.EditorPosition;
            Name = TreeNode.Name;

            CurrentColor = GetColor();//Color.white;
            TargetColor = GetColor();//Color.white;
        }

        public override void OnGUI(int id)
        {
            if (TreeNode.IsRootNode())
                DrawBigTitle("Root");
            
            DrawConnectDots(drawRect);
            DrawParentDot(drawRect);

            SetColor();

            drawRect = GUI.Window(id, drawRect, WindowRoutine, GUIContent.none, GUIStyle.none);//, (GUIStyle) "flow node 0");//(GUIStyle)"flow node hex 0");
            position = drawRect.center;

            TreeNode.EditorPosition = Position;
            Position = new Vector2 ( 
                Position.x - (Position.x % 5),
                Position.y - (Position.y % 5)
            );

            GUI.color = Color.white;
        }

        void WindowRoutine(int id)
        {
            SetColor();

            GUILayout.BeginVertical(SpaceEditorStyles.GraphNodeBackground, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    GUI.color = Color.white;
                    GUILayout.Label(new GUIContent(Name, TreeNode.Description), EditorStyles.largeLabel);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
            
            HandleSelection();
            HandleDeletion();

            GUI.DragWindow();
        }

        void HandleSelection()
        {
            if (Event.current.isMouse
            && Event.current.type == EventType.MouseDown
            && Event.current.button == 0
            && Selection.activeObject != TreeNode)
            {
                Selection.activeObject = TreeNode;
            }
        }

        void HandleDeletion()
        {
            if (Event.current.isKey
            && Event.current.type == EventType.KeyDown
            && Event.current.keyCode == KeyCode.Delete)
            {
                GraphNode.toDelete = this;
                Event.current.Use();
            }
        }

        Color GetColor()
        {
            if (BehaviourTreeEditor.GetInstance() != null
            &&  BehaviourTreeEditor.GetInstance().ExecuteInRuntime())
            {
                switch (TreeNode.LastResult)
                {
                    case NodeResult.Success:
                        return Color.green;
                        break;
                    case NodeResult.Failrue:
                        return Color.red;
                        break;
                    case NodeResult.Running:
                        return Color.yellow;
                        break;
                    default:
                        return TargetColor;
                        break;
                }
            }
            else if (TreeNode.IsRootNode())
            {
                return Color.blue + Color.grey;
            }
            else if (TreeNode.IsParentNode())
            {
                return Color.cyan + Color.grey;
            }
            else
            {
                return Color.white;
            }
        }

        void SetColor()
        {
            TargetColor = GetColor();

            if (BehaviourTreeEditor.GetInstance().ExecuteInRuntime())
            {
                CurrentColor = Color.Lerp(CurrentColor, TargetColor, Time.deltaTime);
            }
            else
            {
                CurrentColor = TargetColor;
            }

            GUI.color = CurrentColor;
        }

        void DrawBigTitle(string title)
        {
            GUI.Label (
                new Rect(drawRect.xMin + 40, drawRect.yMin - 20, 200, 30), 
                title, EditorStyles.boldLabel
            );
        }

        void DrawParentDot(Rect dotRect)
        {
            if (TreeNode && TreeNode.Parent)
            {
                GUI.Box (
                   new Rect(dotRect.center.x - ConnectorSize.x * 0.5f, dotRect.yMin - ConnectorSize.y * 0.5f, ConnectorSize.x, ConnectorSize.y),
                   GUIContent.none, SpaceEditorStyles.DotFlowTarget
                );

                GUI.color = CurrentColor;
                GUI.Box (
                   new Rect(dotRect.center.x - ConnectorSize.x * 0.5f, dotRect.yMin - ConnectorSize.y * 0.5f, ConnectorSize.x, ConnectorSize.y),
                   GUIContent.none, SpaceEditorStyles.DotFlowTargetFill
                );

                GUI.color = Color.white;
            }
        }

        void DrawConnectDots(Rect dotRect)
        {
            Vector2 offset = Vector2.zero;

            if (TreeNode.IsParentNode())
            {
                var asParent = TreeNode.AsParentNode();
                var nodeCount = asParent.GetChildNodes().Count;

                if (nodeCount > 0)
                {
                    offset.x -= ConnectorSize.x * nodeCount * 0.5f;

                    for (int i = 0; i < nodeCount; i++)
                    {
                        GUI.Box(
                            new Rect(dotRect.center.x - ConnectorSize.x * 0.5f + offset.x, dotRect.yMax - ConnectorSize.y * 0.25f, ConnectorSize.x, ConnectorSize.y),
                            new GUIContent(), SpaceEditorStyles.DotFlowTarget
                        );

                        GUI.color = CurrentColor;
                        GUI.Box (
                            new Rect(dotRect.center.x - ConnectorSize.x * 0.5f + offset.x, dotRect.yMax - ConnectorSize.y * 0.25f, ConnectorSize.x, ConnectorSize.y),
                            new GUIContent(), SpaceEditorStyles.DotFlowTargetFill
                        );

                        GUI.color = Color.white;

                        offset.x += ConnectorSize.x;
                    }
                }

                if (asParent.HasChildrenSlots())
                {
                    Vector2 pos  = offset + new Vector2(dotRect.center.x - ConnectorSize.x * 0.5f, dotRect.yMax - ConnectorSize.y * 0.25f);
                    Rect    rect = new Rect(pos.x, pos.y, ConnectorSize.x, ConnectorSize.y);

                    if (rect.Contains(Event.current.mousePosition))
                    {
                        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                        {
                            if (OnNewChildNode != null)
                            {
                                OnNewChildNode(this, rect.center);
                                Event.current.Use();
                            }
                        }
                    }

                    GUI.Box(rect, GUIContent.none, SpaceEditorStyles.DotFlowTarget);

                    /*if (GUI.Button(
                        rect,
                        new GUIContent("+")
                    ))
                    {
                        if (OnNewChildNode != null)
                        {
                            OnNewChildNode(this, rect.center);
                        }
                    }*/
                }
            }
        }
    }
}
