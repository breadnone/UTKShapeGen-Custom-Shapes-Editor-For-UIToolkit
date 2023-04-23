using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    public class UTKFlexible : VisualElement, INotifyValueChanged<string>
    {
        public EventCallback<int> onSelectionChanged;
        public EventCallback<int> onMouseDown;
        public (VisualElement mainContainer, VisualElement header) containers;
        public int selectedIndex { get; set; } = -1;
        private string _value;
        private List<VisualElement> _choices;
        private float defaulWidth;
        private float _sizeMultiplier = 1.7f;
        private float sizeMultiplier
        {
            get
            {
                return _sizeMultiplier;
            }
            set
            {
                _sizeMultiplier = value;
                Construct();
            }
        }
        private List<VisualElement> choices
        {
            get
            {
                return _choices;
            }
            set
            {
                _choices = value;
            }
        }
        private int? _defaultSelected;
        public int defaultSelected
        {
            get
            {
                if (_defaultSelected == null)
                    return 0;
                else
                    return _defaultSelected.Value;
            }
            set
            {
                _defaultSelected = value;
            }
        }

        public string value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        public UTKFlexible(List<VisualElement> choices = null)
        {
            this.FlexGrow(1);
            this.choices = choices;
            containers = (new VisualElement(), new VisualElement());
            containers.mainContainer.Size(100, 70, true, true).FlexRow();
            containers.header.FlexGrow(1);
            this.AddChild(containers.mainContainer).AddChild(containers.header);
            Construct();
        }
        public void Construct()
        {
            if (choices != null && choices.Count > 0)
            {
                for (int i = 0; i < choices.Count; i++)
                {
                    var par = choices[i];

                    if (par != null)
                    {
                        par.Clear();
                    }
                }
            }

            containers.mainContainer.Clear();
            GenerateElements();
        }
        private void GenerateElements()
        {
            if (choices == null)
                return;

            var siz = (float)100 / (float)choices.Count;
            VisualElement dummy = null;

            for (int i = 0; i < choices.Count; i++)
            {
                if (i == choices.Count - 1)
                    dummy = choices[i];

                var root = new VisualElement().Size(siz, 100, false, true).SetOverflow(Overflow.Hidden).FlexGrow(1).BcgColor(Color.blue).Border(2, Color.white);
                root.Add(choices[i]);
                choices[i].FlexGrow(1);
                containers.mainContainer.AddChild(root);

                var idx = i;
                root.OnMouseEnter(x => { PointerEnter(root, idx); });
                root.OnMouseExit(x => { PointerExit(root, idx); });
                root.OnMouseDown(x =>{onMouseDown?.Invoke(idx);});
            }

            containers.mainContainer.schedule.Execute(()=>
            {
                defaulWidth = dummy.parent.resolvedStyle.width;
            });

        }
        public void AddElement(VisualElement visualElement)
        {
            if (choices == null)
                choices = new List<VisualElement>();

            choices.Add(visualElement);
            schedule.Execute(() => Construct());
        }
        public void SetSelection(int index)
        {
            if (index < 0 || index >= choices.Count)
                return;

            RefreshItems();
            var vis = choices[index].parent;
            vis.Width(defaulWidth * sizeMultiplier);
            onSelectionChanged?.Invoke(index);
            selectedIndex = index;
        }
        private void PointerEnter(VisualElement vis, int index)
        {
            RefreshItems();
            vis.Width(defaulWidth * sizeMultiplier);
            selectedIndex = index;
        }
        private void PointerExit(VisualElement vis, int index)
        {
            vis.Width(defaulWidth);
            selectedIndex = -1;
            RefreshItems();
        }
        private void RefreshItems()
        {
            for (int i = 0; i < choices.Count; i++)
            {
                choices[i].parent.Width((float)100 / sizeMultiplier, false);
            }
        }
        public void Stretch(bool stretchOut)
        {
            int sum = stretchOut ? 1 : -1;
            int index = 0;

            if (selectedIndex + sum < 0 || selectedIndex + sum >= choices.Count)
                return;

            if (selectedIndex == -1)
            {
                index = stretchOut ? 0 : choices.Count - 1;
                selectedIndex = index;
            }
            else
            {
                selectedIndex += sum;
            }

            RefreshItems();
            var vis = choices[selectedIndex].parent;
            vis.Width(defaulWidth * sizeMultiplier);
            onSelectionChanged?.Invoke(selectedIndex);
        } 

        public void MoveNext() { Stretch(true); }
        public void MovePrevious() { Stretch(false); }
        public void SetValueWithoutNotify(string value)
        {
            _value = value;
        }
    }
}