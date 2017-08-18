using UnityEngine;
using UnityEngine.UI;

public class UIHorizontalInfiniteScrollViewController : UIBehaviourControllerBase<UIHorizontalInfiniteScrollViewBehaviour>
{
    public ScrollRect Scrollrect;
    public HorizontalLayoutGroup HorLayoutGroup;
    public ContentSizeFitter SizeFitter;

    private float _offset, _topDisablePoint, _bottomDisablePoint;
    private bool _isCompsDisabled;
    private float _treshold = 100f;

    protected override void Awake()
    {
        base.Awake();

        _topDisablePoint = (Scrollrect.viewport.rect.width / 2f);
        _bottomDisablePoint = -_topDisablePoint;

        _isCompsDisabled = false;

        Scrollrect.onValueChanged.AddListener(OnScrollViewDragged);

        _offset = BehaviourList[0].RectTransform.rect.width + HorLayoutGroup.spacing;

        Scrollrect.movementType = ScrollRect.MovementType.Unrestricted;
        Scrollrect.horizontal = true;

        if (Scrollrect.vertical)
            Debug.LogError(gameObject.name + " is set to vertical, EITHER close vertical OR ADD UIVerticalInfiniteScrollView");
    }

    protected override void OnDestroy()
    {
        Scrollrect.onValueChanged.RemoveListener(OnScrollViewDragged);

        base.OnDestroy();
    }

    private void DisableNonUsedComps()
    {
        _isCompsDisabled = true;

        HorLayoutGroup.enabled = false;
        SizeFitter.enabled = false;
    }

    private void OnScrollViewDragged(Vector2 deltaDir)
    {
        if (!_isCompsDisabled)
            DisableNonUsedComps();

        if(Scrollrect.velocity.x < 0)
        {
            for(int i = 0; i < BehaviourList.Count; i++)
            {
                if (Scrollrect.transform.InverseTransformPoint(BehaviourList[i].gameObject.transform.position).x < _bottomDisablePoint - _offset)
                {
                    var lastChildOnHier = BehaviourList.Find(b => b.RectTransform == Scrollrect.content.GetChild(Scrollrect.content.childCount - 1));

                    var newPos = lastChildOnHier.RectTransform.anchoredPosition;
                    newPos.x += (lastChildOnHier.RectTransform.rect.width / 2f) + (BehaviourList[i].RectTransform.rect.width / 2f) + HorLayoutGroup.spacing;

                    BehaviourList[i].RectTransform.anchoredPosition = newPos;

                    BehaviourList[i].RectTransform.SetAsLastSibling();
                }
            }
        }
        else
        {
            for (int i = BehaviourList.Count - 1; i >= 0; i--)
            {
                if (Scrollrect.transform.InverseTransformPoint(BehaviourList[i].gameObject.transform.position).x > _topDisablePoint + _offset + 120f)
                {
                    var firstChildOnHier = BehaviourList.Find(b => b.RectTransform == Scrollrect.content.GetChild(0));

                    var newPos = firstChildOnHier.RectTransform.anchoredPosition;
                    newPos.x -= (firstChildOnHier.RectTransform.rect.width / 2f) + (BehaviourList[i].RectTransform.rect.width / 2f) + HorLayoutGroup.spacing;

                    BehaviourList[i].RectTransform.anchoredPosition = newPos;

                    BehaviourList[i].RectTransform.SetAsFirstSibling();
                }
            }
        }
    }
}
