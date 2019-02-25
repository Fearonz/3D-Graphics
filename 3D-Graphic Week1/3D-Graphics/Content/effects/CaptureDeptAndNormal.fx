matrix World;
matrix View;
matrix Projetion;



//vertex function input
struct VertexShaderInput
{
	float4 Position : POSITION0;
    float3 Normal : NORMAL0;
};

//pixel function input
struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float2 Depth : TEXTCOORD0;
    float3 Normal : TEXTCOORD1;
};

//vertex shader function
VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 world = mul(input.Position, World);
    float4 view = mul(world, View);
    float4 proj = mul(view, Projetion);

    output.Position = proj;
    output.Normal = normalize(mul(input.Normal, World));

    //scaling tranformation
    //Z = vertiex Z Position
    //W = distance camera position (near plane) = far plane
    output.Depth.xy = output.Position.zw;

    return output;
}

struct PixelShaderOutput
{
    float4 Normal : COLOR0;
    float4 Depth : COLOR1;
};

//pixel sahder function
PixelShaderOutput MainPS(VertexShaderOutput input)
{
    PixelShaderOutput output; 

    //normals
    output.Normal.xyz = (input.Normal / 2) + 0.5f;
    output.Normal.a = 1;
    //depth
    float dist = input.Depth.x / input.Depth.y;
    output.Depth = float4(dist, dist, dist, 1);

    return output;
}

technique BasicColorDrawing
{
	pass P0
	{
        VertexShader = compile vs_5_0 MainVS();
        PixelShader = compile ps_5_0 MainPS();
    }
};