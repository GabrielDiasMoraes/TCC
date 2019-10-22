using System;
using System.Collections;
using System.Collections.Generic;
using HedgehogTeam.EasyTouch;
using UnityEngine;
using TouchPhase = UnityEngine.TouchPhase;

public class InputController : MonoBehaviour
{
    private Touch _touch;

    private Vector2 _touchPosition;

    private Quaternion _rotationY;

    [SerializeField] private float _rotateSpeedModifierMobile;
    
    [SerializeField] private float _rotateSpeedModifierStandalone;

    [SerializeField] private GameObject _objectToRotate;

    #region Input PC

    private Vector2 _firstPressPos;
    private Vector2 _secondPressPos;
    private Vector2 _currentSwipe;

    #endregion
    
    public static InputController Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SwipeBehaviorEasyTouch();
    }

    void SwipeBehaviorEasyTouch()
    {
        EasyTouch.On_Drag += delegate(Gesture gesture)
        {
            Vector2 direction = gesture.swipeVector.normalized;

            Vector3 rotateDirection; 
            
            if((direction.x > 0  && direction.y > -0.5f && direction.y < 0.5f ||
               direction.x < 0 && direction.y > -0.5f && direction.y < 0.5f) &&
               (gesture.swipeVector.x > 1f || gesture.swipeVector.x < 1f))
            {
                rotateDirection = new Vector3(0, direction.x, 0);

                #if UNITY_STANDALONE
                    rotateDirection *= _rotateSpeedModifierStandalone;
                #else
                    rotateDirection *= _rotateSpeedModifierMobile;
                #endif
                
                _objectToRotate.transform.Rotate(rotateDirection, Space.World);
            }

            else if ((direction.y > 0 && direction.x > -0.5f && direction.x < 0.5f ||
                      direction.y < 0 && direction.x > -0.5f && direction.x < 0.5f) &&
                     (gesture.swipeVector.y > 1f || gesture.swipeVector.y < 1f))
            {
                if(((_objectToRotate.transform.eulerAngles.x >= 30f && _objectToRotate.transform.eulerAngles.x < 180f) && direction.y < 0) ||
                   ((_objectToRotate.transform.eulerAngles.x <= 330f && _objectToRotate.transform.eulerAngles.x >= 180f) && direction.y > 0))
                    return;
                
                rotateDirection = new Vector3(-direction.y, 0, 0);
                
                #if UNITY_STANDALONE || UNITY_EDITOR
                    rotateDirection *= _rotateSpeedModifierStandalone;
                #else
                    rotateDirection *= _rotateSpeedModifierMobile;
                #endif
                
                _objectToRotate.transform.Rotate(rotateDirection, Space.Self);
            }
            
        };
    }
}
