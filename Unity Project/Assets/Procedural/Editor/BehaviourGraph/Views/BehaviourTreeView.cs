using System;
using System.Collections.Generic;
using System.Linq;
using Procedural.Scripts.BehaviourGraph;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Procedural.Editor.BehaviourGraph.Views
{
  public class BehaviourTreeView : GraphView
  {
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits>
    {
    }

    private const string Path = "Assets/Procedural/Editor/BehaviourGraph/WaveFunction";
    private BehaviourTree _tree;

    public BehaviourTreeView()
    {
      Insert(0, new GridBackground());

      this.AddManipulator(new ContentZoomer());
      this.AddManipulator(new ContentDragger());
      this.AddManipulator(new SelectionDragger());
      this.AddManipulator(new RectangleSelector());

      var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{Path}/WaveEditor.uss");
      styleSheets.Add(styleSheet);
    }

    internal void PopulateView(BehaviourTree tree)
    {
      _tree = tree;

      graphViewChanged -= OnGraphViewChange;
      DeleteElements(graphElements);
      graphViewChanged += OnGraphViewChange;

      _tree.nodes.ForEach(CreateNodeView);
      _tree.nodes.ForEach(CreateEdgeView);
    }

    internal void ClearView()
    {
      graphViewChanged -= OnGraphViewChange;
      _tree = null;
      DeleteElements(graphElements);
    }

    private void CreateNodeView(BehaviourNode node)
    {
      var view = new BehaviourNodeView(node);
      AddElement(view);
    }

    private void CreateEdgeView(BehaviourNode node)
    {
      var relations = _tree.GetRelations(node);
      var a = FindNodeView(node);

      foreach (var (relation, list) in relations)
      {
        foreach (var n in list)
        {
          var b = FindNodeView(n);
          var edge = a.Node.ConnectNodeView(relation, b.Node);
          AddElement(edge);
        }
      }
    }

    private BehaviourNodeView FindNodeView(BehaviourNode node)
    {
      return (BehaviourNodeView)GetNodeByGuid(node.guid);
    }

    private void CreateNode(Type type)
    {
      var node = _tree.CreateNode(type);
      CreateNodeView(node);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
      var tree = Selection.activeObject as BehaviourTree;

      if (tree)
      {
        var type = tree.GetNodeType();
        evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", a => CreateNode(type));

        // var types = TypeCache.GetTypesDerivedFrom(tree.GetNodeType());
        //
        // foreach (var type in types)
        // {
        //   evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", a => CreateNode(type));
        // }
      }

      base.BuildContextualMenu(evt);
    }

    // TODO: deixar essa regra genérica
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
      // return base.GetCompatiblePorts(startPort, nodeAdapter);
      return ports
        .ToList()
        .Where(endPort => endPort.direction != startPort.direction
                          && endPort.orientation == startPort.orientation)
        .ToList();
    }

    private GraphViewChange OnGraphViewChange(GraphViewChange graphViewChange)
    {
      if (graphViewChange.elementsToRemove != null)
      {
        foreach (var element in graphViewChange.elementsToRemove)
        {
          if (element is BehaviourNodeView nodeView)
          {
            _tree.DeleteNode(nodeView.Node);
          }

          if (element is Edge edge)
          {
            if (edge.input.node is BehaviourNodeView a
                && edge.output.node is BehaviourNodeView b)
            {
              var relation = _tree.GetRelation(edge);
              _tree.RemoveRelation(relation, a.Node, b.Node);
            }
          }
          
          AssetDatabase.SaveAssets();
        }
      }

      if (graphViewChange.edgesToCreate != null)
      {
        foreach (var edge in graphViewChange.edgesToCreate)
        {
          if (edge.input.node is BehaviourNodeView a
              && edge.output.node is BehaviourNodeView b)
          {
            var relation = _tree.GetRelation(edge);
            _tree.AddRelation(relation, a.Node, b.Node);
          }
        }
        
        AssetDatabase.SaveAssets();
      }

      return graphViewChange;
    }
  }
}