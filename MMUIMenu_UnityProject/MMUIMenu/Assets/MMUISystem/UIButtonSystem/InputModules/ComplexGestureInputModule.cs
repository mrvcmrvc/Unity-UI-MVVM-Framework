using UnityEngine.EventSystems;

namespace UnityEngine.EventSystems
{
    public interface IBeginPinchHandler : IEventSystemHandler
    {
        void OnBeginPinch(PointerEventData eventData);
    }
    public interface IPinchHandler : IEventSystemHandler
    {
        void OnPinch(PointerEventData eventData);
    }
    public interface IEndPinchHandler : IEventSystemHandler
    {
        void OnEndPinch(PointerEventData eventData);
    }
}

public static class AdvancedExecuteEvents
{
    #region ExecutionHandlers

    private static readonly ExecuteEvents.EventFunction<IBeginPinchHandler> s_BeginPinchHandler = Execute;
    private static readonly ExecuteEvents.EventFunction<IPinchHandler> s_PinchHandler = Execute;
    private static readonly ExecuteEvents.EventFunction<IEndPinchHandler> s_EndPinchHandler = Execute;

    private static void Execute(IBeginPinchHandler handler, BaseEventData eventData)
    {
        handler.OnBeginPinch(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
    }

    private static void Execute(IPinchHandler handler, BaseEventData eventData)
    {
        handler.OnPinch(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
    }

    private static void Execute(IEndPinchHandler handler, BaseEventData eventData)
    {
        handler.OnEndPinch(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
    }

    #endregion

    #region Execution Accessors

    public static ExecuteEvents.EventFunction<IBeginPinchHandler> beginPinchHandler { get { return s_BeginPinchHandler; } }
    public static ExecuteEvents.EventFunction<IPinchHandler> pinchHandler { get { return s_PinchHandler; } }
    public static ExecuteEvents.EventFunction<IEndPinchHandler> endPinchHandler { get { return s_EndPinchHandler; } }

    #endregion
}

public class ComplexGestureInputModule : PointerInputModule
{


    public override void Process()
    {
        //bool usedEvent = SendUpdateEventToSelectedObject();

        //if (eventSystem.sendNavigationEvents)
        //{
        //    if (!usedEvent)
        //        usedEvent |= SendMoveEventToSelectedObject();

        //    if (!usedEvent)
        //        SendSubmitEventToSelectedObject();
        //}

        //ProcessMouseEvent();







        var data = GetBaseEventData();
        ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, AdvancedExecuteEvents.beginPinchHandler);
    }
}
