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

using UnityEngine;
using Breadnone.Extension;
using UnityEngine.UIElements;
using System;


namespace Breadnone
{
    public enum VAxis
    {
        XYZ,
        X,
        Y,
        Z
    }
    public static class VTween
    {
        #region Fast-Move (low alloc)
        ///<summary>Fast low allocation struct-based tweening.</summary>
        //GameObject gameObject, Vector3 destination, float time, Ease ease = Ease.Linear, Action onComplete = null, bool local = false, bool unscaledTime = false
        public static void moveFast(GameObject gameObject, Vector3 to, float time, Ease ease = Ease.Linear, int loopCount = 0, bool pingpong = false, Action onComplete = null, bool localSpace = false, bool unscaledTime = false)
        {
            new STStructMove(gameObject, to, time, ease, loopCount, pingpong, onComplete, localSpace, unscaledTime);
        }
        ///<summary>Fast low allocation struct-based tweening.</summary>
        public static void moveFast(VisualElement visualElement, Vector3 to, float time, Ease ease = Ease.Linear, int loopCount = 0, bool pingPong = false, Action onComplete = null, bool unscaledTime = false, int? customId = null)
        {
            new STStructMoveUI(visualElement, to, time, ease, loopCount, pingPong, onComplete, unscaledTime, customId: customId);
        }
        ///<summary>Fast low allocation struct-based tweening.</summary>
        public static void followFast(GameObject gameObject, Transform target, float speed, Vector3 smoothness)
        {
            new STStructFollow(gameObject, target, speed, smoothness);
        }
        ///<summary>Fast low allocation struct based tweening.</summary>
        public static void rotateFast(GameObject gameObject, float degreeAngle, float time, Vector3 direction, Ease ease = Ease.Linear, Action onComplete = null, bool localSpace = false, bool unscaledTime = false)
        {
            new STStructRotate(gameObject, degreeAngle, direction, time, ease, onComplete, localSpace, unscaledTime);
        }
        ///<summary>Fast low allocation struct based tweening.</summary>
        public static void rotateFast(VisualElement visualElement, float degreeAngle, float time, int loopCount = 0, bool pingPong = false, Ease ease = Ease.Linear, Action onComplete = null, bool unscaledTime = false, int? customId = null)
        {
            new STStructRotateUI(visualElement, degreeAngle, time, loopCount, pingPong, ease, onComplete, customId: customId);
        }
        ///<summary>Fast low allocation struct-based tweening.</summary>
        public static void scaleFast(VisualElement visualElement, Vector3 scale, float time, int loopCount = 0, bool pingPong = false, Ease ease = Ease.Linear, Action onComplete = null, bool unscaledTime = false, int? customId = null)
        {
            new STStructScaleUI(visualElement, scale, time, loopCount, pingPong, ease, onComplete, unscaledTime, customId:customId);
        }
        ///<summary>Fast low allocation struct-based tweening.</summary>
        public static void scaleFast(GameObject gameObject, Vector3 scale, float time, Ease ease = Ease.Linear, Action onComplete = null, bool unscaledTime = false)
        {
            new STStructScale(gameObject, scale, time, ease, onComplete, unscaledTime);
        }
        ///<sumary>Interpolates float value.</summary>
        public static void valueFast(GameObject gameObject, float from, float to, float time, Action<float> callback, int loopCount, bool pingPong, Action onComplete = null, Ease ease = Ease.Linear, bool execOnCompleteOnCancel = false)
        {
            new STStructValue(gameObject, from, to, time, callback, loopCount, pingPong, ease, onComplete, execOnCompleteOnCancel);
        }
        public static void valueFast(VisualElement visualElement, float from, float to, float time, Action<float> callback, int loopCount, bool pingPong, Action onComplete = null, Ease ease = Ease.Linear, bool execOnCompleteOnCancel = false, int? customId = null)
        {
            new STStructValueUI(visualElement, from, to, time, callback, loopCount, pingPong, ease, onComplete, execOnCompleteOnCancel, customId:customId);
        }
        public static void valueFast(VisualElement visualElement, Vector3 from, Vector3 to, float time, Action<Vector3> callback, int loopCount, bool pingPong, Action onComplete = null, Ease ease = Ease.Linear, bool execOnCompleteOnCancel = false, int? customId = null)
        {
            new STStructValueVector3UI(visualElement, from, to, time, callback, loopCount, pingPong, ease, onComplete, execOnCompleteOnCancel, customId:customId);
        }
        ///<summary>Interpolates Vector3 value.</summary>
        public static void valueFast(GameObject gameObject, Vector3 from, Vector3 to, float time, Action<Vector3> callback, bool pingPong = false, int loopCount = 0, Action onComplete = null, Ease ease = Ease.Linear, bool execOnCompleteOnCancel = false)
        {
            new STStructValueVector3(gameObject, from, to, time, callback, loopCount, pingPong, ease, onComplete, execOnCompleteOnCancel);
        }
        ///<summary>Forces cancelling a struct-based tween instance from outside of scope. Not recommended for mass cancelling due to needs to iteration.</summary>
        public static void TryForceCancel(GameObject gameObject)
        {
            Action act = null;
            int id = gameObject.GetInstanceID();

            for(int i = 0; i < VTweenManager.activeStructTweens.Count; i++)
            {
                if(VTweenManager.activeStructTweens[i].id == id)
                {
                    act = VTweenManager.activeStructTweens[i].cancel;
                    break;
                }
            } 
            act?.Invoke();
        }
        public static void TryForceCancel(int customId)
        {
            Action act = null;
            int id = customId;

            for(int i = 0; i < VTweenManager.activeStructTweens.Count; i++)
            {
                if(VTweenManager.activeStructTweens[i].id == id)
                {
                    act = VTweenManager.activeStructTweens[i].cancel;
                    break;
                }
            } 
            act?.Invoke();
        }
        public static void TryForceCancel(VisualElement visualElement)
        {
            Action act = null;
            int id = visualElement.GetHashCode();

            for(int i = 0; i < VTweenManager.activeStructTweens.Count; i++)
            {
                if(VTweenManager.activeStructTweens[i].id == id)
                {
                    act = VTweenManager.activeStructTweens[i].cancel;
                    break;
                }
            } 
            act?.Invoke();
        }
        ///<summary>Forces cancel all struct-based tween instance from outside of scope. Not recommended for mass cancelling due to needs to iteration.</summary>
        public static void TryForceCancelAll()
        {
            if(VTweenManager.activeStructTweens.Count == 0)
                return;

            for(int i = 0; i < VTweenManager.activeStructTweens.Count; i++)
            {
                VTweenManager.activeStructTweens[i].cancel.Invoke();
            } 
        }
        #endregion

