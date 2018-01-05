using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagsViewer : MonoBehaviour
{
    public UISpawnController SpawnController;
    public RectTransform DrawArea;
    public GridLayoutGroup GridLayoutGroup;

    List<TagUIScript> _tagContainerList = new List<TagUIScript>();

    public Action<string, bool> OnTagSelected;
    void FireOnTagSelected(string selectedTag, bool isSelected)
    {
        if (OnTagSelected != null)
            OnTagSelected(selectedTag, isSelected);
    }

    public void DrawTags(List<string> tagList)
    {
        if(GridLayoutGroup != null)
        {
            var drawHeight = DrawArea.rect.height;

            int rowCount = (int)(drawHeight / SpawnController.PlaceholderList[0].GetComponent<RectTransform>().rect.height);

            GridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            GridLayoutGroup.constraintCount = rowCount;
        }

        if (_tagContainerList.Count > 0)
        {
            _tagContainerList.ForEach(t => t.OnTagPressed -= OnTagPressed);
            _tagContainerList.ForEach(t => Destroy(t.gameObject));

            _tagContainerList.Clear();
        }

        _tagContainerList = SpawnController.LoadSpawnables<TagUIScript>(tagList.Count, true);

        _tagContainerList.ForEach(t => t.OnTagPressed += OnTagPressed);

        for(int i = 0; i < tagList.Count; i++)
        {
            _tagContainerList[i].SetTag(tagList[i]);
        }
    }

    public void SetTagsSelected(List<string> tags)
    {
        _tagContainerList.ForEach(tc => tc.SetContainerSelectionActive(false));

        foreach (var tag in tags)
        {
            var targetContainer = _tagContainerList.Find(tc => tc.SegmentTag == tag);

            targetContainer.SetContainerSelectionActive(true);
        }
    }

    private void OnTagPressed(string selectedTag, bool isSelected)
    {
        FireOnTagSelected(selectedTag, isSelected);
    }
}
