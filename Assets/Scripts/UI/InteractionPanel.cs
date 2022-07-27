using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 _firstTouch;
    private bool _isSwiping;
    private int _screenWidth;
    private int _screenHeight;

    private void Awake()
    {
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        _isSwiping = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _isSwiping = true;
        _firstTouch = Input.mousePosition;
    }
    private void Update()
    {
        if (!_isSwiping)
            return;
        if (Mathf.Abs(Input.mousePosition.x - _firstTouch.x) > _screenWidth / 10.5f)
        {
            _isSwiping = false;
            EventsPool.UserSwipedEvent.Invoke(Input.mousePosition.x > _firstTouch.x ? SwipeDirection.Right : SwipeDirection.Left);
        }
        else if(Mathf.Abs(Input.mousePosition.y - _firstTouch.y) > _screenHeight / 10.5f)
        {
            _isSwiping = false;
            EventsPool.UserSwipedEvent.Invoke(Input.mousePosition.y > _firstTouch.y ? SwipeDirection.Up : SwipeDirection.Down);
        }
    }
    public void SpawnBlob()
    {
        EventsPool.ShouldSpawnBlobEvent.Invoke();
    }

}