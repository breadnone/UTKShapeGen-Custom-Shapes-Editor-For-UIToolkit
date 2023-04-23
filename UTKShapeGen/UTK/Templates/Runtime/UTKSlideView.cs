using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using Breadnone;
using Exts;

namespace UTK
{
    public class UTKSlideView : VisualElement, IUState<VisualElement>
    {
        public (VisualElement mainContainer, VisualElement dummyContainer) containers { get; private set; }
        public bool enableFade { get; set; } = true;
        public float slideSpeed { get; set; } = 1f;
        public float fadeSpeed { get; set; } = 0.5f;
        public float width { get; set; } = 30f;
        public float height { get; set; } = 100f;
        public float borderWidth { get; set; } = 5f;
        public float roundRadius { get; set; } = 0f;
        public Ease ease { get; set; }
        public Action<bool> onCompleteSlide;
        public bool isActive { get; set; }
        public UPos position { get; set; } = UPos.Left;
        public UDirection direction { get; set; } = UDirection.Right;
        private bool isAnimating;
        private VisualElement child;
        public UTKSlideView(VisualElement visualElement = null, bool borderless = true)
        {
            (this as IUState<VisualElement>).UTKInit();
            
            focusable = true;
            this.FlexGrow(1);
            containers = (new VisualElement().Size(width, height, true, true), new VisualElement().FlexGrow(1));
            containers.mainContainer.BcgColor("#0063b1");
            this.AddChild(containers.mainContainer);
            containers.mainContainer.SetOverflow(Overflow.Hidden);

            if (visualElement != null)
            {
                containers.mainContainer.AddChild(visualElement);
            }

            position = UPos.Top;
            SetSlideDirection();
            SetPosition();
        }
        public void OnEnable()
        {

        }
        public void OnDisable()
        {

        }
        public float SetSlideDirection()
        {
            float val;
            if (direction == UDirection.Up || direction == UDirection.Down)
            {
                containers.mainContainer.Height(0);
                val = height;
            }
            else
            {
                containers.mainContainer.Width(0);
                val = width;
            }
            return val;
        }
        public void AddElement(VisualElement visualElement)
        {
            containers.mainContainer.AddChild(visualElement);
        }
        public void SetPosition()
        {
            if (this.position == UPos.Top)
            {
                this.FlexColumn();
                containers.mainContainer.BringToFront();
            }
            else if (position == UPos.Down)
            {
                this.FlexColumn();
                containers.mainContainer.SendToBack();
            }
            else if (position == UPos.Left)
            {
                this.FlexRow();
                containers.mainContainer.SendToBack();
            }
            else if (position == UPos.Right)
            {
                this.FlexRow();
                containers.mainContainer.BringToFront();
            }
        }
        public void Open()
        {
            if (isActive)
                return;

            Start(true);
        }
        public void Close()
        {
            if (!isActive)
                return;

            Start(false);
        }

        private void Start(bool play)
        {
            if(isAnimating)
                return;

            var toLerp = SetSlideDirection();

            float ostart = 0;
            float oend = 1;
            float sstart = 0f;
            float send = toLerp;

            if (isActive)
            {
                ostart = 1f;
                oend = 0f;
                sstart = toLerp;
                send = 0f;
            }

            if (enableFade)
            {
                Utk.LerpOpacity(containers.mainContainer, ostart, oend, fadeSpeed, Ease.Linear, onComplete: ()=>
                {
                    containers.mainContainer.Opacity(oend);
                }, customId: UnityEngine.Random.Range(int.MinValue, int.MaxValue));
            }
            else
            {
                if (!isActive)
                    this.Opacity(1f);
            }

            isActive = play;
            
            if(play)
                isAnimating = true;

            Utk.LerpValue(containers.mainContainer, sstart, send, slideSpeed, (float x) =>
            {
                if (direction == UDirection.Left || direction == UDirection.Right)
                    containers.mainContainer.Width(x, true);
                else
                    containers.mainContainer.Height(x, true);

            }, onComplete: () =>
            {
                if(isActive)
                {
                    if (direction == UDirection.Right || direction == UDirection.Left)
                    {
                        containers.mainContainer.Width(width, true);
                    }
                    else
                    {
                        containers.mainContainer.Height(height, true);
                    }
                }
                else
                {
                    if (direction == UDirection.Right || direction == UDirection.Left)
                    {
                        containers.mainContainer.Width(0, false);
                    }
                    else
                    {
                        containers.mainContainer.Height(0, false);
                    }
                }

                isAnimating = false;
                onCompleteSlide?.Invoke(isActive);
            }, ease:ease, customId: UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        }
    }
}