# Tracked Vehicle









![003](https://github.com/user-attachments/assets/3fd4e9c5-9811-491b-b19a-ad53ed998955)






# Code Walkthrough

## ArrowVector Structure

### Definition
```vb
Public Structure ArrowVector
```
- **What it does:** This line defines a new structure named `ArrowVector`. A structure in Visual Basic is a value type that can contain data members and methods, similar to a class but more lightweight.

### Member Variables
```vb
Public Pen As Pen
Public ReversePen As Pen
Public Center As PointF
Public Velocity As Double
Public VelocityVector As PointF
Public Acceleration As PointF
Public MinVelocity As Integer
Public MaxVelocity As Double
Public Length As Integer
Public ReverseLength As Integer
Public MinLength As Integer
Public MaxLength As Integer
Public Width As Single
Public ReverseWidth As Single
Public MinWidth As Integer
Public MaxWidth As Integer
Public AngleInDegrees As Single
Public AngleInRadians As Single
Public EndPoint As PointF
Public ReverseEndPoint As PointF
```
- **What each line does:**
  - `Public Pen As Pen`: This defines a `Pen` object used for drawing the arrow.
  - `Public ReversePen As Pen`: This defines a second `Pen` for drawing the reverse arrow.
  - `Public Center As PointF`: This represents the center point of the arrow in a 2D space.
  - `Public Velocity As Double`: This indicates the speed of the arrow's movement.
  - `Public VelocityVector As PointF`: This holds the X and Y components of the velocity as a point.
  - `Public Acceleration As PointF`: This holds the acceleration values in X and Y directions.
  - `Public MinVelocity`, `Public MaxVelocity`: These define the minimum and maximum velocities for the arrow.
  - `Public Length`, `Public ReverseLength`: These define the lengths of the arrow and its reverse.
  - `Public MinLength`, `Public MaxLength`: These define the minimum and maximum lengths the arrow can have.
  - `Public Width`, `Public ReverseWidth`: These define the widths of the arrow and its reverse.
  - `Public MinWidth`, `Public MaxWidth`: These define the minimum and maximum widths.
  - `Public AngleInDegrees`, `Public AngleInRadians`: These hold the angle of the arrow in degrees and radians.
  - `Public EndPoint`, `Public ReverseEndPoint`: These define where the arrow points and where the reverse arrow points.

### Constructor
```vb
Public Sub New(pen As Pen, center As PointF, angleInDegrees As Single, minLength As Double, maxLength As Double, minWidth As Double, maxWidth As Double, velocity As Double, maxVelocity As Double, acceleration As Double)
```
- **What it does:** This is the constructor for the `ArrowVector` structure. It initializes a new instance of `ArrowVector` with specified parameters.

#### Inside the Constructor
```vb
Me.Pen = pen
pen.StartCap = Drawing2D.LineCap.Round
pen.EndCap = Drawing2D.LineCap.ArrowAnchor
```
- `Me.Pen = pen`: Assigns the provided `pen` to the structure's `Pen` member.
- `pen.StartCap = Drawing2D.LineCap.Round`: Sets the starting cap of the pen to a rounded shape.
- `pen.EndCap = Drawing2D.LineCap.ArrowAnchor`: Sets the end cap of the pen to an arrow shape, making it look like an arrow.

```vb
ReversePen = New Pen(Color.White, 5)
ReversePen.StartCap = Drawing2D.LineCap.Round
ReversePen.EndCap = Drawing2D.LineCap.ArrowAnchor
```
- This creates a new `ReversePen` with a white color and a width of 5. It also sets the start and end caps just like the main pen.

```vb
Me.Center = center
```
- Assigns the provided `center` point to the `Center` member of the structure.

```vb
If angleInDegrees >= 0 AndAlso angleInDegrees <= 360 Then
    Me.AngleInDegrees = angleInDegrees
Else
    Me.AngleInDegrees = 0
End If
```
- This checks if the provided angle is within the valid range (0 to 360 degrees). If it is, it assigns it; otherwise, it defaults to 0.

```vb
AngleInRadians = DegreesToRadians(angleInDegrees)
```
- Converts the angle from degrees to radians for further calculations.

### Calculate EndPoint
```vb
EndPoint = New PointF(center.X + Length * Cos(AngleInRadians), center.Y + Length * Sin(AngleInRadians))
```
- This calculates the endpoint of the arrow using trigonometry. It takes the center point and adds the length of the arrow multiplied by the cosine and sine of the angle to determine the X and Y coordinates.

### Initialize Length and Width
```vb
Me.MinLength = minLength
Me.MaxLength = maxLength
Me.MinWidth = minWidth
Me.MaxWidth = maxWidth
Me.Velocity = velocity
Me.MaxVelocity = maxVelocity
```
- These lines initialize the minimum and maximum lengths and widths, as well as the velocity parameters.

### Set VelocityVector
```vb
VelocityVector.X = Cos(AngleInRadians) * Me.Velocity
VelocityVector.Y = Sin(AngleInRadians) * Me.Velocity
```
- This calculates the X and Y components of the velocity based on the angle. The velocity vector determines how the arrow moves in the 2D space.

### Length and Width Calculation
```vb
Length = GetLength(Me.Velocity, Me.MaxVelocity, Me.MinLength, Me.MaxLength)
Width = GetWidth(Me.Velocity, Me.MaxVelocity, Me.MinWidth, Me.MaxWidth)
```
- These lines call the `GetLength` and `GetWidth` functions to calculate the current length and width of the arrow based on its velocity.

### Pen Width
```vb
pen.Width = Width
ReverseLength = GetReverseLength(velocity, maxVelocity, minLength, maxLength)
ReverseWidth = GetReverseWidth(velocity, maxVelocity, minWidth, maxWidth)
ReversePen.Width = ReverseWidth
```
- Here, the width of the main pen is set to the calculated width. It also calculates the reverse length and width for the reverse arrow and assigns them to the respective pens.

### Initialize Acceleration
```vb
Me.Acceleration.X = acceleration
Me.Acceleration.Y = acceleration
```
- This initializes the acceleration of the arrow in both X and Y directions.

### Update Method
```vb
Public Sub Update(ByVal deltaTime As TimeSpan)
```
- **What it does:** This method updates the state of the arrow vector based on the elapsed time.

#### Inside the Update Method
```vb
Length = GetLength(Velocity, MaxVelocity, MinLength, MaxLength)
Width = GetWidth(Velocity, MaxVelocity, MinWidth, MaxWidth)
Pen.Width = Width
ReversePen.Width = ReverseWidth
```
- These lines recalculate the length and width based on the current velocity and update the pen widths accordingly.

```vb
AngleInRadians = DegreesToRadians(AngleInDegrees)
```
- Converts the angle to radians again for calculations.

```vb
VelocityVector.X = Cos(AngleInRadians) * Velocity
VelocityVector.Y = Sin(AngleInRadians) * Velocity
```
- Updates the velocity vector components based on the current angle.

```vb
EndPoint = New PointF(Center.X + Length * Cos(AngleInRadians), Center.Y + Length * Sin(AngleInRadians))
```
- Recalculates the endpoint of the arrow based on the updated length and angle.

```vb
ReverseLength = GetReverseLength(Velocity, MaxVelocity, MinLength, MaxLength)
ReverseWidth = GetReverseWidth(Velocity, MaxVelocity, MinWidth, MaxWidth)
ReverseEndPoint = New PointF(Center.X + ReverseLength * Math.Cos(ReverseAngleInRadians(AngleInDegrees)), Center.Y + ReverseLength * Math.Sin(ReverseAngleInRadians(AngleInDegrees)))
```
- Updates the reverse length and width, and calculates the reverse endpoint for the reverse arrow.

```vb
UpdateMovement(deltaTime)
```
- Calls the `UpdateMovement` method to adjust the position of the arrow based on its velocity.

### UpdateMovement Method
```vb
Public Sub UpdateMovement(ByVal deltaTime As TimeSpan)
```
- **What it does:** This method updates the position of the arrow based on its velocity and the time elapsed.

#### Inside the UpdateMovement Method
```vb
Center.X += CSng(VelocityVector.X * deltaTime.TotalSeconds)
Center.Y += CSng(VelocityVector.Y * deltaTime.TotalSeconds)
```
- These lines update the center position of the arrow by adding the product of the velocity and the elapsed time to the current position. This effectively moves the arrow.

### Draw Method
```vb
Public Sub Draw(g As Graphics)
```
- **What it does:** This method draws the arrow on the screen using the provided graphics context.

#### Inside the Draw Method
```vb
g.DrawLine(ReversePen, Center, ReverseEndPoint)
```
- Draws the reverse arrow using the reverse pen from the center to the reverse endpoint.

```vb
g.DrawLine(Pen, Center, EndPoint)
```
- Draws the main arrow from the center to the endpoint.

### Length and Width Calculation Functions
These functions calculate the length and width of the arrow based on the velocity and other parameters.

#### GetLength Function
```vb
Function GetLength(velocity As Double, maxVelocity As Double, minlength As Double, maxlength As Double) As Double
```
- **What it does:** This function calculates the length of the arrow based on its velocity.

#### Inside GetLength
```vb
Dim NormalizedVelocity As Double = velocity / maxVelocity
```
- Normalizes the velocity by dividing it by the maximum velocity.

```vb
Dim Length As Double = minlength + NormalizedVelocity * (maxlength - minlength)
```
- Interpolates the length based on the normalized velocity.

#### GetWidth Function
```vb
Function GetWidth(velocity As Double, maxVelocity As Double, minWidth As Double, maxWidth As Double) As Double
```
- **What it does:** This function calculates the width of the arrow based on its velocity.

#### Inside GetWidth
```vb
If velocity > 0 Then
    Dim NormalizedVelocity As Double = velocity / maxVelocity
    Return minWidth + NormalizedVelocity * (maxWidth - minWidth)
Else
    Return minWidth
End If
```
- If the velocity is greater than zero, it normalizes the velocity and calculates the width; otherwise, it returns the minimum width.

### Reverse Length and Width Functions
These functions are similar to the previous ones but calculate values based on the reverse velocity.

### Angle Conversion Functions
```vb
Public Function DegreesToRadians(angleInDegrees As Single) As Single
```
- **What it does:** Converts degrees to radians.

```vb
Return angleInDegrees * (PI / 180)
```
- This line performs the conversion.

### Reverse Angle Function
```vb
Public Function ReverseAngleInRadians(angleInDegrees As Single) As Single
```
- **What it does:** Calculates the reverse angle in radians.





















































## Body Structure
The `Body` structure is similar in concept to `ArrowVector` but is used to represent the physical body of the vehicle, including its position, velocity, and other properties.

### Definition
```vb
Public Structure Body
```
- **What it does:** This line defines a new structure named `Body`. This structure will represent the physical characteristics of a vehicle, including its position, velocity, and dimensions.

### Member Variables
```vb
Public Brush As Brush
Public Center As PointF
Public Velocity As Double
Public VelocityVector As PointF
Public Acceleration As PointF
Public MinVelocity As Integer
Public MaxVelocity As Double
Public AngleInDegrees As Single
Public AngleInRadians As Single
Public Width As Integer
Public Height As Integer
Dim HalfWidth As Integer
Dim HalfHeight As Integer
```
- **What each line does:**
  - `Public Brush As Brush`: This defines a `Brush` object used for filling shapes when drawing the body.
  - `Public Center As PointF`: This represents the center point of the body in a 2D space.
  - `Public Velocity As Double`: This indicates the speed of the body's movement.
  - `Public VelocityVector As PointF`: This holds the X and Y components of the body's velocity.
  - `Public Acceleration As PointF`: This holds the acceleration values in X and Y directions.
  - `Public MinVelocity`, `Public MaxVelocity`: These define the minimum and maximum velocities for the body.
  - `Public AngleInDegrees`, `Public AngleInRadians`: These hold the angle of the body in degrees and radians.
  - `Public Width`, `Public Height`: These define the dimensions of the body.
  - `Dim HalfWidth`, `Dim HalfHeight`: These are calculated as half of the width and height, used for positioning.

### Additional Member Variables
```vb
Private ReadOnly AlineCenterMiddle As StringFormat
Dim KeyboardHints As PointF()
Dim RotatedKeyboardHints As PointF()
Dim Body As PointF()
Dim RotatedBody As PointF()
Public HitBox As Rectangle
Public Min As PointF ' Minimum corner
Public Max As PointF ' Maximum corner
Public KeyboardHintsFont As Font
Public ShowKeyboardHints As Boolean
```
- **What each line does:**
  - `Private ReadOnly AlineCenterMiddle As StringFormat`: This defines a formatting object for centering text.
  - `Dim KeyboardHints As PointF()`: This array holds points for keyboard hints (e.g., where to draw hints on the screen).
  - `Dim RotatedKeyboardHints As PointF()`: This array holds the rotated positions of the keyboard hints.
  - `Dim Body As PointF()`: This array defines the shape of the body using points.
  - `Dim RotatedBody As PointF()`: This array holds the rotated positions of the body.
  - `Public HitBox As Rectangle`: This defines the bounding box for collision detection.
  - `Public Min`, `Public Max`: These points define the minimum and maximum corners of the body for collision checks.
  - `Public KeyboardHintsFont As Font`: This defines the font to be used for keyboard hints.
  - `Public ShowKeyboardHints As Boolean`: This flag determines whether to display keyboard hints.

### Constructor
```vb
Public Sub New(brush As Brush, center As PointF, width As Integer, height As Integer, angleInDegrees As Single, velocity As Double, maxVelocity As Double, acceleration As Double)
```
- **What it does:** This is the constructor for the `Body` structure. It initializes a new instance of `Body` with specified parameters.

#### Inside the Constructor
```vb
AlineCenterMiddle = New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
```
- This initializes the `StringFormat` object to center text both horizontally and vertically.

```vb
Me.Velocity = velocity
Me.MaxVelocity = maxVelocity
Me.Acceleration.X = acceleration
Me.Acceleration.Y = acceleration
```
- These lines initialize the velocity, maximum velocity, and acceleration values for the body.

```vb
Me.Center = center
Me.Brush = brush
Me.Width = width
Me.Height = height
```
- Assigns the provided values to the respective members of the structure.

```vb
HalfWidth = width / 2
HalfHeight = height / 2
```
- Calculates half the width and height, which will be useful for positioning the body.

### Body Shape Definition
```vb
Body = {
    New PointF(-HalfWidth, -HalfHeight),
    New PointF(HalfWidth, -HalfHeight),
    New PointF(HalfWidth, HalfHeight),
    New PointF(-HalfWidth, HalfHeight)
}
```
- This defines the shape of the body as a rectangle using four points relative to the center. The points are defined using the half-width and half-height to create a centered rectangle.

```vb
RotatedBody = New PointF(Body.Length - 1) {}
```
- This initializes the `RotatedBody` array to hold the rotated coordinates of the body.

### Keyboard Hints Initialization
```vb
KeyboardHintsFont = New Font("Segoe UI", 14, FontStyle.Bold)
```
- Initializes the font for the keyboard hints.

```vb
KeyboardHints = {
    New PointF(HalfWidth - 10, -HalfHeight - 20),
    New PointF(HalfWidth - 10, HalfHeight + 20),
    New PointF(HalfWidth + 20, -HalfHeight + Me.Height / 2),
    New PointF(-HalfWidth - 20, -HalfHeight + Me.Height / 2)
}
```
- This defines the positions of keyboard hints relative to the body. These points will be used to display hints for user controls.

```vb
RotatedKeyboardHints = New PointF(KeyboardHints.Length - 1) {}
```
- Initializes the `RotatedKeyboardHints` array to hold the rotated positions of the keyboard hints.

### Angle Validation
```vb
If angleInDegrees >= 0 AndAlso angleInDegrees <= 360 Then
    Me.AngleInDegrees = angleInDegrees
Else
    Me.AngleInDegrees = 0
End If
```
- This checks if the provided angle is within the valid range (0 to 360 degrees). If it is, it assigns it; otherwise, it defaults to 0.

```vb
AngleInRadians = DegreesToRadians(angleInDegrees)
```
- Converts the angle from degrees to radians for further calculations.

### Set Velocity Vector
```vb
VelocityVector.X = Cos(AngleInRadians) * Me.Velocity
VelocityVector.Y = Sin(AngleInRadians) * Me.Velocity
```
- This calculates the X and Y components of the velocity based on the angle, similar to what we did in the `ArrowVector`.

```vb
ShowKeyboardHints = True
```
- Initializes the flag to show keyboard hints.

### Update Method
```vb
Public Sub Update(ByVal deltaTime As TimeSpan)
```
- **What it does:** This method updates the state of the body based on the elapsed time.

#### Inside the Update Method
```vb
AngleInRadians = DegreesToRadians(AngleInDegrees)
RotatedBody = RotatePoints(Body, Center, AngleInRadians)
RotatedKeyboardHints = RotatePoints(KeyboardHints, Center, AngleInRadians)
```
- Converts the angle to radians and calculates the rotated positions of both the body and the keyboard hints.

```vb
VelocityVector.X = Cos(AngleInRadians) * Velocity
VelocityVector.Y = Sin(AngleInRadians) * Velocity
```
- Updates the velocity vector components based on the current angle.

```vb
UpdateMovement(deltaTime)
```
- Calls the `UpdateMovement` method to adjust the position of the body based on its velocity.

### Draw Method
```vb
Public Sub Draw(g As Graphics)
```
- **What it does:** This method draws the body on the screen using the provided graphics context.

#### Inside the Draw Method
```vb
g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
```
- Sets the smoothing mode for high-quality rendering.

```vb
g?.FillEllipse(Brushes.LightGray, Center.X - 72, Center.Y - 72, 144, 144)
```
- Draws a light gray circle around the center of the body.

```vb
g?.FillPolygon(Brush, RotatedBody)
```
- Fills the rotated body shape using the specified brush.

### Keyboard Hints Drawing
```vb
If ShowKeyboardHints Then
    g?.FillEllipse(Brushes.Black, RotatedKeyboardHints(0).X - 17, RotatedKeyboardHints(0).Y - 17, 34, 34)
    g?.DrawString("A", KeyboardHintsFont, Brushes.White, RotatedKeyboardHints(0), AlineCenterMiddle)
    ' Similar code for other keyboard hints...
End If
```
- This block checks if keyboard hints should be displayed. If so, it draws black circles at the hint positions and labels them with the corresponding keys (A, D, W, S).

### UpdateMovement Method
```vb
Public Sub UpdateMovement(ByVal deltaTime As TimeSpan)
```
- **What it does:** This method updates the position of the body based on its velocity and the time elapsed.

#### Inside UpdateMovement
```vb
Center.X += CSng(VelocityVector.X * deltaTime.TotalSeconds)
Center.Y += CSng(VelocityVector.Y * deltaTime.TotalSeconds)
```
- These lines update the center position of the body by adding the product of the velocity and the elapsed time to the current position. This effectively moves the body.

### Collision Detection Methods
#### CheckWallBounce Method
```vb
Public Sub CheckWallBounce(points As PointF(), clientWidth As Integer, clientHeight As Integer)
```
- **What it does:** This method checks for collisions with the screen boundaries and adjusts the position and velocity of the body accordingly.

#### Inside CheckWallBounce
```vb
If Center.X <= 0 Then
    Center.X = 0
    Velocity = -Velocity
ElseIf Center.X >= clientWidth Then
    Center.X = clientWidth
    Velocity = -Velocity
End If
```
- This checks for collisions on the left and right edges of the screen. If a collision is detected, it resets the position and reverses the velocity.

```vb
If Center.Y <= 0 Then
    Center.Y = 0
    Velocity = -Velocity
ElseIf Center.Y >= clientHeight Then
    Center.Y = clientHeight
    Velocity = -Velocity
End If
```
- Similar checks are made for the top and bottom edges of the screen.

### RotatePoints Method
```vb
Private Function RotatePoints(points As PointF(), center As PointF, angleInRadians As Single) As PointF()
```
- **What it does:** This method rotates an array of points around a specified center by a given angle.

#### Inside RotatePoints
```vb
Dim RotatedPoints As PointF()
RotatedPoints = New PointF(points.Length - 1) {}
```
- Initializes an array to hold the rotated points.

```vb
For i As Integer = 0 To points.Length - 1
    Dim x As Single = points(i).X * Cos(angleInRadians) - points(i).Y * Sin(angleInRadians)
    Dim y As Single = points(i).X * Sin(angleInRadians) + points(i).Y * Cos(angleInRadians)
    RotatedPoints(i) = New PointF(x + center.X, y + center.Y)
Next
```
- This loop iterates through each point, calculating its new position after rotation using trigonometric functions. The new position is then adjusted based on the center point.

### Contains Method
```vb
Public Function Contains(point As PointF) As Boolean
```
- **What it does:** This method checks if a given point is inside the bounding box of the body.

#### Inside Contains
```vb
Return point.X >= Min.X AndAlso point.X <= Max.X AndAlso point.Y >= Min.Y AndAlso point.Y <= Max.Y
```
- This line checks if the point's coordinates are within the minimum and maximum bounds of the body.

### Intersects Method
```vb
Public Function Intersects(rectangle As Rectangle) As Boolean
```
- **What it does:** This method checks if the body intersects with a given rectangle.

#### Inside Intersects
```vb
GetMinMax(Body)
```
- Calls the `GetMinMax` method to determine the minimum and maximum points of the body.

```vb
Return Min.X <= rectangle.Right AndAlso Max.X >= rectangle.Left AndAlso Min.Y <= rectangle.Bottom AndAlso Max.Y >= rectangle.Top
```
- This line checks if there is an intersection between the body and the rectangle based on their minimum and maximum coordinates.

### GetMinMax Method
```vb
Private Sub GetMinMax(points As PointF())
```
- **What it does:** This method calculates the minimum and maximum points of an array of points.

#### Inside GetMinMax
```vb
Min = points(0)
Max = points(0)
```
- Initializes the minimum and maximum points to the first point in the array.

```vb
For Each point As PointF In points
    If point.X < Min.X Then Min.X = point.X
    If point.Y < Min.Y Then Min.Y = point.Y
    If point.X > Max.X Then Max.X = point.X
    If point.Y > Max.Y Then Max.Y = point.Y
Next
```
- This loop iterates through each point to find the minimum and maximum X and Y coordinates.

### Conclusion of Body Structure
The `Body` structure is crucial for representing the physical characteristics of a vehicle in the simulation. It allows for movement, rotation, and collision detection, making it an essential component in game development.

## Form1 Class Overview
The `Form1` class serves as the main entry point for the application. It initializes the form, handles user input, updates the game state, and renders graphics on the screen.

### Key Components of Form1
- **Member Variables:** These hold instances of the `Body`, `ArrowVector`, and `AudioPlayer`, along with various states and control flags.
- **Constructor:** Initializes the form, sets up the timer, and loads sound files.
- **Timer Events:** Handles updates to the game state based on elapsed time.
- **User Input Handling:** Processes keyboard input to control the vehicle's movement and actions.
- **Rendering:** Draws the body and arrow on the screen, along with any keyboard hints.

### Conclusion
This lesson has provided a comprehensive walkthrough of the `ArrowVector` and `Body` structures, detailing how they work together to create a dynamic simulation of a tracked vehicle. Understanding these concepts is fundamental for anyone looking to delve into game development or graphical programming.

If you have any questions or need further clarification on any part of the code, feel free to ask! Happy coding!













































