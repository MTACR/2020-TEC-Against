using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Procedural.Scripts.WaveFunction
{
  public class WaveFunction
  {
    private readonly Func<Vector2, Vector2, float> _distance;
    private readonly List<WaveState> _states;

    public WaveFunction(List<WaveState> states, Func<Vector2, Vector2, float> distance)
    {
      _states = states;
      _distance = distance;
    }

    /*
     * Returns true if there are other waves to collapse
     */
    public bool Collapse()
    {
      var state = FindLowestEntropyState();

      if (state == null)
      {
        return false;
      }

      state.Collapse(_distance);

      var neighbors = state.FindNeighbors(_distance);

      foreach (var neighbor in neighbors)
      {
        neighbor.UpdatePossibilities(_distance);
      }

      return true;
    }

    /*
     * Returns the state with lowest entropy,
     * i.e lowest number of possibilities, or null
     */
    private WaveState FindLowestEntropyState()
    {
      var states = _states
        .FindAll(s => !s.IsCollapsed)
        .OrderBy(s => s.Entropy)
        .ToList();

      return states.Count == 0 ? null : states.First();
    }
  }
}