/*
MIT License
Copyright 2023 Stevphanie Ricardo

Permission is hereby granted, free of charge, to any person obtaining a copy of this
software and associated documentation files (the "Software"), to deal in the Software
without restriction, including without limitation the rights to use, copy, modify,
merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Linq;
using Breadnone;

#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEditor;
#endif

///<summary>All animations</summary>
namespace UTK
{
    public static partial class Utk
    {
        ///<summary>Lerps opacity of a visualElement.</summary>
        public static void LerpOpacity<T>(this T visualElement, float from, float to, float duration, Ease ease = Ease.Linear, int loopCount = 0, bool pingPong = false, Action onComplete = null, int? customId = null) where T : VisualElement
        {
            VTween.valueFast(visualElement, from, to, duration, (float x) =>{visualElement.style.opacity = x;}, loopCount, pingPong, onComplete, ease, customId:customId);
        }
        ///<summary>Lerps position of a visualElement.</summary>
        public static void LerpPosition<T> (this T visualElement, Vector3 to, float duration, Ease ease = Ease.Linear, int loopCount = 0, int? customId = null) where T : VisualElement
        {
            VTween.moveFast(visualElement, to, duration, ease, loopCount, customId: customId);
        }
        ///<summary>Lerps scaling of a visualElement.</summary>
        public static void LerpScale<T>(this T visualElement, Vector3 scale, float duration, int loopCount = 0, bool pingPong = false, Ease ease = Ease.Linear, Action onComplete = null, bool unscaledTime = false, int? customId = null) where T : VisualElement
        {
            VTween.scaleFast(visualElement, scale, duration, loopCount, pingPong, ease, onComplete, unscaledTime, customId);
        }
        ///<summary>Lerps floating points.</summary>
        public static void LerpValue<T>(this T visualElement, float from, float to, float duration, Action<float> callback, int loopCount = 0, bool pingPong = false, Ease ease = Ease.Linear, Action onComplete = null, bool unscaledTime = false, int? customId = null) where T : VisualElement
        {
            VTween.valueFast(visualElement, from, to, duration, callback, loopCount, pingPong, onComplete, ease, unscaledTime, customId);
        }
        ///<summary>Lerps rotation of a visualElement.</summary>
        public static void LerpRotate<T>(this T visualElement, float angle, float duration, int loopCount = 0, bool pingPong = false, Ease ease = Ease.Linear, int? customId = null) where T : VisualElement
        {
            VTween.rotateFast(visualElement, angle, duration, loopCount, pingPong, ease, customId:customId);
        }
        public static void LerpCancel(ref int customId)
        {
            VTween.TryForceCancel(customId);
            customId = 0;
        }
        public static void LerpCancel(int customId)
        {
            VTween.TryForceCancel(customId);
        }
    }
}