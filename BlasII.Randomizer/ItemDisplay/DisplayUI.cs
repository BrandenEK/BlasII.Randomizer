using BlasII.Framework.UI;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Files;
using Il2CppTGK.Game.Components.UI;
using Il2CppTMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.ItemDisplay;

public class DisplayUI
{
    private readonly Sprite _frameSprite;
    private readonly Sprite _lightSprite;

    private RectTransform _object;
    private CanvasGroup _group;
    private Image _iconImage;
    private Image _lightImage;
    private UIPixelTextWithShadow _messageText;
    private UIPixelTextWithShadow _nameText;

    public DisplayUI()
    {
        Main.Randomizer.FileHandler.LoadDataAsSprite(Path.Combine("img", "popupframe.png"), out _frameSprite, new SpriteImportOptions()
        {
            Border = new Vector4(360, 0, 70, 0)
        });
        Main.Randomizer.FileHandler.LoadDataAsSprite(Path.Combine("img", "popuplight.png"), out _lightSprite, new SpriteImportOptions()
        {
            Border = new Vector4(130, 0, 130, 0)
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

        float maxWidth = Mathf.Max(_messageText.shadowText.preferredWidth, _nameText.shadowText.preferredWidth, BACK_MIN_WIDTH - EXTRA_WIDTH);
        float textWidth = Mathf.Min(maxWidth, BACK_MAX_WIDTH - EXTRA_WIDTH);

        _object.sizeDelta = new Vector2(textWidth + EXTRA_WIDTH, BACK_HEIGHT);
        _lightImage.rectTransform.sizeDelta = new Vector2(textWidth, LIGHT_HEIGHT);
        _messageText.shadowText.rectTransform.sizeDelta = new Vector2(textWidth, TEXT_HEIGHT);
        _messageText.normalText.rectTransform.sizeDelta = new Vector2(textWidth, TEXT_HEIGHT);
        _nameText.shadowText.rectTransform.sizeDelta = new Vector2(textWidth, TEXT_HEIGHT);
        _nameText.normalText.rectTransform.sizeDelta = new Vector2(textWidth, TEXT_HEIGHT);

        if (info.Image == null)
            return;

        int multiplier = 1;
        while (true)
        {
            Vector2 newSize = info.Image.rect.size * ++multiplier;

            if (newSize.x > ICON_HEIGHT || newSize.y > ICON_HEIGHT || multiplier > 5)
                break;
        }
        _iconImage.rectTransform.sizeDelta = info.Image.rect.size * (multiplier - 1);
    }

    public void UpdateAlpha(float alpha)
    {
        if (!EnsureObjectExists())
            return;

        _group.alpha = alpha;
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

        var frame = holder.AddImage(new ImageCreationOptions()
        {
            Sprite = _frameSprite
        });
        frame.type = Image.Type.Tiled;
        frame.pixelsPerUnitMultiplier = 3;

        // Left side

        var leftholder = UIModder.Create(new RectCreationOptions()
        {
            Name = "LeftHolder",
            Parent = holder,
            Size = new Vector2(ICON_HEIGHT, ICON_HEIGHT),
            XRange = Vector2.zero,
            YRange = Vector2.zero,
            Pivot = Vector2.zero,
            Position = new Vector2(ICON_POS_X, ICON_POS_Y),
        });

        _iconImage = UIModder.Create(new RectCreationOptions()
        {
            Name = "IconImage",
            Parent = leftholder,
            Size = new Vector2(ICON_HEIGHT, ICON_HEIGHT),
        }).AddImage();

        // Right side

        _lightImage = UIModder.Create(new RectCreationOptions()
        {
            Name = "LightImage",
            Parent = holder,
            Size = new Vector2(100, LIGHT_HEIGHT),
            XRange = Vector2.zero,
            YRange = Vector2.zero,
            Pivot = Vector2.zero,
            Position = new Vector2(LIGHT_POS_X, LIGHT_POS_Y),
        }).AddImage(new ImageCreationOptions()
        {
            Sprite = _lightSprite
        });
        _lightImage.type = Image.Type.Tiled;
        _lightImage.pixelsPerUnitMultiplier = 3;

        _messageText = UIModder.Create(new RectCreationOptions()
        {
            Name = "MessageText",
            Parent = holder,
            Size = new Vector2(100, TEXT_HEIGHT),
            XRange = Vector2.zero,
            YRange = Vector2.zero,
            Pivot = Vector2.zero,
            Position = new Vector2(TEXT_POS_X, TEXT_POS_Y),
        }).AddText(new TextCreationOptions()
        {
            Alignment = TextAlignmentOptions.Top,
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
            Size = new Vector2(100, TEXT_HEIGHT),
            XRange = Vector2.zero,
            YRange = Vector2.zero,
            Pivot = Vector2.zero,
            Position = new Vector2(TEXT_POS_X, TEXT_POS_Y),
        }).AddText(new TextCreationOptions()
        {
            Alignment = TextAlignmentOptions.Bottom,
            Color = new Color32(0xFF, 0xE3, 0x8F, 0xFF),
            Font = UIModder.Fonts.Blasphemous,
            FontSize = 32,
            UseRichText = true,
            WordWrap = false
        }).AddShadow();
        _nameText.shadowText.overflowMode = TextOverflowModes.Ellipsis;
        _nameText.normalText.overflowMode = TextOverflowModes.Ellipsis;

        _group = holder.gameObject.AddComponent<CanvasGroup>();
        _group.alpha = 0f;

        _object = holder;
    }

    private const int BACK_MIN_WIDTH = 450;
    private const int BACK_MAX_WIDTH = 900;
    private const int BACK_HEIGHT = 110;
    private const int BACK_OFFSET = 40;
    private const int ICON_POS_X = 64;
    private const int ICON_POS_Y = 25;
    private const int ICON_HEIGHT = 60;
    private const int LIGHT_POS_X = 146;
    private const int LIGHT_POS_Y = 12;
    private const int LIGHT_HEIGHT = 85;
    private const int TEXT_POS_X = 146;
    private const int TEXT_POS_Y = 19;
    private const int TEXT_HEIGHT = 72;
    private const int EXTRA_WIDTH = 200;
}
