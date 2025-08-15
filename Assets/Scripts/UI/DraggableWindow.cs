using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DraggableWindow : MonoBehaviour,
    IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IInitializePotentialDragHandler
{
    [Header("What to move")]
    [SerializeField] private RectTransform window;  

    [Header("Options")]
    [SerializeField] private bool clampToParent = true;
    [SerializeField] private float padding = 8f;

    [SerializeField] private string saveKey = "";

    [SerializeField] private bool bringToFrontOnClick = true;

    [Header("Layout handling")]
    [SerializeField] private bool detachFromLayoutOnDrag = true;
    [SerializeField] private bool loadSavedPositionOnStart = false;

    [Serializable]
    public class Linked
    {
        public RectTransform follower;
        public Vector2 offset = Vector2.zero; 
    }

    [Header("Linked Windows (follow while dragging)")]
    public List<Linked> linkedWindows = new();

    private RectTransform target;
    private RectTransform parentRect;
    private Canvas canvas;
    private LayoutElement layoutElement;   
    private bool layoutDetached;

    private readonly Vector3[] worldCorners = new Vector3[4];
    private readonly Vector3[] localCorners = new Vector3[4];

    private void Awake()
    {
        target = window ? window : (RectTransform)transform;
        parentRect = target ? target.parent as RectTransform : null;
        canvas = target ? target.GetComponentInParent<Canvas>() : null;

        if (!target || !parentRect || !canvas)
        {
            Debug.LogError("[DraggableWindow] Need RectTransform under a Canvas.");
            enabled = false;
            return;
        }

        if (string.IsNullOrEmpty(saveKey))
            saveKey = target.gameObject.name;

        if (loadSavedPositionOnStart)
            LoadPosition();
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false; 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (bringToFrontOnClick) target.SetAsLastSibling();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (detachFromLayoutOnDrag && !layoutDetached)
            DetachFromLayoutNow();
    }

    public void OnDrag(PointerEventData eventData)
    {
        float scale = canvas.scaleFactor == 0 ? 1f : canvas.scaleFactor;
        target.anchoredPosition += eventData.delta / scale;

        if (clampToParent) ClampToParent();

        for (int i = 0; i < linkedWindows.Count; i++)
        {
            var l = linkedWindows[i];
            if (l != null && l.follower) ApplyLink(l);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SavePosition(); 
    }

    private void DetachFromLayoutNow()
    {
        layoutElement = target.GetComponent<LayoutElement>();
        if (!layoutElement) layoutElement = target.gameObject.AddComponent<LayoutElement>();
        layoutElement.ignoreLayout = true;
        layoutDetached = true;
    }

    private void ClampToParent()
    {
        target.GetWorldCorners(worldCorners);
        for (int i = 0; i < 4; i++) localCorners[i] = parentRect.InverseTransformPoint(worldCorners[i]);

        float minX = Mathf.Min(localCorners[0].x, localCorners[1].x, localCorners[2].x, localCorners[3].x);
        float maxX = Mathf.Max(localCorners[0].x, localCorners[1].x, localCorners[2].x, localCorners[3].x);
        float minY = Mathf.Min(localCorners[0].y, localCorners[1].y, localCorners[2].y, localCorners[3].y);
        float maxY = Mathf.Max(localCorners[0].y, localCorners[1].y, localCorners[2].y, localCorners[3].y);

        var r = parentRect.rect;
        float offX = 0f, offY = 0f;

        if (minX < r.xMin + padding) offX = (r.xMin + padding) - minX;
        else if (maxX > r.xMax - padding) offX = (r.xMax - padding) - maxX;

        if (minY < r.yMin + padding) offY = (r.yMin + padding) - minY;
        else if (maxY > r.yMax - padding) offY = (r.yMax - padding) - maxY;

        if (offX != 0f || offY != 0f)
            target.anchoredPosition += new Vector2(offX, offY);
    }

    private void ApplyLink(Linked link)
    {
        var follower = link.follower;
        var followerParent = follower.parent as RectTransform;

        Vector3 worldOffset = followerParent
            ? followerParent.TransformVector((Vector3)link.offset)
            : (Vector3)link.offset;

        follower.position = target.position + worldOffset;
    }

    private void SavePosition()
    {
        if (string.IsNullOrEmpty(saveKey)) return;
        PlayerPrefs.SetFloat(saveKey + "_x", target.anchoredPosition.x);
        PlayerPrefs.SetFloat(saveKey + "_y", target.anchoredPosition.y);
        PlayerPrefs.Save();
    }

    private void LoadPosition()
    {
        if (string.IsNullOrEmpty(saveKey)) return;
        if (PlayerPrefs.HasKey(saveKey + "_x") && PlayerPrefs.HasKey(saveKey + "_y"))
        {
            target.anchoredPosition = new Vector2(
                PlayerPrefs.GetFloat(saveKey + "_x"),
                PlayerPrefs.GetFloat(saveKey + "_y"));
        }
    }
}
