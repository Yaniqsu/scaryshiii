using System;
using System.Collections;
using UnityEngine;

public class SurfaceController : MonoBehaviour
{
    public ESurfaceType CurrentSurface { get; private set; }

    [SerializeField] private float checkFrequency = 0.5f;
    [SerializeField] private float distance = 5f;
    [SerializeField] private LayerMask layerMask;

    private Coroutine _checkRoutine;
    private WaitForSeconds _checkWait;

    private void Awake()
    {
        _checkWait = new WaitForSeconds(checkFrequency);
    }

    private void Start()
    {
        CheckSurface();
    }

    public void BeginCheck()
    {
        if (_checkRoutine == null)
            StartCoroutine(HandleCheckingSurface());
    }

    public void EndCheck()
    {
        if(_checkRoutine != null)
            StopCoroutine(_checkRoutine);
    }

    private IEnumerator HandleCheckingSurface()
    {
        while (true)
        {
            CheckSurface();
            yield return _checkWait;
        }
    }

    private void CheckSurface()
    {
        var ray = Physics.Raycast(transform.position, Vector3.down, out var hitInfo, distance, layerMask);

        if (ray && hitInfo.collider.gameObject.TryGetComponent(out SurfaceIdentifier identifier))
        {
            CurrentSurface = identifier.SurfaceType;
        }
    }
}
