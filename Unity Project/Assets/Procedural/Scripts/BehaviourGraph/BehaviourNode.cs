using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Procedural.Scripts.BehaviourGraph
{
  /*
   * Base class used to create scriptableObjects inside trees.
   * This class should be extended to add properties to nodes.
   */
  public abstract class BehaviourNode : ScriptableObject
  {
    public string guid;
    public Vector2 position;

    public abstract void BuildNodeView(Node node);
    public abstract Edge ConnectNodeView(string relation, BehaviourNode node);
    public abstract Port GetPort(string relation);
    public abstract List<BehaviourNode> GetRelations(string relation);
    public abstract Dictionary<string, List<BehaviourNode>> GetRelations();
    public abstract void AddRelation(string relation, BehaviourNode node);
    public abstract void RemoveRelation(string relation, BehaviourNode node);
  }
}