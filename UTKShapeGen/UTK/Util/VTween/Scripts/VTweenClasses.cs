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

///TODO: The goal here to minimize unnecessary checks as much as we can.
///No checking for position nor final value needed, thus the 0.001 magic number.

using UnityEngine;
using System;
using UnityEngine.UIElements;
using System.Collections.Generic;

//Main VTween classes....
namespace Breadnone.Extension
{
    ///<summary>Base class of VTween. Shares common properties.</summary>
    public class VTweenClass
    {
        public VTProps vprops;
        public List<EventVRegister> registers = new List<EventVRegister>(4);
        public TweenState state;
        private Action exec;
        private Action oncomplete;
        public Ease easeType;
        ///<summary>Assings scaled/unscaled time. Doing it this was so it won't do unnecessary checks every frame. I know it's ugly, sorry :)</summary>
        public void AssingTime(bool unscaled = false)
        {
            if (!unscaled)
                onUpdate(ExecRunningTimeScaled);
            else
                onUpdate(ExecRunningTimeUnscaled);
        }

        ///<summary>Adds/removes invocation of a delegate.</summary>
        public void AddClearEvent(EventVRegister register, bool falseClearTrueAdd)
        {
            if (register.id == 1)
            {
                if (falseClearTrueAdd) exec += register.callback;
                else exec -= register.callback;
            }
            else if (register.id == 2)
            {
                if (falseClearTrueAdd) oncomplete += register.callback;
                else oncomplete -= register.callback;
            }
        }
        ///<summary>Assigns callback to onComplete, onUpdate, exec.</summary>
        public void AddRegister(EventVRegister register)
        {
            registers.Add(register);
            AddClearEvent(register, true);
        }
        ///<summary>Registers init.</summary>
        public VTweenClass()
        {
            vprops = new VTProps();
        }
        public virtual void LoopReset() { }
        ///<summary>Executes scaled/unsclade runningTime.</summary>
        public void ExecRunningTimeScaled() { vprops.runningTime += Time.deltaTime; }
        public void ExecRunningTimeUnscaled() { vprops.runningTime += Time.unscaledDeltaTime; }

