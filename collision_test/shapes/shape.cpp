#include "shape.h"

//---------------------------------Functions-----------------------------------

//##############Shape##################

//Tells if two shapes are colliding using SAT colliision detection.
bool Shape::isCollision(const Shape& shape1, const Shape& shape2)
{
    //Debug, write collision here
}

//##############Line##################

//Checks if 2 2D lines intersect. Since this is 2D, just make sure slopes don't match. If they do, check y_int
bool Line::isIntersecting(const Line& line1, const Line& line2)
{
    if (line1.slope != line2.slope) //Non-parallel lines must always intersect
        return true;
    
    if (line1.y_int == line2.y_int) //If slopes match, but y_ints are the same, lines overlap, so they intersect
        return true;

    return false; //If these both failed, return false
}

//Finds the point of intersection between two lines, assuming they already intersect. 
//Please use doIntersect() to check for intersection
point2D Line::findIntersection(const Line& line1, const Line& line2)
{
    point2D intersection;

    //Algebraically calculate coordinate of x intersection. Could also use 2x2 matrix, but this should be fine
    intersection.x = (line2.y_int - line1.y_int)/(line1.slope - line2.slope);
    intersection.y = line1.slope*intersection.x + line1.y_int; //Just use y = mx + b, since this is a 2D line

    return intersection;
}

//##############LineSegment##################

//----------------------------------Constructors-------------------------------

//##############Line##################
//Constructor that takes two points. This will find the slope and the y-int, and then set them based on the two points.
Line::Line(const point2D& point1, const point2D& point2)
{
    this->slope = point2D::findSlope(point1, point2); //Find the slope of this line
    this->y_int = point1.y - this->slope*point1.x; //Take b = y - mx, using point1 for x and y.
}