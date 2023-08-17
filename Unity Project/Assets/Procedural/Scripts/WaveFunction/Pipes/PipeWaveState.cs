using System.Collections.Generic;
using System.Linq;
using Procedural.Scripts.BehaviourGraph;
using UnityEngine;

namespace Procedural.Scripts.WaveFunction.Pipes
{
  public class PipeWaveState : WaveState
  {
    private readonly SpriteRenderer _renderer;

    public PipeWaveState(
      List<WaveState> states,
      Vector2 position,
      BehaviourTree behaviourTree,
      SpriteRenderer renderer
    ) : base(states, position, behaviourTree.nodes.Cast<WaveNode>().ToList())
    {
      _renderer = renderer;
    }

    public void DrawSprite(Sprite empty)
    {
      if (Possibilities.Count == 1)
      {
        _renderer.sprite = Possibilities[0].sprite;
      }

      if (Possibilities.Count == 0)
      {
        _renderer.sprite = empty;
      }
    }
  }
}