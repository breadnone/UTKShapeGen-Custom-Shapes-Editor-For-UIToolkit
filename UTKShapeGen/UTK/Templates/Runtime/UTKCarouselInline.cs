using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using Exts;

namespace UTK
{
    public class UTKCarouselInline : VisualElement, INotifyValueChanged<string>
    {
        public (VisualElement mainContainer, VisualElement header) containers;
        ///<summary>Item templates.</summary>
        public Func<VisualElement> makeItem;
        ///<summary>Bind item template.</summary>
        public Action<VisualElement, int> bindItem;
        ///<summary>Listens to selection changes.</summary>
        public Action<int> onSelectedChanged;
        ///<summary>Listens to selected item changers.</summary>
        public Action onItemSelected;        
        ///<summary>Zoom ratio.</summary>
        public float zoomRatio { get; set; } = 1.2f;
        private Vector3 defaultScale;
        private List<string> _choices;
        private string _value;
        ///<summary>Shows selection.</summary>
        public bool enableSelection { get; set; } = true;
        ///<summary>Shows children when animations are done.</summary>
        public bool displayOnAnimComplete{get;set;}
        ///<summary>Sets value ignores events.</summary>
        public void SetValueWithoutNotify(string state) { _value = state; }
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
        public List<string> choices
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
        private float _scrollSpeed;
        private bool _enableHeader = true;
        private int _displayAmount = 3;
        public int displayAmount
        {
            get
            {
                return _displayAmount;
            }
            set
            {
                _displayAmount = value;
            }
        }
        
        public bool onHoverZoom { get; set; } = true;
        public int? selectedIndex { get; set; }
        private (int? start, int? end) activeIndex;
        public List<(VisualElement, bool isSelected)> placeHolders;
        private int? selectedPlaceholder;
        public bool enableHeader
        {
            get
            {
                return _enableHeader;
            }
            set
            {
                _enableHeader = value;
            }
        }
        public float scrollSpeed
        {
            get
            {
                return _scrollSpeed;
            }
            set
            {
                _scrollSpeed = value;
            }
        }
        public UTKCarouselInline(List<string> choices, Func<VisualElement> makeItem, Action<VisualElement, int> bindItem, bool horizontalLayout = true)
        {
            this.makeItem = makeItem;
            this.bindItem = bindItem;
            containers = (new VisualElement(), new VisualElement());
            this.AddChild(containers.header).AddChild(containers.mainContainer);
            placeHolders = new(displayAmount);
            this.choices = choices;
            this.FlexGrow(1);
            Construct();
        }
        private void Construct()
        {
            placeHolders.Clear();

            if (this.containers.mainContainer != null && this.containers.mainContainer.childCount > 0)
            {
                this.containers.mainContainer.Clear();
            }

            DrawHeader();

            this.containers.mainContainer.contentContainer.FlexRow();
            this.containers.mainContainer.SetOverflow(Overflow.Hidden).FlexGrow(1).JustifyContent(Justify.Center).AlignItems(Align.Center);
            float defSize = 100f / (float)displayAmount;

            for (int i = 0; i < displayAmount; i++)
            {
                var root = new VisualElement().Size(defSize, defSize, true, true).FlexShrink(0).JustifyContent(Justify.Center).AlignItems(Align.Center).SetOverflow(Overflow.Hidden);
                placeHolders.Add((root, false));
                this.containers.mainContainer.AddChild(root);
                root.OnMouseOver(x => PointerEnter(root));
                root.OnMouseExit(x => PointerExit(root));
            }

            Move(false);
        }
        private void ColorStateChange(VisualElement visualElement)
        {
            Ext.Repeat(placeHolders.Count, x=>
            {
                if(placeHolders[x].Item1 != visualElement)
                {
                    placeHolders[x].Item1.BcgColor(Color.clear);
                }
                else
                {
                    placeHolders[x].Item1.BcgColor("#744da9");
                }
            });
        }
        private void Move(bool next)
        {
            if (makeItem == null || bindItem == null)
                return;

            if (choices == null || choices.Count == 0)
                return;

            int sum = next ? 1 : -1;

            if (!selectedIndex.HasValue)
            {
                selectedIndex = 0;

                if (activeIndex.end > choices.Count)
                {
                    activeIndex = (0, choices.Count);
                }
                else
                {
                    activeIndex = (0, displayAmount);
                }
            }
            else
            {
                if (activeIndex.start.Value + sum > -1 && activeIndex.end.Value + sum <= choices.Count - 1)
                    activeIndex = (activeIndex.start.Value + sum, activeIndex.end.Value + sum);
            }

            int counta = 0; 

            for (int i = activeIndex.start.Value; i < activeIndex.end.Value; i++)
            {
                var item = makeItem.Invoke();
                bindItem.Invoke(item, i);
                placeHolders[counta].Item1.Clear();
                placeHolders[counta].Item1.AddChild(item);

                var idx = i;
                placeHolders[counta].Item1.userData = idx;
            
                var cnt = counta;
                item.OnMouseDown(x => 
                {
                    selectedPlaceholder = cnt; 
                    ColorStateChange(placeHolders[cnt].Item1);

                    if(selectedIndex.HasValue)
                        value = choices[selectedIndex.Value];
                });

                counta++;
            }
        }
        private void DrawHeader()
        {
            if (!enableHeader)
            {
                this.containers.header.Display(DisplayStyle.None);
                return;
            }
            else
            {
                this.containers.header.Display(DisplayStyle.Flex);
            }

            if (this.containers.header != null && this.containers.header.childCount > 0)
            {
                this.containers.header.Clear();
            }

            this.containers.header.Size(100, Utk.ScreenRatio(50), true, false).FlexRow().JustifyContent(Justify.Center);
            var btnPrev = new Button().Size(20, 100, true, true).Text("<<");
            var btnNext = new Button().Size(20, 100, true, true).Text(">>");

            var btnNexSec = new Button().Size(20, 100, true, true).Text("next");
            var btnPrevSec = new Button().Size(20, 100, true, true).Text("prev");

            containers.header.AddChild(btnPrevSec).AddChild(btnPrev).AddChild(btnNext).AddChild(btnNexSec);

            btnPrev.clicked += () => MovePrevious();
            btnNext.clicked += () => MoveNext();
            btnPrevSec.clicked += () => SelectPrevious();
            btnNexSec.clicked += () => SelectNext();
        }

