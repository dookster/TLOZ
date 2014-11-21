using UnityEngine;
using System.Collections;
using UnityEditor;


public class DialogueEditorGizmo : MonoBehaviour {

	[DrawGizmo(GizmoType.SelectedOrChild | GizmoType.NotSelected)]
	static void DrawGameObjectName(Transform transform, GizmoType gizmoType){
		if(transform.tag == "Dialogue"){
			DialogueNode node = transform.GetComponent<DialogueNode>();
			DialogueNode parentNode;
			if(transform.parent != null){
				parentNode = transform.parent.GetComponent<DialogueNode>();
				if(parentNode != null)
					Handles.DrawDottedLine(transform.position, parentNode.transform.position, 5);
			}
			Handles.Label(transform.position, transform.gameObject.name);

			GUIStyle style = node.getGizStyle1();
			GUIStyle style2 = node.getGizStyle2();

			if(node != null){
				if(node.response == null) node.response = "";
				Vector3 labelPos = new Vector3(transform.position.x - transform.localScale.x/4, transform.position.y , transform.position.z);
				Handles.Label(labelPos, "\n\n"+ "  Response:                                                     '\n  " + node.response + " \n .", style2);
				Handles.Label(labelPos, " * " + node.gameObject.name, style);
			}


		}
			
	}
}
