using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootingJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    ShootingManager shootingManager;

    public float Horizontal { get { return (snapX) ? SnapFloat(input.x, s_AxisOption.Horizontal) : input.x; } }
    public float Vertical { get { return (snapY) ? SnapFloat(input.y, s_AxisOption.Vertical) : input.y; } }
    public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

    public float HandleRange
    {
        get { return handleRange; }
        set { handleRange = Mathf.Abs(value); }
    }

    public float DeadZone
    {
        get { return deadZone; }
        set { deadZone = Mathf.Abs(value); }
    }

    public s_AxisOption AxisOption { get { return AxisOption; } set { axisOptions = value; } }
    public bool SnapX { get { return snapX; } set { snapX = value; } }
    public bool SnapY { get { return snapY; } set { snapY = value; } }

    [SerializeField] private float handleRange = 1;
    [SerializeField] private float deadZone = 0;
    [SerializeField] private s_AxisOption axisOptions = s_AxisOption.Both;
    [SerializeField] private bool snapX = false;
    [SerializeField] private bool snapY = false;

    [SerializeField] protected RectTransform background = null;
    [SerializeField] private RectTransform handle = null;
    private RectTransform baseRect = null;

    private Canvas canvas;
    private Camera cam;

    private Vector2 input = Vector2.zero;

    protected virtual void Start()
    {
        shootingManager = GameObject.FindObjectOfType<ShootingManager>().GetComponent<ShootingManager>();

        HandleRange = handleRange;
        DeadZone = deadZone;
        baseRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            Debug.LogError("The Joystick is not placed inside a canvas");

        Vector2 center = new Vector2(0.5f, 0.5f);
        background.pivot = center;
        handle.anchorMin = center;
        handle.anchorMax = center;
        handle.pivot = center;
        handle.anchoredPosition = Vector2.zero;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        shootingManager.CheckJoystickUp(false);

        cam = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;

        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        Vector2 radius = background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * canvas.scaleFactor);
        FormatInput();
        HandleInput(input.magnitude, input.normalized, radius, cam);
        handle.anchoredPosition = input * radius * handleRange;

        float radian = CalculateRadian();
        if (radian >= 0.9f)
        {
            shootingManager.isEndpoint = true;
        }
        else if(0f < radian && radian < 0.9f)
        {
            shootingManager.isEndpoint = false;
        }
    }

    protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
                input = normalised;
        }
        else
            input = Vector2.zero;
    }

    private void FormatInput()
    {
        if (axisOptions == s_AxisOption.Horizontal)
            input = new Vector2(input.x, 0f);
        else if (axisOptions == s_AxisOption.Vertical)
            input = new Vector2(0f, input.y);
    }

    private float SnapFloat(float value, s_AxisOption snapAxis)
    {
        if (value == 0)
            return value;

        if (axisOptions == s_AxisOption.Both)
        {
            float angle = Vector2.Angle(input, Vector2.up);
            if (snapAxis == s_AxisOption.Horizontal)
            {
                if (angle < 22.5f || angle > 157.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }

            else if (snapAxis == s_AxisOption.Vertical)
            {
                if (angle > 67.5f && angle < 112.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }
            return value;
        }
        else
        {
            if (value > 0)
                return 1;
            if (value < 0)
                return -1;
        }
        return 0;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        FormatInput();
        shootingManager.isEndpoint = false;
        shootingManager.CheckJoystickUp(true);
    }

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
        {
            Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
            return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
        }
        return Vector2.zero;
    }

    float CalculateRadian()
    {
        float h = Horizontal;
        if (h <= 0)
            h = -h;
        float a = Mathf.Atan(Vertical / Horizontal); //원 각도 구함
        //각도를 이용하여 cos사용하고 점과 중점사이의 거리를 구함
        float r = h / Mathf.Cos(a);
        //거리가 0.9 이상일때만 스킬 사용

        return r;
    }
}

public enum s_AxisOption { Both, Horizontal, Vertical }