        ///<summary>Amount of loops a single tween.</summary>
        public VTweenClass onLoop(ref int loopCount)
        {
            vprops.loopAmount = loopCount;
            return this;
        }
        ///<summary>Back and forth, pingpong-like animation.</summary>
        public VTweenClass onPingPong(bool state)
        {
            vprops.pingpong = state;
            return this;
        }
        ///<summary>Ease function to be used with the tween.</summary>
        public VTweenClass onEase(Ease ease)
        {
            easeType = ease;
            return this;
        }
        ///<summary>Execute method chain, will be executed every frame.</summary>
        public void Exec()
        {
            if (state != TweenState.Tweening)
                return;

            if (vprops.runningTime + 0.0001f > vprops.duration)
            {
                CheckIfFinished();
                return;
            }

            exec.Invoke();
        }
        ///<summary>Will be executed at the very end, the next frame the tween completed</summary>
        public VTweenClass onComplete(Action callback)
        {
            var t = new EventVRegister { callback = callback, id = 2 };
            AddRegister(t);
            return this;
        }
        ///<summary>Callback to execute every frame while tweening.</summary>
        public VTweenClass onUpdate(Action callback)
        {
            var t = new EventVRegister { callback = callback, id = 1 };
            //Keeps running while tweening.
            AddRegister(t);
            return this;
        }
        ///<summary>Cancels the tween, returns to pool.</summary>
        public void Cancel(bool executeOnComplete = false)
        {
            if (state == TweenState.None)
                return;

            if (executeOnComplete)
            {
                oncomplete.Invoke();
            }

            if (state == TweenState.Paused)
            {
                VTweenManager.pausedTweens.Remove(this);
                Clear(false);
            }
            else
            {
                Clear(true);
            }
        }
        ///<summary>Checks if tweening already was done or still tweening</summary>
        public void CheckIfFinished()
        {
            if (vprops.loopAmount > 0)
            {
                vprops.loopCounter++;

                if (vprops.pingpong)
                {
                    if (vprops.loopCounter < vprops.loopAmount * 2)
                    {
                        vprops.runningTime = 0f;
                        LoopReset();

                        //Mods are quite fast nowadays, shouldn't be aproblem in this case
                        if (vprops.oncompleteRepeat && vprops.loopCounter % 2 == 0)
                        {
                            oncomplete?.Invoke();
                        }

                        return;
                    }
                }
                else
                {
                    if (vprops.loopAmount > vprops.loopCounter)
                    {
                        vprops.runningTime = 0f;
                        LoopReset();

                        //Mods are quite fast nowadays, shouldn't be aproblem in this case
                        if (vprops.oncompleteRepeat && vprops.loopCounter % 2 == 0)
                        {
                            oncomplete?.Invoke();
                        }

                        return;
                    }
                }
            }

            oncomplete?.Invoke();
            Clear(true);
        }
        ///<summary>Set common properties to default value.</summary>
        public void Clear(bool removeFromActiveList)
        {
            for (int i = 0; i < registers.Count; i++)
            {
                var t = registers[i];
                AddClearEvent(t, false);
            }

            if (removeFromActiveList)
            {
                VTweenManager.RemoveFromActiveTween(this);
                this.state = TweenState.None;
            }

            registers.Clear();
        }
        ///<summary>Sets to default for re-use purposes.</summary>
        public void DefaultProperties() { vprops.SetDefault(); }
        ///<summary>Checks if tweening.</summary>
        public bool IsTweening() { return state != TweenState.None; }
        ///<summary>Pauses the tweening.</summary>
        public void Pause()
        {
            if (state != TweenState.Tweening)
                return;

            VTweenManager.PoolToPaused(this);
        }
        ///<summary>Resumes paused tween instances, if any.</summary>
        public void Resume()
        {
            if (state != TweenState.Paused)
                return;

            VTweenManager.UnPoolPaused(this);
        }
    }
    ///<summary>Move class. Moves object to target position.</summary>
    public sealed class VTweenMove : VClass<VTweenMove>
    {
        private Vector3 destination;
        private Transform fromIns;
        private Vector3 defaultPosition;
        ///<summary>Sets base values that aren't common properties of the base class.</summary>
        public void SetBaseValues(Transform trans, ref Vector3 dest, ref float time)
        {
            destination = dest;
            fromIns = trans;
            defaultPosition = trans.position;
            vprops.duration = time;

            void callback() { fromIns.position = Utilv.Vec3Lerp(easeType, ref defaultPosition, ref destination, vprops.runningTime / vprops.duration); }
            void callbackLocal() { fromIns.localPosition = Utilv.Vec3Lerp(easeType, ref defaultPosition, ref destination, vprops.runningTime / vprops.duration); }

            if (!vprops.isLocal)
            {
                var t = new EventVRegister { callback = callback, id = 1 };
                AddRegister(t);
            }
            else
            {
                var t = new EventVRegister { callback = callbackLocal, id = 1 };
                AddRegister(t);
            }

            Action oncomp = () =>
            {
                if (vprops.pingpong)
                {
                    fromIns.position = defaultPosition;
                }
                else
                {
                    fromIns.position = destination;
                }
            };

            onComplete(oncomp);
            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Shuffling the to/from properties.</summary>
        public override void LoopReset()
        {
            if (fromIns is object)
            {
                if (!vprops.isLocal)
                {
                    if (!vprops.pingpong)
                    {
                        fromIns.transform.position = defaultPosition;
                    }
                    else
                        fromIns.transform.position = destination;
                }
                else
                {
                    if (!vprops.pingpong)
                    {
                        fromIns.transform.localPosition = defaultPosition;
                    }
                    else
                        fromIns.transform.localPosition = destination;
                }
            }

            if (vprops.pingpong) { Utilv.SwapRefs<Vector3>(ref destination, ref defaultPosition); }
        }
        ///<summary>Updates position of gameObject when chaining. Only for chaining purposes.</summary>
        public void UpdatePos()
        {
            for (int i = 0; i < registers.Count; i++)
            {
                if (registers[i].id == 1)
                {
                    var t = registers[i];
                    AddClearEvent(t, false);
                    break;
                }
            }

            vprops.runningTime = 0;
            VTweenManager.activeTweens.Remove(this);
            var pos = fromIns.transform.position;
            var dur = vprops.duration;
            SetBaseValues(fromIns, ref destination, ref dur);
        }
        ///<summary>Repositioning initial position of object.</summary>
        public VTweenMove setFrom(Vector3 fromPosition)
        {
            if (!vprops.isLocal)
                fromIns.position = fromPosition;
            else
                fromIns.localPosition = fromPosition;

            defaultPosition = fromPosition;
            return this;
        }
        ///<summary>Sets target to look at while moving.</summary>
        public VTweenMove setLookAt(Vector3 targetToLookAt)
        {
            void evt() { fromIns.transform.LookAt(targetToLookAt); }
            var t = new EventVRegister { callback = evt, id = 1 };
            AddRegister(t);
            return this;
        }
        ///<summary>Destroys gameObject when completed. VisualElements will be removed from parents hierarchy.</summary>
        public override VTweenMove setDestroy(bool state)
        {
            if (!state)
                return this;

            void callback() { GameObject.Destroy(fromIns.gameObject); }
            var t = new EventVRegister { callback = callback, id = 2 };
            AddRegister(t);
            return this;
        }
        ///<summary>Sets gameObject to be inactive. Equal to SetActive(state).</summary>
        public override VTweenMove setInactiveOnComplete(bool state)
        {
            if (!state)
                return this;

            void callback() { fromIns.gameObject.SetActive(state); }
            AddRegister(new EventVRegister { callback = callback, id = 2 });
            return this;
        }
    }
    ///<summary>Move class. Moves object to target position.</summary>
    public sealed class VTweenMoveUI : VClass<VTweenMoveUI>
    {
        private Vector3 destination;
        private VisualElement fromInsUi;
        private Vector3 defaultPosition;
        ///<summary>Sets base values that aren't common properties of the base class.</summary>
        public void SetBaseValues(VisualElement istyle, ref Vector3 dest, ref float time)
        {
            destination = dest;
            fromInsUi = istyle;
            defaultPosition = istyle.resolvedStyle.translate;
            vprops.duration = time;

            void callbackUi()
            {
                var tmp = Utilv.Vec3Lerp(easeType, ref defaultPosition, ref destination, vprops.runningTime / vprops.duration);
                fromInsUi.style.translate = new Translate(tmp.x, tmp.y, tmp.z);
            }

            Action oncomp = () =>
            {
                if (vprops.pingpong)
                {
                    fromInsUi.style.translate = new Translate(defaultPosition.x, defaultPosition.y, defaultPosition.z);
                }
                else
                {
                    fromInsUi.style.translate = new Translate(destination.x, destination.y, destination.z);
                }
            };

            onComplete(oncomp);

            var t = new EventVRegister { callback = callbackUi, id = 1 };
            AddRegister(t);
            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Shuffling the to/from properties.</summary>
        public override void LoopReset()
        {
            if (!vprops.pingpong)
                fromInsUi.style.translate = new Translate(defaultPosition.x, defaultPosition.y, defaultPosition.z);
            else
                fromInsUi.style.translate = new Translate(destination.x, destination.y, destination.z);

            if (vprops.pingpong)
            {
                Utilv.SwapRefs<Vector3>(ref destination, ref defaultPosition);
            }
        }
        public void UpdatePos()
        {
            for (int i = 0; i < registers.Count; i++)
            {
                if (registers[i].id == 1)
                {
                    var t = registers[i];
                    AddClearEvent(t, false);
                    break;
                }
            }

            vprops.runningTime = 0;
            VTweenManager.activeTweens.Remove(this);
            var dur = vprops.duration;
            SetBaseValues(fromInsUi, ref destination, ref dur);
        }
        ///<summary>Repositioning initial position of object.</summary>
        public VTweenMoveUI setFrom(Vector3 fromPosition)
        {
            fromInsUi.style.translate = new Translate(fromPosition.x, fromPosition.y, fromPosition.z);
            defaultPosition = fromPosition;
            return this;
        }
        ///<summary>Destroys gameObject when completed. VisualElements will be removed from parents hierarchy.</summary>
        public override VTweenMoveUI setDestroy(bool state)
        {
            if (!state)
                return this;

            void callback() { ((VisualElement)fromInsUi).RemoveFromHierarchy(); }
            var t = new EventVRegister { callback = callback, id = 2 };
            AddRegister(t);
            return this;
        }
        ///<summary>Sets object to be inactive. For visualElements this equal to SetEnabled(false).</summary>
        public override VTweenMoveUI setInactiveOnComplete(bool state)
        {
            if (!state)
                return this;

            void callback() { fromInsUi.SetEnabled(false); }
            AddRegister(new EventVRegister { callback = callback, id = 2 });
            return this;
        }
    }
    ///<summary>Rotates object</summary>
    public sealed class VTweenRotate : VClass<VTweenRotate>
    {
        private float degreeAngle;
        private Transform transform;
        private ITransform itransform;
        private Quaternion defaultRotation;
        private Quaternion currentRotation;
        ///<summary>Sets base values that aren't common properties of the base class.</summary>
        public void SetBaseValues(Transform trans, ITransform itrans, ref float angle, Vector3 direction, ref float time)
        {
            transform = trans;
            degreeAngle = angle;
            vprops.duration = time;
            defaultRotation = trans.rotation;
            itransform = itrans;

            if (transform is object)
            {
                if (!vprops.isLocal)
                    currentRotation = trans.rotation * Quaternion.AngleAxis(degreeAngle, direction);
                else
                    currentRotation = trans.localRotation * Quaternion.AngleAxis(degreeAngle, direction);

                Action oncomp = () =>
                {
                    if (vprops.pingpong)
                    {
                        trans.rotation = defaultRotation;
                    }
                    else
                    {
                        trans.rotation = currentRotation;
                    }
                };

                onComplete(oncomp);
            }
            else if (itransform is object)
            {
                currentRotation = itrans.rotation * Quaternion.AngleAxis(degreeAngle, direction);
            }

            var zero = 0f;
            void callback() { trans.rotation = Quaternion.AngleAxis(Utilv.FloatLerp(easeType, ref zero, ref degreeAngle, vprops.runningTime / vprops.duration), direction); }
            void callbackLocal() { trans.localRotation = Quaternion.AngleAxis(Utilv.FloatLerp(easeType, ref zero, ref degreeAngle, vprops.runningTime / vprops.duration), direction); }
            void callbackUi() { itrans.rotation = Quaternion.AngleAxis(Utilv.FloatLerp(easeType, ref zero, ref degreeAngle, vprops.runningTime / vprops.duration), direction); }

            if (trans is object)
            {
                if (!vprops.isLocal)
                {
                    var t = new EventVRegister { callback = callback, id = 1 };
                    AddRegister(t);
                }

                else
                {
                    var t = new EventVRegister { callback = callbackLocal, id = 1 };
                    AddRegister(t);
                }
            }
            else
            {
                var t = new EventVRegister { callback = callbackUi, id = 1 };
                AddRegister(t);
            }

            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Resets properties shuffle the destination</summary>
        public override void LoopReset()
        {
            if (transform is object)
            {
                if (!vprops.isLocal)
                {
                    transform.transform.rotation = currentRotation;
                }
                else
                {
                    transform.transform.localRotation = currentRotation;
                }
            }
            else if (itransform is object)
            {
                itransform.rotation = currentRotation;
            }

            if (vprops.pingpong)
            {
                degreeAngle = -degreeAngle;
            }
        }
        ///<summary>Sets target transform to look at while rotating.</summary>
        public VTweenRotate setLookAt(Transform target, Vector3 direction)
        {
            void Upd()
            {
                if (transform is object)
                {
                    var relativePos = target.position - transform.position;
                    transform.rotation = Quaternion.LookRotation(relativePos, direction);
                }
                else if (itransform is object)
                {
                    var relativePos = target.position - itransform.position;
                    itransform.rotation = Quaternion.LookRotation(relativePos, direction);
                }
            }

            onUpdate(Upd);
            return this;
        }
        ///<summary>Repositioning initial position of object.</summary>
        public VTweenRotate setFrom(float fromAngle, Vector3 direction)
        {
            if (transform is object)
            {
                if (!vprops.isLocal)
                {
                    transform.rotation = Quaternion.AngleAxis(fromAngle, direction);
                    defaultRotation = transform.rotation;
                }
                else
                {
                    transform.localRotation = Quaternion.AngleAxis(fromAngle, direction);
                    defaultRotation = transform.localRotation;
                }
            }
            else if (itransform is object)
            {
                defaultRotation = itransform.rotation;
            }

            return this;
        }
        ///<summary>Destroys gameObject/VisualElement on completion.</summary>
        public override VTweenRotate setDestroy(bool state)
        {
            if (!state)
                return this;

            if (transform is object)
            {
                void callback() { GameObject.Destroy(transform.gameObject); }
                var t = new EventVRegister { callback = callback, id = 2 };
                AddRegister(t);
            }
            else
            {
                void callback() { (itransform as VisualElement).RemoveFromHierarchy(); }
                var t = new EventVRegister { callback = callback, id = 2 };
                AddRegister(t);
            }
            return this;
        }
    }
    ///<summary>Interpolates float value of a shader property.</summary>
    public sealed class VTweenShaderFloat : VClass<VTweenShaderFloat>
    {
        private float from;
        private float to;
        private Material mat;
        private string refName;
        ///<summary>Sets base values that aren't common properties of the base class.</summary>
        public void SetBaseValues(Material material, string shaderReferenceName, float fromValue, float toValue, float time)
        {
            vprops.duration = time;
            from = fromValue;
            to = toValue;
            mat = material;
            refName = shaderReferenceName;

            if (!material.HasFloat(shaderReferenceName))
            {
                throw new VTweenException("No reference named " + shaderReferenceName + " in the material/shader.");
            }

            void callback()
            {
                material.SetFloat(shaderReferenceName, Utilv.FloatLerp(easeType, ref from, ref to, vprops.runningTime / vprops.duration));
            }

            var t = new EventVRegister { callback = callback, id = 1 };
            AddRegister(t);
            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Resets properties shuffle the destination</summary>
        public override void LoopReset()
        {
            if (!vprops.pingpong)
            {
                mat.SetFloat(refName, from);
            }
            else
            {
                Utilv.SwapRefs<float>(ref to, ref from);
            }
        }
    }
    ///<summary>Interpolates integer value of a shader property.</summary>
    public sealed class VTweenShaderInt : VClass<VTweenShaderInt>
    {
        private int from;
        private int to;
        private Material mat;
        private string refName;
        ///<summary>Sets base values that aren't common properties of the base class.</summary>
        public void SetBaseValues(Material material, string shaderReferenceName, int fromValue, int toValue, float time)
        {
            vprops.duration = time;
            from = fromValue;
            to = toValue;
            mat = material;
            refName = shaderReferenceName;

            if (!material.HasInt(shaderReferenceName))
            {
                throw new VTweenException("No reference named " + shaderReferenceName + " in the material/shader.");
            }

            void callback()
            {
                var frm = (float)from;
                var tt = (float)to;

                material.SetInt(shaderReferenceName, (int)Utilv.FloatLerp(easeType, ref frm, ref tt, vprops.runningTime / vprops.duration));
            }

            var t = new EventVRegister { callback = callback, id = 1 };
            AddRegister(t);
            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Resets properties shuffle the destination</summary>
        public override void LoopReset()
        {
            if (!vprops.pingpong)
            {
                mat.SetFloat(refName, from);
            }
            else if (vprops.pingpong)
            {
                var dest = to;
                to = from;
                from = dest;
            }
        }
    }
    ///<summary>Interpolates Vector3 value.</summary>
    public sealed class VTweenShaderVector3 : VClass<VTweenShaderVector3>
    {
        private Vector3 from;
        private Vector3 to;
        private Material mat;
        private string refName;
        ///<summary>Sets Vector3 reference in the shader.</summary>
        public void SetBaseValues(Material material, string shaderReferenceName, Vector3 fromValue, Vector3 toValue, float time)
        {
            vprops.duration = time;
            from = fromValue;
            to = toValue;
            mat = material;
            refName = shaderReferenceName;

            if (!material.HasVector(shaderReferenceName))
            {
                throw new VTweenException("No reference named " + shaderReferenceName + " in the material/shader.");
            }

            void callback()
            {
                material.SetVector(shaderReferenceName, Utilv.Vec3Lerp(easeType, ref from, ref to, vprops.runningTime / vprops.duration));
            }

            var t = new EventVRegister { callback = callback, id = 1 };
            AddRegister(t);
            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Interpolates Vector3 reference in the shader/material</summary>
        public override void LoopReset()
        {
            if (!vprops.pingpong)
            {
                mat.SetVector(refName, from);
            }
            else if (vprops.pingpong)
            {
                Utilv.SwapRefs<Vector3>(ref to, ref from);
            }
        }
    }
    ///<summary>Interpolates Vector2 value.</summary>
    public sealed class VTweenShaderVector2 : VClass<VTweenShaderVector2>
    {
        private Vector2 from;
        private Vector2 to;
        private Material mat;
        private string refName;
        ///<summary>Sets Vector3 reference in the shader.</summary>
        public void SetBaseValues(Material material, string shaderReferenceName, Vector2 fromValue, Vector2 toValue, float time)
        {
            vprops.duration = time;
            from = fromValue;
            to = toValue;
            mat = material;
            refName = shaderReferenceName;

            if (!material.HasVector(shaderReferenceName))
            {
                throw new VTweenException("No reference named " + shaderReferenceName + " in the material/shader.");
            }

            void callback()
            {
                material.SetVector(shaderReferenceName, Utilv.Vec2Lerp(easeType, ref from, ref to, vprops.runningTime / vprops.duration));
            }

            var t = new EventVRegister { callback = callback, id = 1 };
            AddRegister(t);
            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Interpolates Vector3 reference in the shader/material</summary>
        public override void LoopReset()
        {
            if (!vprops.pingpong)
            {
                mat.SetVector(refName, from);
            }
            else if (vprops.pingpong)
            {
                Utilv.SwapRefs<Vector2>(ref to, ref from);
            }
        }
    }
    ///<summary>Interpolates float reference in the shader/material.</summary>
    public sealed class VTweenValueFloat : VClass<VTweenValueFloat>
    {
        private float from;
        private float to;
        private float runningValue;

