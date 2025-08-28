using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Renderer SetMaterial(Renderer rendererCube)
    {
        rendererCube.material.color = UnityEngine.Random.ColorHSV();

        return rendererCube;
    }

    public Renderer SetDefaultMaterial(Renderer rendererCube)
    {
        rendererCube.material.color = Color.white;

        return rendererCube;
    }
}
