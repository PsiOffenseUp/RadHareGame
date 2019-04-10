#include "point.h"

//Shape class can be any n-sided polygon, but in order for collision to work, the shape must be convex.
class Shape
{
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
        static bool doIntersect(Line line1, Line line2); //Tells whether 2 lines are intersecting or not
        static point2D findIntersection(Line line1, Line line2); //Finds the point of intersection between 2 lines.

        //Constructors
        Line(float slope, float y_int) { this->slope = slope; this->y_int = y_int;}
};

class LineSegment : Line
{
    point2D start;
    point2D end;
};