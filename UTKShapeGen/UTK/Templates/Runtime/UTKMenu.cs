using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using ULite;

namespace UTK
{
    public class UTKMenu : VisualElement, IUState<VisualElement>, INotifyValueChanged<string>
    {
        public (VisualElement mainContainer, Label title) containers;
        public EventCallback<bool> onExpandChanged;
        private void Expanded(bool isExpanded)
        {
            if(isExpanded == _isExpanded)
                return;

            TitleBorderState(isExpanded);
            _isExpanded = isExpanded;
            this.onExpandChanged?.Invoke(isExpanded);
        }
        public bool isExpanded
        {
            get => _isExpanded;
        }
        public bool onHoverExpand{get;set;} = true;
        private bool _isExpanded = false;
        private string _value;
        private float scalerMultiplier = 60;
        public Sprite arrow { get; set; }
        private (VisualElement active, int elementAt) highlighted = (null, -1);
        public Color selectionColor
        {
            get
            {
                return _selectionColor;
            }
            set
            {
                if (_selectionColor == value)
                    return;

                _selectionColor = value;
            }
        }
        private Color _titleColor = Color.clear;
        private Color _menuColor = Color.clear;
        private Color _selectionColor = Utk.HexColor(Color.clear, "#0063b1");
        public Color menuColor
        {
            get
            {
                return _menuColor;
            }
            set
            {
                if (_menuColor == value)
                    return;

                _menuColor = value;

                for (int i = 0; i < menus.Count; i++)
                {
                    menus[i].titleContainer.BcgColor(value);
                }
            }
        }
        public Color titleColor
        {
            get
            {
                return _titleColor;
            }
            set
            {
                if (_titleColor == value)
                    return;

                _titleColor = value;
                containers.title.BcgColor(value);
            }
        }
        private HashSet<VisualElement> highlightedTree = new();
        private VisualElement activeSubContainer;
        public string value
        {
            get => _value;
            set
            {
                if (value == _value)
                    return;

                using (ChangeEvent<string> evt = ChangeEvent<string>.GetPooled(_value, value))
                {
                    evt.target = this;
                    SetValueWithoutNotify(value);
                    SendEvent(evt);
                }

                OnSelected(value);
            }
        }

        private List<(string menu, Label title, VisualElement titleContainer, VisualElement subContainer, Image icon, VisualElement subParent, Action callback, string[] splitMenu, bool isMain)> menus;
        public void SetValueWithoutNotify(string value) { _value = value; }
        private float defaultSize => Utk.ScreenRatio(scalerMultiplier);
        public UTKMenu(string menuTitle)
        {
            this.SetOverflow(Overflow.Visible).Size(defaultSize * 4, defaultSize, false, false);
            arrow = Resources.Load<Sprite>("triangle-utk-png");
            containers = (new VisualElement(), new Label());
            this.AddChild(containers.title as VisualElement).AddChild(containers.mainContainer);
            containers.title.Text(menuTitle).BorderBottom(Utk.ScreenRatio(7), Utk.HexColor(Color.clear, "#0063b1")).Size(100, 100, true, true).TextAlignment(TextAnchor.MiddleCenter).Name("mainTitle").PickingMode(PickingMode.Position);

            menus = new();
            containers.mainContainer.FlexColumn().Display(DisplayStyle.None).Name("mainContainer");
            containers.mainContainer.BcgColor(Color.red);

            containers.title.OnMouseOver(x =>
            {
                if(!onHoverExpand)
                    return;

                ShowMenu(true);
                TitleBorderState(true);
                x.StopImmediatePropagation();
            });
            containers.title.OnMouseOut(x=>
            {
                TitleBorderState(false);
                x.StopImmediatePropagation();
            });

            containers.title.OnMouseDown(x =>
            {
                bool show = containers.mainContainer.style.display == DisplayStyle.None ? true : false;
                ShowMenu(show);
                ClearTrees();
                x.StopImmediatePropagation();
            });

            this.OnGeometryChanged(x =>
            {
                schedule.Execute(() =>
                {
                    if (menus.Count == 0)
                        return;

                    for (int i = 0; i < menus.Count; i++)
                    {
                        menus[i].titleContainer.Width(this.resolvedStyle.width, false);
                    }
                }).ExecuteLater(1);
            });
        }
        private void TitleBorderState(bool state)
        {
            if(state)
            {
                containers.title.BorderBottom(Utk.ScreenRatio(7), Utk.HexColor(Color.clear, "#c30052"));
            }
            else
            {
                containers.title.BorderBottom(Utk.ScreenRatio(7), Utk.HexColor(Color.clear, "#0063b1"));
            }
        }
        public void ShowMenu(bool state)
        {
            if (!state)
            {
                containers.mainContainer.style.display = DisplayStyle.None;
            }
            else
            {
                containers.mainContainer.style.display = DisplayStyle.Flex;
                var val = new ChangeEvent<bool>();
            }

            Expanded(state);
        }
        private void OnSelected(string menu)
        {
            if (menus.Count == 0)
                return;

            for (int i = 0; i < this.menus.Count; i++)
            {
                if (menu == menus[i].menu)
                {
                    menus[i].callback.Invoke();
                    break;
                }
            }
        }
        public void CloseMenu()
        {
            ShowMenu(false);
            ClearTrees();
        }
        ///<summary>Clears all states and selections.</summary>
        private void ClearTrees(bool clearHighlight = true)
        {
            var lis = highlightedTree.ToList();

            for (int i = 0; i < lis.Count; i++)
            {
                lis[i].BcgColor(Color.clear);
            }

            for (int i = 0; i < menus.Count; i++)
            {
                if (menus[i].subContainer.childCount > 0)
                {
                    var child = menus[i].subContainer.Children().ToList();

                    for (int j = 0; j < child.Count; j++)
                    {
                        if (child[j].style.display == DisplayStyle.None)
                            continue;

                        child[j].Display(DisplayStyle.None);
                    }
                }
            }

            highlightedTree.Clear();

            if (clearHighlight)
                highlighted = (null, -1);
        }

