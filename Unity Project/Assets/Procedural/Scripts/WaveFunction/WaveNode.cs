using System.Collections.Generic;
using System.Linq;
using Procedural.Scripts.BehaviourGraph;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Procedural.Scripts.WaveFunction
{
  public class WaveNode : BehaviourNode
  {
    [SerializeField] private List<WaveNode> topNodes;
    [SerializeField] private List<WaveNode> bottomNodes;
    [SerializeField] private List<WaveNode> leftNodes;
    [SerializeField] private List<WaveNode> rightNodes;

    private Port _topPort;
    private Port _bottomPort;
    private Port _leftPort;
    private Port _rightPort;

    public Sprite sprite;

    /*
     * Check whether a wave, in some direction relative to another wave,
     * has at least one valid property.
     */
    public static bool IsCompatible(List<WaveNode> neighborOptions, WaveNode option, WavePosition position)
    {
      return position switch
      {
        WavePosition.TOP => Intersects(neighborOptions, option.topNodes),
        WavePosition.BOTTOM => Intersects(neighborOptions, option.bottomNodes),
        WavePosition.LEFT => Intersects(neighborOptions, option.leftNodes),
        WavePosition.RIGHT => Intersects(neighborOptions, option.rightNodes),
        _ => false
      };
    }

    /*
     * True if there is an intersection of rules,
     * i.e there is at least one rule compatible
     */
    private static bool Intersects(List<WaveNode> neighborOptions, List<WaveNode> options)
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
      _topPort = Port.Create<Edge>(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));
      _bottomPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
      _leftPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
      _rightPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

      _topPort.portColor = Color.blue;
      _bottomPort.portColor = Color.blue;
      _leftPort.portColor = Color.red;
      _rightPort.portColor = Color.red;

      _topPort.portName = "TOP";
      _bottomPort.portName = "BOTTOM";
      _leftPort.portName = "LEFT";
      _rightPort.portName = "RIGHT";

      node.topContainer.Add(_topPort);
      node.inputContainer.Add(_leftPort);
      node.mainContainer.Add(_bottomPort);
      node.outputContainer.Add(_rightPort);
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
        "TOP" => _topPort,
        "BOTTOM" => _bottomPort,
        "LEFT" => _leftPort,
        "RIGHT" => _rightPort,
        _ => null
      };
    }

    public override List<BehaviourNode> GetRelations(string relation)
    {
      return relation switch
      {
        "TOP" => topNodes.Cast<BehaviourNode>().ToList(),
        "BOTTOM" => bottomNodes.Cast<BehaviourNode>().ToList(),
        "LEFT" => leftNodes.Cast<BehaviourNode>().ToList(),
        "RIGHT" => rightNodes.Cast<BehaviourNode>().ToList(),
        _ => null
      };
    }

    public override Dictionary<string, List<BehaviourNode>> GetRelations()
    {
      return new Dictionary<string, List<BehaviourNode>>
      {
        { "TOP", topNodes.Cast<BehaviourNode>().ToList() },
        { "BOTTOM", bottomNodes.Cast<BehaviourNode>().ToList() },
        { "LEFT", leftNodes.Cast<BehaviourNode>().ToList() },
        { "RIGHT", rightNodes.Cast<BehaviourNode>().ToList() },
      };
    }

    public override void AddRelation(string relation, BehaviourNode node)
    {
      switch (relation)
      {
        case "TOP":
          topNodes.Add(node as WaveNode);
          break;

        case "BOTTOM":
          bottomNodes.Add(node as WaveNode);
          break;

        case "LEFT":
          leftNodes.Add(node as WaveNode);
          break;

        case "RIGHT":
          rightNodes.Add(node as WaveNode);
          break;
      }
    }

    public override void RemoveRelation(string relation, BehaviourNode node)
    {
      switch (relation)
      {
        case "TOP":
          topNodes.Remove(node as WaveNode);
          break;

        case "BOTTOM":
          bottomNodes.Remove(node as WaveNode);
          break;

        case "LEFT":
          leftNodes.Remove(node as WaveNode);
          break;

        case "RIGHT":
          rightNodes.Remove(node as WaveNode);
          break;
      }
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