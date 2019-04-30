If you'd like to map controls, You can do so by changing "controls.txt"
The lines in order correspond to:

Keyboard key to go right
Keyboard key to go left
Keyboard key for up
Keyboard key for down
Keyboard key for jump
Keyboard key for action
Keyboard key for next enemy
Button for right on a controller
Button for left on a controller
Button for up on a controller
Button for down on a controller
Button for jump on a controller
Button for action on a controller
Button for next enemy on a controller

I have these set to encode and decode whenever the file is read from
or saved. Here are the encode for GamePad buttons:

A = A button
B = B button
X = X button
Y = Y button
R = Right trigger
L = Left trigger
ZR = Right bumper
ZL = Left bumper
S = Start
Se = Select
DU = Dpad Up
DD = Dpad Down
DL = Dpad Left
DR = Dpad Right

The keyboard has a similar encoding scheme, with each letter on the 
keyboard being whatever letter it is. 
So if you wanted to move left with the "T" key on your keyboard,
you would change the second line of controls.txt to 'T' minus the single quotes
