# Contours2HeightMap
Compute shader that generates a height map from 2D vector contours. In combination with a tesselation shader this can be used to generate three-dimensional geometry out of two-dimensional polygons. 
I extracted contours out of a video with OpenCV and used these contours as input for the compute shader. Using the height map with a tesselation shader I got a three-dimensional geometry out of a simple video.
Changing the mapping function `1.0 - rcp(1.0 + smallestDistance * curviness)` at the end of the shader will result in different shaping of the geometry.

| Contours | Height Map | Rendered geometry |
| --- | --- | --- |
|![contour](https://user-images.githubusercontent.com/18415215/109659645-2c996e00-7b68-11eb-9fbb-36491a0db6e9.gif)|![height_map](https://user-images.githubusercontent.com/18415215/109654972-1341f300-7b63-11eb-8a35-9a8d28d6a9a0.gif)|![wald](https://user-images.githubusercontent.com/18415215/109654981-14732000-7b63-11eb-8e4c-de145747146e.gif)
