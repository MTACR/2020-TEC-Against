using UnityEngine.UIElements;

namespace Procedural.Editor.BehaviourGraph.Views
{
  public class SplitView : TwoPaneSplitView
  {
    public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits>
    {
    }
  }
}