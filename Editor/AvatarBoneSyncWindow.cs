using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class AvatarBoneSyncWindow : EditorWindow
{
    // ===============================
    // Mode
    // ===============================
    private enum Mode
    {
        Simple,
        Custom
    }

    private Mode currentMode = Mode.Simple;

    // ===============================
    // Scroll
    // ===============================
    private Vector2 scroll;

    // ===============================
    // Source / Target
    // ===============================
    private Transform sourceRoot;
    private Transform targetRoot;

    // ===============================
    // Transform Copy Flags
    // ===============================
    private bool copyPosition = true;
    private bool copyRotation = false; // ★追加
    private bool copyScale = true;

    // ===============================
    // Component Copy Flags
    // key : FullName
    // ===============================
    private readonly Dictionary<string, bool> componentCopyFlags
        = new Dictionary<string, bool>();

    private bool hasScaleAdjuster;

    // ===============================
    // Window
    // ===============================
    [MenuItem("Tools/Avatar Bone Sync for MA")]
    public static void Open()
    {
        GetWindow<AvatarBoneSyncWindow>("Avatar Bone Sync for MA");
    }

    // ===============================
    // GUI
    // ===============================
    private void OnGUI()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        DrawModeSelector();
        EditorGUILayout.Space(8);

        DrawSourceField();
        EditorGUILayout.Space(8);

        DrawDefaultButton();
        EditorGUILayout.Space(8);

        if (currentMode == Mode.Custom)
        {
            DrawTransformOptions();
            EditorGUILayout.Space(8);
            DrawComponentSelector();
            EditorGUILayout.Space(8);
        }

        DrawTargetField();   // ★ 実行ボタン直前へ移動
        EditorGUILayout.Space(12);

        DrawExecuteButton();

        EditorGUILayout.EndScrollView();
    }

    // ===============================
    // Mode Selector
    // ===============================
    private void DrawModeSelector()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Toggle(currentMode == Mode.Simple, "シンプル", EditorStyles.toolbarButton))
            currentMode = Mode.Simple;
        if (GUILayout.Toggle(currentMode == Mode.Custom, "カスタム", EditorStyles.toolbarButton))
            currentMode = Mode.Custom;
        EditorGUILayout.EndHorizontal();
    }

    // ===============================
    // Source
    // ===============================
    private void DrawSourceField()
    {
        EditorGUILayout.LabelField("① コピー元を指定する", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "コピーしたいルートボーンを選択するか、D&Dしてください。",
            MessageType.Info
        );

        EditorGUI.BeginChangeCheck();
        sourceRoot = (Transform)EditorGUILayout.ObjectField(
            sourceRoot,
            typeof(Transform),
            true
        );
        if (EditorGUI.EndChangeCheck())
        {
            RefreshSourceComponents();
        }
    }

    // ===============================
    // Default Button
    // ===============================
    private void DrawDefaultButton()
    {
        if (GUILayout.Button("デフォルト設定"))
        {
            copyPosition = true;
            copyRotation = false;
            copyScale = true;

            foreach (var key in componentCopyFlags.Keys.ToList())
            {
                componentCopyFlags[key] =
                    key.Contains("ModularAvatarScaleAdjuster");
            }
        }
    }

    // ===============================
    // Transform Options
    // ===============================
    private void DrawTransformOptions()
    {
        EditorGUILayout.LabelField("Transform コピー設定", EditorStyles.boldLabel);
        copyPosition = EditorGUILayout.ToggleLeft("Position", copyPosition);
        copyRotation = EditorGUILayout.ToggleLeft("Rotation", copyRotation); // ★追加
        copyScale = EditorGUILayout.ToggleLeft("Scale", copyScale);
    }

    // ===============================
    // Component Selector
    // ===============================
    private void DrawComponentSelector()
    {
        EditorGUILayout.LabelField("・コピーするコンポーネント", EditorStyles.boldLabel);

        if (!hasScaleAdjuster && componentCopyFlags.Count == 0)
        {
            EditorGUILayout.HelpBox(
                "コピー元に Transform 以外の有効なコンポーネントが見つかりません。",
                MessageType.Info
            );
            return;
        }

        DrawComponentGroup("VRC", s => s.StartsWith("VRC"));
        DrawComponentGroup("ModularAvatar", s => s.Contains("ModularAvatar"));
        DrawComponentGroup("Others", s =>
            !s.StartsWith("VRC") && !s.Contains("ModularAvatar"));
    }

    private void DrawComponentGroup(string title, System.Func<string, bool> filter)
    {
        var keys = componentCopyFlags.Keys.Where(filter).ToList();
        if (keys.Count == 0)
            return;

        EditorGUILayout.LabelField(title, EditorStyles.miniBoldLabel);

        foreach (var fullName in keys)
        {
            string label = fullName.Split('.').Last(); // ★ 表示名を短縮

            componentCopyFlags[fullName] = EditorGUILayout.ToggleLeft(
                label,
                componentCopyFlags[fullName]
            );
        }
    }

    // ===============================
    // Target
    // ===============================
    private void DrawTargetField()
    {
        EditorGUILayout.LabelField("② コピー先を指定する", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "コピー元と同名構造のボーンを持つ Transform を選択するか、D&Dしてください。",
            MessageType.Info
        );

        targetRoot = (Transform)EditorGUILayout.ObjectField(
            targetRoot,
            typeof(Transform),
            true
        );
    }

    // ===============================
    // Execute
    // ===============================
    private void DrawExecuteButton()
    {
        GUI.enabled = sourceRoot && targetRoot;

        if (GUILayout.Button("コピー実行"))
        {
            Undo.RegisterFullObjectHierarchyUndo(
                targetRoot.gameObject,
                "AvatarBoneSync Copy"
            );

            CopyRecursive(sourceRoot, targetRoot);
            EditorUtility.SetDirty(targetRoot.gameObject);
        }

        GUI.enabled = true;
    }

    // ===============================
    // Detection
    // ===============================
    private void RefreshSourceComponents()
    {
        componentCopyFlags.Clear();
        hasScaleAdjuster = false;

        if (!sourceRoot)
            return;

        foreach (var t in sourceRoot.GetComponentsInChildren<Transform>(true))
        {
            foreach (var c in t.GetComponents<Component>())
            {
                if (c == null || c is Transform)
                    continue;

                string typeName = c.GetType().FullName;

                // Constraint 系完全除外
                if (typeName.Contains("VRCConstraint")
                    || typeName.Contains("VRCRotationConstraint"))
                    continue;

                if (!componentCopyFlags.ContainsKey(typeName))
                {
                    bool defaultOn =
                        typeName.Contains("ModularAvatarScaleAdjuster");

                    componentCopyFlags[typeName] = defaultOn;

                    if (defaultOn)
                        hasScaleAdjuster = true;
                }
            }
        }
    }

    // ===============================
    // Copy Logic
    // ===============================
    private void CopyRecursive(Transform src, Transform dst)
    {
        if (copyPosition)
            dst.localPosition = src.localPosition;

        if (copyRotation)
            dst.localRotation = src.localRotation;

        if (copyScale)
            dst.localScale = src.localScale;

        CopyComponents(src, dst);

        foreach (Transform srcChild in src)
        {
            var dstChild = dst.Find(srcChild.name);
            if (dstChild)
                CopyRecursive(srcChild, dstChild);
        }
    }

    private void CopyComponents(Transform src, Transform dst)
    {
        foreach (var c in src.GetComponents<Component>())
        {
            if (c == null || c is Transform)
                continue;

            string typeName = c.GetType().FullName;

            if (!componentCopyFlags.TryGetValue(typeName, out bool enabled))
                continue;

            if (!enabled)
                continue;

            System.Type type = c.GetType();

            var existing = dst.GetComponent(type);
            if (existing)
                DestroyImmediate(existing);

            var newComp = dst.gameObject.AddComponent(type);
            UnityEditorInternal.ComponentUtility.CopyComponent(c);
            UnityEditorInternal.ComponentUtility.PasteComponentValues(newComp);
        }
    }
}
