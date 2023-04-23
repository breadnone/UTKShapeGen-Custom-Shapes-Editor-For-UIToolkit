using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    public static class UTKTweenManager
    {
        public static Dictionary<int, UTKTween> runningTweens = new();
    }
    public static class UTKTweenUtil
    {
        public static void BuildLists(VisualElement visualElement, bool build)
        {
            var stylo = visualElement.style;

            if(build)
            {
                if(stylo.transitionDuration.value == null)
                    stylo.transitionDuration = new StyleList<TimeValue>(new List<TimeValue>());
                if(stylo.transitionProperty.value == null)
                    stylo.transitionProperty = new StyleList<StylePropertyName>(new List<StylePropertyName>());
                if(stylo.transitionDelay.value == null)
                    stylo.transitionDelay = new StyleList<TimeValue>(new List<TimeValue>());
                if(stylo.transitionTimingFunction.value == null)
                    stylo.transitionTimingFunction = new StyleList<EasingFunction>(new List<EasingFunction>());
            }
            else
            {
                stylo.transitionDuration = null;
                stylo.transitionProperty = null;
                stylo.transitionDelay = null;
                stylo.transitionTimingFunction = null;
            }
        }
    }
    public class UTKTween
    {
        public VisualElement cachedUIElement { get; set; }
        public int id { get; set; }
        public Action onRunning = null;
        public Action onComplete = null;
        public int loopCount = 0;
        public int loopCounter = 0;
        public bool pingPong = false;
        
        public void Cancel(bool execOncomplete = false)
        {
            if (execOncomplete)
                onComplete?.Invoke();
        }

        public static UTKTween Scale(VisualElement visualElement, Scale scale, float duration, float delay, TimeUnit timeUnit = TimeUnit.Second, EasingMode ease = EasingMode.Linear)
        {
            var utk = new UTKTween();
            utk.id = visualElement.GetHashCode();
            UTKTweenManager.runningTweens.TryAdd(utk.id, utk);
            utk.cachedUIElement = visualElement;

            float oldDuration = duration;
            float oldDelay = delay;
            bool init = false;

            var dur = new TimeValue(duration, timeUnit);
            var del = new TimeValue(delay, timeUnit);
            var prop = new StylePropertyName("scale");
            var eas = new EasingFunction(ease);

            Vector3 destination = scale.value;
            Vector3 oldState = visualElement.resolvedStyle.scale.value;

            void AssingProperties(bool reset = false)
            {
                UTKTweenUtil.BuildLists(visualElement, true);
                
                if(init)
                {
                    visualElement.style.transitionDuration.value.Remove(dur);
                    visualElement.style.transitionProperty.value.Remove(prop);
                    visualElement.style.transitionDelay.value.Remove(del);
                    visualElement.style.transitionTimingFunction.value.Remove(eas);
                }

                if(reset)
                {
                    duration = 0;
                    delay = 0f;
                }
                else
                {
                    duration = oldDuration;
                    delay = oldDelay;
                }

                dur = new TimeValue(duration, timeUnit);
                del = new TimeValue(delay, timeUnit);
                prop = new StylePropertyName("scale");
                eas = new EasingFunction(ease);
                Debug.Log("Duration is ---->  " + duration);

                visualElement.style.transitionDelay.value.Add(del);
                visualElement.style.transitionDuration.value.Add(dur);
                visualElement.style.transitionProperty.value.Add(prop);
                visualElement.style.transitionTimingFunction.value.Add(eas);
                
                init = true;
            }

            AssingProperties(reset:false);

            visualElement.OnTransitionEnd(x =>
            {
                if (CheckLoopCount(utk))
                {
                    utk.onComplete?.Invoke();
                }
                else
                {
                    if(utk.pingPong)
                    {
                        Swap(ref destination, ref oldState);
                        Run();
                    }
                    else
                    {
                        AssingProperties(reset:true);
                        visualElement.style.scale = new Scale(oldState);
                        AssingProperties(reset:false);
                        Run();                  
                    }                    
                }
            }); 

            Run();

            void Run()
            {
                visualElement.schedule.Execute(() =>{visualElement.style.scale = new Scale(destination);});
            }
            return utk;
        }
        public static UTKTween Move(VisualElement visualElement, Translate translate, ref float duration, ref float delay, TimeUnit timeUnit = TimeUnit.Second, EasingMode ease = EasingMode.Linear)
        {
            var utk = new UTKTween();
            utk.id = visualElement.GetHashCode();
            UTKTweenManager.runningTweens.TryAdd(utk.id, utk);
            utk.cachedUIElement = visualElement;

            visualElement.style.transitionDelay = new List<TimeValue> { new TimeValue(delay, timeUnit) };
            visualElement.style.transitionProperty = new List<StylePropertyName> { new StylePropertyName("translate") };
            visualElement.style.transitionTimingFunction = new List<EasingFunction> { new EasingFunction(ease) };
            visualElement.style.transitionDuration = new StyleList<TimeValue>(new List<TimeValue> { new TimeValue(duration, timeUnit) });
            return utk;
        }
        public static UTKTween TextColor(VisualElement visualElement, Color color, ref float duration, ref float delay, TimeUnit timeUnit = TimeUnit.Second, EasingMode ease = EasingMode.Linear)
        {
            var utk = new UTKTween();
            utk.id = visualElement.GetHashCode();
            UTKTweenManager.runningTweens.TryAdd(utk.id, utk);
            utk.cachedUIElement = visualElement;

            visualElement.style.transitionDelay = new List<TimeValue> { new TimeValue(delay, timeUnit) };
            visualElement.style.transitionProperty = new List<StylePropertyName> { new StylePropertyName("color") };
            visualElement.style.transitionTimingFunction = new List<EasingFunction> { new EasingFunction(ease) };
            visualElement.style.transitionDuration = new StyleList<TimeValue>(new List<TimeValue> { new TimeValue(duration, timeUnit) });

            visualElement.schedule.Execute(() =>
            {
                visualElement.style.color = color;
            });
            return utk;
        }
        public static UTKTween Rotate(VisualElement visualElement, Rotate angle, ref float duration, ref float delay, TimeUnit timeUnit = TimeUnit.Second, EasingMode ease = EasingMode.Linear)
        {
            var utk = new UTKTween();
            utk.id = visualElement.GetHashCode();
            UTKTweenManager.runningTweens.TryAdd(utk.id, utk);
            utk.cachedUIElement = visualElement;

            visualElement.style.transitionDelay = new List<TimeValue> { new TimeValue(delay, timeUnit) };
            visualElement.style.transitionProperty = new List<StylePropertyName> { new StylePropertyName("rotate") };
            visualElement.style.transitionTimingFunction = new List<EasingFunction> { new EasingFunction(ease) };
            visualElement.style.transitionDuration = new StyleList<TimeValue>(new List<TimeValue> { new TimeValue(duration, timeUnit) });

            visualElement.schedule.Execute(() =>
            {
                visualElement.style.rotate = angle;
            });

            return utk;
        }
        private static bool CheckLoopCount(UTKTween utk)
        {
            if (utk.loopCount > 0)
            {
                if(!utk.pingPong)
                {
                    utk.loopCounter++;
                    
                    if(!utk.pingPong && utk.loopCount == utk.loopCounter)
                        return true;
                    
                    return false;
                }
                else
                {                        
                    utk.loopCounter++;

                    if(utk.loopCounter == (utk.loopCount * 2))
                        return true;
                    return false;
                }
            }

            return true;
        }
        private static T Swap<T>(ref T source, ref T target)
        {
            var a = source;
            var b = target;
            source = b;
            target = a;
            return target;
        }
    }
}