        #region Move
        ///<summary>Moves object to target position Vector3.</summary>
        public static VTweenMove move(GameObject gameObject, Vector3 to, float duration)
        {
            //return new VTweenMove(gameObject.transform, null, to, duration);// Non pooling solution
            var instance = VExtension.GetInstance<VTweenMove>(gameObject.GetInstanceID());
            var trans = gameObject.transform;
            instance.SetBaseValues(trans, ref to, ref duration);
            return instance;
        }
        ///<summary>Moves object to target position.</summary>
        public static VTweenMove move(Transform transform, Vector3 to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(transform.gameObject.GetInstanceID());
            var trans = transform;
            instance.SetBaseValues(trans, ref to, ref duration);
            return instance;
        }
        ///<summary>Moves object to target position.</summary>
        public static VTweenMove move(Transform transform, Transform to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(transform.gameObject.GetInstanceID());
            var trans = transform;
            var tpos = to.position;
            instance.SetBaseValues(trans, ref tpos, ref duration);
            return instance;
        }
        ///<summary>Moves object based on target's transform.</summary>
        public static VTweenMove move(GameObject gameObject, Transform to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(gameObject.GetInstanceID());
            var trans = gameObject.transform;
            var tpos = to.position;
            instance.SetBaseValues(trans, ref tpos, ref duration);
            return instance;
        }
        ///<summary>Moves object based on target's ITransform on VisualElement.</summary>
        public static VTweenMoveUI move(VisualElement visualElement, Vector3 to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMoveUI>(visualElement.GetHashCode());
            instance.SetBaseValues(visualElement, ref to, ref duration);
            return instance;
        }

