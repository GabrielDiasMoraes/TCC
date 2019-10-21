using System;
using System.Collections;
using System.Collections.Generic;
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
    }
    
    private void Update()
    {
        SwipeBehavior();
    }


    void SwipeBehavior()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }
        if(Input.GetMouseButton(0))
        {
            _secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
       
            _currentSwipe = new Vector2(_secondPressPos.x - _firstPressPos.x, _secondPressPos.y - _firstPressPos.y);
           
            _currentSwipe.Normalize();
 
            if(_currentSwipe.x > 0  && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f ||
               _currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
            {
                Vector3 direction = new Vector3(0, (_currentSwipe.x > 0) ? 1 : -1, 0);

                _objectToRotate.transform.Rotate(direction * _rotateSpeedModifierStandalone, Space.World);
            }
            
            else if ((_currentSwipe.y > 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f ||
                     _currentSwipe.y < 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f))
            {
                
                if((_objectToRotate.transform.rotation.x >= 30/360f && _currentSwipe.y > 0) || 
                   (_objectToRotate.transform.rotation.x <= -30/360f && _currentSwipe.y < 0)) 
                    return;
                Vector3 direction = new Vector3((_currentSwipe.y > 0) ? 1 : -1, 0, 0);

                direction *= _rotateSpeedModifierStandalone;
                
                

                _objectToRotate.transform.Rotate(direction, Space.Self);
            }
        } 
    }
}
