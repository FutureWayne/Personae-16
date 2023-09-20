using UnityEngine;
using DialogueSystem;

namespace UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        
        // Start is called before the first frame update
        void Start()
        {
            GameObject.FindGameObjectWithTag("Player").TryGetComponent(out playerConversant);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