        public void Select()
        {
            if (highlighted.active == null)
                return;

            var items = GetItems(highlighted.active);
            value = items.title.text;

            VisualElement subCon = items.subContainer;

            if (subCon == null)
                return;

            if (subCon.childCount == 0)
            {
                SetSelectionColor(items.title, false);
                items.title.FontStyleAndWeight(FontStyle.Normal);
                ShowMenu(false);
                highlighted = (null, -1);
                ClearTrees();
            }
            else
            {
                ShowHideSubMenu(subCon, true);
                var getSubs = subCon.ElementAt(0);
                highlighted = (getSubs, 0);

                var els = GetItems(subCon);
                SetSelectionColor(els.title);
            }
        }
        private void SetSelectionColor(VisualElement title, bool colorize = true)
        {
            if (colorize)
            {
                title.parent.BcgColor(selectionColor);
                highlightedTree.Add(title.parent);
            }
            else
            {
                title.parent.BcgColor(Color.clear);
                highlightedTree.Remove(title);
            }
        }

        public void MoveNext()
        {
            if(!isExpanded)
            {
                TitleBorderState(true);
                ShowMenu(true);
                ClearTrees();
            }

            MoveNextPrev(true);
        }
        public void MovePrevious()
        {
            if(!isExpanded)
            {
                TitleBorderState(true);
                ShowMenu(true);
                ClearTrees();
            }
            MoveNextPrev(false);
        }
        public void MoveBack()
        {
            var current = GetItems(highlighted.active);
            var prevParent = current.subContainer.parent;

            if (prevParent != null)
            {
                SetSelectionColor(current.title);
                //highlighted = (containers.mainContainer.ElementAt(0), 0);

                VisualElement con = prevParent.parent.Query("subContainer");
                if (con != null)
                {
                    highlighted = (con, 0);
                    var t = con.Children().ToList();

                    for (int i = 0; i < t.Count; i++)
                    {
                        t[i].Display(DisplayStyle.None);
                    }
                }

            }

        }
        public void MoveForward()
        {
            Select();
        }
        private void MoveNextPrev(bool next)
        {
            if (this.containers.mainContainer.childCount > 0)
            {
                var sum = next ? 1 : -1;

                if (highlighted.active == null)
                {
                    this.containers.mainContainer.ElementAt(0);
                    highlighted = (containers.mainContainer.ElementAt(0), 0);

                    var current = GetItems(highlighted.active);
                    SetSelectionColor(current.title);
                }
                else
                {
                    if (highlighted.elementAt + sum > highlighted.active.parent.childCount - 1 || highlighted.elementAt + sum < 0)
                        return;

                    var prev = GetItems(highlighted.active);
                    var newValue = (highlighted.active.parent.ElementAt(highlighted.elementAt + sum), highlighted.elementAt + sum);
                    var current = GetItems(newValue.Item1);
                    SetSelectionColor(prev.title, false);
                    SetSelectionColor(current.title);
                    highlighted = newValue;
                }
            }
        }

