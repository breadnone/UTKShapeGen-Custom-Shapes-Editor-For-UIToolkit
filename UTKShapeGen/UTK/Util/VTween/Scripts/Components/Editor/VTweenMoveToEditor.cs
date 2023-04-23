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

    [CustomEditor(typeof(VTweenMoveTo))]
    public class VTweenMoveToEditor : UnityEditor.Editor
    {
        private VisualElement objectContainer;
        private VisualElement mainRoot;
        private VisualElement objectContainerContent;
        private VisualElement chainContainer;
        public override VisualElement CreateInspectorGUI()
        {
            var t = target as VTweenMoveTo;
            VisualElement roots = new VisualElement();
            mainRoot = roots;
            chainContainer = new VisualElement();
            roots.Add(DrawAxes(t));
            roots.Add(DrawEnumIsGameObject(t));
            roots.Add(DrawEase(t));

            objectContainer = new VisualElement();
            objectContainerContent = DrawGameObjectContainer(t);
            objectContainer.Add(objectContainerContent);

            roots.Add(objectContainer);
            roots.Add(DrawDestination(t));
            roots.Add(DrawDuration(t));
            roots.Add(DrawLoopCount(t));
            roots.Add(DrawDelay(t));
            roots.Add(DrawPingpong(t));
            roots.Add(DrawInactive(t));
            roots.Add(DrawUnscaled(t));
            roots.Add(DrawWorldSpace(t));
            return roots;
        }
        ///<summary>Draws enum object.</summary>
        private VisualElement DrawEnumIsGameObject(VTweenMoveTo t)
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            var lbl = new Label();
            lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
            lbl.text = "Type ";

            var vis = new DropdownField();
            vis.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
            vis.choices = new List<string> { "GameObject", "UIElement" };

            if (t.isGameObject)
            {
                vis.value = "GameObject";
            }
            else
            {
                vis.value = "UIElement";
            }

            vis.RegisterValueChangedCallback((x) =>
            {
                if (x.newValue == "GameObject")
                {
                    t.isGameObject = true;
                    t.visualElement = null;
                    t.targetVisualElement = null;
                }
                else
                {
                    t.isGameObject = false;
                    t.objectToMove = null;
                    t.toTarget = null;
                }

                objectContainerContent.RemoveFromHierarchy();
                objectContainerContent = null;
                objectContainerContent = DrawGameObjectContainer(t);
                objectContainer.Add(objectContainerContent);
            });

            root.Add(lbl);
            root.Add(vis);
            return root;
        }
        ///<summary>Draws object container.</summary>
        private VisualElement DrawGameObjectContainer(VTweenMoveTo t)
        {
            var root = new VisualElement();
            root.style.marginTop = 5;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            if (t.isGameObject)
            {
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
                vis.value = t.objectToMove;

                vis.RegisterValueChangedCallback(s =>
                {
                    t.objectToMove = s.newValue as GameObject;
                });

                var visRootTwo = new VisualElement();
                visRootTwo.style.marginTop = 20;
                visRootTwo.style.flexDirection = FlexDirection.Row;
                visRootTwo.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
                visRootTwo.style.height = new StyleLength(new Length(100, LengthUnit.Percent));

                var lblTwo = new Label();
                lblTwo.text = "Target ";
                lblTwo.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
                lblTwo.style.height = 20;

                var visSub = new ObjectField();
                visSub.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
                visSub.style.height = 20;
                visSub.objectType = typeof(Transform);
                visSub.value = t.toTarget;

                visSub.RegisterValueChangedCallback(s =>
                {
                    t.toTarget = s.newValue as Transform;
                });

                visRootOne.Add(lbl);
                visRootOne.Add(vis);
                visRootTwo.Add(lblTwo);
                visRootTwo.Add(visSub);
                root.Add(visRootOne);
                root.Add(visRootTwo);
                //root.Add(DrawChaininCombo(t));
            }
            else
            {
                var visRootOne = new VisualElement();
                visRootOne.style.marginTop = 5;
                visRootOne.style.flexDirection = FlexDirection.Row;
                visRootOne.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

                var lbl = new Label();
                lbl.text = "UIElement ";
                lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
                lbl.style.height = 20;

                var vis = new ObjectField();
                vis.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
                vis.style.height = 20;
                vis.objectType = typeof(UIDocument);
                vis.value = t.visualElement;
                visRootOne.Add(lbl);
                visRootOne.Add(vis);

                vis.RegisterValueChangedCallback(s =>
                {
                    t.visualElement = s.newValue as UIDocument;
                });

                var visRootTwo = new VisualElement();
                visRootTwo.style.marginTop = 20;
                visRootTwo.style.flexDirection = FlexDirection.Row;
                visRootTwo.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
                visRootTwo.style.height = new StyleLength(new Length(100, LengthUnit.Percent));

                var lblTwo = new Label();
                lblTwo.text = "Target ";
                lblTwo.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
                lblTwo.style.height = 20;

                var visSub = new ObjectField();
                visSub.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
                visSub.style.height = 20;
                visSub.objectType = typeof(UIDocument);
                visRootTwo.Add(lblTwo);
                visRootTwo.Add(visSub);

                visSub.RegisterValueChangedCallback(s =>
                {
                    t.targetVisualElement = s.newValue as UIDocument;
                });

                root.Add(visRootOne);
                root.Add(visRootTwo);
                //root.Add(DrawChainingTargetsVis(t));
            }

            return root;
        }
        public VisualElement DrawDuration(VTweenMoveTo t)
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
        public VisualElement DrawLoopCount(VTweenMoveTo t)
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
        public VisualElement DrawDelay(VTweenMoveTo t)
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
        public VisualElement DrawEase(VTweenMoveTo t)
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
        private VisualElement DrawDestination(VTweenMoveTo t)
        {
            var template = VTweenTemplate.Vec3Field("Destination");
            template.vec3.value = t.to;

            template.vec3.RegisterValueChangedCallback(s =>
            {
                t.to = template.vec3.value;
            });

            return template.root;
        }
        private VisualElement DrawPingpong(VTweenMoveTo t)
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
        private VisualElement DrawInactive(VTweenMoveTo t)
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
        private VisualElement DrawUnscaled(VTweenMoveTo t)
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
        private VisualElement DrawWorldSpace(VTweenMoveTo t)
        {
            var template = VTweenTemplate.LocalSpace();

            if (t.isLocal)
                template.dropdown.value = "Enable";
            else
                template.dropdown.value = "Disable";

            template.dropdown.RegisterValueChangedCallback(x =>
            {
                if (x.newValue == "Enable")
                {
                    t.isLocal = true;
                }
                else
                {
                    t.isLocal = false;
                }
            });

            return template.root;
        }
        private VisualElement DrawAxes(VTweenMoveTo t)
        {
            var template = VTweenTemplate.Axes();
            template.dropdown.value = t.axis.ToString();

            template.dropdown.RegisterValueChangedCallback(x =>
            {
                foreach (var item in Enum.GetValues(typeof(VAxis)))
                {
                    if (x.newValue == item.ToString())
                    {
                        t.axis = (VAxis)item;
                        break;
                    }
                }
            });

            return template.root;
        }
    }
}