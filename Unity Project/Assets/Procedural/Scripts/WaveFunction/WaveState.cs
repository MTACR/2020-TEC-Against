using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Procedural.Scripts.WaveFunction
{
  public abstract class WaveState
  {
    private readonly List<WaveState> _states;
    private readonly List<WaveNode> _possibilities;
    private readonly Vector2 _position;
    
    public bool IsCollapsed => _possibilities.Count <= 1;
    public int Entropy => _possibilities.Count;
    protected ReadOnlyCollection<WaveNode> Possibilities => _possibilities.AsReadOnly();

    protected WaveState(List<WaveState> states, Vector2 position, List<WaveNode> possibilities)
    {
      _states = states;
      _position = position;
      _possibilities = possibilities.OrderBy(_ => Random.value).ToList();
    }

    /*
     * Selects the possibility which matches all neighbors or none
     */
    public void Collapse(Func<Vector2, Vector2, float> distance)
    {
      var best = FindBestPossibility(distance);

      if (best == null)
      {
        _possibilities.Clear();
        return;
      }

      foreach (var possibility in _possibilities.ToList())
      {
        if (possibility.guid != best.guid)
        {
          _possibilities.Remove(possibility);
        }
      }
    }

    /*
     * Returns a list with adjacent waves using a distance function
     */
    public List<WaveState> FindNeighbors(Func<Vector2, Vector2, float> distance)
    {
      return _states.FindAll(w => (int)distance(_position, w._position) == 1);
    }

    /*
     * Remove invalid possibilities from _possibilities list using an array of adjacent waves
     */
    public void UpdatePossibilities(Func<Vector2, Vector2, float> distance)
    {
      if (IsCollapsed)
      {
        return;
      }

      var neighbors = FindNeighbors(distance);

      foreach (var possibility in _possibilities.ToList())
      {
        foreach (var neighbor in neighbors)
        {
          var position = WaveNode.GetPosition(neighbor._position, _position);

          if (!WaveNode.IsCompatible(neighbor._possibilities, possibility, position))
          {
            _possibilities.Remove(possibility);
            break;
          }
        }
      }
    }

    /*
     * Returns the best possibility based on adjacent waves or null
     */
    private WaveNode FindBestPossibility(Func<Vector2, Vector2, float> distance)
    {
      var neighbors = FindNeighbors(distance);

      WaveNode best = null;

      foreach (var possibility in _possibilities.ToList())
      {
        var compatible = true;

        foreach (var neighbor in neighbors)
        {
          var position = WaveNode.GetPosition(neighbor._position, _position);

          if (!WaveNode.IsCompatible(neighbor._possibilities, possibility, position))
          {
            compatible = false;
            break;
          }
        }

        if (compatible)
        {
          best = possibility;
        }
      }

      return best;
    }
  }
}