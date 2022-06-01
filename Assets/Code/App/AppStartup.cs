﻿using Modules.Root.ContainerComponentModel;
using UnityEngine;
using DG.Tweening;
using Facebook.Unity;
using GameAnalyticsSDK;

namespace Modules.App 
{
    /// <summary>
    /// Starting entry point of an App
    /// Use to register services and controll app load process 
    /// </summary>
    public class AppStartup : AMonoInstaller
    {
        private bool _loadStarted = false;

        public override void Install(IContainer container)
        {
            // register project dependencies via container.Bind<Type>(object);
        }

        /// <summary>
        /// load game scene
        /// </summary>
        private void LoadGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        void Start ()
        {
            if (!FB.IsInitialized)
            {
                FB.Init(FBInitCallback);
            }
            else
            {
                FB.ActivateApp();
            }
            
            _loadStarted = false;
            InitCallback();
        }

        // call after all services init
        private void InitCallback ()
        {
            if (_loadStarted)
                return;

#if UNITY_EDITOR
            LoadGame();
#else
            Invoke(nameof(LoadGame), 0.89f);
#endif
           
            _loadStarted = true;
        }

        private void FBInitCallback()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                Debug.Log("Failed to initialize the Facebook SDK");
            }
        }
    }
}
