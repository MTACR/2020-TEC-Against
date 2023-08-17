using System;
using System.Collections.Generic;
using Procedural.Scripts.DataStructure;
using Procedural.Scripts.WaveFunction;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Procedural.Scripts.BehaviourGraph
{
  /*
   * Base class used to create trees of scriptableObjects.
   * This class should be extended to implement the node linking logic.
   */
  public abstract class BehaviourTree : ScriptableObject
  {
    // public BehaviourNode root;
    public List<BehaviourNode> nodes = new();

    public BehaviourNode CreateNode(Type type)
    {
      var node = CreateInstance(type) as BehaviourNode;
      node.name = "Node";
      node.guid = GUID.Generate().ToString();

      nodes.Add(node);

      AssetDatabase.AddObjectToAsset(node, this);
      AssetDatabase.SaveAssets();

      return node;
    }

    public void DeleteNode(BehaviourNode node)
    {
      nodes.Remove(node);

      AssetDatabase.RemoveObjectFromAsset(node);
      AssetDatabase.SaveAssets();
    }

    public abstract void AddRelation(string relation, BehaviourNode a, BehaviourNode b);
    public abstract void RemoveRelation(string relation, BehaviourNode a, BehaviourNode b);
    public abstract Dictionary<string, List<WaveNode>> GetRelations<T>(T node) where T : BehaviourNode;
    public abstract string GetRelation(Edge edge);
    public abstract Type GetNodeType();
  }
}