        ///<summary>Moves object localSpace.</summary>
        public static VTweenMove moveLocal(GameObject gameObject, Vector3 to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(gameObject.GetInstanceID());
            var trans = gameObject.transform;
            instance.vprops.isLocal = true;
            instance.SetBaseValues(trans, ref to, ref duration);
            return instance;
        }
        ///<summary>Moves object based on target's localTransform.</summary>
        public static VTweenMove moveLocal(GameObject gameObject, Transform to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(gameObject.GetInstanceID());
            var trans = gameObject.transform;
            instance.vprops.isLocal = true;
            var tpos = to.localPosition;
            instance.SetBaseValues(trans, ref tpos, ref duration);
            return instance;
        }
        ///<summary>Moves object based on target's localTransform.</summary>
        public static VTweenMove moveLocal(Transform transform, Transform to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(transform.gameObject.GetInstanceID());
            var trans = transform;
            instance.vprops.isLocal = true;
            var tpos = to.localPosition;
            instance.SetBaseValues(trans, ref tpos, ref duration);
            return instance;
        }
        ///<summary>Moves object based on object's X axis.</summary>
        public static VTweenMove moveX(GameObject gameObject, float to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(gameObject.GetInstanceID());
            var trans = gameObject.transform;
            var t = new Vector3(to, trans.position.y, trans.position.z);
            instance.SetBaseValues(trans, ref t, ref duration);
            return instance;
        }
        ///<summary>Moves object based on object's X axis.</summary>
        public static VTweenMove moveX(Transform transform, float to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(transform.gameObject.GetInstanceID());
            var trans = transform;
            var t = new Vector3(to, trans.position.y, trans.position.z);
            instance.SetBaseValues(trans, ref t, ref duration);
            return instance;
        }
        ///<summary>Moves object based on object's X axis.</summary>
        public static VTweenMoveUI moveX(VisualElement visualElement, float to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMoveUI>(visualElement.GetHashCode());
            var trans = visualElement.resolvedStyle.translate;
            var t = new Vector3(to, trans.y, trans.z);
            instance.SetBaseValues(visualElement, ref t, ref duration);
            return instance;
        }
        ///<summary>Moves object based on object's X axis localSpace.</summary>
        public static VTweenMove moveLocalX(GameObject gameObject, float to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(gameObject.GetInstanceID());
            var trans = gameObject.transform;
            instance.vprops.isLocal = true;
            var t = new Vector3(to, trans.localPosition.y, trans.localPosition.z);
            instance.SetBaseValues(trans, ref t, ref duration);
            return instance;
        }
        ///<summary>Moves object based on object's X axis localSpace.</summary>
        public static VTweenMove moveLocalX(Transform transform, float to, float duration)
        {
            var instance = new VTweenMove();
            var trans = transform;
            instance.vprops.isLocal = true;
            var t = new Vector3(to, trans.localPosition.y, trans.localPosition.z);
            instance.SetBaseValues(trans, ref t, ref duration);
            return instance;
        }
        ///<summary>Moves object based on object's Y axis.</summary>
        public static VTweenMove moveY(GameObject gameObject, float to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(gameObject.GetInstanceID());
            var trans = gameObject.transform;
            var t = new Vector3(trans.position.x, to, trans.position.z);
            instance.SetBaseValues(trans, ref t, ref duration);
            return instance;
        }
        ///<summary>Moves object based on object's Y axis.</summary>
        public static VTweenMove moveY(Transform transform, float to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(transform.gameObject.GetInstanceID());
            var trans = transform;
            var t = new Vector3(trans.position.x, to, trans.position.z);
            instance.SetBaseValues(trans, ref t, ref duration);
            return instance;
        }
        ///<summary>Moves object based on object's Y axis of a VisualElement.</summary>
        public static VTweenMoveUI moveY(VisualElement visualElement, float to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMoveUI>(visualElement.GetHashCode());
            var trans = visualElement.resolvedStyle.translate;
            var t = new Vector3(trans.x, to, trans.z);
            instance.SetBaseValues(visualElement, ref t, ref duration);
            return instance;
        }
        ///<summary>Moves object based on object's Y axis localSpace.</summary>
        public static VTweenMove moveLocalY(GameObject gameObject, float to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(gameObject.GetInstanceID());
            var trans = gameObject.transform;
            instance.vprops.isLocal = true;
            var t = new Vector3(trans.localPosition.x, to, trans.localPosition.z);
            instance.SetBaseValues(trans, ref t, ref duration);
            return instance;
        }
        ///<summary>Moves object based on object's Y axis localSpace.</summary>
        public static VTweenMove moveLocalY(Transform transform, float to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(transform.gameObject.GetInstanceID());
            var trans = transform;
            instance.vprops.isLocal = true;
            var t = new Vector3(trans.localPosition.x, to, trans.localPosition.z);
            instance.SetBaseValues(trans, ref t, ref duration);
            return instance;
        }
        ///<summary>Moves object based on object's Z axis.</summary>
        public static VTweenMove moveZ(GameObject gameObject, float to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(gameObject.GetInstanceID());
            var trans = gameObject.transform;
            var t = new Vector3(trans.position.x, trans.position.y, to);
            instance.SetBaseValues(trans, ref t, ref duration);
            return instance;
        }
        ///<summary>Moves object based on object's Z axis.</summary>
        public static VTweenMove moveZ(Transform transform, float to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(transform.gameObject.GetInstanceID());
            var trans = transform;
            var t = new Vector3(trans.position.x, trans.position.y, to);
            instance.SetBaseValues(trans, ref t, ref duration);
            return instance;
        }
        ///<summary>Moves object based on object's local Z axis.</summary>
        public static VTweenMove moveLocalZ(GameObject gameObject, float to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(gameObject.GetInstanceID());
            var trans = gameObject.transform;
            instance.vprops.isLocal = true;
            var t = new Vector3(trans.localPosition.x, trans.localPosition.y, to);
            instance.SetBaseValues(trans, ref t, ref duration);
            return instance;
        }
        ///<summary>Moves object based on object's local Z axis.</summary>
        public static VTweenMove moveLocalZ(Transform transform, float to, float duration)
        {
            var instance = VExtension.GetInstance<VTweenMove>(transform.gameObject.GetInstanceID());
            var trans = transform;
            instance.vprops.isLocal = true;
            var t = new Vector3(trans.localPosition.x, trans.localPosition.y, to);
            instance.SetBaseValues(trans, ref t, ref duration);
            return instance;
        }
        #endregion

