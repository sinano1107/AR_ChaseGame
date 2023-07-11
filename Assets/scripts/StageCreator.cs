﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

//
// This script allows us to create anchors with
// a prefab attached in order to visbly discern where the anchors are created.
// Anchors are a particular point in space that you are asking your device to track.
//

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class StageCreator : MonoBehaviour
{
    [SerializeField]
    GameObject m_StageObject;
    bool _firstTouch = true;
    Vector3 _beforeVec = Vector3.zero;

    public GameObject StageObject
    {
        get => m_StageObject;
        set => m_StageObject = value;
    }

    // Removes all the anchors that have been created.
    public void RemoveAllAnchors()
    {
        foreach (var anchor in m_AnchorPoints)
        {
            Destroy(anchor);
        }
        m_AnchorPoints.Clear();
    }

    // On Awake(), we obtains a reference to all the required components.
    // The ARRaycastManager allows us to perform raycasts so that we know where to place an anchor.
    // The ARPlaneManager detects surfaces we can place our objects on.
    // The ARAnchorManager handles the processing of all anchors and updates their position and rotation.
    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        m_AnchorPoints = new List<ARAnchor>();
    }

    void Update()
    {
        // ボタンなどのUIを触った時は無効化
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            return;
        }

        // タップ時に設置・移動
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began)
                return;

            if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = s_Hits[0].pose;

                m_StageObject.SetActive(true);
                m_StageObject.transform.position = hitPose.position;
            }
        }
        // 二本指で回転
        else if (Input.touchCount == 2)
        {
            var pos1 = Input.GetTouch(0).position;
            var pos2 = Input.GetTouch(1).position;

            // 初めてのタッチ時だけの処理
            if (_firstTouch)
            {
                _beforeVec = pos1 - pos2;
                _firstTouch = false;
                return;
            }

            Vector3 vec = pos1 - pos2;

            float angle = Vector3.Angle(_beforeVec, vec);
            Vector3 cross = Vector3.Cross(_beforeVec, vec);
            if (cross.z < 0) angle *= -1;

            _beforeVec = vec;

            m_StageObject.transform.Rotate(new Vector3(0, angle, 0));
        }
    }

    public void Disable()
    {
        GetComponent<StageCreator>().enabled = false;
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    List<ARAnchor> m_AnchorPoints;

    ARRaycastManager m_RaycastManager;

    ARAnchorManager m_AnchorManager;

    ARPlaneManager m_PlaneManager;
}
