using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; //Allows SceneVisibilityManager accessibility. 
#endif


//Note: This script is optional and is meant to aid our team by providing visibility to the empty object
//used to rotate the giant labyrinth. This is because, by default, Unity's empty objects are invisible. 
//This script does NOT/should not impact any gameplay. 
public class CustomGizmoShape : MonoBehaviour
{
    public Mesh customMesh; //Allows custom mesh to be used as an empty. Visible ONLY in the viewport. 
    public Color gizmoColor = Color.blue;

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
       //Allows the eye icon to control whether the custom gizmo is visible. 
        if (SceneVisibilityManager.instance.IsHidden(gameObject))
        {
            return;
        }
#endif
        //Checks to see if there is a selected custom mesh. If there is, then it is drawn in the viewport. 
        if (customMesh != null)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireMesh(customMesh, transform.position, transform.rotation, transform.lossyScale);
        }
    }
}