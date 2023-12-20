Rain-2D prefab, better use for 2D-Platform games

Some important values:
- Shape > radius			- range of rain, custom this to fill your screen or your map.
- Velocity over life time > .z		- rain speed.
- Renderer > render mode		- size of a raindrop.
  > stretched billboard > speed scale
- Start color > random between 		- make it variety and fit with your background.
					- color alpha better set to around 120, see more realistic.
- Emission > rate over time		- number of raindrops.
- Gravity modifier			- gravity for raindrops.
- Collision > life time loss		- enable collision make raindrop collide with your ground, increase life time loss make
					rain drop disappear faster.

You should set sorting layer ID for your rains (for example: Background layer for Rain_Back, foreground layer for Rain_Front)