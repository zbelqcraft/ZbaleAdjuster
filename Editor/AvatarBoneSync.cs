using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

public static class AvatarBoneSync
{
    // ===============================
    // Entry Point（右クリック）
    // ===============================
    [MenuItem("GameObject/Avatar Bone Sync for MA/Execution", false, 20)]
    private static void Execute(MenuCommand command)
    {
        GameObject start =
            command.context as GameObject
            ?? Selection.activeGameObject;

        if (start == null)
            return;

        // MergeArmature 探索
        Component mergeArmature = FindMergeArmatureDownwards(start.transform);
        if (mergeArmature == null)
        {
            Debug.LogWarning("AvatarBoneSync: ModularAvatar MergeArmature が見つかりません");
            return;
        }

        // コピー元・先確定
        Transform targetRoot = mergeArmature.transform;
        Transform sourceRoot = GetSourceTransformFromMergeArmature(mergeArmature);

        if (sourceRoot == null)
        {
            Debug.LogWarning("AvatarBoneSync: mergeTarget から Transform を取得できません");
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(
            targetRoot.gameObject,
            "AvatarBoneSync for MA Execution"
        );

        CopyRecursive(sourceRoot, targetRoot);

        EditorUtility.SetDirty(targetRoot.gameObject);
        Debug.Log("AvatarBoneSync: Execution 完了");
    }

    [MenuItem("GameObject/Avatar Bone Sync for MA/Execution", true)]
    private static bool Validate()
    {
        return Selection.activeGameObject != null;
    }

    // ===============================
    // MergeArmature 探索（浅い順）
    // ===============================
    private static Component FindMergeArmatureDownwards(Transform root)
    {
        var queue = new Queue<Transform>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Transform current = queue.Dequeue();

            foreach (Component comp in current.GetComponents<Component>())
            {
                if (comp != null && comp.GetType().Name == "ModularAvatarMergeArmature")
                    return comp;
            }

            foreach (Transform child in current)
                queue.Enqueue(child);
        }

        return null;
    }

    // ===============================
    // mergeTarget → Transform 抽出
    // ===============================
    private static Transform GetSourceTransformFromMergeArmature(Component mergeArmature)
    {
        FieldInfo field = mergeArmature.GetType().GetField(
            "mergeTarget",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        );

        if (field == null)
            return null;

        object avatarObjectRef = field.GetValue(mergeArmature);
        if (avatarObjectRef == null)
            return null;

        return FindTransformRecursive(avatarObjectRef, new HashSet<object>());
    }

    // ===============================
    // Transform 再帰探索
    // ===============================
    private static Transform FindTransformRecursive(object obj, HashSet<object> visited)
    {
        if (obj == null || visited.Contains(obj))
            return null;

        visited.Add(obj);

        if (obj is Transform t)
            return t;

        if (obj is GameObject go)
            return go.transform;

        System.Type type = obj.GetType();
        foreach (FieldInfo field in type.GetFields(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            object value;
            try
            {
                value = field.GetValue(obj);
            }
            catch
            {
                continue;
            }

            Transform found = FindTransformRecursive(value, visited);
            if (found != null)
                return found;
        }

        return null;
    }

    // ===============================
    // 再帰コピー本体
    // ===============================
    private static void CopyRecursive(Transform source, Transform target)
    {
        // ルート含め無条件コピー
        target.localPosition = source.localPosition;
        target.localScale = source.localScale;

        CopyScaleAdjuster(source, target);

        // 子は同名対応
        foreach (Transform sourceChild in source)
        {
            Transform targetChild = target.Find(sourceChild.name);
            if (targetChild != null)
            {
                CopyRecursive(sourceChild, targetChild);
            }
        }
    }

    // ===============================
    // ModularAvatar ScaleAdjuster コピー
    // ===============================
    private static void CopyScaleAdjuster(Transform source, Transform target)
    {
        Component sourceAdjuster = null;

        foreach (Component comp in source.GetComponents<Component>())
        {
            if (comp != null && comp.GetType().Name == "ModularAvatarScaleAdjuster")
            {
                sourceAdjuster = comp;
                break;
            }
        }

        if (sourceAdjuster == null)
            return;

        System.Type type = sourceAdjuster.GetType();

        Component existing = target.GetComponent(type);
        if (existing != null)
            Object.DestroyImmediate(existing);

        Component newComp = target.gameObject.AddComponent(type);
        UnityEditorInternal.ComponentUtility.CopyComponent(sourceAdjuster);
        UnityEditorInternal.ComponentUtility.PasteComponentValues(newComp);
    }
}