        ///<summary>Sets base values that aren't common properties of the base class.</summary>
        public void SetBaseValues(float fromValue, float toValue, float time, Action<float> callbackEvent)
        {
            vprops.duration = time;
            from = fromValue;
            to = toValue;

            void callback()
            {
                runningValue = Utilv.FloatLerp(easeType, ref from, ref to, vprops.runningTime / vprops.duration);

                if (callbackEvent is object)
                {
                    callbackEvent.Invoke(runningValue);
                }
            }

            var t = new EventVRegister { callback = callback, id = 1 };
            AddRegister(t);
            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Resets properties shuffle the destination</summary>
        public override void LoopReset()
        {
            if (!vprops.pingpong)
            {
                runningValue = from;
            }
            else if (vprops.pingpong)
            {
                Utilv.SwapRefs<float>(ref to, ref from);
                runningValue = from;
            }
        }
    }
    ///<summary>Interpolates Vector3 value.</summary>
    public sealed class VTweenValueVector2 : VClass<VTweenValueVector2>
    {
        private Vector2 from;
        private Vector2 to;
        private Vector2 runningValue;
        ///<summary>Sets base values that aren't common properties of the base class.</summary>
        public void SetBaseValues(Vector2 fromValue, Vector2 toValue, float time, Action<Vector2> callbackEvent)
        {
            runningValue = Vector2.zero;
            vprops.duration = time;
            from = fromValue;
            to = toValue;

            void callback()
            {
                runningValue = Utilv.Vec2Lerp(easeType, ref from, ref to, vprops.runningTime / vprops.duration);

                if (callbackEvent is object)
                {
                    callbackEvent.Invoke(runningValue);
                }
            }

            var t = new EventVRegister { callback = callback, id = 1 };
            AddRegister(t);
            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Resets properties shuffle the destination</summary>
        public override void LoopReset()
        {
            if (!vprops.pingpong)
            {
                runningValue = from;
            }
            else if (vprops.pingpong)
            {
                Utilv.SwapRefs<Vector2>(ref to, ref from);
                runningValue = from;
            }
        }
    }
    ///<summary>Interpolates Vector3 value.</summary>
    public sealed class VTweenValueVector3 : VClass<VTweenValueVector3>
    {
        private Vector3 from;
        private Vector3 to;
        private Vector3 runningValue;
        ///<summary>Sets base values that aren't common properties of the base class.</summary>
        public void SetBaseValues(Vector3 fromValue, Vector3 toValue, float time, Action<Vector3> callbackEvent)
        {
            runningValue = Vector3.zero;
            vprops.duration = time;
            from = fromValue;
            to = toValue;

            void callback()
            {
                runningValue = Utilv.Vec3Lerp(easeType, ref from, ref to, vprops.runningTime / vprops.duration);

                if (callbackEvent is object)
                {
                    callbackEvent.Invoke(runningValue);
                }
            }

            var t = new EventVRegister { callback = callback, id = 1 };
            AddRegister(t);
            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Resets properties shuffle the destination</summary>
        public override void LoopReset()
        {
            if (!vprops.pingpong)
            {
                runningValue = from;
            }
            else if (vprops.pingpong)
            {
                Utilv.SwapRefs<Vector3>(ref to, ref from);
                runningValue = from;
            }
        }
    }
    ///<summary>Interpolates Vector4 value.</summary>
    public sealed class VTweenValueVector4 : VClass<VTweenValueVector4>
    {
        private Vector4 from;
        private Vector4 to;
        private Vector4 runningValue;
        ///<summary>Sets base values that aren't common properties of the base class.</summary>
        public void SetBaseValues(Vector4 fromValue, Vector4 toValue, float time, Action<Vector4> callbackEvent)
        {
            runningValue = Vector4.zero;
            vprops.duration = time;
            from = fromValue;
            to = toValue;

            void callback()
            {
                runningValue = Utilv.Vec4Lerp(easeType, ref from, ref to, vprops.runningTime / vprops.duration);

                if (callbackEvent is object)
                {
                    callbackEvent.Invoke(runningValue);
                }
            }

            var t = new EventVRegister { callback = callback, id = 1 };
            AddRegister(t);
            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Resets properties shuffle the destination</summary>
        public override void LoopReset()
        {
            if (!vprops.pingpong)
            {
                runningValue = from;
            }
            else if (vprops.pingpong)
            {
                Utilv.SwapRefs<Vector4>(ref to, ref from);
                runningValue = from;
            }
        }
    }
    ///<summary>Scales object. Maclass.</summary>
    public sealed class VTweenScale : VClass<VTweenScale>
    {
        private Vector3 defaultScale;
        private Vector3 targetScale;
        private Transform transform;
        private IStyle itransform;
        ///<summary>Sets base values that aren't common properties of the base class.</summary>
        public void SetBaseValues(Transform trans, IStyle itrans, Vector3 destScale, Vector3 defScale, float time)
        {
            transform = trans;
            itransform = itrans;
            defaultScale = defScale;
            targetScale = destScale;
            vprops.duration = time;

            void callback() { transform.localScale = Utilv.Vec3Lerp(easeType, ref defaultScale, ref targetScale, vprops.runningTime / vprops.duration); }
            void callbackUi() { itransform.scale = new Scale(Utilv.Vec3Lerp(easeType, ref defaultScale, ref targetScale, vprops.runningTime / vprops.duration)); }

            if (transform is object)
            {
                var t = new EventVRegister { callback = callback, id = 1 };
                AddRegister(t);
            }
            else
            {
                var t = new EventVRegister { callback = callbackUi, id = 1 };
                AddRegister(t);
            }

            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Resets properties shuffle the destination</summary>
        public override void LoopReset()
        {
            if (!vprops.pingpong)
            {
                if (transform is object)
                {
                    transform.localScale = defaultScale;
                }
                else if (itransform is object)
                {
                    itransform.scale = new Scale(defaultScale);
                }
            }
            else
            {
                Utilv.SwapRefs<Vector3>(ref targetScale, ref defaultScale);
            }
        }
        ///<summary>Destroys gameObject/VisualElement on completion.</summary>
        public override VTweenScale setDestroy(bool state)
        {
            if (!state)
                return this;

            if (transform is object)
            {
                void callback() { GameObject.Destroy(transform.gameObject); }
                var t = new EventVRegister { callback = callback, id = 2 };
                AddRegister(t);
            }
            else
            {
                void callback() { ((VisualElement)itransform).RemoveFromHierarchy(); }
                var t = new EventVRegister { callback = callback, id = 2 };
                AddRegister(t);
            }

            return this;
        }
    }
    ///<summary>Sets alpha value of a Canvas or the opacity of a VisualeElement.</summary>
    public sealed class VTweenAlpha : VClass<VTweenAlpha>
    {
        private CanvasGroup canvyg;
        private VisualElement visualElement;
        private float fromValue;
        private float toValue;
        ///<summary>Sets base values that aren't common properties of the base class.</summary>
        public void SetBaseValues(CanvasGroup canvas, VisualElement visualelement, float from, float to, float time)
        {
            canvyg = canvas;
            visualElement = visualelement;
            vprops.duration = time;

            if (from < 0)
                from = 0;

            if (to > 1)
                to = 1;

            fromValue = from;
            toValue = to;

            void callback()
            {
                canvyg.alpha = Utilv.FloatLerp(easeType, ref fromValue, ref toValue, vprops.runningTime / vprops.duration);
            }

            void callbackUi()
            {
                visualElement.style.opacity = Utilv.FloatLerp(easeType, ref fromValue, ref toValue, vprops.runningTime / vprops.duration);
            }

            if (canvyg is object)
            {
                var t = new EventVRegister { callback = callback, id = 1 };
                AddRegister(t);
            }
            else
            {
                var t = new EventVRegister { callback = callbackUi, id = 1 };
                AddRegister(t);
            }

            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Resets properties shuffle the destination</summary>
        public override void LoopReset()
        {
            if (!vprops.pingpong)
            {
                if (canvyg is object)
                {
                    canvyg.alpha = fromValue;
                }
                else if (visualElement is object)
                {
                    visualElement.style.opacity = fromValue;
                }
            }
            else
            {
                Utilv.SwapRefs<float>(ref toValue, ref fromValue);
            }
        }
    }
    ///<summary>Sets alpha value of a Canvas or the opacity of a VisualeElement.</summary>
    public sealed class VTweenColor : VClass<VTweenColor>
    {
        private UnityEngine.UI.Image image;
        private VisualElement visualElement;
        private Color fromValue;
        private Color toValue;
        ///<summary>Sets base values that aren't common properties of the base class.</summary>
        public void SetBaseValues(UnityEngine.UI.Image uiimage, VisualElement visualelement, Color from, Color to, float time)
        {
            image = uiimage;
            visualElement = visualelement;
            vprops.duration = time;
            fromValue = from;
            toValue = to;

            void callback()
            {
                Vector4 vecFrom = fromValue;
                Vector4 vecTo = toValue;
                image.color = Utilv.Vec4Lerp(easeType, ref vecFrom, ref vecTo, vprops.runningTime / vprops.duration);
            }

            void callbackUi()
            {
                Vector4 vecFrom = fromValue;
                Vector4 vecTo = toValue;
                visualElement.style.backgroundColor = new StyleColor(Utilv.Vec4Lerp(easeType, ref vecFrom, ref vecTo, vprops.runningTime / vprops.duration));
            }

            if (image is object)
            {
                var t = new EventVRegister { callback = callback, id = 1 };
                AddRegister(t);
            }
            else
            {
                var t = new EventVRegister { callback = callbackUi, id = 1 };
                AddRegister(t);
            }

            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Resets properties shuffle the destination</summary>
        public override void LoopReset()
        {
            if (!vprops.pingpong)
            {
                if (image is object)
                {
                    image.color = fromValue;
                }
                else if (visualElement is object)
                {
                    visualElement.style.backgroundColor = fromValue;
                }
            }
            else
            {
                Utilv.SwapRefs<Color>(ref toValue, ref fromValue);
            }
        }
    }
    //TODO Needs more testing!
    ///<summary>Frame-by-frame animation of array of images for both legacy and UIElements.Image.</summary>
    public sealed class VTweenAnimation : VClass<VTweenAnimation>
    {
        private UnityEngine.UI.Image[] images;
        private UnityEngine.UIElements.Image[] uiImages;
        private int fps = 12;
        private int runningIndex;
        private int prevFrame;
        ///<summary>Sets base values that aren't common properties of the base class. Default sets to 12 frame/second. Use setFps for custom frame per second.</summary>
        public void SetBaseValues(UnityEngine.UI.Image[] legacyimages, UnityEngine.UIElements.Image[] uiimages, int? framePerSecond, float time)
        {
            vprops.duration = time;
            images = legacyimages;
            uiImages = uiimages;

            if (framePerSecond.HasValue)
            {
                fps = framePerSecond.Value;
            }

            vprops.duration = time;
        }
        ///<summary>Resets properties shuffle the destination</summary>
        public override void LoopReset()
        {
            if (runningIndex == images.Length)
            {
                if (!vprops.pingpong)
                {
                    Array.Reverse(images);
                }
                runningIndex = 0;
            }
        }
        ///<summary>Sets color of an object.</summary>
        private void SetColor(bool show)
        {
            if (images is object)
            {
                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i] == null)
                        continue;

