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
    private RectTransform _left;
    private RectTransform _right;

    private CanvasGroup _group;
    private Image _iconImage;
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
        float textWidth = Mathf.Min(maxWidth, BACK_MAX_WIDTH);

        _object.sizeDelta = new Vector2(textWidth + EXTRA_WIDTH, BACK_HEIGHT);
        _right.sizeDelta = new Vector2(textWidth, CONTENT_HEIGHT);

        //_messageText.shadowText.rectTransform.sizeDelta = new Vector2(textWidth, TEXT_HEIGHT);
        //_messageText.normalText.rectTransform.sizeDelta = new Vector2(textWidth, TEXT_HEIGHT);
        //_nameText.shadowText.rectTransform.sizeDelta = new Vector2(textWidth, TEXT_HEIGHT);
        //_nameText.normalText.rectTransform.sizeDelta = new Vector2(textWidth, TEXT_HEIGHT);

        int multiplier = 1;
        while (true)
        {
            Vector2 newSize = info.Image.rect.size * ++multiplier;

            if (newSize.x > CONTENT_HEIGHT || newSize.y > CONTENT_HEIGHT || multiplier > 5)
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

        //var light = UIModder.Create(new RectCreationOptions()
        //{
        //    Name = "x",
        //    Parent = holder,
        //    Size = new Vector2(300, 85),
        //    XRange = Vector2.zero,
        //    YRange = Vector2.zero,
        //    Pivot = Vector2.zero,
        //    Position = new Vector2(100, 12),
        //}).AddImage(new ImageCreationOptions()
        //{
        //    Sprite = _lightSprite
        //});
        //light.type = Image.Type.Tiled;
        //light.pixelsPerUnitMultiplier = 3;

        // Left side

        _left = UIModder.Create(new RectCreationOptions()
        {
            Name = "Left",
            Parent = holder,
            Size = new Vector2(CONTENT_HEIGHT, CONTENT_HEIGHT),
            XRange = Vector2.zero,
            YRange = Vector2.zero,
            Pivot = Vector2.zero,
            Position = new Vector2(ICON_POSX, CONTENT_POSY),
        });

        _iconImage = UIModder.Create(new RectCreationOptions()
        {
            Name = "IconImage",
            Parent = _left,
            Size = new Vector2(CONTENT_HEIGHT, CONTENT_HEIGHT),
        }).AddImage();

        // Right side

        _right = UIModder.Create(new RectCreationOptions()
        {
            Name = "Right",
            Parent = holder,
            Size = new Vector2(100, CONTENT_HEIGHT),
            XRange = Vector2.zero,
            YRange = Vector2.zero,
            Pivot = Vector2.zero,
            Position = new Vector2(TEXT_POSX, CONTENT_POSY),
        });

        _messageText = UIModder.Create(new RectCreationOptions()
        {
            Name = "MessageText",
            Parent = _right,
            XRange = new Vector2(0, 1),
            YRange = new Vector2(0, 1),
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
            Parent = _right,
            XRange = new Vector2(0, 1),
            YRange = new Vector2(0, 1),
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
    private const int BACK_MAX_WIDTH = 800;
    private const int BACK_HEIGHT = 110;
    private const int BACK_OFFSET = 40;
    private const int CONTENT_POSY = 25;
    private const int CONTENT_HEIGHT = 60;
    private const int TEXT_POSX = 146;
    //private const int TEXT_HEIGHT = 40;
    private const int ICON_POSX = 64;
    private const int EXTRA_WIDTH = 200;
    //private const int ICON_SIZE = 60;
}
