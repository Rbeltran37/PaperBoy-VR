using System.IO;
using System.Threading.Tasks;
using Core.Debug;
using UnityEngine;

namespace Core.Persistence
{
    [CreateAssetMenu(fileName = "JsonDataContext_", menuName = "ScriptableObjects/Core/Persistence/JsonDataContext", order = 1)]
    public class JsonDataContext : DataContext
    {
        public string fileName;
        
        private string FilePath => $"{Application.persistentDataPath}/{fileName}.json";
        
        
        public override async Task Load()
        {
            if (!File.Exists(FilePath))
            {
                CustomLogger.EditorOnlyError(nameof(Load), $"{nameof(FilePath)} {FilePath} does not exist. Unable to load.");
                return;
            }
            
            using StreamReader reader = new StreamReader(FilePath);
            string json = await reader.ReadToEndAsync();
            JsonUtility.FromJsonOverwrite(json, gameData);
        }

        public override async Task Save()
        {
            string json = JsonUtility.ToJson(gameData);
            using StreamWriter writer = new StreamWriter(FilePath);
            await writer.WriteAsync(json);
        }
    }
}