                    images[i].gameObject.SetActive(show);
                }
            }
            else if (uiImages is object)
            {
                for (int i = 0; i < uiImages.Length; i++)
                {
                    if (uiImages[i] == null)
                        continue;

                    if (show)
                        uiImages[i].style.display = DisplayStyle.Flex;
                    else
                        uiImages[i].style.display = DisplayStyle.None;
                }
            }
        }
        ///<summary>Main even assignment of Exec method, refers to base class.</summary>
        public void AssignMainEvent()
        {
            SetColor(false);

            void callback()
            {
                if ((prevFrame + fps) > Time.frameCount)
                    return;
                else
                {
                    prevFrame = Time.frameCount;
                }

                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i] == null)
                        continue;

                    if (i == runningIndex)
                    {
                        images[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        images[i].gameObject.SetActive(false);
                    }
                }

                runningIndex++;
            }

            void callbackUi()
            {
                if ((prevFrame + fps) > Time.frameCount)
                    return;
                else
                {
                    prevFrame = Time.frameCount;
                }

                for (int i = 0; i < uiImages.Length; i++)
                {
                    if (i == runningIndex)
                    {
                        uiImages[i].style.display = DisplayStyle.Flex;
                    }
                    else
                    {
                        uiImages[i].style.display = DisplayStyle.None;
                    }
                }

                runningIndex++;
            }

