sampler2D inputTexture : register(s0);

float4 MainPS(float4 position : SV_Position, float4 col : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{
    float4 c = tex2D(inputTexture, uv);
    return 1-c;
}

technique PostProcess
{
    pass P0
    {
        PixelShader = compile ps_4_0_level_9_1 MainPS();
    }
}
