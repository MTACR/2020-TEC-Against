using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Procedural.Scripts.WaveFunction.Waves
{
  public abstract class WaveState<T> where T : WaveProperty
  {
    
    private readonly List<WaveState<T>> _states;
    private readonly List<T> _possibilities;
    private readonly Vector2 _position;
    
    public bool IsCollapsed => _possibilities.Count <= 1;
    public int Entropy => _possibilities.Count;
    protected ReadOnlyCollection<T> Possibilities => _possibilities.AsReadOnly();

    protected WaveState(List<WaveState<T>> states, Vector2 position, IEnumerable<T> possibilities)
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
        if (possibility.name != best.name)
        {
          _possibilities.Remove(possibility);
        }
      }
    }

    /*
     * Returns a list with adjacent waves using a distance function
     */
    public List<WaveState<T>> FindNeighbors(Func<Vector2, Vector2, float> distance)
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
          var position = WaveProperty.GetPosition(neighbor._position, _position);

          if (!WaveProperty.IsCompatible(neighbor._possibilities, possibility, position))
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
    private T FindBestPossibility(Func<Vector2, Vector2, float> distance)
    {
      var neighbors = FindNeighbors(distance);

      T best = null;

      foreach (var possibility in _possibilities.ToList())
      {
        var compatible = true;

        foreach (var neighbor in neighbors)
        {
          var position = WaveProperty.GetPosition(neighbor._position, _position);

          if (!WaveProperty.IsCompatible(neighbor._possibilities, possibility, position))
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