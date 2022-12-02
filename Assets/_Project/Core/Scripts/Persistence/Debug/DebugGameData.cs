using System.Collections.Generic;

namespace Core.Persistence
{
    public class DebugGameData : GameData
    {
        protected override void InitializeValues()
        {
            dataObjectLists = new List<List<DataObject>>();
        }
    }
}
