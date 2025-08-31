using BlasII.Framework.UI;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Files;
using Il2CppTGK.Game.Components.UI;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.ItemDisplay;

public class DisplayUI
{
    private readonly Sprite _backgroundSprite;

    private RectTransform _object;
    private Image _iconImage;
    private UIPixelTextWithShadow _messageText;
    private UIPixelTextWithShadow _nameText;

    public DisplayUI()
    {
        Main.Randomizer.FileHandler.LoadDataAsSprite("randopopup.png", out _backgroundSprite, new SpriteImportOptions()
        {
            Border = new Vector4(308, 0, 24, 0)
        });
    }

    public bool EnsureObjectExists()
    {
        if (_object != null)
            return true;

        try
        {
            ModLog.Info("Creating item display UI");
            CreateDisplay();
            return _object != null;
        }
        catch (System.Exception ex)
        {
            ModLog.Error($"Failed to create UI - {ex}");
            return false;
        }
    }

    public void UpdateDisplay(DisplayInfo info)
    {
        if (!EnsureObjectExists())
            return;

        _iconImage.sprite = info.Image;
        _messageText.SetText(info.Message);
        _nameText.SetText(info.Item);

        float maxWidth = Mathf.Max(_messageText.shadowText.preferredWidth, _nameText.shadowText.preferredWidth, BACK_MIN_WIDTH - TEXT_POS);
        float textWidth = Mathf.Min(maxWidth, BACK_MAX_WIDTH);

        _object.sizeDelta = new Vector2(textWidth + TEXT_POS + 10, BACK_HEIGHT);
        _messageText.shadowText.rectTransform.sizeDelta = new Vector2(textWidth, TEXT_HEIGHT);
        _messageText.normalText.rectTransform.sizeDelta = new Vector2(textWidth, TEXT_HEIGHT);
        _nameText.shadowText.rectTransform.sizeDelta = new Vector2(textWidth, TEXT_HEIGHT);
        _nameText.normalText.rectTransform.sizeDelta = new Vector2(textWidth, TEXT_HEIGHT);
    }

    private void CreateDisplay()
    {
        var holder = UIModder.Create(new RectCreationOptions()
        {
            Name = "RandoItemDisplay",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(BACK_MIN_WIDTH, BACK_HEIGHT),
            XRange = new Vector2(1, 1),
            YRange = new Vector2(0, 0),
            Pivot = new Vector2(1, 0),
            Position = new Vector2(-BACK_OFFSET, BACK_OFFSET),
        });

        var background = holder.AddImage(new ImageCreationOptions()
        {
            Sprite = _backgroundSprite
        });
        background.type = Image.Type.Tiled;
        background.pixelsPerUnitMultiplier = 3;

        // TEMP !!!
        var group = holder.gameObject.AddComponent<CanvasGroup>();
        group.alpha = 1f;

        _iconImage = UIModder.Create(new RectCreationOptions()
        {
            Name = "IconImage",
            Parent = holder,
            Size = new Vector2(ICON_SIZE, ICON_SIZE),
            XRange = Vector2.zero,
            YRange = Vector2.zero,
            Pivot = Vector2.zero,
            Position = new Vector2(ICON_POS, 25),
        }).AddImage();

        _messageText = UIModder.Create(new RectCreationOptions()
        {
            Name = "MessageText",
            Parent = holder,
            Size = new Vector2(100, 40),
            XRange = Vector2.zero,
            YRange = Vector2.zero,
            Pivot = Vector2.zero,
            Position = new Vector2(TEXT_POS, 56),
        }).AddText(new TextCreationOptions()
        {
            Alignment = TextAlignmentOptions.Center,
            Color = new Color32(0xF8, 0xE4, 0xC6, 0xFF),
            Font = UIModder.Fonts.Blasphemous,
            FontSize = 32,
            UseRichText = true,
            WordWrap = false
        }).AddShadow();
        _messageText.shadowText.overflowMode = TextOverflowModes.Ellipsis;
        _messageText.normalText.overflowMode = TextOverflowModes.Ellipsis;

        _nameText = UIModder.Create(new RectCreationOptions()
        {
            Name = "NameText",
            Parent = holder,
            Size = new Vector2(100, 40),
            XRange = Vector2.zero,
            YRange = Vector2.zero,
            Pivot = Vector2.zero,
            Position = new Vector2(TEXT_POS, 14),
        }).AddText(new TextCreationOptions()
        {
            Alignment = TextAlignmentOptions.Center,
            Color = new Color32(0xFF, 0xE3, 0x8F, 0xFF),
            Font = UIModder.Fonts.Blasphemous,
            FontSize = 32,
            UseRichText = true,
            WordWrap = false
        }).AddShadow();
        _nameText.shadowText.overflowMode = TextOverflowModes.Ellipsis;
        _nameText.normalText.overflowMode = TextOverflowModes.Ellipsis;

        _object = holder;
    }

    private const int BACK_MIN_WIDTH = 450;
    private const int BACK_MAX_WIDTH = 800;
    private const int BACK_HEIGHT = 110;
    private const int BACK_OFFSET = 40;
    private const int TEXT_POS = 112;
    private const int TEXT_HEIGHT = 40;
    private const int ICON_POS = 24;
    private const int ICON_SIZE = 60;
}
