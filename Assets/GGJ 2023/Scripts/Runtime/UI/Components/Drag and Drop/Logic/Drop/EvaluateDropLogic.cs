using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Logic - Evaluate Drop", menuName="GGJ/UI/Drag ang Drop/Logic/Evaluate Drop")]
    public class EvaluateDropLogic : DropLogic
    {
        public enum EvaluateType
        {
            Any = 0,
            [InspectorName("All (Drag)")]
            All_Drag = 1,
            [InspectorName("All (Drop)")]
            All_Drop = 2
        }

        [Tooltip("[Any] - Drop is valid as long as drop area and drag target have any overlapping flags.\n\n[All (Drag)] - Drop is valid if drop area has all drag target flags.\n\n[All (Drop)] - Drop is valid if drag target has all drop area flags.")]
        public EvaluateType type = EvaluateType.Any;

        public override bool DoLogic(Draggable dragTarget, Droppable dropArea)
        {
            switch(type)
            {
                case EvaluateType.Any:
                    return ((dropArea.Flags & dragTarget.Flags) > 0);
                case EvaluateType.All_Drag:
                    return ((dropArea.Flags & dragTarget.Flags) == dragTarget.Flags);
                case EvaluateType.All_Drop:
                    return ((dropArea.Flags & dragTarget.Flags) == dropArea.Flags);
            }

            return false;
        }
    }
}