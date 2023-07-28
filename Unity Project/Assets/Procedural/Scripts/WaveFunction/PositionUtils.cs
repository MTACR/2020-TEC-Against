using System;
using UnityEngine;

namespace Procedural.Scripts.WaveFunction
{
  public static class PositionUtils
  {
    /*
     * D8
     */
    public static float ChebyshevDistance(Vector2 a, Vector2 b)
    {
      var x = Math.Abs(a.x - b.x);
      var y = Math.Abs(a.y - b.y);
      return Math.Max(x, y);
    }

    /*
     * D4
     */
    public static float ManhattanDistance(Vector2 a, Vector2 b)
    {
      var x = Math.Abs(a.x - b.x);
      var y = Math.Abs(a.y - b.y);
      return x + y;
    }
  }
}