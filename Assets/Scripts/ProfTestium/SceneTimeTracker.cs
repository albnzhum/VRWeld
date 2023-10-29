using System;
using UnityEngine;

namespace ProfTestium
{
    public class SceneTimeTracker : MonoBehaviour
    {
        private DateTime startTime;
        private DateTime endTime;

        private void Awake()
        {
            startTime = DateTime.Now;
        }

        public DateTime GetSceneStartTime()
        {
            return startTime;
        }
        
        public TimeSpan GetSceneDuration()
        {
            endTime = DateTime.Now;
            TimeSpan timeInScene = endTime - startTime;
            return timeInScene;
        }
    }
}
