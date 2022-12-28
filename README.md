# Voxel2
Pre recording build of MC Unit 2 Voxel World


## MeshGeneratorEditor
MeshGeneratorEditor is a custom editor script for the MeshGenerator class in Unity. It adds buttons to the inspector view of a MeshGenerator object, which can be clicked to generate various meshes.

### OnInspectorGUI()
This method is called whenever the inspector view of a MeshGenerator object is displayed. It adds buttons to the inspector view that, when clicked, will call the various mesh generation methods of the MeshGenerator class.

### OnSceneGUI()
This method is called when the scene view of a MeshGenerator object is displayed. It displays labels next to each vertex in the object's mesh, indicating the index of the vertex.

### MeshGenerator
MeshGenerator is a Unity MonoBehavior that generates meshes for display in the game engine. It contains several methods for generating different types of meshes.

### Properties
vertices: A list of Vector3 objects representing the vertices of the generated mesh.
triangles: A list of integers representing the triangles of the generated mesh. Each group of three integers represents a single triangle, with the integers representing the indices of the vertices in the vertices list.
uvs: A list of Vector2 objects representing the UV coordinates of the generated mesh.
rectangleWidth: The width of the rectangle mesh that will be generated by the GenerateRectangle() method.
rectangleHeight: The height of the rectangle mesh that will be generated by the GenerateRectangle() method.
planeWidth: The width of the plane mesh that will be generated by the GeneratePlane() method.
voxelType: The type of voxel that will be generated by the GenerateVoxel() method.
### Methods
GenerateRectangle(): Generates a rectangle mesh with the dimensions specified by the rectangleWidth and rectangleHeight properties.
GeneratePlane(): Generates a plane mesh with the dimensions specified by the planeWidth property.
GenerateCube(): Generates a cube mesh with dimensions of 1 unit.
GenerateVoxel(Voxel.Type voxelType): Generates a voxel mesh with the specified type. The type of voxel is specified by the voxelType property.
### Dependencies
MeshFilter: This component is required for the MeshGenerator to display a mesh in the game engine.
MeshRenderer: This component is required for the MeshGenerator to display a mesh in the game engine.
