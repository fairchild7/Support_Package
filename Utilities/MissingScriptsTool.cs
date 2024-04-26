using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class MissingScriptsTool : MonoBehaviour
{
    const string k_missingScriptMenuFolder = "Tools/Missing Scripts/";

    [MenuItem(k_missingScriptMenuFolder + "Find")]
    static void FindingMissingScriptsMenuItem()
    {
        foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>(true))
        {
            foreach (Component component in gameObject.GetComponentsInChildren<Component>())
            {
                if (component == null)
                {
                    Debug.Log($"GameObject found with missing script {gameObject.name}", gameObject);
                    break;
                }
            }
        }
    }

    [MenuItem(k_missingScriptMenuFolder + "Delete")]
    static void DeleteMissingScriptsMenuItem()
    {
        foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>(true))
        {
            foreach (Component component in gameObject.GetComponentsInChildren<Component>())
            {
                if (component == null)
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
                    break;
                }
            }
        }
    }
}
#endif
