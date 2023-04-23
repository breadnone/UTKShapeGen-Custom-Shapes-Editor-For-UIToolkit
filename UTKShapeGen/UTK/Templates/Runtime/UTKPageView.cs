using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using Breadnone;

namespace UTK
{
    ///<summary>Paging layout with itemSource as data source binding.</summary>
    public class UTKPageView : VisualElement
    {
        private IList sourceList;
        ///<summary>Pointer down event callback</summary>
        public Action<VisualElement, int> onPointerDown; /// (x, y) x= visualElement, y = selected itemSourec index
        public System.Object selectedItem { get; set; }
        ///<summary>Listen to item changes events.</summary>
        public Action<int> onSelectedItemChanged; //returns x = index in itemsource
        ///<summary>Listen to page changes events.</summary>
        public Action<int, int> onPageChanged;//returns range of items
        ///<summary>Animation opacity.</summary>
        public bool fadeAnim { get; set; }
        public bool zoomAnim { get; set; }
        public Ease opacityEase { get; set; } = Ease.Linear;
        public float animDuration { get; set; } = 0.3f;
        private bool isAnimating = false;
        public bool zoomOnEnter { get; set; } = true;
        public Vector3 zoomScale { get; set; } = new Vector3(1.2f, 1.2f, 1.2f);
        ///<summary>Items padding based on parent's container aspect ratio. Use this to adjust the padding items inside the container when auto resizing.</summary>
        public Vector2 aspectRatioPadding { get; set; } = new Vector2(40, 40);
        ///<summary>ItemSource for visualElement.</summary>
        public List<System.Object> itemSource
        {
            get
            {
                if (sourceList == null)
                {
                    return null;
                }
                else if (sourceList.Count == 0)
                {
                    return new List<object>(0);
                }

                var tmp = new List<System.Object>();
                for (int i = 0; i < sourceList.Count; i++) { tmp.Add(sourceList[i]); }
                return tmp;
            }
            set
            {

                sourceList = value;
            }
        }
        ///<summary>Parent containers of this visualElement.</summary>
        public (VisualElement mainContainer, VisualElement footer) containers { get; set; }
        ///<summary>Empty label indicator.</summary>
        public string emptyText { get; set; } = "No items found";
        ///<summary>Enables page controls in the footer container.</summary>
        public bool enableFooter { get; set; } = true;
        ///<summary>Margin for dsisplayed items.</summary>
        public float itemMargin { get; set; } = 2;
        ///<summary>Total rows will be displayed.
        public int totalRows { get; set; }
        ///<summary>Total columns will be displayed.</summary>
        public int totalColumns { get; set; }
        ///<summary>Visual appearance for mouse events. e.g: selected state</summary>
        public (float borderWidth, Color borderColor, Color selectedBorderColor) mouseEventSettings;
        ///<summary>Templates for displayed items.</summary>
        public Func<VisualElement> makeItem { get; set; }
        ///<summary>Bindings for the templates tied to the itemSource.</summary>
        public Action<VisualElement, int> bindItem { get; set; }
        ///<summary>All roots and child elements shown in the mainContainer.</summary>
        public List<(VisualElement root, VisualElement[] elements)> displayedElements;
        private List<(Action callback, VisualElement element, int selectedIndex, int displayedIndex)> displayedChild;
        ///<summary>Selected VisualElement.</summary>
        public VisualElement selectedElement { get; set; }
        ///<summary>Selected index. Nullable type.</summary>
        public int? selectedIndex { get; private set; }
        private (int? start, int? end) _activeIndex;
        private Vector2 prevSize;
        ///<summary>Range of items currently shown in the mainContainer.</summary>
        public (int? start, int? end) activeIndex
        {
            get
            {
                return _activeIndex;
            }
            set
            {
                if (_activeIndex != value)
                {
                    CheckPageState(this.containers.mainContainer);
                }

                _activeIndex = value;
            }
        }
        private TextField pageIndicator;
        private bool selection = false;
        ///<summary>Visual appearanmce selection state.</summary>
        public bool showSelection
        {
            get { return selection; }
            set
            {
                selection = value;
                activeIndex = (null, null);

                schedule.Execute(() => Construct(false, false));
            }
        }
        private void RecalcAspectRatio()
        {
            if (displayedChild.Count > 0)
            {
                for (int i = 0; i < displayedChild.Count; i++)
                {
                    var vis = displayedChild[i].element;

                    vis.parent.schedule.Execute(() =>
                    {
                        var t = vis.AspectRatio(vis.parent, new Vector2(aspectRatioPadding.x, aspectRatioPadding.y));
                        vis.Size(t.width, t.height, false, false);
                        prevSize = new Vector2(t.width, t.height);
                    });
                }
            }
        }
        public UTKPageView(IList itemSource, Func<VisualElement> makeItem, Action<VisualElement, int> bindItem, int totalRow, int totalColumn, bool enableSelection = false)
        {
            this.focusable = true;
            this.RegisterCallback<AttachToPanelEvent>(evt => this.Focus());
            this.OnGeometryChanged(x => RecalcAspectRatio());

            activeIndex = (null, null);
            sourceList = itemSource;
            this.makeItem = makeItem;
            this.bindItem = bindItem;
            this.totalColumns = totalColumn;
            this.totalRows = totalRow;
            this.mouseEventSettings = (5, Color.green, Color.green);
            this.Size(100, 100, true, true).FlexGrow(1);
            this.containers = (new VisualElement().FlexGrow(1).SetOverflow(Overflow.Hidden), new VisualElement().Size(100, Utk.ScreenRatio(50), true, false));
            this.AddChild(containers.mainContainer).AddChild(containers.footer);
            Construct(false, false);
        }
        ///<summary>Currently this is necessary for both joypad like controls and keyboards for navigation to work. Trigger this at the very beginning of each call.</summary>
        public void TryForceFocus() { this.focusable = true; }
        ///<summary>Reconstruct the whole layout of this visualElement.</summary>
        public void Rebuild() { Construct(false, false); }
        private void Construct(bool previousRange, bool anim)
        {
            if (isAnimating && !fadeAnim)
                isAnimating = false;

            containers.mainContainer.Clear();
            selectedElement = null;
            selectedIndex = null;
            selectedItem = null;
            displayedElements = new();
            displayedChild = new();
            containers.mainContainer.Opacity(0f);

            this.containers.mainContainer.FlexColumn().FlexGrow(1);
            CreateItemColumn(previousRange, anim);
        }
        private void DrawFooter()
        {
            if (containers.footer != null)
                containers.footer.Clear();

            if (!enableFooter)
            {
                this.containers.footer.Display(DisplayStyle.None);
                return;
            }
            else
            {
                this.containers.footer.Display(DisplayStyle.Flex);
            }

            var btnPrev = new Button().Size(100 / 6f, 100, true, true).Text("<<");
            var btnNext = new Button().Size(100 / 6f, 100, true, true).Text(">>");
            var txt = new TextField().Size(100 / 6f, 100, true, true).Border(5, Color.black).RoundCorner(5, true).TextAlignment(TextAnchor.MiddleCenter);
            txt.SetEnabled(false);

            pageIndicator = txt;
            this.containers.footer.Size(100, Utk.ScreenRatio(50), true, false).FlexRow().JustifyContent(Justify.Center).FlexShrink(0);
            this.containers.footer.AddChild(btnPrev).AddChild(txt).AddChild(btnNext);

            btnPrev.clicked += () => { PreviousPage(); };
            btnNext.clicked += () => { NextPage(); };
        }
        private void CreateItemColumn(bool previousRange, bool anim = true)
        {
            DrawFooter();
            this.containers.mainContainer.Clear();

            if (makeItem == null || bindItem == null || totalColumns == 0 || itemSource == null || itemSource.Count == 0)
            {
                this.containers.mainContainer.Add(new Label().Text(emptyText));
                return;
            }

            int counter = 0;

            for (int i = 0; i < totalColumns; i++)
            {
                var root = new VisualElement().FlexRow().Size(100, 100 / (float)totalColumns, true, true).MarginTop(itemMargin, true).MarginBottom(itemMargin, true);

                var vis = new VisualElement().FlexGrow(1);
                root.AddChild(vis);
                VisualElement[] child = new VisualElement[totalRows];

                for (int u = 0; u < child.Length; u++)
                {
                    child[u] = new VisualElement().Size(100 / (float)totalRows, 100, true, true);
                    root.Add(child[u]);
                    counter++;
                }

                this.containers.mainContainer.AddChild(root);
                displayedElements.Add((root, child));
            }

            InsertToRow(previousRange, anim);
        }
        private void OnSelected(MouseDownEvent evt, int index, VisualElement custom = null)
        {
            if (custom == null)
            {
                selectedElement = (VisualElement)evt.target;
            }
            else
            {
                selectedElement = custom;
            }

            selectedItem = itemSource[index];
            selectedIndex = index;

            if (onPointerDown != null)
            {
                onPointerDown.Invoke(selectedElement, index);
            }
        }
        private void PointerEnter(VisualElement visualElement)
        {
            if (zoomOnEnter)
            {
                visualElement.Scale(new Scale(new Vector3(zoomScale.x, zoomScale.y, 1f)));
            }

            if (showSelection)
                visualElement.BcgColor("#744da9");
        }
        private void PointerExit(VisualElement visualElement)
        {
            if (zoomOnEnter)
            {
                visualElement.Scale(new Scale(new Vector3(1f, 1f, 1f)));
            }

            if (showSelection)
            {
                visualElement.BcgColor(Color.clear);
            }
        }
        ///<summary>Go to next page.</summary>
        public void NextPage()
        {
            if (isAnimating)
                return;

            Construct(false, true); selectedIndex = null;
        }
        ///<summary>Go to previous page.</summary>
        public void PreviousPage()
        {
            if (isAnimating)
                return;

            Construct(true, true); selectedIndex = null;
        }
        ///<summary>Go to and select next item.</summary>
        public void NextItem()
        {
            if (isAnimating)
                return;

            NextPreviousItems(true);
        }
        ///Go to and select previous item.
        public void PreviousItem()
        {
            if (isAnimating)
                return;

            NextPreviousItems(false);
        }
        private void CheckItemState(VisualElement container)
        {
            container.schedule.Execute(() =>
            {
                if (selectedIndex.HasValue)
                    onSelectedItemChanged?.Invoke(selectedIndex.Value);
            });
        }
        private void CheckPageState(VisualElement container)
        {
            container.schedule.Execute(() =>
            {
                if (selectedIndex.HasValue)
                    onPageChanged?.Invoke(activeIndex.start.Value, activeIndex.end.Value);
            });
        }
        private int? GetDisplayIndex(int selectedIndex)
        {
            for (int i = 0; i < displayedChild.Count; i++)
            {
                if (displayedChild[i].selectedIndex != selectedIndex)
                    continue;

                return displayedChild[i].displayedIndex;
            }

            return null;
        }
        private void NextPreviousItems(bool nextPrevious)
        {
            if (!selectedIndex.HasValue && activeIndex.start.HasValue)
            {
                if (itemSource != null && itemSource.Count > 0 && itemSource[0] != null)
                {
                    if (nextPrevious)
                    {
                        displayedChild[0].callback.Invoke();
                    }
                    else
                    {
                        if (activeIndex.end.Value - 1 >= 0)
                            displayedChild[activeIndex.end.Value - 1].callback.Invoke();
                    }
                }

                CheckItemState(this.containers.mainContainer);
                return;
            }

            int sum = nextPrevious ? 1 : -1;

            if (nextPrevious)
            {
                if (selectedIndex.Value + sum <= activeIndex.end.Value - 1)
                {
                    var item = GetDisplayIndex(selectedIndex.Value + sum);

                    if (item.HasValue)
                    {
                        displayedChild[item.Value].callback.Invoke();
                        CheckItemState(this.containers.mainContainer);
                    }
                }
                else
                {
                    if (selectedIndex.Value == itemSource.Count - 1)
                        return;

                    selectedIndex = null;
                    Construct(false, true);

                    this.containers.mainContainer.schedule.Execute(x =>
                    {
                        NextPreviousItems(nextPrevious);
                        CheckItemState(this.containers.mainContainer);
                    });
                }
            }
            else
            {
                if (selectedIndex.Value + sum >= activeIndex.start.Value && selectedIndex.Value + sum > -1)
                {
                    var item = GetDisplayIndex(selectedIndex.Value + sum);

                    if (item.HasValue)
                        displayedChild[item.Value].callback.Invoke();
                }
                else
                {
                    if (selectedIndex.Value == 0)
                        return;

                    selectedIndex = null;
                    Construct(true, true);

                    this.containers.mainContainer.schedule.Execute(x =>
                    {
                        NextPreviousItems(nextPrevious);
                        CheckItemState(this.containers.mainContainer);
                    });
                }
            }
        }
        public void JumpToItem(int itemIndex)
        {
            selectedIndex = itemIndex;
            Construct(false, true);
        }
        private void InsertToRow(bool previousRange, bool anim)
        {
            var range = GetRanges(previousRange);
            pageIndicator.value = range.end + "/" + itemSource.Count;
            int lastIndex = 0;
            int start = range.start;

            for (int i = 0; i < displayedElements.Count; i++)
            {
                var emt = displayedElements[i];

                if (emt.root.childCount == totalRows)
                    continue;

                for (int j = start; j < range.end; j++)
                {
                    if (lastIndex == totalColumns)
                    {
                        start += totalRows;
                        lastIndex = 0;
                        break;
                    }

                    var vis = makeItem().AlignSelf(Align.Center);

                    if (prevSize.x != 0)
                    {
                        //workaround for flickering
                        vis.Size(prevSize.x, prevSize.y, false, false);
                    }

                    bindItem(vis, j);

                    var index = j;
                    vis.OnMouseDown(x => { OnSelected(x, index); });
                    displayedChild.Add((new Action(() => OnSelected(null, index, vis)), vis, index, displayedChild.Count));
                    var dat = emt.elements;

                    for (int u = 0; u < dat.Length; u++)
                    {
                        if (dat[u].childCount == 0)
                        {
                            dat[u].AddChild(vis);
                            dat[u].JustifyContent(Justify.Center);

                            dat[u].OnMouseEnter(x => { var idx = u; PointerEnter(dat[idx]); });
                            dat[u].OnMouseExit(x => { var idx = u; PointerExit(dat[idx]); });
                            break;
                        }
                    }

                    lastIndex++;
                }
            }

            containers.mainContainer.schedule.Execute(() => RecalcAspectRatio());

            this.schedule.Execute(() =>
            {
                containers.mainContainer.Opacity(1f);

                if(anim)
                    UIAnim();
            }).ExecuteLater(15);

        }
        private (int start, int end) GetRanges(bool previousRanges = false)
        {
            var count = (totalRows * totalColumns);
            var len = itemSource.Count;

            (int start, int end) val;

            if (!activeIndex.start.HasValue)
            {
                if (count > len)
                {
                    val = (0, count - len);
                }
                else
                {
                    val = (0, count);
                }
            }

            else
            {
                if (previousRanges)
                {
                    if (activeIndex.start.Value == 0)
                    {
                        activeIndex = (activeIndex.start.Value, activeIndex.end.Value);
                        return (activeIndex.start.Value, activeIndex.end.Value);
                    }

                    val = (activeIndex.start.Value + -count, activeIndex.start.Value);
                }
                else
                {
                    if (activeIndex.end.Value == len)
                    {
                        activeIndex = (activeIndex.start.Value, activeIndex.end.Value);
                        return (activeIndex.start.Value, activeIndex.end.Value);
                    }

                    val = (activeIndex.end.Value, activeIndex.end.Value + count);
                }
            }
 
            if (val.start > len) { val = (len, len); }
            if (val.end > len) { val = (val.start, len); }

            activeIndex = val;
            return val;
        }
        private List<int> pooledAnimation = new();
        private void CancelAnim()
        {
            if (!isAnimating)
                return;

            for (int i = 0; i < pooledAnimation.Count; i++)
            {
                var val = pooledAnimation[i];
                Utk.LerpCancel(ref val);
            }

            pooledAnimation.Clear();
        }
        private void UIAnim()
        {
            CancelAnim();

            isAnimating = true;

            UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
            var fadeId = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            pooledAnimation.Add(fadeId);

            Utk.LerpOpacity(containers.mainContainer, 0, 1f, animDuration, opacityEase, 0, false, () =>
            {
                containers.mainContainer.Opacity(1f);
                isAnimating = false;
            }, customId: fadeId);

            if (zoomAnim)
            {
                containers.mainContainer.schedule.Execute(() =>
                {
                    UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
                    var scaleId = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
                    pooledAnimation.Add(scaleId);
                    var defScale = containers.mainContainer.resolvedStyle.scale.value;
                    var calcScale = new Vector3(defScale.x / 2f, defScale.y, defScale.z);
                    containers.mainContainer.style.scale = new Scale(new Vector3(0.5f, 0.5f, 0.5f));
                    Utk.LerpScale(containers.mainContainer, defScale, animDuration, 0, false, Ease.Linear, () => { containers.mainContainer.Scale(new Scale(defScale)); }, true, customId: scaleId);
                });
            }
        }
    }
}