using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class UILocalization : MonoBehaviour
{
	public string key;
    public TextMeshProUGUI textMesh = null;
	private bool started = false;

    public string value
    {

        set
        {

            textMesh.text = value;

        }

    }

	

	private void OnEnable ()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying) return;
#endif
		if (started == true) OnLocalize();
	}


	private void Start ()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying) return;
#endif
		started = true;
		OnLocalize();
	}


	private void OnLocalize ()
	{



        if (string.IsNullOrEmpty(key) == false)
        {

            value = Localization.Get(key);
        }

	}
}
