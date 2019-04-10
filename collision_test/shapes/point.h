#include <iostream>

struct point2D
{
    //Member variables
    float x;
    float y;

    //Constructors
    point2D()
    {
    }

    point2D(float x, float y)
    {
        this->x = x;
        this->y = y;
    }
};

//Operator overloads
point2D operator +(const point2D& p1, const point2D& p2)
{
    point2D r_point;
    r_point.x = p1.x + p2.x;
    r_point.y = p1.y + p2.y;

    return r_point;
}

point2D operator -(const point2D& p1, const point2D& p2)
{
    point2D r_point;
    r_point.x = p1.x - p2.x;
    r_point.y = p1.y - p2.y;

    return r_point;
}

point2D operator *(const float& scalar, const point2D& point) //Multiplication between a number and a point
{
    point2D r_point;
    r_point.x = scalar*point.x;
    r_point.y = scalar*point.y;

    return r_point;
}

void print(point2D point)
{
    std::cout << "(" << point.x << "," << point.y << ")";
}