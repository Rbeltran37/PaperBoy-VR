using PaperBoy.Procedural;

namespace PaperBoy.ObjectPool
{
    public class StructurePool : ObjectPool
    {
        private StructureSO _structureSO;
        

        private void Awake()
        {
            _structureSO = ObjectPoolSO.PrefabToPool.GetComponent<Structure>().StructureSO;
        }
    }
}
