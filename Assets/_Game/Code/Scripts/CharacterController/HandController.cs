using UnityEngine;

public class HandController : MonoBehaviour
{
    private struct HandData
    {
        public HandState state;
        public GameObject gameObject;
        public int layer;
    }

    private enum HandState
    {
        Free = 0,
        ItemHidden = 1,
        ItemVisible = 2
    }
    
    private static readonly int Visible = Animator.StringToHash("Visible");
    
    [SerializeField] private Transform handsHolder;
    [SerializeField] private new Transform camera;
    [SerializeField] private Animator[] hands;
    [SerializeField] private float followSpeed;
    [SerializeField] private float xLagAmount;
    [SerializeField] private float yLagAmount;
    
    private HandData[] _handDates;
    private Vector3 _lastCameraPos;
    private Quaternion _lastCameraRotation;
    private static int _viewmodelLayer;

    private void Awake()
    {
        _handDates = new HandData[hands.Length];
        _viewmodelLayer = LayerMask.NameToLayer("Viewmodel");

        for (var i = 0; i < _handDates.Length; i++)
        {
            _handDates[i] = new HandData
            {
                state = HandState.Free,
                gameObject = null,
                layer = 0
            };
        }

        _lastCameraPos = transform.position;
        _lastCameraRotation = camera.rotation;
    }

    private void Update()
    {
        ApplyPositionLag();
        ApplyRotationLag();
    }

    private void ApplyPositionLag()
    {
        
    }

    private void ApplyRotationLag()
    {
        var rotationDelta = camera.rotation * Quaternion.Inverse(_lastCameraRotation);
        
        var deltaEuler = rotationDelta.eulerAngles;
        
        deltaEuler.x = Mathf.DeltaAngle(0, deltaEuler.x);
        deltaEuler.y = Mathf.DeltaAngle(0, deltaEuler.y);
        
        var targetRotation = new Vector3(
            -deltaEuler.x * xLagAmount,
            -deltaEuler.y * yLagAmount,
            0f
        );

        Quaternion target = Quaternion.Euler(targetRotation);

        handsHolder.localRotation = Quaternion.Slerp(
            handsHolder.localRotation,
            target,
            Time.deltaTime * followSpeed
        );

        _lastCameraRotation = camera.rotation;
    }

    public void OccupyLeftHand(GameObject item, bool showOnStart = true) => OccupyHand(0,  item, showOnStart);
    
    public void OccupyRightHand(GameObject item, bool showOnStart = true) => OccupyHand(1,  item, showOnStart);

    public void ToggleLeftHand(bool visible) => ToggleHand(0, visible);
    
    public void ToggleRightHand(bool visible) => ToggleHand(1, visible);

    public GameObject FreeLeftHand() => FreeHand(0);
    public GameObject FreeRightHand() => FreeHand(1);
    

    public GameObject GetLeftHandItem() => _handDates[0].gameObject;
    public GameObject GetRightHandItem() => _handDates[1].gameObject;
    

    private void OccupyHand(int index, GameObject item, bool showOnStart = true)
    {
        if (_handDates[index].state != HandState.Free)
            return;

        _handDates[index] = new HandData
        {
            state = HandState.ItemHidden,
            gameObject = item,
            layer = item.layer
        };
        item.transform.SetParent(hands[index].transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        
        if(showOnStart)
            ToggleHand(index, true);
    }

    private void ToggleHand(int index, bool visible)
    {
        if (_handDates[index].state == HandState.Free)
            return;

        var data = _handDates[index];
        ChangeObjectLayer(data.gameObject.transform, _viewmodelLayer);
        
        _handDates[index].state = visible ? HandState.ItemVisible : HandState.ItemHidden;
        hands[index].SetBool(Visible, visible);
    }

    private GameObject FreeHand(int index)
    {
        if (_handDates[index].state == HandState.Free)
            return null;
        
        var data = _handDates[index];
        var item = data.gameObject; 
        ChangeObjectLayer(data.gameObject.transform, data.layer);

        _handDates[index] = new HandData
        {
            state = HandState.Free,
            gameObject = null,
            layer = 0
        };
        item.transform.SetParent(null);
        hands[index].SetBool(Visible, false);

        return item;
    }

    private static void ChangeObjectLayer(Transform objectTransform, int layer)
    {
        var childCount = objectTransform.childCount;
        objectTransform.gameObject.layer = layer;

        for (var i = 0; i < childCount; i++)
        {
            ChangeObjectLayer(objectTransform.GetChild(i), layer);
        }
    }
}
