using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Скрипт создания окна редактора для объектов диалога
public class ExaminationEditor : EditorWindow
{
    // Текущий сериализуемый объект
    private SerializedObject selectedSerializedObj;
    // Имя текущего объекта
    private string currentName;
    // Список всех сериализуемых объектов
    private List<SerializedObject> allSerializedObj;
    // Переменная флаг подтверждения удаления объекта
    private bool deleteAccept = false;
    // Позиция ползунка для скролинга
    private Vector2 scrollPosition = Vector2.zero;

    // Функция отображения окна редактора
    [MenuItem("Window/Examination Editor")]
    public static void ShowWindow()
    {
        GetWindow<ExaminationEditor>("Examination Editor");
    }

    // Функция запускаемая при запуске скрипта
    private void Awake()
    {
        // Инициализация списка сериализуемых объектов
        allSerializedObj = new List<SerializedObject>();
    }

    // Функция работы окна редактора
    private void OnGUI()
    {
        // Очистка списка сериализуемых объектов
        allSerializedObj.Clear();
        // Загрузка всех объектов диалогов
        var allDialogues = Resources.LoadAll<Dialogue>("");
        // Заполнение списка сериализуемых объектов из числа диалогов
        foreach (Dialogue dialogue in allDialogues)
        {
            SerializedObject obj = new SerializedObject(dialogue);
            allSerializedObj.Add(obj);
        }

        EditorGUILayout.BeginHorizontal();

        // Задание меню кнопок выбора диалога для редактирования
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
        // Задание кнопки создания нового диалога "Zzz"
        if (GUILayout.Button("+"))
        {
            Dialogue asset = ScriptableObject.CreateInstance<Dialogue>();
            AssetDatabase.CreateAsset(asset, "Assets/Resources/Zzz.asset");
            AssetDatabase.SaveAssets();

            deleteAccept = false;
        }
        EditorGUILayout.EndVertical();

        // Задание меню для редактирования выбранного диалога
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        // Настройка скроллинга
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

        // Поставлено условие, что сериализуемый объект не нулевой
        if (selectedSerializedObj != null)
        {
            // Получение сериализуемых свойств объекта диалога
            SerializedProperty isTask = selectedSerializedObj.FindProperty("_isTask");
            SerializedProperty text = selectedSerializedObj.FindProperty("_text");
            SerializedProperty level = selectedSerializedObj.FindProperty("_level");
            SerializedProperty questions = selectedSerializedObj.FindProperty("_questions");

            // Создание настройки имени диалога
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

            // Добавление в меня полей для редактирования свойств объекта
            // диалога
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
            // Сохранение всех изменений, произведённых в полях выше
            selectedSerializedObj.ApplyModifiedProperties();

            // Создание кнопки удаления объекта диалога и кнопок
            // подтверждения или опровержения удаления
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
