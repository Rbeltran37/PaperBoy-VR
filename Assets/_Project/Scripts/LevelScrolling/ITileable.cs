using UnityEngine;

namespace PaperBoy.LevelScrolling
{
    public interface ITileable
    {
        public void Move(Vector3 movement);
        public float GetFarthestPointZ();
        public Vector3 GetSpawnPosition(float worldX, float worldY, float worldZ);
    }
}
