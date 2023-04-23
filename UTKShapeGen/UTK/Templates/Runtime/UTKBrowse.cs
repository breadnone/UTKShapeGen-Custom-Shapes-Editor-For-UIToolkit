
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;
using UnityEditor;

namespace UTK
{
    ///<summary>Parent visualElement must be set to get the correct scaling.</summary>
    public class UTKBrowse : VisualElement, INotifyValueChanged<UnityEngine.Object>
    {
        public (VisualElement titleContainer, Label titleLabel, ScrollView itemContainer, Button okButton, Button cancelButton, VisualElement buttonContainer, VisualElement searchContainer) elements { get; private set; }
        public Type objectType { get; set; }
        private Color defColor; 
        private Color itemDefaulColor;
        private List<VisualElement> itemList = new List<VisualElement>();
        private VisualElement selected;
        public UnityEngine.Object value { get; set; }
        public Action<UnityEngine.Object> onSelected { get; set; }
        public VisualElement dragNDropParent{get;set;}
        public UTKBrowse(Type objectType, string title = "Browse", string okButtonText = "OK", string cancelButtonText = "Cancel", VisualElement highestPriorityParentForDragNdrop = null)
        {
            dragNDropParent = highestPriorityParentForDragNdrop;
            defColor = this.style.backgroundColor.value;
            itemDefaulColor = new Color(defColor.r + 0.2f, defColor.g + 0.1f, defColor.b + 0.3f);

            this.Border(2, Color.black).RoundCorner(10, true);

            this.objectType = objectType;
            elements = (new VisualElement(), new Label(), new ScrollView(), new Button(), new Button(), new VisualElement(), new VisualElement());

            elements.searchContainer.Size(100, 13, true, true).BcgColor(defColor).Padding(2, true);

            var searchTxt = Utk.TextField("Search ", "");
            searchTxt.label.Size(20, 100, true, true).TextAlignment(TextAnchor.MiddleLeft);
            searchTxt.obj.Size(80, 100, true, true);
            elements.searchContainer.AddChild(searchTxt.root);
            elements.searchContainer.userData = searchTxt.obj;

            this.Size(100, 100, true, true).Padding(2, true).FlexColumn();
            this.AddChild(elements.titleContainer).AddChild(elements.searchContainer).AddChild(elements.itemContainer).AddChild(elements.buttonContainer);
            elements.titleContainer.AddChild(elements.titleLabel);

            elements.okButton.text = okButtonText;
            elements.cancelButton.text = cancelButtonText;
            elements.buttonContainer.AddChild(elements.okButton).AddChild(elements.cancelButton);

            searchTxt.root.Size(100, 100, true, true);
            SetTitleStyle(title, new Vector2(100, 13), 12, Utk.ColorOne, Color.white, 2f);
            SetItemContainerStyle(ScrollViewMode.Vertical, new Vector2(100, 64));
            SetConfirmBoxStyle(new Vector2(100, 13), 100, 12, "Ok", "Cancel", Utk.ColorTwo, Color.blue, Color.red, 2f);
        }
        public virtual void SetTitleStyle(string text, Vector2 size, float textSize, Color backgroundColor, Color textColor, float padding)
        {
            var txt = elements.searchContainer.userData as TextField;

            txt.OnFocusOut(x =>
            {
                schedule.Execute(() =>
                {
                    if(!String.IsNullOrEmpty(txt.value))
                        ScrollTo(txt.value);
                });
            });
            
            var val = this.style.height.value.value;
            elements.titleContainer.Size(size.x, size.y, true, true).BcgColor(backgroundColor).Padding(padding, true).RoundCorner(3, true);
            elements.titleLabel.Text(text).Color(textColor).TextAlignment(TextAnchor.MiddleCenter).FontSize(textSize);
        }
        public virtual void SetItemContainerStyle(ScrollViewMode mode, Vector2 size)
        {
            elements.itemContainer.Size(size.x, size.y, true, true).BcgColor(new Color(defColor.r + 0.2f, defColor.g + 0.1f, defColor.b + 0.3f));
            elements.itemContainer.verticalScrollerVisibility = ScrollerVisibility.AlwaysVisible;
            elements.itemContainer.mode = mode;
            this.OnGeometryChanged(x => RefreshSizesOnChanged());
            CreateShowAssets();
        }
        public virtual void SetConfirmBoxStyle(Vector2 size, float buttonSize, float textSize, string confirmButtonText, string cancelButtonText, Color backgroundColor, Color confirmColor, Color cancelColor, float padding)
        {
            elements.buttonContainer.BcgColor(backgroundColor).Size(size.x, size.y, true, true).Padding(padding).FlexRow();
            elements.okButton.Text(confirmButtonText).BcgColor(confirmColor).Size(buttonSize / 2f, 100, true, true).FontSize(textSize, true);
            elements.cancelButton.Text(cancelButtonText).BcgColor(cancelColor).Size(buttonSize / 2f, 100, true, true).FontSize(textSize, true);
            elements.buttonContainer.AddChild(elements.okButton).AddChild(elements.cancelButton);
        }
        private void CreateShowAssets()
        {
            var assets = FindAssetsByType();

            if (assets == null || assets.Count == 0)
                return;

            for (int i = 0; i < assets.Count; i++)
            {
                if (assets[i].thumbnail == null || assets[i].obj == null)
                    continue;

                CreateItemTemplate(assets[i].thumbnail, assets[i].obj);
            }
        }
        private List<(UnityEngine.Object obj, Texture2D thumbnail)> FindAssetsByType()
        {
            string[] guids;
            List<(UnityEngine.Object, Texture2D)> lis = new();
            guids = AssetDatabase.FindAssets("t:" + objectType.Name);

            if (guids == null || guids.Length == 0)
                return lis;

            foreach (string guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var obj = AssetDatabase.LoadAssetAtPath(path, objectType);
                var prev = AssetPreview.GetAssetPreview(obj);
                lis.Add((obj, prev));
            }

            return lis;
        }
        private void CreateItemTemplate(Texture2D texture, UnityEngine.Object obj)
        {
            var siz = elements.itemContainer.style.width.value.value / 2.2f;
            var root = new VisualElement().Size(100, siz, true, false).BorderBottom(2, Color.grey).FlexRow();
            var img = new Image().Size(30, 100, true, true);
            img.scaleMode = ScaleMode.ScaleToFit;
            img.name = "imgContainer";
            img.sprite = obj as Sprite;
            img.userData = this;

            var lbl = new Label().Size(70, 100, true, true).TextAlignment(TextAnchor.MiddleLeft);
            lbl.Text("   " + obj.name);
            root.userData = (obj, lbl);
            lbl.userData = obj.name;

            root.OnMouseDown(x =>
            {
                value = obj;
                selected = root;
                SelectChangeColor(root);
                onSelected?.Invoke(obj);
            });

            lbl.OnMouseOver(x =>
            {
                root.Size(100, siz* 1.2f, true, false);

                if(root != selected)
                    root.BcgColor(Color.blue*0.2f);
            });
            lbl.OnMouseOut(x =>
            {
                root.Size(100, siz, true, false);

                if(root != selected)
                    root.BcgColor(defColor);
            });

            root.AddChild(lbl).AddChild(img);
            elements.itemContainer.contentContainer.Add(root);
            itemList.Add(root);
        }
        private void RefreshSizesOnChanged()
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                var siz = elements.itemContainer.style.width.value.value / 2.2f;
                itemList[i].Size(100, siz, true, false);
            }
        }
        private void ScrollTo(string fileName)
        {
            schedule.Execute(() =>
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    var t = ((UnityEngine.Object, Label))itemList[i].userData;
                    var str = (string)t.Item2.userData;

                    if (str.Contains(fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        elements.itemContainer.ScrollTo(itemList[i]);
                        SelectChangeColor(itemList[i]);
                        value = t.Item1;
                        break;
                    }
                }
            });
        }
        public void SetValueWithoutNotify(UnityEngine.Object val){value = val;}
        private void SelectChangeColor(VisualElement visualElement)
        {
            visualElement.BcgColor(Utk.ColorOne);

            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i] != visualElement)
                {
                    itemList[i].BcgColor(itemDefaulColor);
                }
            }
        }
        public void OnCancelPressed(Action action)
        {
            elements.cancelButton.clicked +=()=>
            {
                action?.Invoke();
                this.RemoveFromHierarchy();
            };
        }
        public void RegisterSelectorLogic(Action action, bool mouseDown)
        {
            for(int i = 0; i < itemList.Count; i++)
            {
                if(mouseDown)
                {
                    itemList[i].OnMouseDown(x =>
                    {
                        action.Invoke();
                    });
                }
                else
                {
                    itemList[i].OnMouseUp(x =>
                    {
                        action.Invoke();
                    });
                }
            }
        }
    }
}
#endif