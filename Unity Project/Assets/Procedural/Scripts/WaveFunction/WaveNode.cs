using System;
using System.Collections.Generic;
using Procedural.Scripts.BehaviourGraph;
using Procedural.Scripts.DataStructure;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Procedural.Scripts.WaveFunction
{
  public class WaveNode : BehaviourNode
  {
    public Dictionary<string, List<WaveNode>> relations = new()
    {
      { "TOP", new List<WaveNode>() },
      { "BOTTOM", new List<WaveNode>() },
      { "LEFT", new List<WaveNode>() },
      { "RIGHT", new List<WaveNode>() },
    };

    private Port _top;
    private Port _bottom;
    private Port _left;
    private Port _right;
    public Sprite sprite;

    /*
     * Check whether a wave, in some direction relative to another wave,
     * has at least one valid property.
     */
    public static bool IsCompatible<T>(List<T> neighborOptions, T option, WavePosition position) where T : WaveNode
    {
      return position switch
      {
        WavePosition.TOP => Intersects(neighborOptions, option.relations["TOP"]),
        WavePosition.BOTTOM => Intersects(neighborOptions, option.relations["BOTTOM"]),
        WavePosition.LEFT => Intersects(neighborOptions, option.relations["LEFT"]),
        WavePosition.RIGHT => Intersects(neighborOptions, option.relations["RIGHT"]),
        _ => false
      };
    }

    /*
     * True if there is an intersection of rules,
     * i.e there is at least one rule compatible
     */
    private static bool Intersects<T>(List<T> neighborOptions, List<WaveNode> options) where T : WaveNode
    {
      return neighborOptions.Count == 0 || neighborOptions.Exists(aa => options.Exists(bb => aa.guid == bb.guid));
    }

    /*
     * Dictionary to map the position of a wave relative to another
     */
    private static readonly Dictionary<(int, int), WavePosition> Positions = new()
    {
      { (-1, 1), WavePosition.TOP_LEFT },
      { (0, 1), WavePosition.TOP },
      { (1, 1), WavePosition.TOP_RIGHT },

      { (-1, 0), WavePosition.LEFT },
      { (0, 0), WavePosition.CENTER },
      { (1, 0), WavePosition.RIGHT },

      { (-1, -1), WavePosition.BOTTOM_LEFT },
      { (0, -1), WavePosition.BOTTOM },
      { (1, -1), WavePosition.BOTTOM_RIGHT },
    };

    /*
     * Function to map the position of a wave relative to another
     */
    public static WavePosition GetPosition(Vector2 a, Vector2 b)
    {
      var x = (int)(a.x - b.x);
      var y = (int)(a.y - b.y);
      return Positions[(x, y)];
    }

    public override void BuildNodeView(Node node)
    {
      _top = Port.Create<Edge>(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));
      _bottom = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
      _left = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
      _right = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

      _top.portColor = Color.blue;
      _bottom.portColor = Color.blue;
      _left.portColor = Color.red;
      _right.portColor = Color.red;

      _top.portName = "TOP";
      _bottom.portName = "BOTTOM";
      _left.portName = "LEFT";
      _right.portName = "RIGHT";

      node.topContainer.Add(_top);
      node.inputContainer.Add(_left);
      node.mainContainer.Add(_bottom);
      node.outputContainer.Add(_right);
    }

    public override Edge ConnectNodeView(string relation, BehaviourNode node)
    {
      if (node is not WaveNode) return null;
      
      var invRelation = InvRelation(relation);

      return GetPort(relation).ConnectTo(node.GetPort(invRelation));
    }

    public override Port GetPort(string relation)
    {
      return relation switch
      {
        "TOP" => _top,
        "BOTTOM" => _bottom,
        "LEFT" => _left,
        "RIGHT" => _right,
        _ => null
      };
    }
    
    public static string InvRelation(string relation)
    {
      return relation switch
      {
        "TOP" => "BOTTOM",
        "BOTTOM" => "TOP",
        "LEFT" => "RIGHT",
        "RIGHT" => "LEFT",
        _ => relation
      };
    }
  }
}