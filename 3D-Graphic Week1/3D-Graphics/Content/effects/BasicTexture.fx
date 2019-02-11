//vertex function input

matrix World;
matrix View;
matrix Projection;

float3 Color = float3(1, 1, 1);

Texture2D OverlayTexture;
Texture2D ModelTexture;
SamplerState TextureSample
{

};

struct VertexShaderInput
{
	float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
};

//pixel function input
struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float2 UV : TEXCOORD0;
};

//vertex shader function
VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderInput output;

    float4 world = mul(input.Position, World);
    float4 view = mul(world, View);
    float4 projection = mul(view, Projection);

    output.Position = projection;
    output.UV = input.UV * 8;

    return output;
}

//pixel sahder function
float4 MainPS(VertexShaderOutput input) : COLOR
{
            float4 textColor = ModelTexture.Sample(TextureSample, input.UV);
    float4 overlayColor = OverlayTexture.Sample(TextureSample, input.UV);
    return float4(Color * (textColor /* + overlayColor*/).rgb, 1);

}

technique BasicColorDrawing
{
	pass P0
	{
        VertexShader = compile vs_5_0 MainVS();
        PixelShader = compile ps_5_0 MainPS();
    }
};