        #region Rotate
        ///<summary>Rotates object based on angle value.</summary>
        public static VTweenRotate rotate(GameObject gameObject, float angle, Vector3 direction, float duration)
        {
            var instance = new VTweenRotate();
            var trans = gameObject.transform;
            instance.SetBaseValues(trans, null, ref angle, direction, ref duration);
            return instance;
        }
        ///<summary>Rotates VisualElement based on angle value.</summary>
        public static VTweenRotate rotate(VisualElement visualObject, float angle, Vector3 direction, float duration)
        {
            var instance = new VTweenRotate();
            var trans = visualObject.transform;
            instance.SetBaseValues(null, trans, ref angle, direction, ref duration);
            return instance;
        }
        ///<summary>Rotates object based on angle value.</summary>
        public static VTweenRotate rotateLocal(GameObject gameObject, float to, Vector3 direction, float duration)
        {
            var instance = new VTweenRotate();
            var trans = gameObject.transform;
            instance.vprops.isLocal = true;
            instance.SetBaseValues(trans, null, ref to, direction, ref duration);
            return instance;
        }
        ///<summary>Scales object based on Vector3.</summary>
        public static VTweenScale scale(GameObject gameObject, Vector3 targetScale, float duration)
        {
            var instance = new VTweenScale();
            var trans = gameObject.transform;
            instance.SetBaseValues(trans, null, targetScale, trans.localScale, duration);
            return instance;
        }
        ///<summary>Scales object based on Vector3.</summary>
        public static VTweenScale scale(GameObject gameObject, Transform targetTransform, float duration)
        {
            var instance = new VTweenScale();
            var trans = gameObject.transform;
            instance.SetBaseValues(trans, null, targetTransform.localScale, trans.localScale, duration);
            return instance;
        }
        ///<summary>Scales object based on Vector3.</summary>
        public static VTweenScale scale(VisualElement visualElement, Vector3 scaleValue, float duration)
        {
            var instance = new VTweenScale();
            instance.SetBaseValues(null, visualElement.style, scaleValue, visualElement.resolvedStyle.scale.value, duration);
            return instance;
        }
        ///<summary>Scales object based on X axis.</summary>
        public static VTweenScale scaleX(GameObject gameObject, Transform targetTransform, float duration)
        {
            var instance = new VTweenScale();
            var trans = gameObject.transform;
            instance.SetBaseValues(trans, null, targetTransform.localScale, new Vector3(targetTransform.localScale.x, trans.localScale.y, trans.localScale.z), duration);
            return instance;
        }
        ///<summary>Scales object based on Y axis.</summary>
        public static VTweenScale scaleY(GameObject gameObject, Transform targetTransform, float duration)
        {
            var instance = new VTweenScale();
            var trans = gameObject.transform;
            instance.SetBaseValues(trans, null, targetTransform.localScale, new Vector3(trans.localScale.x, targetTransform.localScale.y, trans.localScale.z), duration);
            return instance;
        }
        ///<summary>Scales object based on Z axis.</summary>
        public static VTweenScale scaleZ(GameObject gameObject, Transform targetTransform, float duration)
        {
            var instance = new VTweenScale();
            var trans = gameObject.transform;
            instance.SetBaseValues(trans, null, targetTransform.localScale, new Vector3(trans.localScale.x, trans.localScale.y, targetTransform.localScale.z), duration);
            return instance;
        }
        ///<summary>Scales object based on Vector3.</summary>
        public static VTweenScale scale(Transform transform, Transform targetTransform, float duration)
        {
            var instance = new VTweenScale();
            var trans = transform;
            instance.SetBaseValues(trans, null, targetTransform.localScale, trans.localScale, duration);
            return instance;
        }
        ///<summary>Scales object based on X axis.</summary>
        public static VTweenScale scaleX(Transform transform, Transform targetTransform, float duration)
        {
            var instance = new VTweenScale();
            var trans = transform;
            instance.SetBaseValues(trans, null, targetTransform.localScale, new Vector3(trans.localScale.x, targetTransform.localScale.y, targetTransform.localScale.z), duration);
            return instance;
        }
        ///<summary>Scales object based on X axis.</summary>
        public static VTweenScale scaleY(Transform transform, Transform targetTransform, float duration)
        {
            var instance = new VTweenScale();
            var trans = transform;
            instance.SetBaseValues(trans, null, targetTransform.localScale, new Vector3(targetTransform.localScale.x, trans.localScale.y, targetTransform.localScale.z), duration);
            return instance;
        }
        ///<summary>Scales object based on X axis.</summary>
        public static VTweenScale scaleZ(Transform transform, Transform targetTransform, float duration)
        {
            var instance = new VTweenScale();
            var trans = transform;
            instance.SetBaseValues(trans, null, targetTransform.localScale, new Vector3(trans.localScale.x, trans.localScale.y, targetTransform.localScale.z), duration);
            return instance;
        }
        ///<summary>Scales object based on Vector3.</summary>
        public static VTweenScale scale(Transform transform, Vector3 targetScale, float duration)
        {
            var instance = new VTweenScale();
            var trans = transform;
            instance.SetBaseValues(trans, null, targetScale, trans.localScale, duration);
            return instance;
        }
        #endregion

