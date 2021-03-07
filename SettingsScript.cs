using System;
using UnityEngine;

namespace BetterExperienceSystem
{
    //Why yes, it does seem very wasteful to use an entire Monobehaviour to run one method doesn't it?
    //Unfortunately GameEvents apparently don't work before a game is loaded, so here we are.
    //Even though I shouldn't need a script to run in the Space Centre, I'm running one anyway.
    public class SettingsScript : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            GameEvents.onGameStateLoad.Add(OnLoad);
        }

        private static void OnLoad(ConfigNode configNode)
        {
            Settings.LoadSettings();
        }
        
        private void OnDisable()
        {
            GameEvents.onGameStateLoad.Remove(OnLoad);
        }
    }
}