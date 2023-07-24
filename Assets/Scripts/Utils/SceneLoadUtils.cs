using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Game.SceneUtils
{
    public static class SceneLoadUtils
    {
        public static async void LoadScene(string name, Action onLoadedCallback)
        {
            var operation = SceneManager.LoadSceneAsync(name);

            while (!operation.isDone)
                await Task.Yield();

            onLoadedCallback?.Invoke();
        }
    }
}