        #region Value
        ///<summary>Interpolates float value.</summary>
        public static VTweenValueFloat value(float from, float to, float time)
        {
            var instance = new VTweenValueFloat();
            instance.SetBaseValues(from, to, time, null);
            return instance;
        }
        ///<summary>Interpolates float value with custom callback.</summary>
        public static VTweenValueFloat value(float from, float to, float time, Action<float>callback)
        {
            var instance = new VTweenValueFloat();
            instance.SetBaseValues(from, to, time, callback);
            return instance;
        }
        ///<summary>Interpolates Vector3 value with custom callback.</summary>
        public static VTweenValueVector3 value(Vector3 from, Vector3 to, float time, Action<Vector3> callback)
        {
            var instance = new VTweenValueVector3();
            instance.SetBaseValues(from, to, time, callback);
            return instance;
        }
        ///<summary>Interpolates Vector2 value</summary>
        public static VTweenValueVector2 value(Vector2 from, Vector2 to, float time, Action<Vector2> callback)
        {
            var instance = new VTweenValueVector2();
            instance.SetBaseValues(from, to, time, callback);
            return instance; 
        }
        ///<summary>Interpolates Vector3 value.</summary>
        public static VTweenValueVector3 value(Vector3 from, Vector3 to, float time)
        {
            var instance = new VTweenValueVector3();
            instance.SetBaseValues(from, to, time, null);
            return instance;
        }
        ///<summary>Interpolates Vector4 value.</summary>
        public static VTweenValueVector4 value(Vector4 from, Vector4 to, float time)
        {
            var instance = new VTweenValueVector4();
            instance.SetBaseValues(from, to, time, null);
            return instance;
        }
        ///<summary>Interpolates Vector4 value.</summary>
        public static VTweenValueVector4 value(Vector4 from, Vector4 to, float time, Action<Vector4> callback)
        {
            var instance = new VTweenValueVector4();
            instance.SetBaseValues(from, to, time, callback);
            return instance;
        }
        ///<summary>Waits certain amounnt of time before executing.</summary>
        public static VTweenExecLater execLater(float time, Action callback)
        {
            var instance = new VTweenExecLater();
            instance.SetBaseValues(ref time, callback);
            instance.Init();
            return instance;
        }
        #endregion

