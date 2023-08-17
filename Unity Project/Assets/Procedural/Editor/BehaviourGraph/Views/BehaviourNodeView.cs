using Procedural.Scripts.BehaviourGraph;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Procedural.Editor.BehaviourGraph.Views
{
  public class BehaviourNodeView : Node
  {
    public readonly BehaviourNode Node;

    public BehaviourNodeView(BehaviourNode node)
    {
      Node = node;
      title = node.name;
      viewDataKey = node.guid;
      style.left = node.position.x;
      style.top = node.position.y;

      node.BuildNodeView(this);
    }

    public sealed override string title
    {
      get => base.title;
      set => base.title = value;
    }

    // TODO: spawnar na posição do mouse
    public override void SetPosition(Rect newPos)
    {
      base.SetPosition(newPos);
      Node.position.x = newPos.x;
      Node.position.y = newPos.y;
    }
  }
}