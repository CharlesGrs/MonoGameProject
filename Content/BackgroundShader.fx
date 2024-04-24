sampler2D inputTexture : register(s0);

cbuffer MatrixBuffer : register(b1)
{
     matrix ViewMatrix;
     matrix ProjectionMatrix;

     matrix InverseViewMatrix;
     matrix InverseProjectionMatrix;
}

cbuffer ScreenDimensionsBuffer : register(b0)
{
    float2 ScreenDimensions;
}

float rand(float2 co)
{
    return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
}

float4 SampleTile(float2 uv)
{
    return 0;
}

float4 ScreenSpaceToWorld(float4 screenPosition)
{
        
    float4 clipSpacePosition;
    clipSpacePosition.xy = (screenPosition.xy / ScreenDimensions) * 2.0 - 1.0;  //remap to [-1, 1]
    clipSpacePosition.z = 0;  // depth 
    clipSpacePosition.w = 1.0;
    
    return clipSpacePosition;


    // Transform to view space
    float4 viewSpacePosition = mul(clipSpacePosition, InverseProjectionMatrix);
    viewSpacePosition /= viewSpacePosition.w;
    
    return viewSpacePosition;

    // Transform to world space
    float4 worldPosition = mul(viewSpacePosition, InverseViewMatrix);

    return worldPosition;
}

float4 MainPS(float4 position : SV_Position, float4 col : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{
    float4 screenPosition = float4(uv.x * ScreenDimensions.x, (1 - uv.y) * ScreenDimensions.y, 0, 1);
    float4 worldPosition = ScreenSpaceToWorld(screenPosition);
    
    return worldPosition;
}

technique PostProcess
{
    pass P0
    {
        PixelShader = compile ps_4_0_level_9_1 MainPS();
    }
}