        #region Alpha
        ///<summary>Interpolates the alpha/opacity value of a CanvasGroup or VisualElement.</summary>
        public static VTweenAlpha alpha(CanvasGroup canvasGroup, float from, float to, float time)
        {
            var instance = new VTweenAlpha();
            instance.SetBaseValues(canvasGroup, null, from, to, time);
            return instance;
        }
        ///<summary>Interpolates the alpha/opacity value of a CanvasGroup or VisualElement.</summary>
        public static VTweenAlpha alpha(VisualElement visualElement, float from, float to, float time)
        {
            var instance = new VTweenAlpha();
            instance.SetBaseValues(null, visualElement, from, to, time);
            return instance;
        }
        #endregion

        #region Color
        ///<summary>Interpolates two colors.</summary>
        public static VTweenColor color(UnityEngine.UI.Image image, Color to, float time)
        {
            var instance = new VTweenColor();
            instance.SetBaseValues(image, null, image.color, to, time);
            return instance;
        }
        ///<summary>Interpolates two colors.</summary>
        public static VTweenColor color(VisualElement visualElement, Color to, float time)
        {
            var instance = new VTweenColor();
            instance.SetBaseValues(null, visualElement, visualElement.resolvedStyle.backgroundColor, to, time);
            return instance;
        }
        #endregion
        #region Follow
        ///<summary>Follows gameObject.</summary>
        public static VTweenFollow follow(GameObject gameObject, Transform target, Vector3 smoothness, float time)
        {
            var instance = new VTweenFollow();
            instance.SetBaseValues(gameObject.transform, target, smoothness, time);
            return instance;
        }
        #endregion

        #region Shader Properties
        ///<summary>Interpolates float value.</summary>
        public static VTweenShaderFloat shaderFloat(Material material, string referenceName, float from, float to, float time)
        {
            var instance = new VTweenShaderFloat();
            instance.SetBaseValues(material, referenceName, from, to, time);
            return instance;
        }
        ///<summary>Interpolates Vector2 value.</summary>
        public static VTweenShaderVector2 shaderVector2(Material material, string referenceName, Vector2 from, Vector2 to, float time)
        {
            var instance = new VTweenShaderVector2();
            instance.SetBaseValues(material, referenceName, from, to, time);
            return instance;
        }
        ///<summary>Interpolates integer value.</summary>
        public static VTweenShaderInt shaderInt(Material material, string referenceName, int from, int to, float time)
        {
            var instance = new VTweenShaderInt();
            instance.SetBaseValues(material, referenceName, from, to, time);
            return instance;
        }
        ///<summary>Interpolates Vector3 value.</summary>
        public static VTweenShaderVector3 shaderVector3(Material material, string referenceName, Vector3 from, Vector3 to, float time)
        {
            var instance = new VTweenShaderVector3();
            instance.SetBaseValues(material, referenceName, from, to, time);
            return instance;
        }
        #endregion

