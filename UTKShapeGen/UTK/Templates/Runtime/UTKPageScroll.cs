using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Linq;

namespace UTK
{
    ///<summary>ScrollView with paging support.</summary>
    public class UTKPageScroll : VisualElement, INotifyValueChanged<NavigationMoveEvent>
    {
        private IList originalList { get; set; } = new List<System.Object>();
        public NavigationMoveEvent value{get;set;}
        public void SetValueWithoutNotify(NavigationMoveEvent evt){}
        public IList itemSource
        {
            get { return originalList; }
            set
            {
                originalList = value;
            }
        }
        public List<Action<VisualElement, int>> visibleElements = new();
        public (VisualElement header, ScrollView scrollView, VisualElement customContainer, Slider zoomSlider, Slider spaceSlider, VisualElement sliderContainer) containers { get; private set; }
        public int selectedIndex { get; set; }
        public System.Object selectedItem { get; set; }
        public int displayAmount { get; set; } = 10;
        public Func<VisualElement> makeItem { get; set; }
        public Action<VisualElement, int> bindItem { get; set; }
        public List<VisualElement> displayedElements { get; private set; } = new();
        private (int start, int end) displayedRange;
        private (Vector2? defaultItemSize, float itemMargin, Color defColor)settings;
        private (bool _footerIsHeader, bool headerIsVisible, bool showSliders)headerSettings;
        public bool showSlider
        {
            get
            {
                return headerSettings.showSliders;
            }
            set
            {
                headerSettings.showSliders = value;
                DrawSliders();
            }
        }
        public bool showHeader
        {
            get
            {return headerSettings.headerIsVisible;}

            set
            {
                headerSettings.headerIsVisible = value;

                if(value)
                {
                    DrawHeader();
                }
                else
                {
                    this.containers.header.Clear();
                }
            }
        }
        public bool footerIsHeader
        {
            get
            {
                return headerSettings._footerIsHeader;
            }
            set
            {
                if(value)
                    this.containers.header.SendToBack();
                else
                    this.containers.header.BringToFront();

                headerSettings._footerIsHeader = value;
            }
        }
        public ScrollViewMode mode
        {
            get { return containers.scrollView.mode; }
            set
            {
                containers.scrollView.mode = value;

                containers.scrollView.schedule.Execute(() =>
                {
                    ConstructLayout(true);
                });
            }
        }
        public ScrollerVisibility horizontalScrollerVisibility
        {
            get { return containers.scrollView.horizontalScrollerVisibility; }
            set
            {
                containers.scrollView.horizontalScrollerVisibility = value;

                containers.scrollView.schedule.Execute(() =>
                {
                    ConstructLayout(true);
                });
            }
        }
        public ScrollerVisibility verticalScrollerVisibility
        {
            get { return containers.scrollView.verticalScrollerVisibility; }
            set
            {
                containers.scrollView.verticalScrollerVisibility = value;
                containers.scrollView.schedule.Execute(() =>
                {
                    ConstructLayout(true);
                });
            }
        }
        public void AddElement(VisualElement visualElement)
        {
            containers.scrollView.Add(visualElement);

            containers.scrollView.schedule.Execute(() =>
            {
                Utk.TryForceRefresh(containers.scrollView);
            });
        }
        public UTKPageScroll(IList itemSource, Func<VisualElement> makeItem, Action<VisualElement, int> bindItem, int perPageAmount)
        {
            this.settings = (null, 2, Color.clear);
            this.headerSettings = (false, true, false);
            this.settings.defColor = this.style.backgroundColor.value;
            this.makeItem = makeItem;
            this.bindItem = bindItem;
            this.displayAmount = perPageAmount;
            this.itemSource = itemSource;
            this.FlexGrow(1).FlexColumn();

            containers = (new VisualElement().Size(100, Utk.ScreenRatio(50), true, false), new ScrollView(), new VisualElement().FlexWrap(Wrap.Wrap).FlexRow(), new Slider(), new Slider(), new VisualElement());

            (this as VisualElement).AddChild(containers.scrollView).AddChild(containers.header).AddChild(containers.sliderContainer);            
            ConstructLayout();
        }
        private void  CheckHeaderSize()
        {
            if(showHeader)
            {
            }
        }
        private void ConstructLayout(bool clear = false)
        {
            containers.scrollView.Clear();
            containers.customContainer.Clear();

            if(headerSettings.headerIsVisible)
                DrawHeader();

            //scrollview
            containers.scrollView.Size(100, 100, true, true).FlexGrow(1).Padding(2, true);
            containers.scrollView.AddChild(containers.customContainer);
            Rebuild();
        }
        private void IterateElements(Action<VisualElement> callback)
        {
            for(int i = 0; i < displayedElements.Count; i++)
            {
                callback(displayedElements[i]);
            }
        }
        private void DrawSliders()
        {
            containers.sliderContainer.Clear();

            if(!headerSettings.showSliders)
            {
                containers.sliderContainer.Display(DisplayStyle.None);
                return;
            }

            containers.sliderContainer.Display(DisplayStyle.Flex);
            containers.sliderContainer.Size(100, Utk.ScreenRatio(50) /1.3f, true, false).FlexRow().JustifyContent(Justify.Center);
            containers.sliderContainer.AddChild(containers.zoomSlider).AddChild(containers.spaceSlider);
            
            containers.zoomSlider.ShowInputField(true).LowValue(1).HighValue(10);
            containers.spaceSlider.ShowInputField(true).LowValue(1).HighValue(10);
            containers.zoomSlider.Size(50, 100, true, true).Text("zoom");
            containers.spaceSlider.Text("space").Size(50, 100, true, true);
        }
        private void DrawHeader()
        {
            if(headerSettings.showSliders)
            {
                DrawSliders();
            }

            containers.header.Clear();
            containers.header.BcgColor(Utk.ColorTwo).FlexRow().JustifyContent(Justify.Center);

            containers.zoomSlider.OnValueChanged(x=>
            {
                IterateElements(y =>
                {
                    if(!settings.defaultItemSize.HasValue)
                    {
                        settings.defaultItemSize = new Vector2(y.resolvedStyle.width, y.resolvedStyle.height);
                    }

                    y.Size(settings.defaultItemSize.Value.x * x.newValue, settings.defaultItemSize.Value.y * x.newValue);
                
                });
            });
            containers.spaceSlider.OnValueChanged(x =>
            {
                IterateElements(y =>
                {
                    y.Margin(settings.itemMargin * x.newValue);
                });
            });

            //Header controls
            var indicator = new Label().Size(13, 100, true, true).TextAlignment(TextAnchor.MiddleCenter).BcgColor(Color.grey).Text("1 of 99").RoundCorner(3, true);
            var btnPrev = new Button().Size(13, 100, true, true).Text("<<").RoundCorner(5, true);
            var btnNext = new Button().Size(13, 100, true, true).Text(">>").RoundCorner(5, true);
            var btnJump = new Button().Size(13, 100, true, true).Text("Jump").RoundCorner(5, true);
            var txtJump = new TextField().Size(13, 100, true, true).RoundCorner(5, true);
            containers.header.AddChild(btnPrev).AddChild(indicator).AddChild(btnNext).AddChild(btnJump).AddChild(txtJump);

            btnPrev.clicked += () =>
            {

            };

            btnNext.clicked += () =>
            {

            };

            if(headerSettings._footerIsHeader)
            {
                containers.header.SendToBack();
            }
            else
            {
                containers.header.BringToFront();
            }
        }
        public UTKPageScroll Rebuild(int? from = null, int? to = null)
        {
            if (from.HasValue && to.HasValue)
            {
                displayedRange = (from.Value, to.Value);
            }
            else
            {
                displayedRange = (0, displayAmount);
            }

            if (itemSource == null || itemSource.Count == 0)
            {
                originalList.Clear();
                containers.scrollView.Add(new Label().Text("No items to be displayed."));
                return this;
            }

            for (int i = displayedRange.start; i < displayedRange.end; i++)
            {
                if (i > itemSource.Count - 1 || i == -1)
                    break;

                VisualElement visualElement = makeItem();
                bindItem.Invoke(visualElement, i);
                containers.customContainer.Add(visualElement.Margin(settings.itemMargin, true).FlexShrink(0));
                displayedElements.Add(visualElement);

                visualElement.OnMouseDown(x =>
                {
                    var idx = i;
                    OnSelection(idx, itemSource[idx], visualElement);
                });

                visualElement.OnMouseEnter(x=>
                {
                    var idx = i;
                    OnSelection(idx, itemSource[idx], visualElement);
                });
                visualElement.OnMouseExit(x=>
                {   
                    var idx = i;
                    OnSelection(idx, itemSource[idx], visualElement, true);
                });
            }

            selectedIndex = 0;
            selectedItem = null;

            containers.scrollView.schedule.Execute(() =>
            {
                Utk.TryForceRefresh(containers.scrollView.contentContainer);
            });
            return this;
        }

