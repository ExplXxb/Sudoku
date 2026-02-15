using UnityEditor;
using System.Collections;

class LevelPlayEditorCoroutines
{
    IEnumerator mRoutine;

    public static LevelPlayEditorCoroutines StartEditorCoroutine(IEnumerator routine)
    {
        if (routine == null)
        {
            return null;
        }

        LevelPlayEditorCoroutines coroutine = new LevelPlayEditorCoroutines(routine);
        coroutine.Start();
        return coroutine;
    }

    LevelPlayEditorCoroutines(IEnumerator routine)
    {
        mRoutine = routine;
    }

    void Start()
    {
        EditorApplication.update += Update;
    }

    void Update()
    {
        if (mRoutine != null && mRoutine.MoveNext())
        {
            if (mRoutine.Current is IEnumerator nestedRoutine)
            {
                // If the current yield return value is another IEnumerator,
                // start it as a nested coroutine
                StartEditorCoroutine(nestedRoutine);
            }
        }
        else
        {
            StopEditorCoroutine();
        }
    }

    public void StopEditorCoroutine()
    {
        EditorApplication.update -= Update;
    }
}