            if (images is object)
            {
                var t = new EventVRegister { callback = callback, id = 1 };
                AddRegister(t);
            }

            else
            {
                var t = new EventVRegister { callback = callbackUi, id = 1 };
                AddRegister(t);
            }

            VTweenManager.InsertToActiveTween(this);
        }
        ///<summary>Sets frame-per-second used for the timing.</summary>
        public VTweenAnimation setFps(int framePerSecond)
        {
            fps = framePerSecond;
            return this;
        }
        ///<summary>Sets active frames for the animation.</summary>
        public VTweenAnimation setActiveFrame(int index)
        {
            runningIndex = index;
            return this;
        }
        ///<summary>Sets disable an object once the tween completed.</summary>
        public VTweenAnimation setDisableOnComplete(bool state)
        {
            if (state)
            {
                void act() { SetColor(false); };
                var tx = new EventVRegister { callback = act, id = 2 };
                AddRegister(tx);
            }

            return this;
        }
    }
    ///<summary>Follows target object. Custom dampening can be applied.</summary>
    public sealed class VTweenFollow : VClass<VTweenFollow>
    {
        private Vector3 smoothTime;
        private float speedTime;

        ///<summary>Sets base values that aren't common properties of the base class.</summary>
        public void SetBaseValues(Transform trans, Transform targetTransform, Vector3 smoothness, float speed)
        {
            smoothTime = smoothness;
            speedTime = speed;

            void callback()
            {
                trans.position = Vector3.SmoothDamp(trans.position, targetTransform.position, ref smoothTime, speedTime);
            }

            var t = new EventVRegister { callback = callback, id = 1 };
            AddRegister(t);
            VTweenManager.InsertToActiveTween(this);
        }
    }

