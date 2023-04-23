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
    ///<summary>Browsable image container.</summary>
    public class UTKImage : Image, INotifyValueChanged<Sprite>
    {
        private List<Action> Registers = new List<Action>();
        private Sprite _value;
        public Sprite value
        { 
            get
            {
                return _value;
            } 
            set
            {
                SetValueWithoutNotify(value);
            } 
        }
        public Label label{get;set;}
        private UTKImage visualElement;
        private (Color enter, float enterWidth, Color exit, float exitWidth) borders;
        private string defaultText;

        #if UNITY_EDITOR
        public ObjectField objectField { get; set; }
        #endif

        public UTKImage(float width = 100f, string emptyImagetext = "NO_IMAGE", float height = 100f, bool dynamic = false)
        {
            visualElement = this;
            this.Size(width, height, dynamic);
            defaultText = emptyImagetext;
            Construct();
        }

        private void Construct()
        {
            borders.exit = Utk.ColorTwo;
            borders.enter = Color.green;

            SetBorder(5, Utk.ColorTwo);
            var lbl = new Label().FlexGrow(1).BcgColor(Utk.ColorOne).TextAlignment(TextAnchor.MiddleCenter).Text("<b>+</b>\n <size=12>" + defaultText + "</size>").FontSize(22, true);
            this.AddChild(lbl as VisualElement).AddChild(ObjectPicker());
            label = lbl;

            OnDispose(x =>
            {
                if (Registers.Count == 0)
                    return;

                for (int i = 0; i < Registers.Count; i++)
                {
                    Registers[i]?.Invoke();
                }
            });

            MouseDown(x =>
            {
                if(value != null)
                {

                }
            });

            var f = new ObjectField();

            MouseEnter(x => { SetBorder(5, borders.enter); });
            MouseExit(x => { SetBorder(5, borders.exit); });
        }
        public void SetValueWithoutNotify(Sprite srpite){_value = sprite;}
        public void SetOnExitBorder(Color color, float width) { borders.exit = color; borders.exitWidth = width; }
        public void SetOnEnterBorder(Color color, float width) { borders.enter = color; borders.enterWidth = width; }
        private void OnDispose(EventCallback<DetachFromPanelEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown)
        {
            this.OnDetachedFromPanel(callback);
        }
        public virtual UTKImage MouseDown(EventCallback<MouseDownEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown)
        {
            Registers.Add(() => this.UnregisterCallback<MouseDownEvent>(x => callback.Invoke(x), trickleDown));
            this.OnMouseDown(callback, trickleDown);
            return this;
        }
        public virtual UTKImage MouseEnter(EventCallback<MouseEnterEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown)
        {
            Registers.Add(() => { this.UnregisterCallback<MouseEnterEvent>(x => callback.Invoke(x), trickleDown); });
            this.OnMouseEnter(callback, trickleDown);
            return this;
        }
        public virtual UTKImage MouseExit(EventCallback<MouseLeaveEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown)
        {
            Registers.Add(() => { this.UnregisterCallback<MouseLeaveEvent>(x => callback.Invoke(x), trickleDown); });
            this.OnMouseExit(callback, trickleDown);
            return this;
        }
        public void SetBorder(float width, Color color) { this.Border(width, color); }
        public void SetBorder(Vector4 thickness, Color color) { this.Border(thickness, color); }
        
        #if UNITY_EDITOR
        private VisualElement ObjectPicker()
        {
            var root = new VisualElement().Size(100, 100, true, true).FlexGrow(1);
            root.style.position = Position.Absolute;
            var obj = new ObjectField().Size(100, 100, true, true);
            obj.allowSceneObjects = false;
            obj.objectType = typeof(Sprite);
            obj.style.display = DisplayStyle.None;

            obj.RegisterValueChangedCallback(x =>
            {
                value = x.newValue as Sprite;
                visualElement.sprite = x.newValue as Sprite;

                if(x.newValue != null)
                {
                    label.RemoveFromHierarchy();
                }
                else
                {
                    visualElement.AddChild(label as VisualElement);
                }
            });

            // build the picker button (rip it out of the object field)
            VisualElement picker = obj[0][1];
            picker.style.backgroundImage = null;
            picker.style.opacity = 0f;
            picker.AlignSelf(Align.Center);
            picker.Size(100, 100, true, true);
            picker.style.position = Position.Absolute;
            root.AddChild(picker);

            root.Add(obj);
            root.Add(picker);
            return root;
        }

        public string GetAssetPath()
        {
            if (value == null)
                return null;
            else
                return AssetDatabase.GetAssetPath(value);
        }
        #endif
    }
}