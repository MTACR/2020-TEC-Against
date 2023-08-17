using Procedural.Editor.BehaviourGraph.Views;
using Procedural.Scripts.WaveFunction;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Procedural.Editor.BehaviourGraph.WaveFunction
{
  public class WaveEditor : EditorWindow
  {
    private const string Path = "Assets/Procedural/Editor/BehaviourGraph/WaveFunction";

    [SerializeField] private VisualTreeAsset visualTreeAsset;

    private InspectorView _inspectorView;
    private BehaviourTreeView _behaviourTreeView;

    [MenuItem("Window/UI Toolkit/WaveEditor")]
    private static void OpenWindow()
    {
      var wnd = GetWindow<WaveEditor>();
      wnd.titleContent = new GUIContent("WaveEditor");
    }

    private void CreateGUI()
    {
      var root = rootVisualElement;

      var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{Path}/WaveEditor.uxml");
      visualTree.CloneTree(root);

      var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{Path}/WaveEditor.uss");
      root.styleSheets.Add(styleSheet);

      _behaviourTreeView = root.Q<BehaviourTreeView>();
      _inspectorView = root.Q<InspectorView>();

      OnSelectionChange();
    }

    private void OnSelectionChange()
    {
      if (Selection.activeObject is WaveTree tree)
      {
        _behaviourTreeView.PopulateView(tree);
      }
      else
      {
        _behaviourTreeView.ClearView();
      }
    }
  }
}