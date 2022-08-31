using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionPanel : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    private Vector3 _firstTouch;

    private float _elapsed = 0;
    private float _waitTime = 0.2f;

    private Plane _plane;
    private int _layerMask;
    private Blob _blob;
    private Vector2 _lastPos;
    private void Awake()
    {

        _layerMask = LayerMask.GetMask("Tower");
        _plane = new Plane(Vector3.up, new Vector3(0, 0, 0));
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        var data = (PointerEventData)eventData;
        _lastPos = data.position;
        Ray ray = Camera.main.ScreenPointToRay(_lastPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            _blob = hit.transform.GetComponent<Blob>();
            _blob.StartDrag();
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_blob == null)
            return;

        _blob.SwipeToDirection(Vector3.zero);
        _blob = null;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (_blob == null)
            return;
        var data = (PointerEventData)eventData;
        if ((data.position - _lastPos).magnitude < Screen.width / 3f)
            return;
        var direction = (data.position - _lastPos).normalized;
        _blob.SwipeToDirection(direction);
        _blob = null;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        }
    }
    public void SpawnBlob()
    {
        EventsPool.ShouldSpawnBlobEvent.Invoke();
    }

}
