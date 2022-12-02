using Core.UpdateManager;
using UnityEngine;

namespace Core.Sets
{
    [CreateAssetMenu(fileName = "UpdateRuntimeSet_", menuName = "ScriptableObjects/Core/Sets/UpdateRuntimeSet", order = 1)]
    public class UpdateRuntimeSet : RuntimeSet<IUpdateable>
    {
        
    }
}
