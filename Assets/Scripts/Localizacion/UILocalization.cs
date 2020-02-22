using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class UILocalization : MonoBehaviour
{
	public string key;
    public TextMeshProUGUI textMesh = null;

    public string value
    {

        set
        {

            textMesh.text = value;

        }

    }

//	public string value
//	{
//		set
//		{
//			if (!string.IsNullOrEmpty(value))
//			{
//				var w = this.GetComponent<TextMeshPro>();
//				//UILabel lbl = w as UILabel;
//				//UISprite sp = w as UISprite;

//				if (lbl != null)
//				{
//					// If this is a label used by input, we should localize its default value instead
//					UIInput input = NGUITools.FindInParents<UIInput>(lbl.gameObject);
//					if (input != null && input.label == lbl) input.defaultText = value;
//					else lbl.text = value;
//#if UNITY_EDITOR
//					if (!Application.isPlaying) NGUITools.SetDirty(lbl);
//#endif
//				}
////				else if (sp != null)
////				{
////					UIButton btn = NGUITools.FindInParents<UIButton>(sp.gameObject);
////					if (btn != null && btn.tweenTarget == sp.gameObject)
////						btn.normalSprite = value;

////					sp.spriteName = value;
////					sp.MakePixelPerfect();
////#if UNITY_EDITOR
////					if (!Application.isPlaying) NGUITools.SetDirty(sp);
////#endif
////				}
//			}
//		}
//	}

	bool mStarted = false;

	/// <summary>
	/// Localize the widget on enable, but only if it has been started already.
	/// </summary>

	void OnEnable ()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying) return;
#endif
		if (mStarted) OnLocalize();
	}

	/// <summary>
	/// Localize the widget on start.
	/// </summary>

	void Start ()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying) return;
#endif
		mStarted = true;
		OnLocalize();
	}

	/// <summary>
	/// This function is called by the Localization manager via a broadcast SendMessage.
	/// </summary>

	void OnLocalize ()
	{



        if (string.IsNullOrEmpty(key) == false)
        {

            value = Localization.Get(key);
        }

	}
}
