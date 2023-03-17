using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using System.Text;

public class SaveFieldEditorWindow : EditorWindow
{
    List<SaveFieldInfo> savedMembers = new List<SaveFieldInfo>();
    [MenuItem("Tools/SaveField")]
    public static void Open()
    {
        SaveFieldEditorWindow window = CreateWindow<SaveFieldEditorWindow>("Save Field");
    }

    private void OnEnable()
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach(Assembly assembly in assemblies) 
        {
            
            Type[] types = assembly.GetTypes();

            foreach(Type type in types)
            {
                BindingFlags flags =
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.NonPublic;

                MemberInfo[] members = type.GetFields(flags);
                foreach(MemberInfo member in members) 
                {
                    if(member.CustomAttributes.ToArray().Length > 0)
                    {
                        SaveField attribute = member.GetCustomAttribute<SaveField>();
                        if(attribute != null)
                        {
                            Type reflectedType = member.ReflectedType;
                            object obj = FindObjectOfType(reflectedType);
                            savedMembers.Add(new SaveFieldInfo(member, attribute, obj));
                        }
                    }
                }
            }
        }
    }

    public void OnGUI()
    {
        EditorGUILayout.LabelField("Saved Properties", EditorStyles.boldLabel);
        StringBuilder stringBuilder = new StringBuilder();
        Type reflectedType = null;

        foreach (SaveFieldInfo save in savedMembers) 
        {
            MemberInfo memberInfo = save.memberInfo;
            SaveField saveField = save.saveField;
            FieldInfo field = memberInfo as FieldInfo;
            reflectedType = memberInfo.ReflectedType;

            stringBuilder.Clear();
            stringBuilder.Append(field.GetValue(save.obj).ToString().Trim());
            stringBuilder.Append(" - ");
            stringBuilder.Append(reflectedType);

            EditorGUILayout.LabelField(stringBuilder.ToString(), EditorStyles.boldLabel);
        }
    }
}

public struct SaveFieldInfo
{
    public MemberInfo memberInfo;
    public SaveField saveField;
    public object obj;

    public SaveFieldInfo(MemberInfo info, SaveField field, object obj)
    {
        memberInfo = info;
        saveField = field;
        this.obj = obj;
    }
}
