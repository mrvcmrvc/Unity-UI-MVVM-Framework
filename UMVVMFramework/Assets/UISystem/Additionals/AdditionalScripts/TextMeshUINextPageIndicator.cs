using TMPro;
using UnityEngine;

namespace MVVM
{
    public class TextMeshUINextPageIndicator : MonoBehaviour
    {
        public Animator NextPageIndicatorAnim;

        private TextMeshProUGUI _textMesh;
        private const string NEXT_PAGE_INDICATOR_ANIM_NAME = "NextPageIndicatorAnim";

        private void Awake()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
        }

        public void CheckForNextPageIndicator()
        {
            int curPage = _textMesh.pageToDisplay;
            int totalPage = _textMesh.textInfo.pageCount;

            if (curPage < totalPage)
                ActivateIndicator();
            else
                DeactivateIndicator();
        }

        /// <summary>
        /// Manually deactivates indicator
        /// </summary>
        public void DeactivateIndicator()
        {
            NextPageIndicatorAnim.StopPlayback();

            NextPageIndicatorAnim.gameObject.SetActive(false);
        }

        /// <summary>
        /// Manually activates indicator
        /// </summary>
        public void ActivateIndicator()
        {
            NextPageIndicatorAnim.gameObject.SetActive(true);

            NextPageIndicatorAnim.Play(NEXT_PAGE_INDICATOR_ANIM_NAME);
        }
    }
}