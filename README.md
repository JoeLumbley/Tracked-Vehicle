# Tracked Vehicle


An application simulating a tracked vehicle.






![003](https://github.com/user-attachments/assets/3fd4e9c5-9811-491b-b19a-ad53ed998955)






# Code Walkthrough

## ArrowVector Structure

### Definition
```vb
Public Structure ArrowVector
```
- **What it does:** This line defines a new structure named `ArrowVector`. A structure in is a value type that can contain data members and methods, similar to a class but more lightweight.

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













































# Form1 Class

The `Form1` class serves as the main entry point for the application. It initializes the form, handles user input, updates the game state, and renders graphics on the screen.



### Class Declaration
```vb
Public Class Form1
```
- **What it does:** This line defines the `Form1` class, which inherits from the base form class provided by Windows Forms. This class will contain all the necessary logic and UI elements for our application.

### Member Variables
```vb
Private ClientCenter As Point = New Point(ClientSize.Width / 2, ClientSize.Height / 2)
```
- **What it does:** Initializes a `Point` that represents the center of the client area of the form. This is useful for positioning elements relative to the center of the window.

```vb
Private MyBody As New Body(Brushes.Gray, New PointF(500, 500), 128, 64, 0, 0, 400, 30)
```
- **What it does:** Creates a new instance of the `Body` structure with specified parameters, including color, initial position, dimensions, angle, velocity, maximum velocity, and acceleration.

```vb
Dim myArrow As New ArrowVector(New Pen(Color.Black, 10), New PointF(640, 360), 0, 60, 70, 10, 15, 0, MyBody.MaxVelocity, 30)
```
- **What it does:** Creates a new instance of the `ArrowVector`, representing the direction and movement of the vehicle.

```vb
Private DeltaTime As New DeltaTimeStructure(Now, Now, TimeSpan.Zero)
```
- **What it does:** Initializes a `DeltaTimeStructure` to keep track of the time elapsed between frames, which is crucial for smooth animations and movements.

### Input State Variables
```vb
Private ADown As Boolean
Private DDown As Boolean
Private WDown As Boolean
Private SDown As Boolean
Private EDown As Boolean
Private F1Down As Boolean
Private F1DownHandled As Boolean
```
- **What it does:** These boolean variables track the state of various keys (A, D, W, S, E, F1) to handle user input effectively.

### Instructions and Audio Player
```vb
Private InstructionsFont As New Font("Segoe UI", 14)
Private InstructionsLocation As New PointF(0, 0)
Private InstructionsText As New String("Use A or D keys to rotate the vehicle" & Environment.NewLine & "W for forward, S for reverse" & Environment.NewLine & "E for emergency stop and " & Environment.NewLine & "F1 to Show/Hide Keyboard Hints.")
Private Player As AudioPlayer
```
- **What it does:**
  - `InstructionsFont`: Initializes a font for displaying instructions.
  - `InstructionsLocation`: Sets the location for the instructions text.
  - `InstructionsText`: Contains the instructions for controlling the vehicle.
  - `Player`: Declares an instance of the `AudioPlayer` class to manage sound effects.

### Constructor
```vb
Public Sub New()
    InitializeComponent()
    InitializeForm()
    InitializeTimer()
    CreateSoundFiles()
```
- **What it does:** The constructor initializes the form and its components. It calls several methods to set up the UI, timer, and audio files.

#### Inside the Constructor
```vb
Dim FilePath As String = Path.Combine(Application.StartupPath, "idle.mp3")
Player.AddSound("idle", FilePath)
Player.SetVolume("idle", 300)
Player.LoopSound("idle")
```
- **What it does:** This block loads the "idle" sound file, sets its volume, and enables looping. Similar blocks are present for "running" and "emergency stop" sounds.

### InitializeForm Method
```vb
Private Sub InitializeForm()
    CenterToScreen()
    SetStyle(ControlStyles.UserPaint, True)
    SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
    Text = "Tracked Vehicle - Code with Joe"
    WindowState = FormWindowState.Maximized
End Sub
```
- **What it does:**
  - `CenterToScreen()`: Centers the form on the screen.
  - `SetStyle(...)`: Enables custom painting and reduces flickering by using double buffering.
  - `Text`: Sets the title of the window.
  - `WindowState`: Maximizes the window when it opens.

### Timer Initialization
```vb
Private Sub InitializeTimer()
    Timer1.Interval = 15
    Timer1.Start()
End Sub
```
- **What it does:** Initializes the timer to tick every 15 milliseconds, which is a suitable interval for frame updates in a game-like application.

### Timer Tick Event
```vb
Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
```
- **What it does:** This event handler is called every time the timer ticks. It updates the game state and renders the graphics.

#### Inside Timer1_Tick
```vb
DeltaTime.Update()
HandleKeyPresses()
```
- **What it does:** Updates the delta time and processes any key presses to control the vehicle.

```vb
myArrow.Center = MyBody.Center
myArrow.AngleInDegrees = MyBody.AngleInDegrees
myArrow.Velocity = MyBody.Velocity
```
- **What it does:** Updates the arrow's position, angle, and velocity to match the body.

```vb
myArrow.Update(DeltaTime.ElapsedTime)
MyBody.Update(DeltaTime.ElapsedTime)
```
- **What it does:** Calls the update methods for both the arrow and the body, passing the elapsed time to ensure smooth movement.

```vb
MyBody.CheckWallBounce(MyBody.Body, ClientSize.Width, ClientSize.Height)
```
- **What it does:** Checks for collisions with the walls of the form and adjusts the position and velocity of the body accordingly.

```vb
If MyBody.Velocity <> 0 Then
    If Not Player.IsPlaying("running") Then
        Player.LoopSound("running")
    End If
    If Player.IsPlaying("idle") Then
        Player.PauseSound("idle")
    End If
Else
    If Not Player.IsPlaying("idle") Then
        Player.LoopSound("idle")
    End If
    If Player.IsPlaying("running") Then
        Player.PauseSound("running")
    End If
End If
```
- **What it does:** This block manages sound playback based on the vehicle's movement. If the vehicle is moving, it plays the running sound; if it is stationary, it plays the idle sound.

```vb
Invalidate()
```
- **What it does:** This method triggers a repaint of the form, causing the `Paint` event to fire and update the visual representation of the vehicle and arrow.

### HandleKeyPresses Method
```vb
Private Sub HandleKeyPresses()
```
- **What it does:** This method checks the state of the keys and updates the vehicle's angle and velocity accordingly.

#### Inside HandleKeyPresses
```vb
If ADown Then
    If MyBody.AngleInDegrees > 0 Then
        MyBody.AngleInDegrees -= 1 ' Rotate counterclockwise
    Else
        MyBody.AngleInDegrees = 360
    End If
End If
```
- **What it does:** If the "A" key is pressed, it rotates the vehicle counterclockwise by decreasing the angle. If the angle goes below 0, it wraps around to 360 degrees.

```vb
If DDown Then
    If MyBody.AngleInDegrees < 360 Then
        MyBody.AngleInDegrees += 1 ' Rotate clockwise
    Else
        MyBody.AngleInDegrees = 0
    End If
End If
```
- **What it does:** If the "D" key is pressed, it rotates the vehicle clockwise by increasing the angle. If the angle exceeds 360 degrees, it resets to 0.

```vb
If WDown Then
    If MyBody.Velocity < MyBody.MaxVelocity Then
        MyBody.Velocity += 1 ' Accelerate forward
    End If
End If
```
- **What it does:** If the "W" key is pressed, it increases the vehicle's velocity, accelerating it forward.

```vb
If SDown Then
    If MyBody.Velocity > -MyBody.MaxVelocity Then
        MyBody.Velocity -= 1 ' Accelerate backward
    End If
End If
```
- **What it does:** If the "S" key is pressed, it decreases the vehicle's velocity, allowing it to reverse.

```vb
If EDown Then
    MyBody.Velocity = 0 ' Emergency stop
End If
```
- **What it does:** If the "E" key is pressed, it sets the vehicle's velocity to 0, effectively stopping it.

```vb
If F1Down AndAlso Not F1DownHandled Then
    ShowKeyboardHints = Not ShowKeyboardHints ' Toggle keyboard hints
    F1DownHandled = True
End If
```
- **What it does:** If the "F1" key is pressed, it toggles the visibility of keyboard hints and marks the key as handled to prevent multiple toggles during a single press.

### Paint Event
```vb
Protected Overrides Sub OnPaint(e As PaintEventArgs)
```
- **What it does:** This method overrides the default paint behavior to customize what is drawn on the form.

#### Inside OnPaint
```vb
Dim g As Graphics = e.Graphics
```
- **What it does:** Retrieves the graphics object used for drawing.

```vb
MyBody.Draw(g)
myArrow.Draw(g)
```
- **What it does:** Calls the draw methods for both the body and the arrow, rendering them on the form.

```vb
If ShowKeyboardHints Then
    g.DrawString(InstructionsText, InstructionsFont, Brushes.Black, InstructionsLocation)
End If
```
- **What it does:** If keyboard hints are enabled, it draws the instructions text on the form.

### Key Down and Key Up Events
```vb
Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
```
- **What it does:** This event handler is triggered when a key is pressed.

#### Inside KeyDown
```vb
Select Case e.KeyCode
    Case Keys.A
        ADown = True
    Case Keys.D
        DDown = True
    Case Keys.W
        WDown = True
    Case Keys.S
        SDown = True
    Case Keys.E
        EDown = True
    Case Keys.F1
        F1Down = True
End Select
```
- **What it does:** This block checks which key was pressed and sets the corresponding boolean variable to `True`.

```vb
End Sub
```
- **What it does:** Ends the `KeyDown` event handler.

```vb
Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
```
- **What it does:** This event handler is triggered when a key is released.

#### Inside KeyUp
```vb
Select Case e.KeyCode
    Case Keys.A
        ADown = False
    Case Keys.D
        DDown = False
    Case Keys.W
        WDown = False
    Case Keys.S
        SDown = False
    Case Keys.E
        EDown = False
    Case Keys.F1
        F1Down = False
        F1DownHandled = False
End Select
```
- **What it does:** This block checks which key was released and sets the corresponding boolean variable to `False`. It also resets the `F1DownHandled` flag to allow toggling hints again.

### Conclusion of Form1 Class
The `Form1` class orchestrates the entire application, handling user input, updating the game state, and rendering graphics. It connects the `Body` and `ArrowVector` structures, allowing for dynamic interaction and movement of the tracked vehicle. Understanding the flow of this class is essential for grasping how the simulation operates.

## Final Thoughts
This comprehensive walkthrough of the `ArrowVector`, `Body`, and `Form1` classes provides a solid foundation for understanding how to implement basic graphics and movement in a Visual Basic application. With this knowledge, you can experiment further with game development concepts, such as collision detection, user input handling, and graphical rendering.

If you have any questions or need clarification on specific parts of the code, please don't hesitate to ask! Happy coding!

























