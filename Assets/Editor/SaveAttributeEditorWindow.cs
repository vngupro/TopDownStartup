using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using System.Text;

#if UNITY_EDITOR
public class SaveAttributeEditorWindow : EditorWindow
{
    List<SaveAttributeInfo> savedMembers = new List<SaveAttributeInfo>();
    [MenuItem("Tools/SaveAttribute")]
    public static void Open()
    {
        SaveAttributeEditorWindow window = CreateWindow<SaveAttributeEditorWindow>("Save Attribute");
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
                    BindingFlags.NonPublic | 
                    BindingFlags.Static;

                MemberInfo[] members = type.GetFields(flags);
                foreach(MemberInfo member in members) 
                {
                    if(member.CustomAttributes.ToArray().Length > 0)
                    {
                        SaveAttribute attribute = member.GetCustomAttribute<SaveAttribute>();
                        if(attribute != null)
                        {
                            Type reflectedType = member.ReflectedType;
                            object obj = FindObjectOfType(reflectedType);
                            savedMembers.Add(new SaveAttributeInfo(member, attribute, obj));
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

        foreach (SaveAttributeInfo save in savedMembers) 
        {
            MemberInfo memberInfo = save.memberInfo;
            SaveAttribute saveField = save.saveField;
            FieldInfo field = memberInfo as FieldInfo;
            reflectedType = memberInfo.ReflectedType;

            stringBuilder.Clear();
            stringBuilder.Append(reflectedType);
            stringBuilder.Append(" - ");
            stringBuilder.Append(field.Name);
            stringBuilder.Append(" - ");
            stringBuilder.Append(field.GetValue(save.obj).ToString().Trim());

            EditorGUILayout.LabelField(stringBuilder.ToString(), EditorStyles.boldLabel);
        }
    }
}

public struct SaveAttributeInfo
{
    public MemberInfo memberInfo;
    public SaveAttribute saveField;
    public object obj;

    public SaveAttributeInfo(MemberInfo info, SaveAttribute field, object obj)
    {
        memberInfo = info;
        saveField = field;
        this.obj = obj;
    }
}
#endif
