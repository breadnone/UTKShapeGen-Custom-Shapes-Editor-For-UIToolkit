using UnityEngine;
using UnityEditor;
using Breadnone;

namespace EditorDelta
{   
    public class EditorInit
    {
        [InitializeOnLoadMethod]
        static void OnProjectLoadedInEditor()
        {
            EditorDeltaTime.StartStopEditorTime(true);
            AssemblyReloadEvents.beforeAssemblyReload += () => EditorDeltaTime.StartStopEditorTime(false);
            AssemblyReloadEvents.afterAssemblyReload += () => EditorDeltaTime.StartStopEditorTime(true); ;
        }

        // ensure class initializer is called whenever scripts recompile
        [InitializeOnLoadAttribute]
        public static class PlayModeStateChangedExample
        {
            // register an event handler when the class is initialized
            static PlayModeStateChangedExample()
            {
                EditorApplication.playModeStateChanged += LogPlayModeState;
            }

            private static void LogPlayModeState(PlayModeStateChange state)
            {
                if (state.ToString().Equals("EnteredPlayMode"))
                {
                    EditorDeltaTime.StartStopEditorTime(false);
                }
                else if (state.ToString().Equals("ExitingPlayMode"))
                {
                    EditorDeltaTime.StartStopEditorTime(true);
                }
            }
        }
    }
}