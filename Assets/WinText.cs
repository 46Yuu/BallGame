using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinText : MonoBehaviour
{
    float rainbowSpeed = 0.8f;
    private TMPro.TMP_Text winText;

    private void Awake()
    {
        winText = GetComponent<TMPro.TMP_Text>();
    }

    void Update()
    {
        winText.ForceMeshUpdate();
        var textInfo = winText.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }
            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            for (int j = 0; j < 4; j++)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2 + orig.x * 0.01f) * 5, 0);
            }
        }

        for(int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            winText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }

        Color rainbowColor = Color.HSVToRGB(Time.time * rainbowSpeed % 1f, 1f, 1f);
        winText.color = rainbowColor;
    }
}
