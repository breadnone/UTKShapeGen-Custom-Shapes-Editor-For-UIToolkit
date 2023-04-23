using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EditorDelta
{
    ///This is NOT ACCURATE! Just rough estimation and will not run all the time.
    public class EditorDeltaTime
    {
        public static CancellationTokenSource EditorTimeToken;
        private static Stopwatch timeProp;
        public static bool EditorWorkerIsRunning { get; private set; }
        private static float time;
        private static float coldTime;
        public static float Time
        {
            get
            {
                #if UNITY_EDITOR
                lastRequested = EditorApplication.timeSinceStartup;
                #endif
                
                if(!EditorWorkerIsRunning)
                {
                    StartStopEditorTime(true);
                }

                return time;
            }
        }

        private static double oneFrame;
        #if UNITY_EDITOR
        private static double lastRequested;
        #endif
        private static async Task EditorTimeWorker()
        {
            lastRequested = EditorApplication.timeSinceStartup;
            EditorTimeToken = new CancellationTokenSource();
            EditorWorkerIsRunning = true;
            
            if(timeProp == null)
            {
                timeProp = new Stopwatch();
            }

            timeProp.Stop();

            while (EditorWorkerIsRunning)
            {
                if (EditorTimeToken.IsCancellationRequested)
                {
                    timeProp.Stop();
                    break;
                }

                timeProp.Restart();
                await Task.Delay(TimeSpan.FromMilliseconds(oneFrame), EditorTimeToken.Token);
                time = (float)timeProp.Elapsed.TotalSeconds;
                timeProp.Stop();
                 
                if((lastRequested + 30) < EditorApplication.timeSinceStartup)
                {
                    break;
                }
            }

            StartStopEditorTime(false);
        }
        public static async void StartStopEditorTime(bool state)
        {
            if (state)
            {
                var refValue = Screen.currentResolution.refreshRateRatio.value;
                oneFrame = (1d / refValue) * 1000;

                if (EditorWorkerIsRunning)
                    return;

                await EditorTimeWorker();
            }
            else
            {
                if (EditorWorkerIsRunning)
                {
                    EditorWorkerIsRunning = false;
                }

                if (EditorTimeToken != null)
                {
                    EditorTimeToken.Cancel();
                    EditorTimeToken.Dispose();
                    EditorTimeToken = null;
                }

                lastRequested = 0;
            }
        }
    }
}