using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Breadnone;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Linq;

///<summary>Global templates.</summary>
namespace Breadnone.Editor
{
    ///<summary>Ease global template.</summary>
    public static class VTweenTemplate
    {
        public static (VisualElement root, DropdownField dropdown, Label label) EaseEditor()
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            var lbl = new Label();
            lbl.text = "Ease ";
            lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
            lbl.style.height = 20;

            var drop = new DropdownField();
            drop.choices = Enum.GetNames(typeof(Ease)).ToList();
            drop.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
            drop.style.height = 20;
            drop.value = "Linear";

            root.Add(lbl);
            root.Add(drop);
            return (root, drop, lbl);
        }
        public static (VisualElement root, DropdownField dropdown, Label label) VectorDirectionEditor()
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            var lbl = new Label();
            lbl.text = "Ease ";
            lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
            lbl.style.height = 20;

            var drop = new DropdownField();
            drop.choices = Enum.GetNames(typeof(VectorDirection)).ToList();
            drop.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
            drop.style.height = 20;
            drop.value = "Vector3Forward";

            root.Add(lbl);
            root.Add(drop);
            return (root, drop, lbl);
        }
        public static (VisualElement root, Vector3Field vec3, Label label) Vec3Field(string lblName)
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            var lbl = new Label();
            lbl.text = lblName;
            lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
            lbl.style.height = 20;

            var drop = new Vector3Field();
            drop.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
            drop.style.height = 20;

            root.Add(lbl);
            root.Add(drop);
            return (root, drop, lbl);
        }
        public static (VisualElement root, Vector2Field vec2, Label label) Vec2Field(string lblName)
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            var lbl = new Label();
            lbl.text = lblName;
            lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
            lbl.style.height = 20;

            var drop = new Vector2Field();
            drop.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
            drop.style.height = 20;

            root.Add(lbl);
            root.Add(drop);
            return (root, drop, lbl);
        }
        ///<summary>Float field.</summary>
        public static (VisualElement root, FloatField floatField, Label label) FloatField(string lblName)
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            var lbl = new Label();
            lbl.text = lblName;
            lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
            lbl.style.height = 20;

            var drop = new FloatField();
            drop.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
            drop.style.height = 20;

            root.Add(lbl);
            root.Add(drop);
            return (root, drop, lbl);
        }
        ///<summary>Integer field</summary>
        public static (VisualElement root, IntegerField intField, Label label) IntegerField(string lblName)
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            var lbl = new Label();
            lbl.text = lblName;
            lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
            lbl.style.height = 20;

            var drop = new IntegerField();
            drop.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
            drop.style.height = 20;

            root.Add(lbl);
            root.Add(drop);
            return (root, drop, lbl);
        }
        ///<summary>Pingpong global template.</summary>
        public static (VisualElement root, DropdownField dropdown, Label label) PingPong(string str = "PingPong ")
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            var lbl = new Label();
            lbl.text = str;
            lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
            lbl.style.height = 20;

            var drop = new DropdownField();
            drop.choices = new List<string>{"Enable", "Disable"};
            drop.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
            drop.style.height = 20;
            drop.value = "Disable";

            root.Add(lbl);
            root.Add(drop);
            return (root, drop, lbl);
        }
        public static (VisualElement root, DropdownField dropdown, Label label) LocalSpace()
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            var lbl = new Label();
            lbl.text = "WorldSpace ";
            lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
            lbl.style.height = 20;

            var drop = new DropdownField();
            drop.choices = new List<string>{"Enable", "Disable"};
            drop.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
            drop.style.height = 20;
            drop.value = "Disable";

            root.Add(lbl);
            root.Add(drop);
            return (root, drop, lbl);
        }
        public static (VisualElement root, DropdownField dropdown, Label label) Axes()
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            var lbl = new Label();
            lbl.text = "Axis ";
            lbl.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
            lbl.style.height = 20;

            var drop = new DropdownField();
            drop.choices = Enum.GetNames(typeof(VAxis)).ToList();
            drop.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
            drop.style.height = 20;
            drop.value = "XYZ";

            root.Add(lbl);
            root.Add(drop);
            return (root, drop, lbl);
        }
    }
}