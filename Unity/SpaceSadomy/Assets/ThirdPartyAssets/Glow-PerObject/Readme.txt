v1.1
AntiAliasing fix for D3D added.
v2.0
Completely changed the workflow.

HOW TO USE:
1- Add "RenderGlow.cs" to your camera.
2- Add Supplied Black texture to the RenderGlow scripts suitable spot.
3- Whatever the object you want to glow, add "GlowTexture.cs" to the transform
which has the mesh renderer. (Other non glowing objects will use the black texture)
4- Add the glowtexture to the suitable spot of the GlowTexture script.
5- Finally Add one of the supplied image effects "PP_Glow_SM2 or SM3" to your camera.

Check the Demo scene to see how it is setup.

IMPORTANT NOTE:
Your glow textures background(non glowing parts) should be in pure black color.
Other colors will glow.

HOW IT WORKS:
We render all the glow objects to a rendertexture with the supplied glowtextures.
Then we add this to framebuffer texture with some blur and bloom.
Thats how we achieve the effect.

KNOWN ISSUES:
None at the moment.

Support:
For any questions, you can contact me from aubergine2010@gmail.com.

Thanks for buying the package.