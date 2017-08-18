using UnityEngine;
using UnityEngine.UI;

public class UIVerticalInfiniteScrollViewController : UIBehaviourControllerBase<UIVerticalInfiniteScrollViewBehaviour>
{
    public ScrollRect Scrollrect;
    public VerticalLayoutGroup VertLayoutGroup;
    public ContentSizeFitter SizeFitter;

    private float _offset, _topDisablePoint, _bottomDisablePoint;
    private bool _isCompsDisabled;
    private float _treshold = 100f;

    protected override void Awake()
    {
        base.Awake();

        _topDisablePoint = (Scrollrect.viewport.rect.height / 2f);
        _bottomDisablePoint = -_topDisablePoint;

        _isCompsDisabled = false;

        Scrollrect.onValueChanged.AddListener(OnScrollViewDragged);

        _offset = BehaviourList[0].RectTransform.rect.height + VertLayoutGroup.spacing;

        Scrollrect.movementType = ScrollRect.MovementType.Unrestricted;
        Scrollrect.vertical = true;

        if (Scrollrect.horizontal)
            Debug.LogError(gameObject.name + " is set to horizontal, EITHER close horizontal OR ADD UIHorizontalInfiniteScrollView");
    }

    private void DisableNonUsedComps()
    {
        _isCompsDisabled = true;

        VertLayoutGroup.enabled = false;
        SizeFitter.enabled = false;
    }

    protected override void OnDestroy()
    {
        Scrollrect.onValueChanged.RemoveListener(OnScrollViewDragged);

        base.OnDestroy();
    }

    private void OnScrollViewDragged(Vector2 deltaDir)
    {
        if(!_isCompsDisabled)
            DisableNonUsedComps();

        if (Scrollrect.velocity.y > 0)
        {
            for (int i = 0; i < BehaviourList.Count; i++)
            {
                if (Scrollrect.transform.InverseTransformPoint(BehaviourList[i].gameObject.transform.position).y > _topDisablePoint + _offset + _treshold)
                {
                    var lastChildOnHier = BehaviourList.Find(b => b.RectTransform == Scrollrect.content.GetChild(Scrollrect.content.childCount - 1));

                    var newPos = lastChildOnHier.RectTransform.anchoredPosition;
                    newPos.y -= (lastChildOnHier.RectTransform.rect.height / 2f) + (BehaviourList[i].RectTransform.rect.height / 2f) + VertLayoutGroup.spacing;

                    BehaviourList[i].RectTransform.anchoredPosition = newPos;

                    BehaviourList[i].RectTransform.SetAsLastSibling();
                }
            }
        }
        else
        {
            for (int i = BehaviourList.Count - 1; i >= 0; i--)
            {
                if (Scrollrect.transform.InverseTransformPoint(BehaviourList[i].gameObject.transform.position).y < _bottomDisablePoint - _offset)
                {
                    var firstChildOnHier = BehaviourList.Find(b => b.RectTransform == Scrollrect.content.GetChild(0));

                    var newPos = firstChildOnHier.RectTransform.anchoredPosition;
                    newPos.y += (firstChildOnHier.RectTransform.rect.height / 2f) + (BehaviourList[i].RectTransform.rect.height / 2f) + VertLayoutGroup.spacing;

                    BehaviourList[i].RectTransform.anchoredPosition = newPos;

                    BehaviourList[i].RectTransform.SetAsFirstSibling();
                }
            }
        }
    }
}
