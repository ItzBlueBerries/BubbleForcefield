using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

internal static class Utility
{
    public static Texture2D LoadTextureFromAssembly(string filename)
    {
        var a = Assembly.GetExecutingAssembly();
        var spriteData = a.GetManifestResourceStream(a.GetName().Name + "." + filename + ".png");
        var rawData = new byte[spriteData.Length];
        spriteData.Read(rawData, 0, rawData.Length);
        var tex = new Texture2D(1, 1);
        tex.LoadImage(rawData);
        tex.filterMode = FilterMode.Bilinear;
        return tex;
    }

    public static Sprite ConvertToSprite(this Texture2D texture) => Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1);

    public static T GetResource<T>(string name) where T : UnityEngine.Object => Resources.FindObjectsOfTypeAll<T>().FirstOrDefault(found => found.name.Equals(name));

    public static Color LoadHex(string hexCode)
    {
        if (!hexCode.Contains("#"))
            hexCode = "#" + hexCode;
        ColorUtility.TryParseHtmlString(hexCode, out var returnedColor);
        return returnedColor;
    }
}