        private (Label title, Image icon, VisualElement subContainer) GetItems(VisualElement titleContainer)
        {
            Label lbl = titleContainer.Query<Label>().First();
            Image img = titleContainer.Query<Image>().First();
            VisualElement subCon = titleContainer.Query("subContainer");
            return (lbl, img, subCon);
        }
        public UTKMenu AddMenu(string menu, Action callback)
        {
            if (String.IsNullOrEmpty(menu) || callback == null)
                return this;

            var split = menu.Split('/');
            Exts.Ext.Repeat(split.Length, x => split[x].Trim());

            if (menus.Exists(x => x.menu == menu))
                return this;

            if (split.Length == 1)
            {
                split = new string[1];
                split[0] = menu.Trim();
            }
            else
            {
                int prevIndex = -1;
                int startSplit = 0;
                string skipWord = null;

                for (int i = 0; i < menus.Count; i++)
                {
                    for (int j = split.Length; j-- > 0;)
                    {
                        if (menus[i].menu == split[j])
                        {
                            prevIndex = i;
                            skipWord = split[j];
                            break;
                        }
                    }
                }

                //check if exists
                for (int i = startSplit; i < split.Length; i++)
                {
                    if (prevIndex > -1 && i == 0)
                        continue;

                    if (skipWord == split[i] || menus.Exists(x => split[i] == x.menu))
                        continue;

                    (string menu, Label title, VisualElement titleContainer, VisualElement subContainer, Image icon, VisualElement subParent, Action callback, string[] split, bool isMain) add = (split[i], new Label(), new VisualElement(), new VisualElement(), new Image(), null, callback, split, false);

                    add.titleContainer.FlexRow().FlexGrow(1).AlignSelf(Align.FlexStart).Size(defaultSize * 4, defaultSize, false, false);
                    add.icon.name = "icon";
                    add.subContainer.name = "subContainer";
                    add.title.name = "title";

                    var wrap = new VisualElement().FlexRow().Size(100, 100, true, true).BorderBottom(Utk.ScreenRatio(3), Utk.HexColor(Color.clear, "#0078d7"));
                    wrap.name = "titleWrapper";
                    wrap.AddChild(add.title.Size(80, 100, true, true)).AddChild(add.icon.Size(20, 100, true, true));
                    add.titleContainer.AddChild(wrap).AddChild(add.subContainer);

                    add.titleContainer.Position(Position.Absolute).Name("titleContainer");
                    add.icon.scaleMode = ScaleMode.ScaleToFit;
                    add.icon.JustifyContent(Justify.Center);
                    add.subContainer.Position(Position.Absolute).Left(100, true);
                    add.title.Text(split[i]).TextAlignment(TextAnchor.MiddleLeft);

                    if (prevIndex < 0)
                    {
                        add.titleContainer.Top(defaultSize * containers.mainContainer.childCount);
                        containers.mainContainer.AddChild(add.titleContainer);
                        add.isMain = true;
                    }
                    else
                    {
                        add.titleContainer.Top(defaultSize * menus[prevIndex].subContainer.childCount);
                        menus[prevIndex].subContainer.AddChild(add.titleContainer);
                        menus[prevIndex].icon.sprite = arrow;
                        add.titleContainer.Display(DisplayStyle.None);
                        add.subParent = menus[prevIndex].subContainer;
                    }

                    menus.Add(add);
                    prevIndex = menus.Count - 1;
                    AddEvent(add.title, add.subContainer, add.titleContainer, add.isMain);
                }

                return this;
            }

            menus.Add((menu, new Label(), new VisualElement(), new VisualElement(), new Image(), null, callback, split, false));
            var last = menus[menus.Count - 1];
            last.titleContainer.FlexRow().FlexGrow(1).AlignSelf(Align.FlexStart).Size(defaultSize * 4, defaultSize, false, false);
            last.subContainer.name = "subContainer";
            last.titleContainer.name = "titleContainer";
            last.icon.name = "icon";
            last.title.name = "title";

            var titleWrapper = new VisualElement().FlexRow().Size(100, 100, true, true).BorderBottom(Utk.ScreenRatio(3), Utk.HexColor(Color.clear, "#0078d7"));
            titleWrapper.name = "titleWrapper";
            titleWrapper.AddChild(last.title.Size(80, 100, true, true)).AddChild(last.icon.Size(20, 100, true, true));
            last.titleContainer.AddChild(titleWrapper).AddChild(last.subContainer);

            last.icon.scaleMode = ScaleMode.ScaleToFit;
            last.icon.JustifyContent(Justify.Center);
            last.subContainer.Position(Position.Absolute).Left(100, true);
            last.title.Text(menu).TextAlignment(TextAnchor.MiddleLeft);
            last.titleContainer.Top(defaultSize * containers.mainContainer.childCount).Position(Position.Absolute);
            containers.mainContainer.AddChild(last.titleContainer);

            last.isMain = true;
            AddEvent(last.title, last.subContainer, last.titleContainer, last.isMain);
            return this;
        }
        private void ShowHideSubMenu(VisualElement subContainer, bool show)
        {
            if (subContainer.childCount > 0)
            {
                foreach (var child in subContainer.Children().ToList())
                {
                    if (!show)
                        child.Display(DisplayStyle.None);
                    else
                        child.Display(DisplayStyle.Flex);
                }
            }
        }
        private void AddEvent(Label title, VisualElement subContainer, VisualElement titleContainer, bool isMain)
        {
            title.parent.OnMouseEnter(x =>
            {
                if (isMain)
                {
                    if (activeSubContainer != null)
                    {
                        var c = activeSubContainer.Query("subContainer").ToList();

                        for (int i = 0; i < c.Count; i++)
                        {
                            if (c[i].childCount == 0)
                                continue;

                            var child = c[i].Children().ToList();

                            for (int j = 0; j < child.Count; j++)
                            {
                                child[j].Display(DisplayStyle.None);
                            }
                        }
                    }

                    activeSubContainer = subContainer;
                }
                else
                {
                    var tmpChild = subContainer.Query().AtIndex(0);

                    if (tmpChild != null)
                    {
                        var tmp = tmpChild.Query("subContainer");

                        if (tmp != null)
                        {
                            var child = subContainer.Query().AtIndex(0);
                            if (child != null)
                            {
                                var items = child.Query("subContainer").ToList();

                                if (items.Count > 1)
                                {
                                    for (int i = 0; i < items.Count; i++)
                                    {
                                        var lis = items[i].Children().ToList();
                                        for (int j = 0; j < lis.Count; j++)
                                        {
                                            lis[j].style.display = DisplayStyle.None;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                TitleBorderState(true);
                title.FontStyleAndWeight(FontStyle.BoldAndItalic);
                ShowHideSubMenu(subContainer, true);
                SetSelectionColor(title);
                x.StopImmediatePropagation();
            });

            title.parent.OnMouseExit(x =>
            {
                title.FontStyleAndWeight(FontStyle.Normal);
                SetSelectionColor(title, false);
                x.StopImmediatePropagation();
            });

            title.parent.OnMouseDown(x =>
            {
                TitleBorderState(false);
                title.FontStyleAndWeight(FontStyle.Normal);
                value = title.text;
                ClearTrees();
                ShowMenu(false);
                x.StopImmediatePropagation();
            });
        }

        public UTKMenu AddMenus(List<(string menu, Action callback)> menuList)
        {
            if (menuList == null || menuList.Count == 0)
                throw new Exception("UTK : Empty or null List!");

            menus = new();

            for (int i = 0; i < menuList.Count; i++)
            {
                if (String.IsNullOrEmpty(menuList[i].menu) || menuList[i].callback == null)
                    throw new Exception("UTK : Menu or callback can't be null/empty!");

                AddMenu(menuList[i].menu, menuList[i].callback);
            }

            this.schedule.Execute(() => MarkDirtyRepaint());
            return this;
        }

        public virtual void OnEnable()
        {

        }
        public virtual void OnDisable()
        {

        }
    }
}