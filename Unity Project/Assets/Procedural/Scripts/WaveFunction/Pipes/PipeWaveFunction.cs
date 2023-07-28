using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Procedural.Scripts.WaveFunction.Waves;
using UnityEngine;

namespace Procedural.Scripts.WaveFunction.Pipes
{
  public class PipeWaveFunction : MonoBehaviour
  {
    [SerializeField] private List<PipeWaveProperty> possibilities;
    [SerializeField] private SpriteRenderer pipePrefab;
    [SerializeField] private Sprite emptyPrefab;
    [SerializeField] [Range(0, 1)] private float collapseSpeed;

    private WaveFunction<PipeWaveProperty> _waveFunction;
    private List<WaveState<PipeWaveProperty>> _states;

    private void Awake()
    {
      _states = new List<WaveState<PipeWaveProperty>>();

      for (var i = 0; i < 10; i++)
      {
        for (var j = 0; j < 10; j++)
        {
          _states.Add(
            new PipeWaveState(
              _states,
              new Vector2(i, j),
              possibilities,
              Instantiate(
                pipePrefab,
                new Vector3(i, j, 0),
                Quaternion.identity
              )
            )
          );
        }
      }

      _waveFunction = new WaveFunction<PipeWaveProperty>(_states, PositionUtils.ManhattanDistance);
    }

    private void OnGUI()
    {
      foreach (var state in _states.Cast<PipeWaveState>())
      {
        state.DrawSprite(emptyPrefab);
      }
    }

    private void OnMouseDown()
    {
      StartCoroutine(Draw());
    }

    private IEnumerator Draw()
    {
      while (_waveFunction.Collapse())
      {
        yield return new WaitForSeconds(collapseSpeed);
      }
    }
  }
}