
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonUIHandlerScene : MonoBehaviour
{
   [SerializeField] private string sceneValue;

   [SerializeField] private GameObject _panelToActivate;

   private void Start()
   {
      
      
      GetComponent<Button>().onClick.RemoveAllListeners();     
      GetComponent<Button>().onClick.AddListener(delegate
      {
         if (_panelToActivate != null) _panelToActivate.SetActive(true);
         if(string.IsNullOrEmpty(sceneValue)) return;
         DataController.Instance.CurrentLevel = sceneValue;
         SceneManager.LoadScene(sceneValue);
      });
   }
}
