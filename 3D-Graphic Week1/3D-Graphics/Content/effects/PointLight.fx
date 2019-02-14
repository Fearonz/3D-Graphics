matrix World;
matrix View;
matrix Projetion;

float3 AmbientColor = float3(0.15f, 0.15f, 0.15f);
float3 DiffuseColor = float3(1, 1, 1);
float3 LightColor = float3(1, 1, 1);

float3 Position = float3(0, 0, 0);
float Attenuation = 40;
float FallOff = 2;


Texture2D ModelTexture;
SamplerState TextureSample
{

};


struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
    float3 Normal : NORMAL0;
};

//pixel function input
struct VertexShaderOutput
{
    

    float4 Position : SV_POSITION;
    float2 UV : TEXCOORDD0;
    float3 Normal : TEXCOORD1;
    float3 WorldPosition : TEXTCOORD2;
};

//vertex shader function
VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 world = mul(input.Position, World);
    float4 view = mul(world, View);
    float4 proj = mul(view, Projetion);

    output.Position = proj;
    output.UV = input.UV;
    output.Normal = normalize(mul(input.Normal, World));
    output.WorldPosition = world;

    return output;

}

//pixel sahder function
float4 MainPS(VertexShaderOutput input) : COLOR
{
    

    float3 color = DiffuseColor;

    float3 textureColor = ModelTexture.Sample(TextureSample, input.UV);
    color *= textureColor;

    float3 lightingColor = AmbientColor;
    float3 lightDirection = normalize(Position - input.WorldPosition);
    float3 dist = distance(Position, input.WorldPosition);

    float3 angle = saturate(dot(input.Normal, lightDirection));

    float atten = 1 - pow(clamp(dist / Attenuation, 0, 1), FallOff);

    lightingColor += saturate(angle * atten * LightColor);

    return float4(lightingColor * color, 1);

}

technique BasicColorDrawing
{
	pass P0
	{
        VertexShader = compile vs_5_0 MainVS();
        PixelShader = compile ps_5_0 MainPS();
    }
};