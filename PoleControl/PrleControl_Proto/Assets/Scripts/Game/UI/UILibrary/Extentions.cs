using DG.Tweening;
using UnityEngine;

namespace Rudrac.Control.UI
{
    public static class Extentions
    {
        public static void SetOnTop(this Canvas canvas, int sortinglayer = 100)
        {
            canvas.overrideSorting = true;
            canvas.sortingLayerID = 100;
        }

        public static void SetNormal(this Canvas canvas)
        {
            canvas.overrideSorting = false;
        }

        public static void Show(this CanvasGroup canvasGroup, float time = 0.25f)
        {
            canvasGroup.DOFade(1, time);
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
        public static void Hide(this CanvasGroup canvasGroup, float time = 0.25f)
        {
            canvasGroup.DOFade(0, time);
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}
