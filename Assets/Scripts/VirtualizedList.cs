using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualizedList : MonoBehaviour
{
	private const int ELEMENT_HEIGHT = 50;

	[Header("References")]
	[SerializeField] private RectTransform parent;
	[SerializeField] private Scrollbar scroll;

	[SerializeField] private VirtualizedListEntryView entryPrefab;

	[Header("DEBUG")]
	[SerializeField] private int initialEntries = 30;

	private List<Entry> entries = new();
	private List<VirtualizedListEntryView> rendered = new();

	private int maxEntries = 0;

	private void Awake()
	{
		float maxH = parent.rect.height;
		maxEntries = (int)(maxH / ELEMENT_HEIGHT);

		for(int i = 0; i < maxEntries; ++i)
		{
			VirtualizedListEntryView entry = Instantiate(entryPrefab, parent);
			entry.Visibility = false;

			RectTransform rect = entry.transform as RectTransform;
			rect.anchoredPosition = new Vector2(0, -i * ELEMENT_HEIGHT);

			rendered.Add(entry);
		}

		for(int i = 0; i < initialEntries; ++i)
			AddEntry();
		Refresh(0);

		scroll.onValueChanged.AddListener(Refresh);
	}
	private void OnDestroy()
	{
		scroll.onValueChanged.RemoveListener(Refresh);
	}

	private void Update()
	{
		if(Input.mouseScrollDelta.y != 0)
		{
			int steps = scroll.numberOfSteps - 2;
			if(steps < 1)
				steps = 1;
			float diff = Input.mouseScrollDelta.y / -steps;
			float val = Mathf.Clamp(
				scroll.value + diff, 0, 1
			);
			scroll.value = val;
		}
	}

	private void Refresh(float value)
	{
		int steps = entries.Count - maxEntries;
		if(steps < 0)
			steps = 0;
		scroll.numberOfSteps = steps;

		int offset = Mathf.FloorToInt(steps * value);
		for(int i = 0; i < rendered.Count; ++i)
		{
			int idx = offset + i;
			rendered[i].Setup(idx < entries.Count? entries[idx]: null);
		}
	}

	public void AddEntry()
	{
		entries.Add(new Entry() {
			label = $"Entry {Random.Range(0, 2137)}",
			toggle = false
		});
		Refresh(scroll.value);
	}
	public void RemoveChecked()
	{
		entries.RemoveAll(e => e.toggle);
		Refresh(scroll.value);
	}

	public class Entry
	{
		public string label;
		public bool toggle;
	}
}
