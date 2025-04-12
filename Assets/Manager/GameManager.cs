using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PreggoJam.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        private void Awake()
        {
            Instance = this;

            if (!SceneManager.GetAllScenes().Any(x => x.name == "Map"))
            {
                SceneManager.LoadScene("Map", LoadSceneMode.Additive);
            }
        }

        public bool CanPlay { set; get; } = true;

        public void ReloadMap()
        {
            StartCoroutine(ReloadMapInternal());
        }
        private IEnumerator ReloadMapInternal()
        {
            yield return SceneManager.UnloadSceneAsync("Map");
            yield return SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
            CanPlay = true;
        }
    }
}