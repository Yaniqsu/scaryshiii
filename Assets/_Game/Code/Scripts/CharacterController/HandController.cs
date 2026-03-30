using UnityEngine;

public class HandController : MonoBehaviour
{
    private enum HandState
    {
        Free = 0,
        ItemHidden = 1,
        ItemVisible = 2
    }
    
    [SerializeField] private Transform handsHolder;
    [SerializeField] private new Transform camera;
    [SerializeField] private Transform[] hands;
    [SerializeField] private float followSpeed;
    [SerializeField] private float xLagAmount;
    [SerializeField] private float yLagAmount;
    
    private HandState[] _handStates;
    private Vector3 _lastCameraPos;
    private Quaternion _lastCameraRotation;

    private void Awake()
    {
        _handStates = new HandState[hands.Length];

        for (var i = 0; i < _handStates.Length; i++)
        {
            _handStates[i] = HandState.Free;
            hands[i].gameObject.SetActive(false);
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

    public void OccupyLeftHand(GameObject item, bool showOnStart = true)
        => OccupyHand(0,  item, showOnStart);
    
    public void OccupyRightHand(GameObject item, bool showOnStart = true)
        => OccupyHand(1,  item, showOnStart);

    public void ToggleLeftHand(bool visible)
        => ToggleHand(0, visible);
    
    public void ToggleRightHand(bool visible)
        => ToggleHand(1, visible);
    

    private void OccupyHand(int index, GameObject item, bool showOnStart = true)
    {
        if (_handStates[index] != HandState.Free)
            return;

        _handStates[index] = HandState.ItemHidden;
        item.transform.SetParent(hands[index]);
        item.transform.localPosition = Vector3.zero;
        
        if(showOnStart)
            ToggleHand(index, true);
    }

    private void ToggleHand(int index, bool visible)
    {
        if (_handStates[index] == HandState.Free)
            return;
        
        _handStates[index] = visible ? HandState.ItemVisible : HandState.ItemHidden;
        hands[index].gameObject.SetActive(visible);
    }
}
