# Counter Strike DS Unity Tool Project

This Unity project is used to simplify some aspects of the Counter Strike DS game project.

#### The project exports guns sprites, and all of this as code :
- Collisions
- Stairs
- Culling zones
- Shadow zones
- Bomb zones
- Path finding matrices and waypoints

### **How to export code :**
- Open a map (Scenes/Maps)
- Enter in play mode
- Copy theses variables from the **"Exporter"** script : **"Stairs code"**, **"Collisions Code"**, **"Triggers Code"**, **"Shadows Triggers Code"** and **"Zones Code"**
- Copy theses variables from the **"Path Finding"** script : **"Waypoints code"** and **"All Matrices Code"**
- Put copied code in the game code

### **How to export gun sprites :**
- Open the **"Gun_Renderer"** scene
- Set the Unity game screen resolution to 96x96
- Set the save path folder in the **"Gun Renderer"** script on the **"All Guns"** gameobject
- Enter in play mode and wait some seconds

### **Add Collisions/Stairs... :**

#### For collisions : 
- Open the **"AllCollisions"** gameobject
- Add a **"CollX"** gameobject with a box collider and a **"Collision"** script

Note : X is a number, collisions needs to be from 0 to X, do not make missing number like 0,1,3,4. The collision Zone is the Mesh zone/part index.
#### For stairs : 
- Open the **"AllStairs"** gameobject
- Add a **"StairsX"** gameobject with a **"Stairs"** Script
- Add 4 gameobject in the new gameobject, (**"xA", "xB", "zA", "zB"**)
- put xA and xB at the bottom of the stairs
- put zA and zB at the top of the stairs
- Make a patern like this 
        xA--------zA
bottom  |         | Top
        xB--------zB

- You can ajust Start and End Y Offset if needed
Note : xA needs to be in front of zA and xb needs to be in front of zb.

#### For bomb zones : 
- Create a gameobject named **"ZoneA"** or **"ZoneB"**
- Add a Box Collider and a **"Bomb Trigger"** script to it
- Set the nearest waypoint in the **"Bomb Trigger"** script 
- Add the point in the **"Exporter"** script

#### For waypoints :
- Open the **"PathFinding"** gameobject
- Add a **"PX"** gameobject
- Add the **"Point"** script to it
- Add all connected point in the **"Edge"** variable of the **"Point"** script

Note : X is a number, points needs to be from 0 to X, do not make missing number like 0,1,3,4.

#### For zones triggers :
- Open the **"Zones triggers"** gameobject
- Add a **"TriggerX"** gameobject
- Add a Box Collider and a **"Trigger"** script to it

#### For shadows triggers :
- Open the **"Shadows triggers"** gameobject
- Add a **"ShadowTriggerX"** gameobject
- Add a Box Collider and a **"Shadow Trigger"** script to it

Note : X is a number, triggers needs to be from 0 to X, do not make missing number like 0,1,3,4.

<img src="https://user-images.githubusercontent.com/39272935/176427102-248458c3-ce47-4a55-b80c-604b51b71a7e.png" width="220">
<img src="https://user-images.githubusercontent.com/39272935/176427368-4fda5ce1-f56a-4a73-8066-8b424a88ba4a.png" width="700">

### TODO :
- [ ] Export code by clicking a button instead of enter in play mode.
