using UnityEngine;
using UnityEngine.UIElements;

class DragManipulator : MouseManipulator
{
    #region Init
    private Vector2 m_Start;
    protected bool m_Active;
    protected bool disregardParentLayout;
    protected bool lockXaxis;
    protected bool lockYaxis;

    public DragManipulator(bool disregardParentLayout = false, bool lockXaxis = false, bool lockYaxis = false)
    {
        this.lockXaxis = lockXaxis;
        this.lockYaxis = lockYaxis;

        this.disregardParentLayout = disregardParentLayout;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        m_Active = false;
        
    }

    #endregion

    #region Registrations
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
    }
    #endregion

    #region OnMouseDown
    protected void OnMouseDown(MouseDownEvent e)
    {
        if (m_Active)
        {
            e.StopImmediatePropagation();
            return;
        }

        if (CanStartManipulation(e))
        {
            m_Start = e.localMousePosition;

            m_Active = true;
            target.CaptureMouse();
            e.StopPropagation();
        }
    }
    #endregion

    #region OnMouseMove
    protected void OnMouseMove(MouseMoveEvent e)
    {
        if (!m_Active || !target.HasMouseCapture())
            return;

        Vector2 diff = e.localMousePosition - m_Start;

        if (!disregardParentLayout)
        {
            if(!lockXaxis)
                target.style.top = Mathf.Clamp(target.layout.y + diff.y, -target.layout.height / 2, 0);
            
            if(!lockYaxis)
                target.style.left = Mathf.Clamp(target.layout.x + diff.x, -target.layout.height / 2, 0);
        }
        else
        {
            if(!lockXaxis)
            target.style.top = target.layout.y + diff.y;
            if(!lockYaxis)
            target.style.left = target.layout.x + diff.x;
        }

        e.StopPropagation();
    }
    #endregion

    #region OnMouseUp
    protected void OnMouseUp(MouseUpEvent e)
    {
        if (!m_Active || !target.HasMouseCapture() || !CanStopManipulation(e))
            return;

        m_Active = false;
        target.ReleaseMouse();
        e.StopPropagation();
    }
    #endregion
}