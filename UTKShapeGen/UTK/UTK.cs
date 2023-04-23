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

#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEditor;
#endif

namespace UTK
{
    public static partial class Utk
    {
        private static string upath = "Assets/UTK/Resources/utk-hn-template.asset";
        private static (float width, float height, bool xdynamic, bool ydynamic) template1 = (50, 20, true, false);
        private static (float width, float height, bool xdynamic, bool ydynamic) template2 = (50, 20, true, false);

#if UNITY_EDITOR
        public static (VisualElement root, Label label, UTKImage obj) ImageBrowse(string title, string emptySlot = "EMPTY", bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new UTKImage(emptyImagetext: emptySlot);

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        public static (VisualElement root, Label label, ObjectField obj) ObjectField(string title, Type type, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new ObjectField();
            sobj.objectType = type;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }

        public static (VisualElement root, Label label, FloatField obj) FloatField(string title, float initValue = 0, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new FloatField();
            sobj.value = initValue;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        public static (VisualElement root, Label label, IntegerField obj) IntegerField(string title, int initValue = 0, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new IntegerField();
            sobj.value = initValue;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        ///<summary>Force refresh the hierarchy by re-assigning rect's layout</summary>
        public static void TryForceRefresh(VisualElement container)
        {
            container.schedule.Execute(() =>
            {
                var fakeOldRect = Rect.zero;
                var fakeNewRect = container.layout;

                using var evt = GeometryChangedEvent.GetPooled(fakeOldRect, fakeNewRect);
                evt.target = container;
                container.SendEvent(evt);
            });
        }
        public static T AddToParent<T>(this T visualElement, T parent) where T : VisualElement
        {
            parent.AddParent(visualElement);
            return visualElement;
        }
        public static T AddInline<T>(this T visualElement, T child, float width, float height, bool dynamic = false) where T : VisualElement
        {
            child.Size(width, height, dynamic);
            visualElement.FlexRow();
            visualElement.AddChild(child);
            return visualElement;
        }
        public static T AddAlign<T>(this T visualElement, T child, float width, float height, bool dynamic = false) where T : VisualElement
        {
            child.Size(width, height, dynamic);
            visualElement.FlexColumn();
            visualElement.AddChild(child);
            return visualElement;
        }
        public static T ChangeParent<T>(this T visualElement, T newParent) where T : VisualElement
        {
            visualElement.RemoveFromHierarchy();
            newParent.AddChild(visualElement);
            return visualElement;
        }
        public static (X, Y, Z) AddToRoot<X, Y, Z>(this (X, Y, Z) visualElement, VisualElement child) where X : VisualElement where Z : VisualElement where Y : VisualElement
        {
            visualElement.Item1.Add(child);
            return visualElement;
        }
        public static (X, Y, Z) AddToMain<X, Y, Z>(this (X, Y, Z) visualElement, VisualElement child) where X : VisualElement where Z : VisualElement where Y : VisualElement
        {
            visualElement.Item3.Add(child);
            return visualElement;
        }
        public static (X, Y, Z) AddToLabel<X, Y, Z>(this (X, Y, Z) visualElement, VisualElement child) where X : VisualElement where Z : VisualElement where Y : VisualElement
        {
            visualElement.Item2.Add(child);
            return visualElement;
        }
        public static (VisualElement root, Label label, DoubleField obj) DoubleField(string title, double initValue = 0, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new DoubleField();
            sobj.value = initValue;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        public static (VisualElement root, Label label, Vector3Field obj) Vector3Field(string title, Vector3 initValue, bool useTemplate)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new Vector3Field();
            sobj.value = initValue;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        public static (VisualElement root, Label label, Vector2Field obj) Vector2Field(string title, Vector2 initValue, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new Vector2Field();
            sobj.value = initValue;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        public static (VisualElement root, Label label, Vector3IntField obj) Vector3IntField(string title, Vector3Int initValue, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new Vector3IntField();
            sobj.value = initValue;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        public static (VisualElement root, Label label, Vector2IntField obj) Vector2IntField(string title, Vector2Int initValue, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new Vector2IntField();
            sobj.value = initValue;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        public static (VisualElement root, Label label, RectField obj) RectField(string title, Rect initValue, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new RectField();
            sobj.value = initValue;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        public static (VisualElement root, Label label, RectIntField obj) RectIntField(string title, RectInt initValue, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new RectIntField();
            sobj.value = initValue;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        public static (VisualElement root, Label label, ColorField obj) ColorField(string title, Color initValue, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new ColorField();
            sobj.value = initValue;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
#endif /////////////////////////////////////////////////////////////////////
        public static (VisualElement root, Label label, Label obj) Label(string title, string content, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new Label();
            sobj.text = content;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        public static (VisualElement root, Label label, ScrollView obj) ScrollView(string title, bool? isHorizontal, VisualElement[] elements = null, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new ScrollView();

            if (isHorizontal.HasValue)
            {
                if (!isHorizontal.Value)
                    sobj.mode = ScrollViewMode.Horizontal;
                else
                    sobj.mode = ScrollViewMode.Vertical;
            }
            else
            {
                sobj.mode = ScrollViewMode.VerticalAndHorizontal;
            }

            if (elements != null && elements.Length > 0)
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    if (elements[i] == null)
                        continue;

                    sobj.Add(elements[i]);
                }
            }

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        public static (VisualElement root, Label label, DropdownField obj) DropDownField(string title, List<string> choices, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new DropdownField();
            sobj.choices = choices;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        public static (VisualElement root, Label label, TextField obj) TextField(string title, string initValue, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new TextField();
            sobj.value = initValue;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        public static (VisualElement root, Label label, Slider obj) Slider(string title, float lowValue, float highValue, float initValue, bool useTemplate = false)
        {
            var vis = new VisualElement();
            vis.style.flexDirection = FlexDirection.Row;

            var lbl = new Label();
            lbl.text = title;

            var sobj = new Slider();
            sobj.showInputField = true;
            sobj.value = initValue;
            sobj.lowValue = lowValue;
            sobj.highValue = highValue;

            if (useTemplate)
            {
                lbl.GetTemplate();
                sobj.GetTemplate();
            }

            vis.Add(lbl);
            vis.Add(sobj);
            return (vis, lbl, sobj);
        }
        public static TextField TextArea(this TextField textField, float height)
        {
            textField.style.flexDirection = FlexDirection.Column;
            textField.style.overflow = Overflow.Visible;
            textField.style.whiteSpace = WhiteSpace.Normal;
            textField.style.unityOverflowClipBox = OverflowClipBox.ContentBox;
            textField.style.height = height;
            textField.multiline = true;
            return textField;
        }
        public static UnityEngine.Color HexColor(UnityEngine.Color defaultColor, string hexColor)
        {
            if (String.IsNullOrEmpty(hexColor) || hexColor[0] != '#')
                return defaultColor;

            UnityEngine.Color nuColor;

            var t = ColorUtility.TryParseHtmlString(hexColor, out nuColor);

            if (t == false)
                nuColor = defaultColor;

            return nuColor;
        }
        public static T BcgColor<T>(this T visualElement, string hexColor) where T : VisualElement
        {
            return visualElement.BcgColor(HexColor(visualElement.style.backgroundColor.value, hexColor));
        }
        public static T LowValue<T>(this T visualElement, float value) where T : BaseSlider<float>
        {
            visualElement.lowValue = value;
            return visualElement;
        }
        public static T HighValue<T>(this T visualElement, float value) where T : BaseSlider<float>
        {
            visualElement.highValue = value;
            return visualElement;
        }
        public static T ShowInputField<T>(this T visualElement, bool state) where T : BaseSlider<float>
        {
            visualElement.showInputField = state;
            return visualElement;
        }
        public static T SliderValue<T>(this T visualElement, float value) where T : BaseSlider<float>
        {
            visualElement.value = value;
            return visualElement;
        }
        public static T WhiteSpaces<T>(this T visualElement, WhiteSpace whiteSpace) where T : TextElement
        {
            visualElement.style.whiteSpace = whiteSpace;
            return visualElement;
        }
        public static T Multiline<T>(this T visualElement, bool state) where T : TextField
        {
            visualElement.multiline = state;
            return visualElement;
        }
        public static T MaskChar<T>(this T visualElement, char character) where T : TextField
        {
            visualElement.maskChar = character;
            return visualElement;
        }
        public static T MaxLength<T>(this T visualElement, char character) where T : TextField
        {
            visualElement.maskChar = character;
            return visualElement;
        }
        public static T LabelElement<T>(this T visualElement) where T : BaseField<string>
        {
            return visualElement.labelElement as T;
        }
        public static T IsDelayed<T>(this T visualElement, bool state) where T : TextInputBaseField<string>
        {
            visualElement.isDelayed = state;
            return visualElement;
        }
        public static T IsPasswordField<T>(this T visualElement, bool state) where T : TextInputBaseField<string>
        {
            visualElement.isPasswordField = state;
            return visualElement;
        }
        public static T IsReadOnly<T>(this T visualElement, bool state) where T : TextInputBaseField<string>
        {
            visualElement.isReadOnly = state;
            return visualElement;
        }
        public static int SelectIndex<T>(this T visualElement) where T : TextInputBaseField<string>
        {
            return visualElement.selectIndex;
        }
        public static Color SelectionColor<T>(this T visualElement, Color color) where T : TextInputBaseField<string>
        {
            return visualElement.selectionColor;
        }
        public static T DoubleClickSelectWords<T>(this T visualElement, bool state) where T : TextField
        {
            visualElement.doubleClickSelectsWord = state;
            return visualElement;
        }
        public static T TintColor<T>(this T visualElement, Color color) where T : Image
        {
            visualElement.tintColor = color;
            return visualElement;
        }
        public static T Sprite<T>(this T visualElement, Sprite sprite) where T : Image
        {
            visualElement.sprite = sprite;
            return visualElement;
        }
        public static T VectorImage<T>(this T visualElement, VectorImage vectorImage) where T : Image
        {
            visualElement.vectorImage = vectorImage;
            return visualElement;
        }

        public static T UV<T>(this T visualElement, Rect uv) where T : Image
        {
            visualElement.uv = uv;
            return visualElement;
        }
        public static T Image<T>(this T visualElement, Texture texture) where T : Image
        {
            visualElement.image = texture;
            return visualElement;
        }
        public static T TrippleClickSelectLine<T>(this T visualElement, bool state) where T : TextField
        {
            visualElement.tripleClickSelectsLine = state;
            return visualElement;
        }
        public static T GenerateVisualContent<T>(this T visualElement, Action<MeshGenerationContext> meshContext) where T : VisualElement
        {
            visualElement.generateVisualContent = meshContext;
            return visualElement;
        }
        public static T TextArea<T>(this T textField, float height) where T : TextElement
        {
            textField.style.flexDirection = FlexDirection.Column;
            textField.style.overflow = Overflow.Visible;
            textField.style.whiteSpace = WhiteSpace.Normal;
            textField.style.unityOverflowClipBox = OverflowClipBox.ContentBox;
            textField.style.height = height;
            return textField;
        }
        public static T Size<T>(this T visualElement, int? width, int? height, bool? widthDynamic, bool? heightDynamic) where T : VisualElement
        {
            if (width != null && widthDynamic.Value)
                visualElement.style.width = new StyleLength(new Length(width.Value));
            else if (width != null && !widthDynamic.Value)
                visualElement.style.width = width.Value;

            if (height != null && heightDynamic.Value)
                visualElement.style.height = new StyleLength(new Length(height.Value));
            else if (height != null && !heightDynamic.Value)
                visualElement.style.height = height.Value;

            return visualElement;
        }
        public static Color ColorOne { get { return new Color(0.4f, 0.3f, 0.4f); } }
        public static Color ColorTwo { get { return new Color(0.3f, 0.1f, 0.3f); } }
        public static T Size<T>(this T visualElement, float x, float y, bool xdynamic = false, bool ydynamic = false) where T : VisualElement
        {
            var xlen = new Length(x, LengthUnit.Pixel);
            var ylen = new Length(y, LengthUnit.Pixel);

            if (xdynamic)
            {
                xlen = new Length(x, LengthUnit.Percent);
            }

            if (ydynamic)
            {
                ylen = new Length(y, LengthUnit.Percent);
            }

            visualElement.style.width = new StyleLength(xlen);
            visualElement.style.height = new StyleLength(ylen);
            return visualElement;
        }
        public static T Border<T>(this T vis, float width, StyleColor color) where T : VisualElement
        {
            StyleFloat len = new StyleFloat(width);

            vis.style.borderBottomWidth = len;
            vis.style.borderLeftWidth = len;
            vis.style.borderRightWidth = len;
            vis.style.borderTopWidth = len;

            vis.style.borderBottomColor = color;
            vis.style.borderLeftColor = color;
            vis.style.borderRightColor = color;
            vis.style.borderTopColor = color;
            return vis;
        }
        public static T BorderLeft<T>(this T vis, float width, StyleColor color) where T : VisualElement
        {
            StyleFloat len = new StyleFloat(width);
            vis.style.borderLeftWidth = len;
            vis.style.borderLeftColor = color;
            return vis;
        }
        public static T BorderRight<T>(this T vis, float width, StyleColor color) where T : VisualElement
        {
            StyleFloat len = new StyleFloat(width);
            vis.style.borderRightWidth = len;
            vis.style.borderRightColor = color;
            return vis;
        }
        public static T BorderTop<T>(this T vis, float width, StyleColor color) where T : VisualElement
        {
            StyleFloat len = new StyleFloat(width);
            vis.style.borderTopWidth = len;
            vis.style.borderTopColor = color;
            return vis;
        }
        public static T BorderBottom<T>(this T vis, float width, StyleColor color) where T : VisualElement
        {
            StyleFloat len = new StyleFloat(width);
            vis.style.borderBottomWidth = len;
            vis.style.borderBottomColor = color;
            return vis;
        }
        public static T Border<T>(this T vis, Vector4 value, StyleColor color) where T : VisualElement
        {
            vis.style.borderLeftWidth = value.x;
            vis.style.borderRightWidth = value.y;
            vis.style.borderTopWidth = value.z;
            vis.style.borderBottomWidth = value.w;
            vis.style.borderBottomColor = color;
            vis.style.borderLeftColor = color;
            vis.style.borderRightColor = color;
            vis.style.borderTopColor = color;
            return vis;
        }
        public static T Padding<T>(this T vis, float value, bool isDynamic = true) where T : VisualElement
        {
            Length len;

            if (isDynamic)
            {
                len = new Length(value, LengthUnit.Percent);
            }
            else
            {
                len = new Length(value, LengthUnit.Pixel);
            }

            vis.style.paddingLeft = new StyleLength(len);
            vis.style.paddingRight = new StyleLength(len);
            vis.style.paddingTop = new StyleLength(len);
            vis.style.paddingBottom = new StyleLength(len);
            return vis;
        }
        public static T PaddingLeft<T>(this T vis, float value, bool isDynamic = true) where T : VisualElement
        {
            Length len;

            if (isDynamic)
            {
                len = new Length(value, LengthUnit.Percent);
            }
            else
            {
                len = new Length(value, LengthUnit.Pixel);
            }

            vis.style.paddingLeft = new StyleLength(len);
            return vis;
        }
        public static T PaddingRight<T>(this T vis, float value, bool isDynamic = true) where T : VisualElement
        {
            Length len;

            if (isDynamic)
            {
                len = new Length(value, LengthUnit.Percent);
            }
            else
            {
                len = new Length(value, LengthUnit.Pixel);
            }

            vis.style.paddingRight = new StyleLength(len);
            return vis;
        }
        public static T PaddingTop<T>(this T vis, float value, bool isDynamic = true) where T : VisualElement
        {
            Length len;

            if (isDynamic)
            {
                len = new Length(value, LengthUnit.Percent);
            }
            else
            {
                len = new Length(value, LengthUnit.Pixel);
            }

            vis.style.paddingTop = new StyleLength(len);
            return vis;
        }
        public static T PaddingBottom<T>(this T vis, float value, bool isDynamic = true) where T : VisualElement
        {
            Length len;

            if (isDynamic)
            {
                len = new Length(value, LengthUnit.Percent);
            }
            else
            {
                len = new Length(value, LengthUnit.Pixel);
            }

            vis.style.paddingBottom = new StyleLength(len);
            return vis;
        }
        public static T Padding<T>(this T vis, Vector4 value, bool isDynamic = true) where T : VisualElement
        {
            Length xleft;
            Length xright;
            Length xtop;
            Length xbottom;

            if (isDynamic)
            {
                xleft = new Length(value.x, LengthUnit.Percent);
                xright = new Length(value.y, LengthUnit.Percent);
                xtop = new Length(value.z, LengthUnit.Percent);
                xbottom = new Length(value.w, LengthUnit.Percent);
            }
            else
            {
                xleft = new Length(value.x, LengthUnit.Pixel);
                xright = new Length(value.y, LengthUnit.Pixel);
                xtop = new Length(value.z, LengthUnit.Pixel);
                xbottom = new Length(value.w, LengthUnit.Pixel);
            }

            vis.style.paddingLeft = new StyleLength(xleft);
            vis.style.paddingRight = new StyleLength(xright);
            vis.style.paddingTop = new StyleLength(xtop);
            vis.style.paddingBottom = new StyleLength(xbottom);
            return vis;
        }
        public static T Padding<T>(this T vis, float left, float right, float top, float bottom, bool isDynamic = true) where T : VisualElement
        {
            Length xleft;
            Length xright;
            Length xtop;
            Length xbottom;

            if (isDynamic)
            {
                xleft = new Length(left, LengthUnit.Percent);
                xright = new Length(right, LengthUnit.Percent);
                xtop = new Length(top, LengthUnit.Percent);
                xbottom = new Length(bottom, LengthUnit.Percent);
            }
            else
            {
                xleft = new Length(left, LengthUnit.Pixel);
                xright = new Length(right, LengthUnit.Pixel);
                xtop = new Length(top, LengthUnit.Pixel);
                xbottom = new Length(bottom, LengthUnit.Pixel);
            }

            vis.style.paddingLeft = new StyleLength(xleft);
            vis.style.paddingRight = new StyleLength(xright);
            vis.style.paddingTop = new StyleLength(xtop);
            vis.style.paddingBottom = new StyleLength(xbottom);
            return vis;
        }
        public static T MarginLeft<T>(this T vis, float value, bool isDynamic = true) where T : VisualElement
        {
            Length len;

            if (isDynamic)
            {
                len = new Length(value, LengthUnit.Percent);
            }
            else
            {
                len = new Length(value, LengthUnit.Pixel);
            }

            vis.style.marginLeft = new StyleLength(len);
            return vis;
        }
        public static T MarginRight<T>(this T vis, float value, bool isDynamic = true) where T : VisualElement
        {
            Length len;

            if (isDynamic)
            {
                len = new Length(value, LengthUnit.Percent);
            }
            else
            {
                len = new Length(value, LengthUnit.Pixel);
            }

            vis.style.marginRight = new StyleLength(len);
            return vis;
        }
        public static T MarginTop<T>(this T vis, float value, bool isDynamic = true) where T : VisualElement
        {
            Length len;

            if (isDynamic)
            {
                len = new Length(value, LengthUnit.Percent);
            }
            else
            {
                len = new Length(value, LengthUnit.Pixel);
            }

            vis.style.marginTop = new StyleLength(len);
            return vis;
        }
        public static T MarginBottom<T>(this T vis, float value, bool isDynamic = true) where T : VisualElement
        {
            Length len;

            if (isDynamic)
            {
                len = new Length(value, LengthUnit.Percent);
            }
            else
            {
                len = new Length(value, LengthUnit.Pixel);
            }

            vis.style.marginBottom = new StyleLength(len);
            return vis;
        }
        public static T Margin<T>(this T vis, float value, bool isDynamic = true) where T : VisualElement
        {
            Length len;

            if (isDynamic)
            {
                len = new Length(value, LengthUnit.Percent);
            }
            else
            {
                len = new Length(value, LengthUnit.Pixel);
            }

            vis.style.marginLeft = new StyleLength(len);
            vis.style.marginRight = new StyleLength(len);
            vis.style.marginTop = new StyleLength(len);
            vis.style.marginBottom = new StyleLength(len);
            return vis;
        }
        public static T Margin<T>(this T vis, float left, float right, float top, float bottom, bool isDynamic = true) where T : VisualElement
        {
            Length vleft;
            Length vright;
            Length vtop;
            Length vbottom;

            if (isDynamic)
            {
                vleft = new Length(left, LengthUnit.Percent);
                vright = new Length(right, LengthUnit.Percent);
                vtop = new Length(top, LengthUnit.Percent);
                vbottom = new Length(bottom, LengthUnit.Percent);
            }
            else
            {
                vleft = new Length(left, LengthUnit.Pixel);
                vright = new Length(right, LengthUnit.Pixel);
                vtop = new Length(top, LengthUnit.Pixel);
                vbottom = new Length(bottom, LengthUnit.Pixel);
            }

            vis.style.marginLeft = new StyleLength(vleft);
            vis.style.marginRight = new StyleLength(vright);
            vis.style.marginTop = new StyleLength(vtop);
            vis.style.marginBottom = new StyleLength(vbottom);
            return vis;
        }
        public static T Margin<T>(this T vis, Vector4 value, bool isDynamic = true) where T : VisualElement
        {
            Length vleft;
            Length vright;
            Length vtop;
            Length vbottom;

            if (isDynamic)
            {
                vleft = new Length(value.x, LengthUnit.Percent);
                vright = new Length(value.y, LengthUnit.Percent);
                vtop = new Length(value.z, LengthUnit.Percent);
                vbottom = new Length(value.w, LengthUnit.Percent);
            }
            else
            {
                vleft = new Length(value.x, LengthUnit.Pixel);
                vright = new Length(value.y, LengthUnit.Pixel);
                vtop = new Length(value.z, LengthUnit.Pixel);
                vbottom = new Length(value.w, LengthUnit.Pixel);
            }

            vis.style.marginLeft = new StyleLength(vleft);
            vis.style.marginRight = new StyleLength(vright);
            vis.style.marginTop = new StyleLength(vtop);
            vis.style.marginBottom = new StyleLength(vbottom);
            return vis;
        }
        public static T RoundCorner<T>(this T vis, float radius, bool isDynamic = false) where T : VisualElement
        {
            Length val = new Length(radius, LengthUnit.Pixel);

            if (isDynamic)
            {
                val = new Length(radius, LengthUnit.Percent);
            }

            vis.style.borderTopLeftRadius = radius;
            vis.style.borderTopRightRadius = radius;
            vis.style.borderBottomLeftRadius = radius;
            vis.style.borderBottomRightRadius = radius;
            return vis;
        }
        public static T RoundCorner<T>(this T vis, Vector4 radius, bool isDynamic = false) where T : VisualElement
        {
            Length xtopleft = new Length(radius.x, LengthUnit.Pixel);
            Length xtopright = new Length(radius.y, LengthUnit.Pixel);
            Length xbottomleft = new Length(radius.z, LengthUnit.Pixel);
            Length xbottomright = new Length(radius.w, LengthUnit.Pixel);

            if (isDynamic)
            {
                xtopleft = new Length(radius.x, LengthUnit.Percent);
                xtopright = new Length(radius.y, LengthUnit.Percent);
                xbottomleft = new Length(radius.z, LengthUnit.Percent);
                xbottomright = new Length(radius.w, LengthUnit.Percent);
            }

            vis.style.borderTopLeftRadius = new StyleLength(xtopleft);
            vis.style.borderTopRightRadius = new StyleLength(xtopright);
            vis.style.borderBottomLeftRadius = new StyleLength(xbottomleft);
            vis.style.borderBottomRightRadius = new StyleLength(xbottomright);
            return vis;
        }

        public static T AddParent<T>(this T visualElement, T parent) where T : VisualElement
        {
            parent.Add(visualElement);
            return visualElement;
        }
        public static T AddChild<T>(this T parent, T child) where T : VisualElement
        {
            parent.Add(child);
            return parent;
        }
        public static T AddCopy<T>(this T parent, T child) where T : VisualElement
        {
            parent.Add(child);
            return parent;
        }
        public static T AddNested<T>(this T parent, T child, FlexDirection flexDirection = FlexDirection.Column) where T : VisualElement
        {
            parent.style.flexDirection = flexDirection;
            parent.Add(child);
            return child;
        }
        public static T AddCopySize<T>(this T parent, T child) where T : VisualElement
        {
            parent.Add(child);
            child.style.width = new StyleLength(parent.style.width.value);
            child.style.height = new StyleLength(parent.style.height.value);
            return parent;
        }
        public static T AddCopyWidth<T>(this T parent, T child) where T : VisualElement
        {
            parent.Add(child);
            child.schedule.Execute(() =>
            {
                child.style.width = parent.style.width;
            });

            return parent;
        }
        public static T AddCopyHeight<T>(this T parent, T child) where T : VisualElement
        {
            parent.Add(child);
            child.style.height = new StyleLength(parent.style.width.value);
            return parent;
        }
        public static T FlexRow<T>(this T vis) where T : VisualElement
        {
            vis.style.flexDirection = FlexDirection.Row;
            return vis;
        }
        public static T FlexWrap<T>(this T vis, Wrap wrap) where T : VisualElement
        {
            vis.style.flexWrap = wrap;
            return vis;
        }
        public static T FlexBasis<T>(this T vis, StyleLength flexBasis) where T : VisualElement
        {
            vis.style.flexBasis = flexBasis;
            return vis;
        }
        public static T FlexShrink<T>(this T vis, StyleFloat flexShrink) where T : VisualElement
        {
            vis.style.flexShrink = flexShrink;
            return vis;
        }
        public static T FlexGrow<T>(this T vis, StyleFloat flexGrow) where T : VisualElement
        {
            vis.style.flexGrow = flexGrow;
            return vis;
        }
        public static T FlexColumn<T>(this T vis) where T : VisualElement
        {
            vis.style.flexDirection = FlexDirection.Column;
            return vis;
        }
        public static T FlexColumnReverse<T>(this T vis) where T : VisualElement
        {
            vis.style.flexDirection = FlexDirection.ColumnReverse;
            return vis;
        }
        public static T Left<T>(this T vis, float value, bool isDynamic = false) where T : VisualElement
        {
            Length len = new Length(value, LengthUnit.Pixel);

            if (isDynamic)
            {
                len = new Length(value, LengthUnit.Percent);
            }

            vis.style.left = new StyleLength(len);
            return vis;
        }
        public static T Right<T>(this T vis, float value, bool isDynamic = false) where T : VisualElement
        {
            Length len = new Length(value, LengthUnit.Pixel);

            if (isDynamic)
            {
                len = new Length(value, LengthUnit.Percent);
            }

            vis.style.right = new StyleLength(len);
            return vis;
        }
        public static T Top<T>(this T vis, float value, bool isDynamic = false) where T : VisualElement
        {
            Length len = new Length(value, LengthUnit.Pixel);

            if (isDynamic)
            {
                len = new Length(value, LengthUnit.Percent);
            }

            vis.style.top = new StyleLength(len);
            return vis;
        }
        public static T Bottom<T>(this T vis, float value, bool isDynamic = false) where T : VisualElement
        {
            Length len = new Length(value, LengthUnit.Pixel);

            if (isDynamic)
            {
                len = new Length(value, LengthUnit.Percent);
            }

            vis.style.bottom = new StyleLength(len);
            return vis;
        }
        public static T AlignSelf<T>(this T vis, Align align) where T : VisualElement
        {
            vis.style.alignSelf = align;
            return vis;
        }
        public static T AlignContent<T>(this T vis, Align align) where T : VisualElement
        {
            vis.style.alignContent = align;
            return vis;
        }
        public static T AlignItems<T>(this T vis, Align align) where T : VisualElement
        {
            vis.style.alignItems = align;
            return vis;
        }
        public static T Width<T>(this T vis, float value, bool isDynamic = false) where T : VisualElement
        {
            var len = new Length(value, LengthUnit.Pixel);

            if (isDynamic)
                len = new Length(value, LengthUnit.Percent);

            vis.style.width = new StyleLength(len);
            return vis;
        }
        public static T Width<T>(this T vis, Length value) where T : VisualElement
        {
            vis.style.width = new StyleLength(value);
            return vis;
        }
        public static T Height<T>(this T vis, float value, bool isDynamic = false) where T : VisualElement
        {
            var len = new Length(value, LengthUnit.Pixel);

            if (isDynamic)
                len = new Length(value, LengthUnit.Percent);

            vis.style.height = new StyleLength(len);
            return vis;
        }
        public static T Height<T>(this T vis, Length value) where T : VisualElement
        {
            vis.style.height = new StyleLength(value);
            return vis;
        }
        public static T Color<T>(this T vis, Color color) where T : VisualElement
        {
            vis.style.color = color;
            return vis;
        }
        public static T Color<T>(this T vis, string hexColor) where T : VisualElement
        {
            vis.Color(HexColor(vis.style.color.value, hexColor));
            return vis;
        }
        public static T SetSlice<T>(this T vis, int left, int right, int top, int bottom) where T : VisualElement
        {
            vis.style.unitySliceLeft = new StyleInt(left);
            vis.style.unitySliceRight = new StyleInt(right);
            vis.style.unitySliceTop = new StyleInt(top);
            vis.style.unitySliceBottom = new StyleInt(bottom);
            return vis;
        }
        public static T TextOutline<T>(this T vis, float width, StyleColor color) where T : VisualElement
        {
            vis.style.unityTextOutlineColor = color;
            return vis;
        }
        public static T BcgColor<T>(this T vis, StyleColor color, float opacity = 1f) where T : VisualElement
        {
            vis.style.backgroundColor = color;
            return vis;
        }
        public static T BcgImageTint<T>(this T vis, StyleColor color, float opacity = 1f) where T : VisualElement
        {
            vis.style.unityBackgroundImageTintColor = color;
            return vis;
        }
        public static TextField TextValue(this TextField vis, string text, bool withoutNotify = false)
        {
            if (!withoutNotify)
                vis.value = text;
            else
                vis.SetValueWithoutNotify(text);
            return vis;
        }
        public static T Value<T>(this T vis, string value) where T : TextInputBaseField<string>
        {
            vis.value = value;
            return vis;
        }
        public static T Value<T>(this T vis, float value) where T : TextInputBaseField<float>
        {
            vis.value = value;
            return vis;
        }
        public static T Value<T>(this T vis, double value) where T : TextInputBaseField<double>
        {
            vis.value = value;
            return vis;
        }
        public static T Value<T>(this T vis, int value) where T : TextInputBaseField<int>
        {
            vis.value = value;
            return vis;
        }
        public static T Value<T>(this T vis, long value) where T : TextInputBaseField<long>
        {
            vis.value = value;
            return vis;
        }
        public static DropdownField Value(this DropdownField vis, string value)
        {
            vis.value = value;
            return vis;
        }
        public static T SetValueWithoutNotify<T>(this T visualElement, float value) where T : INotifyValueChanged<float>
        {
            visualElement.SetValueWithoutNotify(value);
            return visualElement;
        }
        public static T SetValueWithoutNotify<T>(this T visualElement, double value) where T : INotifyValueChanged<double>
        {
            visualElement.SetValueWithoutNotify(value);
            return visualElement;
        }
        public static T SetValueWithoutNotify<T>(this T visualElement, Vector2 value) where T : INotifyValueChanged<Vector2>
        {
            visualElement.SetValueWithoutNotify(value);
            return visualElement;
        }
        public static T SetValueWithoutNotify<T>(this T visualElement, Vector3 value) where T : INotifyValueChanged<Vector3>
        {
            visualElement.SetValueWithoutNotify(value);
            return visualElement;
        }
        public static T SetValueWithoutNotify<T>(this T visualElement, Vector4 value) where T : INotifyValueChanged<Vector4>
        {
            visualElement.SetValueWithoutNotify(value);
            return visualElement;
        }
        public static T SetValueWithoutNotify<T>(this T visualElement, string value) where T : INotifyValueChanged<string>
        {
            visualElement.SetValueWithoutNotify(value);
            return visualElement;
        }
        public static T SetValueWithoutNotify<T>(this T visualElement, bool value) where T : INotifyValueChanged<bool>
        {
            visualElement.SetValueWithoutNotify(value);
            return visualElement;
        }
        public static T SetValueWithoutNotify<T>(this T visualElement, Matrix4x4 value) where T : INotifyValueChanged<Matrix4x4>
        {
            visualElement.SetValueWithoutNotify(value);
            return visualElement;
        }
        public static T SetValueWithoutNotify<T>(this T visualElement, int value) where T : INotifyValueChanged<int>
        {
            visualElement.SetValueWithoutNotify(value);
            return visualElement;
        }
        public static T SetValueWithoutNotify<T>(this T visualElement, Enum value) where T : INotifyValueChanged<Enum>
        {
            visualElement.SetValueWithoutNotify(value);
            return visualElement;
        }
        public static T SetValueWithoutNotify<T>(this T visualElement, long value) where T : INotifyValueChanged<long>
        {
            visualElement.SetValueWithoutNotify(value);
            return visualElement;
        }
        public static T SetValueWithoutNotify<T>(this T visualElement, UnityEngine.Object value) where T : INotifyValueChanged<UnityEngine.Object>
        {
            visualElement.SetValueWithoutNotify(value);
            return visualElement;
        }
        public static T SetValueWithoutNotify<T>(this T visualElement, object value) where T : INotifyValueChanged<object>
        {
            visualElement.SetValueWithoutNotify(value);
            return visualElement;
        }
        public static T SetValueWithoutNotify<T>(this T visualElement, Rect value) where T : INotifyValueChanged<Rect>
        {
            visualElement.SetValueWithoutNotify(value);
            return visualElement;
        }

        public static T Value<T>(this T vis, UnityEngine.Object value) where T : BaseField<UnityEngine.Object>
        {
            vis.value = value;
            return vis;
        }
        public static T Value<T>(this T vis, Rect value) where T : BaseField<Rect>
        {
            vis.value = value;
            return vis;
        }
        public static Foldout Value(this Foldout vis, bool value)
        {
            vis.value = value;
            return vis;
        }
        public static Foldout Text(this Foldout vis, string value)
        {
            vis.text = value;
            return vis;
        }
#if UNITY_EDITOR
        public static T Value<T>(this T vis, Gradient value) where T : GradientField
        {
            vis.value = value;
            return vis;
        }
#endif
        public static Slider ShowInputField<T>(this Slider vis, bool state) where T : BaseField<bool>
        {
            vis.showInputField = state;
            return vis;
        }
        public static T Value<T>(this T vis, Bounds value) where T : BaseField<Bounds>
        {
            vis.value = value;
            return vis;
        }
        public static T Value<T>(this T vis, Enum value) where T : BaseField<Enum>
        {
            vis.value = value;
            return vis;
        }
        public static T Value<T>(this T vis, bool value) where T : BaseField<bool>
        {
            vis.value = value;
            return vis;
        }
        public static T Value<T>(this T vis, Color value) where T : BaseField<Color>
        {
            vis.value = value;
            return vis;
        }
        public static T Value<T>(this T vis, Vector2 value) where T : BaseField<Vector2>
        {
            vis.value = value;
            return vis;
        }
        public static T Value<T>(this T vis, Vector3 value) where T : BaseField<Vector3>
        {
            vis.value = value;
            return vis;
        }
        public static T Value<T>(this T vis, Vector4 value) where T : BaseField<Vector4>
        {
            vis.value = value;
            return vis;
        }
        public static VisualElement MakeHorizontalSplit(int amount, Vector2 size, bool isDynamic = true)
        {
            var vis = new VisualElement();
            vis.FlexRow();

            if (isDynamic)
                vis.Size(size.x, size.y, true, true);
            else
                vis.Size(size.x, size.y, false, false);

            vis.name = "horizontalSplitParent";
            VisualElement[] xsplits = new VisualElement[amount];
            float width = 100f / amount;

            for (int i = 0; i < xsplits.Length; i++)
            {
                var t = new VisualElement();
                xsplits[i] = t;
                t.name = "panel-" + i;
                t.Size(width, 100, true, true);
                vis.Add(vis);
            }

            return vis;
        }
        public static VisualElement MakeVerticalSplit(int amount, Vector2 size, bool isDynamic = true)
        {
            var vis = new VisualElement();

            if (isDynamic)
                vis.Size(size.x, size.y, true, true);
            else
                vis.Size(size.x, size.y, false, false);

            vis.name = "verticalSplitParent";
            VisualElement[] ysplits = new VisualElement[amount];
            float height = 100f / amount;

            for (int i = 0; i < ysplits.Length; i++)
            {
                var t = new VisualElement();
                ysplits[i] = t;
                t.name = "panel-" + i;
                t.Size(100, height, true, true);
                vis.Add(t);
            }
            return vis;
        }
        public static T Name<T>(this T visualElement, string name) where T : VisualElement
        {
            visualElement.name = name;
            return visualElement;
        }
        public static VisualElement Parent<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.parent;
        }
        public static int GetChildCount<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.childCount;
        }
        public static T Index<T>(this T visualElement, int value) where T : DropdownField
        {
            visualElement.index = value;
            return visualElement;
        }
        public static int Index<T>(this T visualElement) where T : DropdownField
        {
            return visualElement.index;
        }
        public static T ShowMixedValue<T>(this T visualElement, bool state) where T : DropdownField
        {
            visualElement.showMixedValue = state;
            return visualElement;
        }
        public static bool ShowMixedValue<T>(this T visualElement) where T : DropdownField
        {
            return visualElement.showMixedValue;
        }
        public static VisualElement Container<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.contentContainer;
        }
        public static bool EnabledInHierarchy<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.enabledInHierarchy;
        }
        public static bool EnabledSelf<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.enabledSelf;
        }
        public static VisualElement.Hierarchy GetHierarchy<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.hierarchy;
        }
        public static Rect Layout(this VisualElement visualElement)
        {
            return visualElement.layout;
        }
        public static Rect LocalBound<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.localBound;
        }
        public static string Name<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.name;
        }
        public static ScrollView Mode<T>(this T visualElement, ScrollViewMode scrollviewMode) where T : ScrollView
        {
            visualElement.mode = scrollviewMode;
            return visualElement;
        }
        public static IPanel PadingRect<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.panel;
        }
        public static T PickingMode<T>(this T visualElement, PickingMode pickingMode) where T : VisualElement
        {
            visualElement.pickingMode = pickingMode;
            return visualElement;
        }
        public static void Schedule<T>(this T visualElement, Action action, int milliseconds = 1) where T : VisualElement
        {
            visualElement.schedule.Execute(action).ExecuteLater(milliseconds);
        }
        public static VisualElement This<T>(this T visualElement, int index) where T : VisualElement
        {
            return visualElement[index];
        }
        public static T UserData<T>(this T visualElement, System.Object obj) where T : VisualElement
        {
            visualElement.userData = obj;
            return visualElement;
        }
        public static Rect WorldBound<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.worldBound;
        }
        public static Matrix4x4 WorldTransform<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.worldTransform;
        }
        public static T BringToFront<T>(this T visualElement) where T : VisualElement
        {
            visualElement.BringToFront();
            return visualElement;
        }
        public static T SendToBack<T>(this T visualElement) where T : VisualElement
        {
            visualElement.SendToBack();
            return visualElement;
        }
        public static IEnumerable<VisualElement> Children<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.Children();
        }
        public static bool SendToBack<T>(this T visualElement, string value) where T : VisualElement
        {
            return visualElement.ClassListContains(value);
        }
        public static T Clear<T>(this T visualElement) where T : VisualElement
        {
            visualElement.Clear();
            return visualElement;
        }
        public static bool Contains<T>(this T visualElement, T child) where T : VisualElement
        {
            return visualElement.Contains(child);
        }
        public static bool ContainsPoints<T>(this T visualElement, Vector2 localPoint) where T : VisualElement
        {
            return visualElement.ContainsPoint(localPoint);
        }
        public static VisualElement ElementAt<T>(this T visualElement, int index) where T : VisualElement
        {
            return visualElement.ElementAt(index);
        }
        public static T EnableClassList<T>(this T visualElement, string name, bool state) where T : VisualElement
        {
            visualElement.EnableInClassList(name, state);
            return visualElement;
        }
        public static object FindAccessorUserData<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.FindAncestorUserData();
        }
        public static object FindCommonAnsestor<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.FindAncestorUserData();
        }
        public static IEnumerable<string> GetClasses<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.GetClasses();
        }
        public static T GetFirstAncestorOfType<T>(this T visualElement) where T : VisualElement
        {
            return visualElement.GetFirstAncestorOfType();
        }
        public static int IndexOf<T>(this T visualElement, T target) where T : VisualElement
        {
            return visualElement.IndexOf(target);
        }
        public static T Insert<T>(this T visualElement, int index, T target)
        {
            visualElement.Insert(index, target);
            return visualElement;
        }
        public static T Repaint<T>(this T visualElement) where T : VisualElement
        {
            visualElement.MarkDirtyRepaint();
            return visualElement;
        }
        public static T PlaceBehind<T>(this T visualElement, T target) where T : VisualElement
        {
            visualElement.PlaceBehind(target);
            return visualElement;
        }
        public static T PlaceInFront<T>(this T visualElement, T target) where T : VisualElement
        {
            visualElement.PlaceInFront(target);
            return visualElement;
        }
        public static T Remove<T>(this T visualElement, T element) where T : VisualElement
        {
            visualElement.Remove(element);
            return visualElement;
        }
        public static T RemoveAt<T>(this T visualElement, int index) where T : VisualElement
        {
            visualElement.RemoveAt(index);
            return visualElement;
        }
        public static T RemoveFromHierarchy<T>(this T visualElement) where T : VisualElement
        {
            visualElement.RemoveFromHierarchy();
            return visualElement;
        }
        public static T RemoveFromClassList<T>(this T visualElement, string className) where T : VisualElement
        {
            visualElement.RemoveFromClassList(className);
            return visualElement;
        }
        public static T SendEvent<T>(this T visualElement, EventBase evt) where T : VisualElement
        {
            visualElement.SendEvent(evt);
            return visualElement;
        }
        public static T Sort<T>(this T visualElement, Comparison<T> element) where T : VisualElement
        {
            visualElement.Sort(element);
            return visualElement;
        }
        public static T ToggleClassInList<T>(this T visualElement, string value) where T : VisualElement
        {
            visualElement.ToggleInClassList(value);
            return visualElement;
        }
        public static bool CanGrabFocus(this VisualElement visualElement)
        {
            return visualElement.canGrabFocus;
        }
        public static bool DelegateFocus(this VisualElement visualElement)
        {
            return visualElement.delegatesFocus;
        }
        public static T DelegateFocus<T>(this T visualElement, bool state) where T : VisualElement
        {
            visualElement.delegatesFocus = state;
            return visualElement;
        }
        public static T Focusable<T>(this T visualElement, bool value) where T : VisualElement
        {
            visualElement.focusable = value;
            return visualElement;
        }
        public static bool Focusable(this VisualElement visualElement)
        {
            return visualElement.focusable;
        }
        public static FocusController FocusController(this VisualElement visualElement)
        {
            return visualElement.focusController;
        }
        public static int TabIndex(this VisualElement visualElement)
        {
            return visualElement.tabIndex;
        }
        public static T TabIndex<T>(this T visualElement, int index) where T : VisualElement
        {
            visualElement.tabIndex = index;
            return visualElement;
        }
        public static bool HasBubbleUpHandler(this VisualElement visualElement)
        {
            return visualElement.HasBubbleUpHandlers();
        }
        public static bool HasTrickleDownHandler(this VisualElement visualElement)
        {
            return visualElement.HasTrickleDownHandlers();
        }
        public static T Blur<T>(this T visualElement) where T : VisualElement
        {
            visualElement.Blur();
            return visualElement;
        }
        public static T Focus<T>(this T visualElement) where T : VisualElement
        {
            visualElement.Focus();
            return visualElement;
        }
        public static T FontSize<T>(this T visualElement, float size, bool dynamic = false) where T : VisualElement
        {
            Length len = new Length(size, LengthUnit.Pixel);

            if (dynamic)
            {
                len = new Length(size, LengthUnit.Percent);
            }

            visualElement.style.fontSize = new StyleLength(len);
            return visualElement;
        }
        public static T Opacity<T>(this T visualElement, float opacity) where T : VisualElement
        {
            visualElement.style.opacity = opacity;
            return visualElement;
        }
        public static T FontStyleAndWeight<T>(this T visualElement, FontStyle fontStyle) where T : VisualElement
        {
            visualElement.style.unityFontStyleAndWeight = fontStyle;
            return visualElement;
        }

        public static T Font<T>(this T visualElement, StyleFont font) where T : VisualElement
        {
            visualElement.style.unityFont = font;
            return visualElement;
        }
        public static T FontDefinition<T>(this T visualElement, StyleFontDefinition fontDefinition) where T : VisualElement
        {
            visualElement.style.unityFontDefinition = fontDefinition;
            return visualElement;
        }
        public static T OverflowClipbox<T>(this T visualElement, OverflowClipBox overflowClipBox) where T : VisualElement
        {
            visualElement.style.unityOverflowClipBox = overflowClipBox;
            return visualElement;
        }
        public static T ParagraphSpacing<T>(this T visualElement, StyleLength value) where T : VisualElement
        {
            visualElement.style.unityParagraphSpacing = value;
            return visualElement;
        }
        public static T OverflowPosition<T>(this T visualElement, TextOverflowPosition textOverflow) where T : VisualElement
        {
            visualElement.style.unityTextOverflowPosition = textOverflow;
            return visualElement;
        }
        public static T Cursor<T>(this T visualElement, StyleCursor cursor) where T : VisualElement
        {
            visualElement.style.cursor = cursor;
            return visualElement;
        }
        public static T Position<T>(this T visualElement, Position position) where T : VisualElement
        {
            visualElement.style.position = position;
            return visualElement;
        }
        public static T Display<T>(this T visualElement, DisplayStyle display) where T : VisualElement
        {
            visualElement.style.display = display;
            return visualElement;
        }
        public static T JustifyContent<T>(this T visualElement, Justify justify) where T : VisualElement
        {
            visualElement.style.justifyContent = justify;
            return visualElement;
        }
        public static T LetterSpacing<T>(this T visualElement, StyleLength value) where T : VisualElement
        {
            visualElement.style.letterSpacing = value;
            return visualElement;
        }
        public static T SetOverflow<T>(this T visualElement, Overflow overflow) where T : VisualElement
        {
            visualElement.style.overflow = overflow;
            return visualElement;
        }
        public static T Rotate<T>(this T visualElement, Rotate rotate) where T : VisualElement
        {
            visualElement.style.rotate = rotate;
            return visualElement;
        }
        public static T Scale<T>(this T visualElement, StyleScale scale) where T : VisualElement
        {
            visualElement.style.scale = scale;
            return visualElement;
        }
        public static T TextOverflow<T>(this T visualElement, TextOverflow textOverflow) where T : VisualElement
        {
            visualElement.style.textOverflow = textOverflow;
            return visualElement;
        }
        public static T Choices<T>(this T visualElement, List<string> choices) where T : DropdownField
        {
            visualElement.choices = choices;
            return visualElement;
        }
        public static T ItemSource<T>(this T visualElement, IList itemSource) where T : BaseVerticalCollectionView
        {
            visualElement.itemsSource = itemSource;
            return visualElement;
        }
        public static T MakeItem<T>(this T visualElement, Func<VisualElement> makeItem) where T : ListView
        {
            visualElement.makeItem = makeItem;
            return visualElement;
        }

        public static T BindItem<T>(this T visualElement, Action<VisualElement, int> bindItem) where T : ListView
        {
            visualElement.bindItem = bindItem;
            return visualElement;
        }
        public static T Reorderable<T>(this T visualElement, bool state) where T : BaseVerticalCollectionView
        {
            visualElement.reorderable = state;
            return visualElement;
        }
        public static T ShowBorder<T>(this T visualElement, bool state) where T : BaseVerticalCollectionView
        {
            visualElement.showBorder = state;
            return visualElement;
        }
        public static (Vector4 width, (StyleColor left, StyleColor right, StyleColor top, StyleColor bottom) color) Border<T>(this T visualElement) where T : VisualElement
        {
            var val = visualElement.style;
            return (new Vector4(val.borderLeftWidth.value, val.borderRightWidth.value, val.borderTopWidth.value, val.borderBottomWidth.value), (val.borderLeftColor, val.borderRightColor, val.borderTopColor, val.borderBottomColor));
        }
        public static T TextShadow<T>(this T visualElement, StyleTextShadow textShadow) where T : VisualElement
        {
            visualElement.style.textShadow = textShadow;
            return visualElement;
        }
        public static T UnbindItem<T>(this T visualElement, Action<VisualElement, int> unbindItem) where T : ListView
        {
            visualElement.unbindItem = unbindItem;
            return visualElement;
        }
        public static T SelectionType<T>(this T visualElement, SelectionType selectionType) where T : BaseVerticalCollectionView
        {
            visualElement.selectionType = selectionType;
            return visualElement;
        }
        public static T HorizontalScrollingEnabled<T>(this T visualElement, bool state) where T : BaseVerticalCollectionView
        {
            visualElement.horizontalScrollingEnabled = state;
            return visualElement;
        }
        public static T ScrollTo<T>(this T visualElement, VisualElement value) where T : BaseVerticalCollectionView
        {
            visualElement.ScrollTo(value);
            return visualElement;
        }
        public static T ScrollToId<T>(this T visualElement, int value) where T : BaseVerticalCollectionView
        {
            visualElement.ScrollToItemById(value);
            return visualElement;
        }
        public static T ScrollToItem<T>(this T visualElement, VisualElement value) where T : BaseVerticalCollectionView
        {
            visualElement.ScrollToItem(value);
            return visualElement;
        }
        public static int SelectedIndex<T>(this T visualElement) where T : BaseVerticalCollectionView
        {
            return visualElement.selectedIndex;
        }
        public static IEnumerable<int> SelectedIndices<T>(this T visualElement) where T : BaseVerticalCollectionView
        {
            return visualElement.selectedIndices;
        }
        public static object SelectedItem<T>(this T visualElement) where T : BaseVerticalCollectionView
        {
            return visualElement.selectedItem;
        }
        public static IEnumerable<object> SelectedItems<T>(this T visualElement) where T : BaseVerticalCollectionView
        {
            return visualElement.selectedItems;
        }
        public static T ShowAlternatingRow<T>(this T visualElement, AlternatingRowBackground state) where T : BaseVerticalCollectionView
        {
            visualElement.showAlternatingRowBackgrounds = state;
            return visualElement;
        }
        public static T VirualizationMethod<T>(this T visualElement, CollectionVirtualizationMethod state) where T : BaseVerticalCollectionView
        {
            visualElement.virtualizationMethod = state;
            return visualElement;
        }
        public static T TransformOrigin<T>(this T visualElement, StyleTransformOrigin transformOrigin) where T : VisualElement
        {
            visualElement.style.transformOrigin = transformOrigin;
            return visualElement;
        }
        public static T TransitionDelay<T>(this T visualElement, StyleList<TimeValue> timeValue) where T : VisualElement
        {
            visualElement.style.transitionDelay = timeValue;
            return visualElement;
        }
        public static T TransitionDuration<T>(this T visualElement, StyleList<TimeValue> timeValue) where T : VisualElement
        {
            visualElement.style.transitionDuration = timeValue;
            return visualElement;
        }
        public static T TransitionProperty<T>(this T visualElement, StyleList<StylePropertyName> transitionProperty) where T : VisualElement
        {
            visualElement.style.transitionProperty = transitionProperty;
            return visualElement;
        }
        public static T TransitionTiming<T>(this T visualElement, StyleList<EasingFunction> easing) where T : VisualElement
        {
            visualElement.style.transitionTimingFunction = easing;
            return visualElement;
        }
        public static T Translate<T>(this T visualElement, StyleTranslate translate) where T : VisualElement
        {
            visualElement.style.translate = translate;
            return visualElement;
        }
        public static T CopySize<T>(this T visualElement, T source) where T : VisualElement
        {
            source.schedule.Execute(() =>
            {
                visualElement.style.width = source.resolvedStyle.width;
                visualElement.style.height = source.resolvedStyle.height;
            });
            return visualElement;
        }
        public static T CopyPadding<T>(this T visualElement, T source) where T : VisualElement
        {
            source.schedule.Execute(() =>
            {
                visualElement.style.paddingBottom = source.resolvedStyle.paddingBottom;
                visualElement.style.paddingLeft = source.resolvedStyle.paddingLeft;
                visualElement.style.paddingRight = source.resolvedStyle.paddingRight;
                visualElement.style.paddingTop = source.resolvedStyle.paddingTop;
            });
            return visualElement;
        }
        public static T CopyMargin<T>(this T visualElement, T source) where T : VisualElement
        {
            source.schedule.Execute(() =>
            {
                visualElement.style.marginBottom = source.resolvedStyle.marginBottom;
                visualElement.style.marginLeft = source.resolvedStyle.marginLeft;
                visualElement.style.marginRight = source.resolvedStyle.marginRight;
                visualElement.style.marginTop = source.resolvedStyle.marginTop;
            });

            return visualElement;
        }
        public static T CopyBorder<T>(this T visualElement, T source) where T : VisualElement
        {
            source.schedule.Execute(() =>
            {
                visualElement.style.borderBottomLeftRadius = source.resolvedStyle.borderBottomLeftRadius;
                visualElement.style.borderBottomRightRadius = source.resolvedStyle.borderBottomRightRadius;
                visualElement.style.borderTopRightRadius = source.resolvedStyle.borderTopRightRadius;
                visualElement.style.borderTopLeftRadius = source.resolvedStyle.borderTopLeftRadius;

                visualElement.style.borderBottomColor = source.resolvedStyle.borderBottomColor;
                visualElement.style.borderRightColor = source.resolvedStyle.borderRightColor;
                visualElement.style.borderTopColor = source.resolvedStyle.borderTopColor;
                visualElement.style.borderLeftColor = source.resolvedStyle.borderLeftColor;

                visualElement.style.borderBottomWidth = source.resolvedStyle.borderBottomWidth;
                visualElement.style.borderLeftWidth = source.resolvedStyle.borderLeftWidth;
                visualElement.style.borderRightWidth = source.resolvedStyle.borderRightWidth;
                visualElement.style.borderBottomWidth = source.resolvedStyle.borderBottomWidth;
            });
            return visualElement;
        }
        public static T TextAlignment<T>(this T visualElement, TextAnchor align) where T : VisualElement
        {
            visualElement.style.unityTextAlign = align;
            return visualElement;
        }
        public static T MinWidth<T>(this T visualElement, float minWidth, bool isDynamic = false) where T : VisualElement
        {
            Length len = new Length(minWidth, LengthUnit.Pixel);

            if (isDynamic)
            {
                len = new Length(minWidth, LengthUnit.Percent);
            }

            visualElement.style.minWidth = new StyleLength(len);
            return visualElement;
        }
        public static T MaxWidth<T>(this T visualElement, float maxWidth, bool isDynamic = false) where T : VisualElement
        {
            Length len = new Length(maxWidth, LengthUnit.Pixel);

            if (isDynamic)
            {
                len = new Length(maxWidth, LengthUnit.Percent);
            }

            visualElement.style.maxWidth = new StyleLength(len);
            return visualElement;
        }
        public static T MinHeight<T>(this T visualElement, float minHeight, bool isDynamic = false) where T : VisualElement
        {
            Length len = new Length(minHeight, LengthUnit.Pixel);

            if (isDynamic)
            {
                len = new Length(minHeight, LengthUnit.Percent);
            }

            visualElement.style.minHeight = new StyleLength(len);
            return visualElement;
        }
        public static T MaxHeight<T>(this T visualElement, float maxHeight, bool isDynamic = false) where T : VisualElement
        {
            Length len = new Length(maxHeight, LengthUnit.Pixel);

            if (isDynamic)
            {
                len = new Length(maxHeight, LengthUnit.Percent);
            }

            visualElement.style.maxHeight = new StyleLength(len);
            return visualElement;
        }
        public static T Visibility<T>(this T visualElement, Visibility visibility) where T : VisualElement
        {
            visualElement.style.visibility = visibility;
            return visualElement;
        }
        public static T Text<T>(this T visualElement, string text) where T : VisualElement
        {
            if (visualElement is TextField txtf)
            {
                txtf.value = text;
            }
            if (visualElement is TextElement vis)
            {
                vis.text = text;
            }
            return visualElement;
        }
        public static T Size<T>(this T visualElement, Vector2 size, bool isDynamic = false) where T : VisualElement
        {
            Length xwidth = new Length(size.x, LengthUnit.Pixel);
            Length xheight = new Length(size.y, LengthUnit.Pixel);

            if (isDynamic)
            {
                xwidth = new Length(size.x, LengthUnit.Percent);
                xheight = new Length(size.y, LengthUnit.Percent);
            }

            visualElement.style.width = new StyleLength(xwidth);
            visualElement.style.width = new StyleLength(xheight);
            return visualElement;
        }
        public static T OnValueChanged<T>(this T visualElement, EventCallback<ChangeEvent<Color>> callback) where T : VisualElement, INotifyValueChanged<Color>
        {
            visualElement.OnDetachedFromPanel(x => { visualElement.UnregisterValueChangedCallback(callback); });
            visualElement.RegisterValueChangedCallback(callback);
            return visualElement;
        }
        public static T OnValueChanged<T>(this T visualElement, EventCallback<ChangeEvent<string>> callback) where T : VisualElement, INotifyValueChanged<string>
        {
            visualElement.OnDetachedFromPanel(x => { visualElement.UnregisterValueChangedCallback(callback); });
            visualElement.RegisterValueChangedCallback(callback);
            return visualElement;
        }
        public static T OnValueChanged<T>(this T visualElement, EventCallback<ChangeEvent<float>> callback) where T : VisualElement, INotifyValueChanged<float>
        {
            visualElement.OnDetachedFromPanel(x => { visualElement.UnregisterValueChangedCallback(callback); });
            visualElement.RegisterValueChangedCallback(callback);
            return visualElement;
        }
        public static T OnValueChanged<T>(this T visualElement, EventCallback<ChangeEvent<double>> callback) where T : VisualElement, INotifyValueChanged<double>
        {
            visualElement.OnDetachedFromPanel(x => { visualElement.UnregisterValueChangedCallback(callback); });
            visualElement.RegisterValueChangedCallback(callback);
            return visualElement;
        }
        public static T OnValueChanged<T>(this T visualElement, EventCallback<ChangeEvent<bool>> callback) where T : VisualElement, INotifyValueChanged<bool>
        {
            visualElement.OnDetachedFromPanel(x => { visualElement.UnregisterValueChangedCallback(callback); });
            visualElement.RegisterValueChangedCallback(callback);
            return visualElement;
        }
        public static T OnValueChanged<T>(this T visualElement, EventCallback<ChangeEvent<Vector2>> callback) where T : VisualElement, INotifyValueChanged<Vector2>
        {
            visualElement.OnDetachedFromPanel(x => { visualElement.UnregisterValueChangedCallback(callback); });
            visualElement.RegisterValueChangedCallback(callback);
            return visualElement;
        }
        public static T OnValueChanged<T>(this T visualElement, EventCallback<ChangeEvent<Vector3>> callback) where T : VisualElement, INotifyValueChanged<Vector3>
        {
            visualElement.OnDetachedFromPanel(x => { visualElement.UnregisterValueChangedCallback(callback); });
            visualElement.RegisterValueChangedCallback(callback);
            return visualElement;
        }
        public static T OnValueChanged<T>(this T visualElement, EventCallback<ChangeEvent<Vector4>> callback) where T : VisualElement, INotifyValueChanged<Vector4>
        {
            visualElement.OnDetachedFromPanel(x => { visualElement.UnregisterValueChangedCallback(callback); });
            visualElement.RegisterValueChangedCallback(callback);
            return visualElement;
        }
        public static T OnValueChanged<T>(this T visualElement, EventCallback<ChangeEvent<Matrix4x4>> callback) where T : VisualElement, INotifyValueChanged<Matrix4x4>
        {
            visualElement.OnDetachedFromPanel(x => { visualElement.UnregisterValueChangedCallback(callback); });
            visualElement.RegisterValueChangedCallback(callback);
            return visualElement;
        }
        public static T OnValueChanged<T>(this T visualElement, EventCallback<ChangeEvent<decimal>> callback) where T : VisualElement, INotifyValueChanged<decimal>
        {
            visualElement.RegisterValueChangedCallback(callback);
            return visualElement;
        }
        public static T OnValueChanged<T>(this T visualElement, EventCallback<ChangeEvent<UnityEngine.Object>> callback) where T : VisualElement, INotifyValueChanged<UnityEngine.Object>
        {
            visualElement.OnDetachedFromPanel(x => { visualElement.UnregisterValueChangedCallback(callback); });
            visualElement.RegisterValueChangedCallback(callback);
            return visualElement;
        }
        public static T OnValueChanged<T>(this T visualElement, EventCallback<ChangeEvent<object>> callback) where T : VisualElement, INotifyValueChanged<object>
        {
            visualElement.OnDetachedFromPanel(x => { visualElement.UnregisterValueChangedCallback(callback); });
            visualElement.RegisterValueChangedCallback(callback);
            return visualElement;
        }
        public static T OnValueChanged<T>(this T visualElement, EventCallback<ChangeEvent<int>> callback) where T : VisualElement, INotifyValueChanged<int>
        {
            visualElement.OnDetachedFromPanel(x => { visualElement.UnregisterValueChangedCallback(callback); });
            visualElement.RegisterValueChangedCallback(callback);
            return visualElement;
        }
        public static T OnValueChanged<T>(this T visualElement, EventCallback<ChangeEvent<long>> callback) where T : VisualElement, INotifyValueChanged<long>
        {
            visualElement.OnDetachedFromPanel(x => { visualElement.UnregisterValueChangedCallback(callback); });
            visualElement.RegisterValueChangedCallback(callback);
            return visualElement;
        }
        public static T CopyStyle<T>(this T visualElement, VisualElement source) where T : VisualElement
        {
            source.schedule.Execute(() =>
            {
                visualElement.style.alignContent = source.resolvedStyle.alignContent;
                visualElement.style.alignItems = source.resolvedStyle.alignItems;
                visualElement.style.alignSelf = source.resolvedStyle.alignSelf;

                visualElement.CopyBorder(source);

                visualElement.style.color = source.resolvedStyle.color;
                visualElement.style.cursor = source.style.cursor;
                visualElement.style.display = source.resolvedStyle.display;

                visualElement.style.flexBasis = source.style.flexBasis;
                visualElement.style.flexDirection = source.resolvedStyle.flexDirection;
                visualElement.style.flexGrow = source.resolvedStyle.flexGrow;
                visualElement.style.flexShrink = source.resolvedStyle.flexShrink;
                visualElement.style.flexWrap = source.resolvedStyle.flexWrap;

                visualElement.style.fontSize = source.resolvedStyle.fontSize;
                visualElement.style.height = source.resolvedStyle.height;
                visualElement.style.width = source.resolvedStyle.width;
                visualElement.style.justifyContent = source.style.justifyContent;

                visualElement.style.left = source.resolvedStyle.left;
                visualElement.style.right = source.resolvedStyle.right;
                visualElement.style.top = source.resolvedStyle.top;
                visualElement.style.bottom = source.resolvedStyle.bottom;

                visualElement.style.letterSpacing = source.resolvedStyle.letterSpacing;
                visualElement.CopyMargin(source);

                visualElement.style.maxWidth = source.style.maxWidth;
                visualElement.style.maxHeight = source.style.maxHeight;
                visualElement.style.minHeight = source.style.minHeight;

                visualElement.style.opacity = source.resolvedStyle.opacity;
                visualElement.style.overflow = source.style.overflow;

                visualElement.CopyPadding(source);

                visualElement.style.position = source.resolvedStyle.position;
                visualElement.style.rotate = source.resolvedStyle.rotate;
                visualElement.style.scale = source.resolvedStyle.scale;
                visualElement.style.textOverflow = source.resolvedStyle.textOverflow;
                visualElement.style.textShadow = source.style.textShadow;
                visualElement.style.transformOrigin = source.style.transformOrigin;
                visualElement.style.transitionDelay = source.style.transitionDelay;
                visualElement.style.transitionDuration = source.style.transitionDuration;
                visualElement.style.transitionProperty = source.style.transitionProperty;
                visualElement.style.transitionTimingFunction = source.style.transitionTimingFunction;
                visualElement.style.translate = source.style.translate;

                visualElement.style.unityBackgroundImageTintColor = source.resolvedStyle.unityBackgroundImageTintColor;
                visualElement.style.unityFont = source.resolvedStyle.unityFont;
                visualElement.style.unityFontDefinition = source.resolvedStyle.unityFontDefinition;
                visualElement.style.unityFontStyleAndWeight = source.resolvedStyle.unityFontStyleAndWeight;
                visualElement.style.unityOverflowClipBox = source.style.unityOverflowClipBox;
                visualElement.style.unityParagraphSpacing = source.resolvedStyle.unityParagraphSpacing;
                visualElement.style.unitySliceBottom = source.resolvedStyle.unitySliceBottom;
                visualElement.style.unitySliceLeft = source.resolvedStyle.unitySliceLeft;
                visualElement.style.unitySliceRight = source.resolvedStyle.unitySliceRight;
                visualElement.style.unitySliceTop = source.resolvedStyle.unitySliceTop;
                visualElement.style.unityTextAlign = source.resolvedStyle.unityTextAlign;
                visualElement.style.unityTextOutlineColor = source.resolvedStyle.unityTextOutlineColor;
                visualElement.style.unityTextOutlineWidth = source.resolvedStyle.unityTextOutlineWidth;
                visualElement.style.unityTextOverflowPosition = source.resolvedStyle.unityTextOverflowPosition;
                visualElement.style.visibility = source.resolvedStyle.visibility;
                visualElement.style.whiteSpace = source.resolvedStyle.whiteSpace;
                visualElement.style.wordSpacing = source.resolvedStyle.wordSpacing;
            });
            return visualElement;
        }
        public static VisualElement Separator(float width, float height = 5f, bool widthIsDynamic = false, bool heightIsDynamic = false)
        {
            var lbl = new Label();
            lbl.BcgColor(ColorTwo);
            lbl.Width(width, widthIsDynamic);
            lbl.Height(height, heightIsDynamic);
            return lbl;
        }
        ///<summary>Use screen height divission for sizes.</summary>
        public static float GetScreenScale(float value) { return Screen.currentResolution.height / value; }
        public static T GetTemplate<T>(this T visualElement, string useTemplate = "template1") where T : VisualElement
        {
            if (useTemplate == "template1")
                visualElement.Size(template1.width, GetScreenScale(34), template1.xdynamic, template1.ydynamic);
            if (useTemplate == "template2")
                visualElement.Size(template2.width, GetScreenScale(34), template2.xdynamic, template2.ydynamic);
            return visualElement;
        }
        public static VisualElement FindRoot<T>(this T visualElement, int iteration = 50) where T : VisualElement
        {
            VisualElement prevParent = null;

            for (int i = 0; i < iteration; i++)
            {
                VisualElement parent = null;

                if (prevParent == null)
                    parent = visualElement.parent;
                else
                    parent = prevParent.parent;

                if (parent != null)
                {
                    prevParent = parent;
                }
                else
                {
                    return prevParent;
                }
            }

            return null;
        }
        public static VisualElement FindRootEditor<T>(this T visualElement, int iteration = 50) where T : VisualElement
        {
            VisualElement prevParent = null;

            while (iteration > 0)
            {
                VisualElement parent = null;

                if (prevParent == null)
                    parent = visualElement.parent;
                else
                    parent = prevParent.parent;

                if (parent != null)
                {
                    prevParent = parent;
                }
                else
                {
                    foreach(var f in prevParent.Children())
                    {
                        if(f.name.Contains("rootVisualContainer"))
                        {
                            return f;
                        }
                    }

                    return null;
                }

                iteration--;
            }

            return null;
        }
        public static List<VisualElement> GetChild<T>(this T visualElement, string name) where T : VisualElement
        {
            var q = visualElement.Query<VisualElement>(name: "name").ToList();
            return q;
        }
        ///<summary>Calculates aspect ratio of parent and maintain the size while resizing.</summary>
        public static (float width, float height) AspectRatio<T>(this T visualElement, VisualElement parent, Vector2 padding) where T : VisualElement
        {
            float targetArea = (parent.resolvedStyle.width - padding.x) * (parent.resolvedStyle.height - padding.y);
            float new_width = Mathf.Sqrt((visualElement.resolvedStyle.width / visualElement.resolvedStyle.height) * targetArea);
            float new_height = targetArea / new_width;

            float w = Mathf.Round(new_width); // round to the nearest integer
            float h = Mathf.Round(new_height - (w - new_width)); // adjust the rounded width with height    
            return (w, h);
        }
        public static float ScreenRatio(float value)
        {
            return ((float)Screen.currentResolution.height / (float)Screen.currentResolution.width) * value;
        }

#if UNITY_EDITOR
        public static T SaveTemplate<T>(this T visualElement, string name, bool replace = false) where T : VisualElement
        {
            var tmp = AssetDatabase.LoadAssetAtPath<UTKScriptable>(upath);

            if (tmp == null)
            {
                return visualElement;
            }

            if (!tmp.Template.Exists(x => x.templateName == name))
            {
                tmp.Template.Add(new UTKTemplate { templateName = name, template = visualElement });
            }
            else
            {
                if (!replace)
                    Debug.LogWarning("Template with the same name exists! Failed to save use the overload replace: true to replace.");
                else
                {
                    var idx = tmp.Template.FindIndex(x => x.templateName == name);
                    tmp.Template[idx].template = visualElement;
                }
            }
            return visualElement;
        }
#endif
    }

    public enum UDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    public enum UPos
    {
        Top,
        Down,
        Left,
        Right
    }
}