        ///<summary>Scrolls to next set of items.</summary>
        public virtual void MoveNext()
        {
            ClearSelection().Move(true);
        }
        ///<summary>Scrolls to previous set of items.</summary>
        public virtual void MovePrevious()
        {
            ClearSelection().Move(false);
        }
        ///<summary>Clears selection.</summary>
        public UTKCarouselInline ClearSelection()
        {
            if(selectedPlaceholder.HasValue)
            {
                placeHolders[selectedPlaceholder.Value].Item1.BcgColor(Color.clear);
            }

            if (hovered != null)
            {
                hovered.Scale(new Scale(defaultScale));
                hovered = null;
            }

            selectedPlaceholder = null;
            return this;
        }

        private VisualElement hovered;
        private void PointerEnter(VisualElement visualElement)
        {
            if (!onHoverZoom)
                return;

            if (hovered != visualElement)
            {
                hovered = visualElement;
                defaultScale = visualElement.resolvedStyle.scale.value;
                visualElement.Scale(new Scale(new Vector3(defaultScale.x * zoomRatio, defaultScale.y * zoomRatio, defaultScale.z)));
            }

            visualElement.BcgColor("#744da9");
        }
        private void PointerExit(VisualElement visualElement)
        {
            if (!onHoverZoom)
                return;

            visualElement.Scale(new Scale(defaultScale));

            if (hovered == visualElement)
                hovered = null;

            if(selectedPlaceholder.HasValue && placeHolders[selectedPlaceholder.Value].Item1 != visualElement)
                visualElement.BcgColor(Color.clear);
            else
                visualElement.BcgColor(Color.clear);
        }
        private void SetSelection(int index, bool notifyEvent)
        {
            selectedIndex = index;
            
            if(notifyEvent)
                onItemSelected?.Invoke();

            if(!enableSelection)
                return;

            if(selectedPlaceholder.HasValue)
            {
                placeHolders[selectedPlaceholder.Value].Item1.BcgColor("#744da9");
            }
        }
        ///<summary>Selects next item.</summary>
        public void SelectNext(bool notify = true)
        {
            SelectNextPrev(true, notify);
        }
        ///<summary>Selects previous item.</summary>
        public void SelectPrevious(bool notify = true)
        {
            SelectNextPrev(false, notify);
        }
        private void SelectNextPrev(bool next, bool notify)
        {
            var sum = next ? 1 : -1;
            
            if(selectedPlaceholder.HasValue)
            {
                if(selectedPlaceholder + sum > -1 && selectedPlaceholder + sum < placeHolders.Count)
                {
                    PointerExit(placeHolders[selectedPlaceholder.Value].Item1);
                    placeHolders[selectedPlaceholder.Value].Item1.BcgColor(Color.clear);
                    selectedPlaceholder = selectedPlaceholder + sum;
                    SetSelection((int)placeHolders[selectedPlaceholder.Value].Item1.userData , notify);
                    onSelectedChanged?.Invoke((int)placeHolders[selectedPlaceholder.Value].Item1.userData);

                    PointerEnter(placeHolders[selectedPlaceholder.Value].Item1);
                }
            }
            else
            {
                selectedPlaceholder = 0;
                SetSelection((int)placeHolders[selectedPlaceholder.Value].Item1.userData , notify);
                onSelectedChanged?.Invoke((int)placeHolders[selectedPlaceholder.Value].Item1.userData);
                PointerEnter(placeHolders[selectedPlaceholder.Value].Item1);
            }
        }
    }
}