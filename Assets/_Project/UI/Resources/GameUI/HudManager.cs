using UnityEngine;
using UnityEngine.UIElements;

public class HudManager : MonoBehaviour
{
    public VisualTreeAsset counterUXML;

    private VisualElement woodCounterContainer;
    private VisualElement rockCounterContainer;
    private VisualElement waveTimerContainer;

    private Label counterWoodLabel;
    private Label counterStoneLabel;
    private Label counterWaveLabel;

    [Range(0, 100)]
    public int sliderValue = 0;

    public Sprite woodIcon;
    public Sprite stoneIcon;

    private void Start()
    {
        var hudUIDocument = GetComponent<UIDocument>();
        var hudRootElement = hudUIDocument.rootVisualElement;

        woodCounterContainer = hudRootElement.Q<VisualElement>("WoodCounter");
        rockCounterContainer = hudRootElement.Q<VisualElement>("StoneCounter");
        waveTimerContainer = hudRootElement.Q<VisualElement>("WaveTimer");

        if (woodCounterContainer != null && rockCounterContainer != null && waveTimerContainer != null)
        {
            LoadCounterUI(woodCounterContainer);
            LoadCounterUI(rockCounterContainer);
            LoadCounterUI(waveTimerContainer);
        }
        else
        {
            Debug.LogError("Nie znaleziono kontenerów 'woodCounter' lub 'rockCounter' w HUD!");
        }

        counterWoodLabel = woodCounterContainer.Q<Label>("counter");
        counterStoneLabel = rockCounterContainer.Q<Label>("counter");
        counterWaveLabel = waveTimerContainer.Q<Label>("counter");

        // Dodanie obrazka z rozmiarem w procentach
        AddImageToContainer(woodCounterContainer, woodIcon);
        AddImageToContainer(rockCounterContainer, stoneIcon);
        UpdateCounterLabels();
    }

    private void LoadCounterUI(VisualElement container)
    {
        var counterRoot = counterUXML.CloneTree();
        counterRoot.style.flexGrow = 1;
        counterRoot.style.width = Length.Percent(100);
        counterRoot.style.height = Length.Percent(100);
        container.Add(counterRoot);
    }

    private void AddImageToContainer(VisualElement container, Sprite sprite)
    {
        if (sprite == null)
        {
            Debug.LogWarning("Brak podpiêtego obrazka w Inspectorze!");
            return;
        }

        // ZnajdŸ istniej¹cy VisualElement o nazwie "Image" w kontenerze
        VisualElement imageContainer = container.Q<VisualElement>("Image");

        if (imageContainer == null)
        {
            Debug.LogError("Nie znaleziono elementu VisualElement o nazwie 'Image' w kontenerze!");
            return;
        }

        // Tworzenie nowego elementu Image
        Image image = new Image();
        image.sprite = sprite;

        // Ustawienie stylów dla obrazka
        image.style.width = Length.Percent(100);  // Obrazek zajmuje 100% szerokoœci kontenera "Image"
        image.style.height = Length.Percent(100); // Obrazek zajmuje 100% wysokoœci kontenera "Image"

        // Wyczyœæ zawartoœæ imageContainer (na wypadek, gdyby coœ tam by³o)
        imageContainer.Clear();

        // Dodaj obrazek do istniej¹cego elementu "Image"
        imageContainer.Add(image);
    }

    private void UpdateCounterLabels()
    {
        if (counterWoodLabel != null) counterWoodLabel.text = $"{sliderValue}";
        if (counterStoneLabel != null) counterStoneLabel.text = $"{sliderValue}";
        if (counterStoneLabel != null) counterWaveLabel.text = $"{(int)(sliderValue / 60)}:{(sliderValue % 60):00}";
    }

    private void Update()
    {
        UpdateCounterLabels();
    }
}