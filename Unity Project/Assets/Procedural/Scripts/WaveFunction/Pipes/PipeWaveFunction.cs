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
    private int _x;

    private void Awake()
    {
      _states = new List<WaveState<PipeWaveProperty>>();
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
      for (var i = 0; i < 10; i++, _x++)
      {
        for (var j = 0; j < 10; j++)
        {
          _states.Add(
            new PipeWaveState(
              _states,
              new Vector2(_x, j),
              possibilities,
              Instantiate(
                pipePrefab,
                new Vector3(_x, j, 0),
                Quaternion.identity
              )
            )
          );
        }
      }
      
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