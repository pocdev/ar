Shader "Depth Mask Simple (Terrain)" 
{
  SubShader 
  {
    Tags {"Queue" = "Background+10" } // earlier = hides stuff later in queue
    Lighting Off
    ZTest LEqual
    ZWrite On
    ColorMask 0
    Pass {}
  }
}