using UnityEngine;
using UnityEngine.UI;

public static class UiHelper
{
    public static void SnapTo(this ScrollRect scroller, RectTransform child)
    {
        Canvas.ForceUpdateCanvases();

        var contentPos = (Vector2)scroller.transform.InverseTransformPoint(scroller.content.position);
        var childPos = (Vector2)scroller.transform.InverseTransformPoint(child.position);
        var endPos = contentPos - childPos;
        Vector2 position = scroller.content.anchoredPosition;
        if (scroller.horizontal) position.x = endPos.x;
        if (scroller.vertical) position.y = endPos.y;
        scroller.content.anchoredPosition = position;
    }
}