        #region  Utility
        public static VTweenQueue queue{get{return new VTweenQueue();}}

        ///<summary>Frame-by-frame animation based on array legacy UI Image.</summary>
        public static VTweenAnimation animation(UnityEngine.UI.Image[] legacyImages, float time, int fps = 12)
        {
            var instance = new VTweenAnimation();
            instance.SetBaseValues(legacyImages, null, fps, time);
            instance.AssignMainEvent();
            return instance;
        }
        ///<summary>Frame-by-frame animation based on array of UIElements.Image.</summary>
        public static VTweenAnimation animation(UnityEngine.UIElements.Image[] uiimages, float time, int fps = 12)
        {
            var instance = new VTweenAnimation();
            instance.SetBaseValues(null, uiimages, fps, time);
            instance.AssignMainEvent();
            return instance;
        }
        public static int ActiveTweenCount
        {
            get
            {
                var t = VExtension.GetActiveTweens();

                if (t == null)
                    return 0;
                else
                    return t.Length;
            }
        }
        ///<summary>Returns int of total paused tween instances.</summary>
        public static int PausedTweenCount
        {
            get
            {
                var t = VExtension.GetPausedTweens();

                if (t == null)
                    return 0;
                else
                    return t.Length;
            }
        }
        ///<summary>Pauses single isntance of active tween.</summary>
        public static void Pause(VTweenClass vtween){VExtension.Pause(vtween, false);}
        ///<summary>Resume single instance of tween.</summary>
        public static void Resume(VTweenClass vtween){VExtension.Resume(vtween, false);}
        ///<summary>Resumes all tweens.</summary>
        public static void ResumeAll(){VExtension.Resume(null, true);}
        ///<summary>Pauses all tweens.</summary>
        public static void PauseAll(){VExtension.Pause(null, true);}
        ///<summary>Cancels all tweens.</summary>
        public static void CancelAll(){VExtension.Cancel(null, true);}
        ///<summary>Cancels VTween instance.</summary>
        public static void Cancel(GameObject gameObject, bool onComplete)
        {
            VExtension.Cancel(gameObject.GetInstanceID(), onComplete);
        }
        ///<summary>Cancels VTween instance.</summary>
        public static void Cancel(VisualElement visualElement, bool onComplete)
        {
            VExtension.Cancel(visualElement.GetHashCode(), onComplete);
        }
        ///<summary>Cancel single instance of active tween.</summary>
        public static void Cancel(VTweenClass vtween, bool executeOnComplete = false)
        {
            VExtension.Cancel(vtween, false);
        }

        ///<summary>Returns array of active tweens.</summary>
        private static VTweenClass[] GetActiveTween(VTweenClass t)
        {
            var val = VExtension.GetActiveTweens();
            
            if(val == null)
                return null;
            else
                return VExtension.GetActiveTweens();
        }
        ///<summary>Checks if an instance is tweening.</summary>
        public static bool IsTweening(VTweenClass vtween){return vtween.IsTweening();}
        ///<summary>Check if tween instance is tweening.</summary>
        public static bool IsTweening(int customId)
        {
            for(int i = 0; i < VTweenManager.activeTweens.Count; i++)
            {
                var val = VTweenManager.activeTweens[i];
                
                if(val  is object && val.vprops.id == customId)
                    return true;
            }

            return false;
        }
        ///<summary>Flush and resize the pools.</summary>
        public static void FlushPools(int poolSize){ VTweenManager.FlushPools(poolSize);}
        ///<summary>Resize the registers in VTweenClass.</summary>
        public static void ResizeRegisters(int newSize){VTweenManager.RegisterLength = newSize;}
        #endregion
    }
}