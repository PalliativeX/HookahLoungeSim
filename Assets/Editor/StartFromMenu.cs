
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class StartMenu
{
	static StartMenu()
	{
		EditorApplication.playModeStateChanged += LoadDefaultScene;
	}

	static void LoadDefaultScene(PlayModeStateChange state)
	{
		if (state == PlayModeStateChange.ExitingEditMode)
		{
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		}

		if (state == PlayModeStateChange.EnteredPlayMode)
		{
			EditorSceneManager.LoadScene(0);
		}
	}
}
#endif
