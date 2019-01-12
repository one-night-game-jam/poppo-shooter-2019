using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagers
{
    public class Reloader : MonoBehaviour
    {
        [SerializeField]
        private string sceneName;

        public void Reload()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}