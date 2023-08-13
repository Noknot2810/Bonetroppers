using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// ������ �������� ���� ��������� ��� �������� �������
public class ExaminationEditor : EditorWindow
{
    // ������� ������������� ������
    private SerializedObject selectedSerializedObj;
    // ��� �������� �������
    private string currentName;
    // ������ ���� ������������� ��������
    private List<SerializedObject> allSerializedObj;
    // ���������� ���� ������������� �������� �������
    private bool deleteAccept = false;
    // ������� �������� ��� ���������
    private Vector2 scrollPosition = Vector2.zero;

    // ������� ����������� ���� ���������
    [MenuItem("Window/Examination Editor")]
    public static void ShowWindow()
    {
        GetWindow<ExaminationEditor>("Examination Editor");
    }

    // ������� ����������� ��� ������� �������
    private void Awake()
    {
        // ������������� ������ ������������� ��������
        allSerializedObj = new List<SerializedObject>();
    }

    // ������� ������ ���� ���������
    private void OnGUI()
    {
        // ������� ������ ������������� ��������
        allSerializedObj.Clear();
        // �������� ���� �������� ��������
        var allDialogues = Resources.LoadAll<Dialogue>("");
        // ���������� ������ ������������� �������� �� ����� ��������
        foreach (Dialogue dialogue in allDialogues)
        {
            SerializedObject obj = new SerializedObject(dialogue);
            allSerializedObj.Add(obj);
        }

        EditorGUILayout.BeginHorizontal();

        // ������� ���� ������ ������ ������� ��� ��������������
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        
        for (int i = 0; i < allSerializedObj.Count; i++)
        {
            if (GUILayout.Button(allSerializedObj[i].targetObject.name))
            {
                selectedSerializedObj = allSerializedObj[i];
                currentName = allSerializedObj[i].targetObject.name;

                deleteAccept = false;
                GUI.FocusControl("DeleteButton");
            }
        }
        // ������� ������ �������� ������ ������� "Zzz"
        if (GUILayout.Button("+"))
        {
            Dialogue asset = ScriptableObject.CreateInstance<Dialogue>();
            AssetDatabase.CreateAsset(asset, "Assets/Resources/Zzz.asset");
            AssetDatabase.SaveAssets();

            deleteAccept = false;
        }
        EditorGUILayout.EndVertical();

        // ������� ���� ��� �������������� ���������� �������
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        // ��������� ����������
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

        // ���������� �������, ��� ������������� ������ �� �������
        if (selectedSerializedObj != null)
        {
            // ��������� ������������� ������� ������� �������
            SerializedProperty isTask = selectedSerializedObj.FindProperty("_isTask");
            SerializedProperty text = selectedSerializedObj.FindProperty("_text");
            SerializedProperty level = selectedSerializedObj.FindProperty("_level");
            SerializedProperty questions = selectedSerializedObj.FindProperty("_questions");

            // �������� ��������� ����� �������
            EditorGUILayout.BeginHorizontal();
            currentName = EditorGUILayout.TextField(currentName);
            if (GUILayout.Button("Apply", GUILayout.MaxWidth(150)))
            {
                if (currentName != selectedSerializedObj.targetObject.name)
                {
                    var path = string.Format("Assets/Resources/{0}.asset", 
                        selectedSerializedObj.targetObject.name);
                    _ = AssetDatabase.RenameAsset(path, currentName);
                }
            }
            EditorGUILayout.EndHorizontal();

            // ���������� � ���� ����� ��� �������������� ������� �������
            // �������
            if (isTask.boolValue == true)
            {
                EditorGUILayout.PropertyField(isTask, true);
                EditorGUILayout.PropertyField(level, true);
                EditorGUILayout.PropertyField(questions, true);
            }
            else
            {
                EditorGUILayout.PropertyField(isTask, true);
                EditorGUILayout.PropertyField(text, true);           
            }
            // ���������� ���� ���������, ������������ � ����� ����
            selectedSerializedObj.ApplyModifiedProperties();

            // �������� ������ �������� ������� ������� � ������
            // ������������� ��� ������������ ��������
            GUILayout.FlexibleSpace();
            if (deleteAccept == false)
            {
                GUI.SetNextControlName("DeleteButton");
                if (GUILayout.Button("Delete", GUILayout.MaxWidth(150)))
                {
                    deleteAccept = true;
                }
            }
            else
            {
                GUILayout.Label("Are you sure?");
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Yes", GUILayout.MaxWidth(100)))
                {
                    var path = string.Format("Assets/Resources/{0}.asset", 
                        selectedSerializedObj.targetObject.name);
                    _ = AssetDatabase.DeleteAsset(path);
                    selectedSerializedObj = null;
                }
                if (GUILayout.Button("No", GUILayout.MaxWidth(100)))
                {
                    deleteAccept = false;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            EditorGUILayout.LabelField("Choose any dialogue to edit...");
        }

        GUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}
