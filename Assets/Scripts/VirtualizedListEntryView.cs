using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VirtualizedListEntryView : MonoBehaviour
{
	public bool Visibility
	{
		get => gameObject.activeSelf;
		set => gameObject.SetActive(value);
	}

	public bool Toggle
	{
		get => toggle.isOn;
		set => toggle.isOn = value;
	}

	public string Label
	{
		get => label.text;
		set => label.SetText(value);
	}

	[Header("References")]
	[SerializeField] private TMP_Text label;
	[SerializeField] private Toggle toggle;

	private VirtualizedList.Entry bound;

	public void Setup(VirtualizedList.Entry entry)
	{
		if(entry == null)
		{
			bound = null;
			Visibility = false;
			return;
		}

		toggle.onValueChanged = new();

		label.SetText(entry.label);
		toggle.isOn = entry.toggle;
		toggle.onValueChanged.AddListener(state => {
			entry.toggle = state;
		});

		bound = entry;
		Visibility = true;
	}
}
