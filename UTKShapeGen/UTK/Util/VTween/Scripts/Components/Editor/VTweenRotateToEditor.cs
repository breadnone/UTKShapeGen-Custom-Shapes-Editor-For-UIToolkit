using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Linq;
using System;

namespace Breadnone.Editor
{

    [CustomEditor(typeof(VTweenRotateTo))]
    public class VTweenRotateToEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var t = target as VTweenRotateTo;
            VisualElement roots = new VisualElement();

            roots.Add(DrawEase(t));
            roots.Add(DrawGameObjectContainer(t));
            roots.Add(DrawDirection(t));
            roots.Add(DrawDestination(t));
            roots.Add(DrawDuration(t));
            roots.Add(DrawLoopCount(t));
            roots.Add(DrawDelay(t));
            roots.Add(DrawPingpong(t));
            roots.Add(DrawInactive(t));
            roots.Add(DrawUnscaled(t));
            return roots;
        }
        ///<summary>Draws object container.</summary>
        private VisualElement DrawGameObjectContainer(VTweenRotateTo t)
        {
            var root = new VisualElement();
            root.style.marginTop = 5;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

                var visRootOne = new VisualElement();
                visRootOne.style.marginTop = 5;
                visRootOne.style.flexDirection = FlexDirection.Row;
                visRootOne.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

                var lbl = new Label();
                lbl.text = "GameObject ";
                lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
                lbl.style.height = 20;

                var vis = new ObjectField();
                vis.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
                vis.style.height = 20;
                vis.objectType = typeof(GameObject);
                vis.value = t.objectToRotate;

                vis.RegisterValueChangedCallback(s =>
                {
                    t.objectToRotate = s.newValue as GameObject;
                });

                visRootOne.Add(lbl);
                visRootOne.Add(vis);
                root.Add(visRootOne);
                //root.Add(DrawChaininCombo(t));

            return root;
        }
        public VisualElement DrawDuration(VTweenRotateTo t)
        {
            var root = new VisualElement();
            root.style.marginTop = 5;
            root.style.flexDirection = FlexDirection.Row;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            var lbl = new Label();
            lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
            lbl.style.height = 20;
            lbl.text = "Duration ";

            var flt = new FloatField();
            flt.value = t.duration;
            flt.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
            flt.style.height = 20;

            flt.RegisterValueChangedCallback(x =>
            {
                t.duration = x.newValue;
            });

            root.Add(lbl);
            root.Add(flt);
            return root;
        }
        public VisualElement DrawDelay(VTweenRotateTo t)
        {
            var root = new VisualElement();
            root.style.marginTop = 5;
            root.style.flexDirection = FlexDirection.Row;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            var lbl = new Label();
            lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
            lbl.style.height = 20;
            lbl.text = "Delay ";

            var flt = new FloatField();
            flt.value = t.delay;
            flt.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
            flt.style.height = 20;
            flt.RegisterValueChangedCallback(x =>
            {
                t.delay = x.newValue;
            });

            root.Add(lbl);
            root.Add(flt);
            return root;
        }
        public VisualElement DrawEase(VTweenRotateTo t)
        {
            var template = VTweenTemplate.EaseEditor();
            template.dropdown.value = t.ease.ToString();

            template.dropdown.RegisterValueChangedCallback(x =>
            {
                foreach (var item in Enum.GetValues(typeof(Ease)))
                {
                    if (item.ToString() == x.newValue)
                    {
                        t.ease = (Ease)item;
                        break;
                    }
                }
            });

            return template.root;
        }
        public VisualElement DrawDirection(VTweenRotateTo t)
        {
            var template = VTweenTemplate.VectorDirectionEditor();
            template.dropdown.value = t.direction.ToString();

            template.dropdown.RegisterValueChangedCallback(x =>
            {
                foreach (var item in Enum.GetValues(typeof(VectorDirection)))
                {
                    if (item.ToString() == x.newValue)
                    {
                        t.direction = (VectorDirection)item;
                        break;
                    }
                }
            });

            return template.root;
        }
        private VisualElement DrawDestination(VTweenRotateTo t)
        {
            var template = VTweenTemplate.FloatField("Angle");
            template.floatField.value = t.angle;

            template.floatField.RegisterValueChangedCallback(s =>
            {
                t.angle = template.floatField.value;
            });

            return template.root;
        }
        public VisualElement DrawLoopCount(VTweenRotateTo t)
        {
            var root = new VisualElement();
            root.style.marginTop = 5;
            root.style.flexDirection = FlexDirection.Row;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            var lbl = new Label();
            lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
            lbl.style.height = 20;
            lbl.text = "Loop ";

            var flt = new IntegerField();
            flt.value = t.loopCount;
            flt.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
            flt.style.height = 20;

            flt.RegisterValueChangedCallback(x =>
            {
                t.loopCount = x.newValue;
            });

            root.Add(lbl);
            root.Add(flt);
            return root;
        }
        private VisualElement DrawPingpong(VTweenRotateTo t)
        {
            var template = VTweenTemplate.PingPong();

            if (t.pingpong)
                template.dropdown.value = "Enable";
            else
                template.dropdown.value = "Disable";

            template.dropdown.RegisterValueChangedCallback(x =>
            {
                if (x.newValue == "Enable")
                {
                    t.pingpong = true;
                }
                else
                {
                    t.pingpong = false;
                }
            });

            return template.root;
        }
        private VisualElement DrawInactive(VTweenRotateTo t)
        {
            var template = VTweenTemplate.PingPong("InactiveOnComplete");

            if (t.setActive)
                template.dropdown.value = "Enable";
            else
                template.dropdown.value = "Disable";

            template.dropdown.RegisterValueChangedCallback(x =>
            {
                if (x.newValue == "Enable")
                {
                    t.setActive = true;
                }
                else
                {
                    t.setActive = false;
                }
            });

            return template.root;
        }
        private VisualElement DrawUnscaled(VTweenRotateTo t)
        {
            var template = VTweenTemplate.PingPong("UnscaledTime ");

            if (t.unscaledTime)
                template.dropdown.value = "Enable";
            else
                template.dropdown.value = "Disable";

            template.dropdown.RegisterValueChangedCallback(x =>
            {
                if (x.newValue == "Enable")
                {
                    t.unscaledTime = true;
                }
                else
                {
                    t.unscaledTime = false;
                }
            });

            return template.root;
        }
    }
}