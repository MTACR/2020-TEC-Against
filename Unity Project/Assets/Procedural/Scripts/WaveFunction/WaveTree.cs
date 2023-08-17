using System;
using System.Collections.Generic;
using Procedural.Scripts.BehaviourGraph;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Procedural.Scripts.WaveFunction
{
  [CreateAssetMenu]
  public class WaveTree : BehaviourTree
  {
    public override void AddRelation(string relation, BehaviourNode a, BehaviourNode b)
    {
      if (a is not WaveNode nodeA || b is not WaveNode nodeB) return;
      
      var invRelation = WaveNode.InvRelation(relation);
        
      nodeA.AddRelation(relation, nodeB);
      nodeB.AddRelation(invRelation, nodeA);
    }

    public override void RemoveRelation(string relation, BehaviourNode a, BehaviourNode b)
    {
      if (a is not WaveNode nodeA || b is not WaveNode nodeB) return;
      
      var invRelation = WaveNode.InvRelation(relation);
        
      nodeA.RemoveRelation(relation, nodeB);
      nodeB.RemoveRelation(invRelation, nodeA);
    }

    public override Dictionary<string, List<BehaviourNode>> GetRelations(BehaviourNode node)
    {
      if (node is WaveNode wave)
      {
        return wave.GetRelations();
      }

      return new Dictionary<string, List<BehaviourNode>>();
    }

    /*
     * "Caras... É uma gambiarra... Mas funciona.
     * O importante é que funciona e funciona muito bem"
     * - Agostini, Luciano.
     */
    public override string GetRelation(Edge edge)
    {
      if (edge.input.orientation == Orientation.Vertical)
      {
        return "TOP";
      }

      if (edge.output.orientation == Orientation.Vertical)
      {
        return "BOTTOM";
      }

      if (edge.input.orientation == Orientation.Horizontal)
      {
        return "LEFT";
      }

      if (edge.output.orientation == Orientation.Horizontal)
      {
        return "RIGHT";
      }

      return null;
    }

    public override Type GetNodeType()
    {
      return typeof(WaveNode);
    }
  }
}