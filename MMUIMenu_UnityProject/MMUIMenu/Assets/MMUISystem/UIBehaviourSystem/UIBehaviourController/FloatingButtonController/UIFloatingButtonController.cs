using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class UIFloatingButtonController : UIBehaviourControllerBase<UIFloatingButtonBehaviour>
{
    public int MaxOpenFloatingButton;

    private Queue<UIFloatingButtonBehaviour> _floatingButtonQueue;

    protected override void Awake()
    {
        base.Awake();

        _floatingButtonQueue = new Queue<UIFloatingButtonBehaviour>();

        BehaviourList.ForEach(b => b.ResetUI(false));
    }

    public override void OnSubContainerPressDown(UIFloatingButtonBehaviour interactedContainer, PointerEventData eventData)
    {
        if(interactedContainer.IsActive)
        {
            if(_floatingButtonQueue.Contains(interactedContainer))
            {
                var queueToList = _floatingButtonQueue.ToList();
                queueToList.Remove(interactedContainer);

                _floatingButtonQueue.Clear();

                queueToList.ForEach(b => _floatingButtonQueue.Enqueue(b));
            }

            interactedContainer.Deactivate();
        }
        else
        {
            if (_floatingButtonQueue.Count > 0 && _floatingButtonQueue.Count == MaxOpenFloatingButton)
            {
                var buttonToClose = _floatingButtonQueue.Dequeue();
                buttonToClose.Deactivate();
            }

            _floatingButtonQueue.Enqueue(interactedContainer);
            interactedContainer.Activate();
        }

        base.OnSubContainerPressDown(interactedContainer, eventData);
    }
}