    ///<summary>State of the tweening instance.</summary>
    public enum TweenState
    {
        Paused,
        Tweening,
        None
    }
    ///<summary>Base abstract class.</summary>
    public abstract class VClass<T> : VTweenClass where T : VTweenClass
    {
        public virtual T setInactiveOnComplete(bool state)
        {
            return this as T;
        }
        ///<summary>Will be called when tweening was completed</summary>
        public T setOnComplete(Action callback)
        {
            this.onComplete(callback);
            return this as T;
        }
        ///<summary>Will be called every frame while tweening.</summary>
        public T setOnUpdate(Action callback)
        {
            this.onUpdate(callback);
            return this as T;
        }
        ///<summary>Easing type.</summary>
        public T setEase(Ease ease)
        {
            this.onEase(ease);
            return this as T;
        }
        ///<summary>Loop count for each tween instance.</summary>
        public T setLoop(int loopCount)
        {
            this.onLoop(ref loopCount);
            return this as T;
        }
        ///<summary>Whether it will be affacted by Time.timeScale or not.</summary>
        public T setUnscaledTime(bool state)
        {
            if (state)
            {
                this.AddRegister(new EventVRegister { callback = this.ExecRunningTimeUnscaled, id = 1 });
                this.AssingTime(true);
            }

            return this as T;
        }
        ///<summary>Back and forth or pingpong-like interpolation.</summary>
        public T setPingPong(bool state)
        {
            this.onPingPong(state);
            return this as T;
        }
        ///<summary>Back and forth or pingpong-like interpolation.</summary>
        public T setOnCompleteRepeat(bool state)
        {
            this.vprops.oncompleteRepeat = state;
            return this as T;
        }
        ///<summary>Delays startup execution.</summary>
        public T setDelay(float delayTime)
        {
            if (delayTime > 0f)
            {
                this.vprops.delayedTime = delayTime;
            }

            for (int i = 0; i < registers.Count; i++)
            {
                if (registers[i].id == 1)
                {
                    var t = registers[i];
                    AddClearEvent(t, false);

                    void delayed()
                    {
                        //Wait for delayed time to be 0.
                        if (vprops.delayedTime.HasValue && vprops.delayedTime.Value > 0)
                        {
                            vprops.delayedTime -= Time.deltaTime;
                            return;
                        }

                        registers[i].callback.Invoke();
                    };

                    var tt = new EventVRegister { callback = delayed, id = 1 };
                    AddRegister(tt);
                    break;
                }
            }
            return this as T;
        }
        ///<summary>Destroys gameObject when completed (if it's a visualElement, it will be removed from the hierarchy).</summary>
        public virtual T setDestroy(bool state) { return this as T; }
    }
    ///<summary>VTweenClass events/delegates.</summary>
    public sealed class EventVRegister
    {
        public Action callback;
        public int id;
    }
    ///<summary>VTweenClass events/delegates.</summary>
    public struct StructVRegister
    {
        public Action callback;
        public int id;
    }
    ///<summary>Common properties of VTweenClass.</summary>
    public sealed class VTProps
    {
        public int id { get; set; }
        public int loopAmount { get; set; }
        public int loopCounter { get; set; }
        public bool isLocal { get; set; }
        public bool pingpong { get; set; }
        public float duration { get; set; }
        public float runningTime { get; set; }
        public float? delayedTime { get; set; }
        public bool oncompleteRepeat { get; set; }
        ///<summary>Sets to default value to be reused in a pool. If not then will be normally disposed.</summary>
        public void SetDefault()
        {
            id = 0;
            loopAmount = 0;
            isLocal = false;
            pingpong = false;
            loopCounter = 0;
            duration = 0;
            runningTime = 0;
            delayedTime = null;
            oncompleteRepeat = false;
        }
    }
}