using System;
using System.Collections.Generic;
using Procedural.Scripts.BehaviourGraph;
using Procedural.Scripts.DataStructure;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Procedural.Scripts.WaveFunction
{
  [CreateAssetMenu]
  public class WaveTree : BehaviourTree
  {
    public override void AddRelation(string relation, BehaviourNode a, BehaviourNode b)
    {
      if (a is WaveNode nodeA && b is WaveNode nodeB)
      {
        var invRelation = WaveNode.InvRelation(relation);
        
        nodeA.relations[relation].Add(nodeB);
        nodeB.relations[invRelation].Add(nodeA);
      }
    }

    public override void RemoveRelation(string relation, BehaviourNode a, BehaviourNode b)
    {
      if (a is WaveNode nodeA && b is WaveNode nodeB)
      {
        var invRelation = WaveNode.InvRelation(relation);
        
        nodeA.relations[relation].Remove(nodeB);
        nodeB.relations[invRelation].Remove(nodeA);
      }
    }

    public override Dictionary<string, List<WaveNode>> GetRelations<T>(T node)
    {
      if (node is WaveNode wave)
      {
        return wave.relations;
      }

      return new Dictionary<string, List<WaveNode>>();
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