# Contours2HeightMap
A Compute shader that generates a height map from 2D vector contours. In combination with a tesselation shader this can be used to generate three-dimensional geometry out of two-dimensional polygons. 
We extracted contours out of a video with OpenCV and used these contours as input for the compute shader. Using the height map with a tesselation shader you get a three-dimensional geometry out of a simple video.
Changing the mapping function `1.0 - rcp(1.0 + smallestDistance * curviness)` at the end of the shader will result in different shaping of the geometry.

This shader was written as part of the research publication [Real-Time Relighting of Video Streams for Augmented Virtuality Scenes](https://vsvr.medien.hs-duesseldorf.de/publications/gi-vrar2021-relighting/) and a corresponding [master's thesis](https://vsvr.medien.hs-duesseldorf.de/education/diplom/2021/davin-master2021.html) at the Virtual Sets and Virtual Environments Laboratory of the University of Applied Sciences Düsseldorf.

| Contours | Height Map | Rendered geometry |
| --- | --- | --- |
|![contour](https://user-images.githubusercontent.com/18415215/109659645-2c996e00-7b68-11eb-9fbb-36491a0db6e9.gif)|![height_map](https://user-images.githubusercontent.com/18415215/109654972-1341f300-7b63-11eb-8a35-9a8d28d6a9a0.gif)|![wald](https://user-images.githubusercontent.com/18415215/109654981-14732000-7b63-11eb-8e4c-de145747146e.gif)
