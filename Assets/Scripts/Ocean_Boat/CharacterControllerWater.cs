using UnityEngine;
using Unity.Mathematics;
using StylizedWater3;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerWater : MonoBehaviour
{
    [Header("Water References")]
    [Tooltip("The material applied to your Stylized Water 3 water object.")]
    public Material waterMaterial;
    [Tooltip("The wave profile assigned to the Stylized Water 3 material.")]
    public WaveProfile waveProfile;
    [Tooltip("The baseline Y world position of your flat water surface.")]
    public float baseWaterLevel = 0f;

    [Header("Movement & Physics")]
    public float gravity = -9.81f;
    [Tooltip("How strongly the water pushes the player upward.")]
    public float buoyancyStrength = 20f;
    [Tooltip("How fast the player slows down in water (simulates drag).")]
    public float waterResistance = 5f;

    public bool inWater = false;

    private CharacterController controller;
    private Vector3 velocity;
    private HeightQuerySystem.Sampler heightSampler;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (waterMaterial == null)
        {
            Debug.LogError("Please assign the Water Material to this script!", this);
        }

        if (waveProfile == null)
        {
            Debug.LogError("Please assign the Stylized Water 3 Wave Profile to this script!", this);
        }

        heightSampler = new HeightQuerySystem.Sampler();
        heightSampler.SetSampleCount(1, cpu: true);
    }

    void OnDestroy()
    {
        if (heightSampler != null)
        {
            heightSampler.Dispose();
        }
    }

    void Update()
    {
        if (waterMaterial == null || waveProfile == null) return;

        Vector3 currentPos = transform.position;

        heightSampler.SetSamplePosition(0, new float3(currentPos.x, currentPos.y, currentPos.z));
        heightSampler.heightValues[0] = baseWaterLevel;
        Gerstner.ComputeHeight(heightSampler, waveProfile, baseWaterLevel, waterMaterial);
        float absoluteWaterHeight = heightSampler.heightValues[0];

        if (currentPos.y >= absoluteWaterHeight)
        {
            // PlayerController owns gravity and jumping while the character is out of the water.
            velocity.y = 0f;

            inWater = false;
            return;
        }

        // Apply buoyancy only while submerged; PlayerController handles normal gravity and jumping.
        inWater = true;
        float depth = absoluteWaterHeight - currentPos.y;
        float displacementMultiplier = Mathf.Clamp01(depth / Mathf.Max(controller.height, 0.01f));
        float buoyancyForce = buoyancyStrength * displacementMultiplier;
        velocity.y += (gravity + buoyancyForce) * Time.deltaTime;
        velocity.y = Mathf.Lerp(velocity.y, 0f, waterResistance * Time.deltaTime);

        controller.Move(velocity * Time.deltaTime);
    }
}
