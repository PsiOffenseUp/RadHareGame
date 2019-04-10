#include "point.h"

//Shape class can be any n-sided polygon. Can be used for collision detection, but 
//in order for collision to work, the shape must be convex.
class Shape
{
    public:
        bool isCollision(const Shape& shape1, const Shape& shape2); //Function that checks collision between 2 shapes
        void setVertices(point2D vertexArr[]); //Sets the vertices for this shape
    
    private:
        point2D* vertices[]; //Array of 2D points to represent the vertices of this shape
};

//Line with a slope and a y-intercept
class Line
{
    public:
        //Fields
        float slope;
        float y_int;

        //Function declarations
        static bool isIntersecting(const Line& line1, const Line& line2); //Tells whether 2 lines are intersecting or not
        static point2D findIntersection(const Line& line1, const Line& line2); //Finds the point of intersection between 2 lines.

        //Constructors
        Line(float slope, float y_int) { this->slope = slope; this->y_int = y_int;}
};

class LineSegment : Line
{
    point2D start; //"Start" point for the line
    point2D end; //"End" point for the line.
};