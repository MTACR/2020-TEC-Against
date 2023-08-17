using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Procedural.Editor.BehaviourGraph.Views
{
  public class ToolbarMenuView : ToolbarMenu
  {
    public new class UxmlFactory : UxmlFactory<ToolbarMenuView, UxmlTraits>
    {
    }
  }
}