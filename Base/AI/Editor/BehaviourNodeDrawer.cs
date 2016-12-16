﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.AI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BehaviourTreeNode), true)]
public class BehaviourNodeDrawer : Editor
{
    public BehaviourTreeNode Node
    {
        get { return (BehaviourTreeNode)target; }
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Description", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(Node.Description);

        EditorGUILayout.Space();
        GUILayout.Label("Parameters", EditorStyles.boldLabel);

        DrawDefaultInspector();
    }
}