        private void OnSelection(int index, System.Object obj, VisualElement visualElement, bool onMouseExit = false)
        {
            selectedIndex = index;
            selectedItem = itemSource[index] as System.Object;

            if(!onMouseExit)
                visualElement.Border(5, Color.green);
            else
            {
                visualElement.Border(0, Color.clear);
            }
            for (int i = 0; i < displayedElements.Count; i++)
            {
                if (displayedElements[i] != visualElement)
                    displayedElements[i].Border(0, Color.clear);
            }
        }
        public virtual UTKPageScroll NextPage(VisualElement visualElement)
        {
            if (itemSource == null || itemSource.Count == 0)
                return this;

            var calcRange = (displayedRange.end, displayedRange.end + displayAmount);
            Rebuild(calcRange.Item1, calcRange.Item2);
            return this;
        }
        public virtual UTKPageScroll PreviousPage(VisualElement visualElement)
        {
            if (itemSource == null || itemSource.Count == 0)
                return this;

            var calcRange = (displayedRange.start, displayedRange.start - displayAmount);
            Rebuild(calcRange.Item1, calcRange.Item2);
            return this;
        }
        public virtual UTKPageScroll JumpTo(int pageNumber)
        {
            if (pageNumber < 0 || pageNumber > itemSource.Count - 1)
                return this;

            var lis = new List<(int start, int end)>();
            Check(true);

            void Check(bool fillList)
            {
                if (fillList)
                {
                    for (int i = 0; i < itemSource.Count; i++)
                    {
                        if (i % displayAmount == 2)
                        {
                            lis.Add((i - displayAmount, i));
                        }
                    }

                    if (lis[lis.Count - 1].end + 1 != itemSource.Count)
                    {
                        lis.Add((lis[lis.Count - 1].end, itemSource.Count - 1));
                    }
                }

                if (fillList)
                {
                    Check(false);
                    return;
                }

                SecondCheck();

                void SecondCheck()
                {
                    for (int i = 0; i < lis.Count; i++)
                    {
                        for (int j = lis[i].start; j < lis[i].end; j++)
                        {
                            if (j != pageNumber)
                                continue;

                            Rebuild(lis[i].start, lis[i].end);

                            containers.scrollView.schedule.Execute(x =>
                            {
                                OnSelection(pageNumber, itemSource[pageNumber], displayedElements[pageNumber].parent);
                            });

                            return;
                        }
                    }
                }
            }

            return this;
        }
    }
}