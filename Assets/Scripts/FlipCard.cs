using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource), typeof(AudioSource))][SelectionBase]
public class FlipCard : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Maximal hover value on Y-axis")]
    float HoverY = 10.0f;

    [SerializeField]
    [Tooltip("Visual transform that is going to flip inside the parent")]
    Transform transformToFlip;

    [Tooltip("Text on Back of Card")]
    public TMP_Text BackText;

    [Tooltip("Image on Back of Card")]
    public Image BackImage;

    [Header("Material Settings")]

    [SerializeField]
    [Tooltip("Vertex shader Image")]
    Image HoverImage;


    [SerializeField]
    [Tooltip("Maximal change of vertex shader border value")]
    float HoverBorderValue = 0.1f;

    [SerializeField]
    public float AnimationTime = 0.25f;


    [HideInInspector]
    public bool isFlippedFront = true;


    float initY;
    AudioSource audioSourceFlip;
    AudioSource audioSourceHover;
    bool CardDisabled = false;

    // Delegate Signatures
    public delegate void HoverOver(bool isHovering);
    public delegate void CardFlip();

    /// <summary>
    /// Is the Viewer hovering over the object
    /// </summary>
    public HoverOver OnHoverOver;

    /// <summary>
    /// Card is Flipped
    /// </summary>
    public CardFlip OnCardFlip;


    void Awake()
    {
        DOTween.Init();
        OnHoverOver += HoverTransformChange;
        OnCardFlip += CardFlipRotation;

        transformToFlip = transformToFlip ?? transform;
        audioSourceFlip = GetComponents<AudioSource>()[0];
        audioSourceHover = GetComponents<AudioSource>()[1];

        if (HoverImage)
        {
            // Create Material for HoverImage
            // This is a workaround, because CanvasRenderer does not have a MaterialPropertyBlock to use PerRendererData
            Material mat = Instantiate(HoverImage.material);
            HoverImage.material = mat;
        }
    }


    private void Start()
    {
        initY = transform.localPosition.y;
    }

    void HoverTransformChange(bool isHovering)
    {
        float hovering = Convert.ToSingle(isHovering);

        if (HoverImage)
        {
            ChangeHoverVertexShader(ref hovering);
        }

        // Move the complete Card
        transformToFlip.DOLocalMoveY(initY + (HoverY * hovering), AnimationTime);

        if (isHovering && audioSourceHover.clip && !audioSourceHover.isPlaying)
        {
            audioSourceHover.Play();
        }
    }

    IEnumerator WaitForAnimationFinish(bool isHovering)
    {
        yield return new WaitForSeconds(AnimationTime);

        HoverTransformChange(isHovering);
    }

    void ChangeHoverVertexShader(ref float hovering)
    {
        HoverImage.material.DOFloat(1f + (HoverBorderValue * hovering), "_VertexMultiplier", AnimationTime);
    }

    /// <summary>
    /// Rotate based on Front site
    /// </summary>
    /// <param name="isFlipToBack"></param>
    void CardFlipRotation()
    {

        // Only rotate the Visual Card
        transformToFlip.DOLocalRotate(new Vector3(0f, 0f, 180f * Convert.ToSingle(isFlippedFront)), AnimationTime);

        if (audioSourceFlip.clip && !audioSourceFlip.isPlaying)
        {
            audioSourceFlip.Play();
        }

        isFlippedFront = !isFlippedFront;
    }

    public void SetCardDisabled(bool disable = true)
    {
        CardDisabled = disable;
    }

    public bool GetCardDisabled()
    {
        return CardDisabled;
    }
}
