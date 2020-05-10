using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

public class HelperFunctions
{

}

public static class MyExtensions
{
    public static void DeleteChildren(this Transform parent)
    {
        if (parent != null)
        {
            foreach (Transform child in parent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public static T GenerateRandomElement<T>(this List<T> list)
    {
        var index = UnityEngine.Random.Range(0, list.Count);

        return list[index];
    }

    public static Vector2 ToRectPos(this Vector3 worldPos, RectTransform parentRect)
    {
        return ((Vector2)worldPos).ToRectPos(parentRect);
    }

    public static Vector2 ToRectPos(this Vector2 worldPos, RectTransform parentRect)
    {
        Vector2 localPoint;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, worldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPoint, null, out localPoint);

        return localPoint;
    }
}