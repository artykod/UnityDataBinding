using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class MyCustomEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset _visualTreeAsset;

    [SerializeField]
    private VisualTreeAsset _itemVisualTreeAsset;

    [MenuItem("Test/MyCustomEditor")]
    public static void ShowExample()
    {
        var wnd = GetWindow<MyCustomEditor>();

        wnd.titleContent = new GUIContent("MyCustomEditor");
    }

    public void CreateGUI()
    {
        var root = rootVisualElement;

        root.Add(_visualTreeAsset.Instantiate());

        root.Q<Button>("button2").RegisterCallback<ClickEvent>(OnClick);

        var list = root.Q<ListView>("list");

        list.makeItem = MakeItem;
        list.bindItem = BindItem;
        list.itemsSource = new List<string>
        {
            "item 1",
            "item 2",
            "item 3",
            "item 1",
            "item 2",
            "item 3",
            "item 1",
            "item 2",
            "item 3",
            "item 1",
            "item 2",
            "item 3",
            "item 1",
            "item 2",
            "item 3",
            "item 1",
            "item 2",
            "item 3",
            "item 1",
            "item 2",
            "item 3",
            "item 1",
            "item 2",
            "item 3",
            "item 1",
            "item 2",
            "item 3",
            "item 1",
            "item 2",
            "item 3",
        };

        VisualElement MakeItem()
        {
            Debug.Log("Make item");

            return _itemVisualTreeAsset.Instantiate();
        }

        void BindItem(VisualElement element, int idx)
        {
            var prevIdx = element.userData != null ? (int)element.userData : -1;

            Debug.Log($"Bind from {prevIdx} to {idx}");

            var title = element.Q<Label>();

            title.text = $"item #{idx}";

            element.userData = idx;
        }
    }

    private void OnClick(ClickEvent e)
    {
        Debug.Log(e.target);
    }
}