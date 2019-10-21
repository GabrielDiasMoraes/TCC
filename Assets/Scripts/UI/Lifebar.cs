using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lifebar : MonoBehaviour
{

    private GameObject targetToFollow;

    [SerializeField] private Slider _slider;

    public Slider Slider => _slider;

    public GameObject TargetToFollow
    {
        get => targetToFollow;
        set => targetToFollow = value;
    }


    private void LateUpdate()
    {
        Vector3 newPos = targetToFollow.transform.position;
        newPos.y = newPos.y + 6f;
        transform.position = newPos;

        if (GameController.Instance.MainCamera == null) return;
        transform.LookAt(GameController.Instance.MainCamera.gameObject.transform);


    }
}
