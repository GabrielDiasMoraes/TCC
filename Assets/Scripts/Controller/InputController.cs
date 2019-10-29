using HedgehogTeam.EasyTouch;
using UnityEngine;

public class InputController : MonoBehaviour
{

    [SerializeField] private float _rotateSpeedModifierMobile;
    
    [SerializeField] private float _rotateSpeedModifierStandalone;

    [SerializeField] private GameObject _objectToRotate;
   
    public static InputController Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
        //DontDestroyOnLoad(gameObject);
        EasyTouch.On_Drag += SwipeBehaviorEasyTouch;
    }

    private void OnDestroy()
    {
        EasyTouch.On_Drag -= SwipeBehaviorEasyTouch;
    }

    void SwipeBehaviorEasyTouch(Gesture gesture)
    {
        if (_objectToRotate == null)
            return;
        
        
        Vector2 direction = gesture.swipeVector.normalized;

        Vector3 rotateDirection; 
        
        if((direction.x > 0  && direction.y > -0.5f && direction.y < 0.5f ||
           direction.x < 0 && direction.y > -0.5f && direction.y < 0.5f) &&
           (gesture.swipeVector.x > 1f || gesture.swipeVector.x < 1f))
        {
            rotateDirection = new Vector3(0, direction.x, 0);

            #if UNITY_STANDALONE || UNITY_EDITOR
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
    }
}
