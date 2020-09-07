using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuItems : EditorWindow {

	[MenuItem("Custom/ResetTransform")]
	static void ResetBaseTransform()
	{
		var parentTransform = Selection.activeGameObject.transform;
		var objectsToModify = new Object[parentTransform.childCount + 1];
		objectsToModify[0] = parentTransform;
		for (var i = 0; i < parentTransform.childCount; ++i)
		{
			objectsToModify[i + 1] = parentTransform.GetChild(i);
		}
		Undo.RecordObjects(objectsToModify, "Reset Parent Transform Position and Scale");
		var positionChange = parentTransform.position;
		var localScale = parentTransform.localScale;
		var scaleChange = new Vector3(localScale.x, localScale.y, localScale.z);
		Selection.activeGameObject.transform.position = Vector3.zero;
		Selection.activeGameObject.transform.localScale = Vector3.one;
		foreach (Transform childTransform in parentTransform)
		{
			var oldPosition = childTransform.position;
			childTransform.position = new Vector3(
				localScale.x * oldPosition.x,
				localScale.y * oldPosition.y,
				localScale.z * oldPosition.z) + positionChange;
			var localChildScale = childTransform.localScale;
			childTransform.localScale = new Vector3(
				localChildScale.x * scaleChange.x,
				localChildScale.y * scaleChange.y,
				localChildScale.z * scaleChange.z);
		}
	}

	[MenuItem("Custom/ResetTransform", true)]
	static bool ValidateResetBaseTransform()
	{
		return Selection.activeGameObject != null && Selection.gameObjects.Length == 1;
	}
}
