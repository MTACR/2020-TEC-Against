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
    }
}