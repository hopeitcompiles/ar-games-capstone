using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Touchable))]
public class ObjectManipulator : MonoBehaviour
{
    private Material  _material;
    private Renderer _renderer;
    private float skeletonConfidence = 0.0001f;
    private bool isGrabbing;
    private string handTag;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();   
        _material= _renderer.GetComponent<Material>();
        handTag = "handTag";
    }

    // Update is called once per frame
    void Update()
    {
        ManomotionManager.Instance.ShouldCalculateGestures(true);
        var currentGesture = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_trigger;
        if(currentGesture==ManoGestureTrigger.GRAB_GESTURE)
        {
            isGrabbing = true;
        }
        else if(currentGesture==ManoGestureTrigger.RELEASE_GESTURE)
        {
            isGrabbing = false;

        }
        bool hasConfidence = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.confidence > skeletonConfidence;
        if(!hasConfidence)
        {
            _renderer.sharedMaterial = _material;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(handTag))
        {
            _renderer.material = MaterialProvider.Instance.GlowMaterial;
        }else if (isGrabbing)
        {
            transform.parent = other.transform;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(handTag) && isGrabbing)
        {
            transform.parent = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        transform.parent = null;
        _renderer.material = _material;
    }
}
