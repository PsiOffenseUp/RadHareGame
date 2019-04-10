#include "shape.h"

//Checks if 2 2D lines intersect. Since this is 2D, just make sure slopes don't match. If they do, check y_int
bool Line::doIntersect(Line line1, Line line2)
{
    if (line1.slope != line2.slope) //Non-parallel lines must always intersect
        return true;
}

//Finds the point of intersection between two lines, assuming they already intersect. 
//Please use doIntersect() to check for intersection
point2D Line::findIntersection(Line line1, Line line2)
{
    point2D intersection;

    //Algebraically calculate coordinate of x intersection. Could also use 2x2 matrix, but this should be fine
    intersection.x = (line2.y_int - line1.y_int)/(line1.slope - line2.slope);
    intersection.y = line1.slope*intersection.x + line1.y_int; //Just use y = mx + b, since this is a 2D line

    return intersection;
}