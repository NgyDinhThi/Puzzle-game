using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;




[CustomEditor(typeof(Shapedata), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class ShapeDataDrawer : Editor
{
    private Shapedata ShapedataInstance => target as Shapedata;
    public override void OnInspectorGUI()
    {
       serializedObject.Update();
        ClearBoardButton();
        EditorGUILayout.Space();

        DrawColumnsInputFields();
        EditorGUILayout.Space();

        if (ShapedataInstance.board != null && ShapedataInstance.columns > 0 && ShapedataInstance.rows > 0)
        {
            DrawBoardTable();
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(ShapedataInstance);
        }
    }

    private void ClearBoardButton()
    {
        if (GUILayout.Button("Clear Board"))
        {
            ShapedataInstance.Clear();
        }

    }    

    private void DrawColumnsInputFields()
    {
        var columnTemp = ShapedataInstance.columns;
        var rowTemp = ShapedataInstance.rows;

        ShapedataInstance.columns = EditorGUILayout.IntField("Columns", ShapedataInstance.columns);
        ShapedataInstance.rows = EditorGUILayout.IntField("Rows", ShapedataInstance.rows);

        if ((ShapedataInstance.columns != columnTemp || ShapedataInstance.rows != rowTemp) && ShapedataInstance.columns > 0 && ShapedataInstance.rows > 0)
        {
            ShapedataInstance.CreatNewBoard();
        }
    }    

    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnstyle = new GUIStyle();
        headerColumnstyle.fixedWidth = 65;
        headerColumnstyle.alignment = TextAnchor.MiddleCenter;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldStyle.normal.background = Texture2D.grayTexture;
        dataFieldStyle.onNormal.background = Texture2D.whiteTexture;

        for (var row = 0; row < ShapedataInstance.rows; row++)
        {
            EditorGUILayout.BeginHorizontal(headerColumnstyle);
            for ( var column = 0; column < ShapedataInstance.columns; column++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(ShapedataInstance.board[row].column[column], dataFieldStyle);
                ShapedataInstance.board[row].column[column] = data;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();
        }
    }    

}

