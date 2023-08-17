using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Procedural.Editor.BehaviourGraph.Views
{
  public class ToolbarView : Toolbar
  {
    public new class UxmlFactory : UxmlFactory<ToolbarView, UxmlTraits>
    {
    }
  }
}