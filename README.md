# P1-2DPlatformer-KrysPalys
Scripting 2 - Assignment #2

**Developer:** Krys Palys
**Student ID:** 2530015

## Controls
* **Keyboard:** WASD or Arrow Keys for movement, Space to jump.
* **Gamepad:** Left stick for movement, A/South button to jump.

## Technical Implementation Details

* **Physics Approach:** I utilized `Rigidbody2D` with velocity manipulation. This approach allows me to leverage Unity's built-in physics engine to handle gravity natively, while giving me precise, code-driven control over horizontal momentum through custom acceleration and deceleration rates.
* **Ground Detection Method:** I used `Physics2D.OverlapCircle` positioned at the character's base. This is paired with a LayerMask to ensure the ground detection only triggers when touching designated ground layers, avoiding false positives.
* **Advanced Jump Technique:** I implemented both Coyote Time and Jump Buffering. This allows the player to jump for a brief moment after walking off a ledge, and queues up jump inputs pressed slightly before landing, making the controls feel highly responsive.
