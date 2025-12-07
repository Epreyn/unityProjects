using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour {


    [Header("Inputs")] 
    [SerializeField] private KeyCode oneKey;
    [SerializeField] private KeyCode twoKey;
    [SerializeField] private KeyCode threeKey;
    [SerializeField] private KeyCode leftArrow;
    [SerializeField] private KeyCode upArrow;
    [SerializeField] private KeyCode rightArrow;
    [SerializeField] private KeyCode downArrow;
    
    [Header("Sweet Spot")]
    public Transform sweetSpot;
    public float sweetSpotSpeed;
    public Camera mainCamera;
    public GraphicRaycaster graphicRayCaster;
    private PointerEventData _pointerEventData;
    public EventSystem eventSystem;
    
    private void MoveSweetSpot() {
        Vector3 targetPosition;

        if (Input.GetKey(leftArrow)) {
            targetPosition = new Vector3(-180, 0, 0);
        }
        else if (Input.GetKey(upArrow)) {
            targetPosition = new Vector3(0, 180, 0);
        }
        else if (Input.GetKey(rightArrow)) {
            targetPosition = new Vector3(180, 0, 0);
        }
        else if (Input.GetKey(downArrow)) {
            targetPosition = new Vector3(0, -180, 0);
        }
        else {
            targetPosition = Vector3.zero;
        }
        
        sweetSpot.localPosition = Vector3.Lerp(sweetSpot.localPosition, targetPosition, Time.deltaTime * sweetSpotSpeed);
    }

    
    private void CheckTapHit() {
        _pointerEventData = new PointerEventData(eventSystem) {
            position = mainCamera.WorldToScreenPoint(sweetSpot.position)
        };
        var results = new List<RaycastResult>();
        graphicRayCaster.Raycast(_pointerEventData, results);

        foreach (var rayCastResult in results)
        {
            if (rayCastResult.gameObject.CompareTag("Tap_1") && Input.GetKeyDown(oneKey)) {
                StartCoroutine(rayCastResult.gameObject.GetComponent<Tap>().DestroyTap());
            }
            else if (rayCastResult.gameObject.CompareTag("Tap_2") && Input.GetKeyDown(twoKey)) { 
                StartCoroutine(rayCastResult.gameObject.GetComponent<Tap>().DestroyTap());
            }   
            else if (rayCastResult.gameObject.CompareTag("Tap_3") && Input.GetKeyDown(threeKey)) {
                StartCoroutine(rayCastResult.gameObject.GetComponent<Tap>().DestroyTap());
            }
        }
    }
    
    public void Update() {
        CheckTapHit();
        MoveSweetSpot();
    }
}
