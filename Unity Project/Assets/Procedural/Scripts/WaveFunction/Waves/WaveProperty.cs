using System.Collections.Generic;
using UnityEngine;

namespace Procedural.Scripts.WaveFunction.Waves
{
  public abstract class WaveProperty : ScriptableObject
  {
    [SerializeField] private List<WaveProperty> top;
    [SerializeField] private List<WaveProperty> bottom;
    [SerializeField] private List<WaveProperty> left;
    [SerializeField] private List<WaveProperty> right;

    /*
     * Check whether a wave, in some direction relative to another wave,
     * has at least one valid property.
     */
    public static bool IsCompatible<T>(List<T> neighborOptions, T option, WavePosition position) where T : WaveProperty
    {
      return position switch
      {
        WavePosition.TOP => Intersects(neighborOptions, option.top),
        WavePosition.BOTTOM => Intersects(neighborOptions, option.bottom),
        WavePosition.LEFT => Intersects(neighborOptions, option.left),
        WavePosition.RIGHT => Intersects(neighborOptions, option.right),
        _ => false
      };
    }

    /*
     * True if there is an intersection of rules,
     * i.e there is at least one rule compatible
     */
    private static bool Intersects<T>(List<T> neighborOptions, List<WaveProperty> options) where T : WaveProperty
    {
      return neighborOptions.Count == 0 || neighborOptions.Exists(aa => options.Exists(bb => aa.name == bb.name));
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
  }
}