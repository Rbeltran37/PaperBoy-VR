using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.SceneManagement
{
    public class AppLaunch : MonoBehaviour
    {
        private const int NEXT_SCENE = 1;
        
        private void Awake()
        {
            SceneManager.LoadScene(NEXT_SCENE);
        }
    }
}
