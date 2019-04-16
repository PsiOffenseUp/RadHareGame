#include "RadRabbitEngine.h"

int main()
{
    //DEBUG, let's see how well the collision does
	//Make a square
	pnt_F testSquareCoor[4];
	testSquareCoor[0] = *(new pnt_F(0.0, 0.0));
	testSquareCoor[1] = *(new pnt_F(3.0, 0.0));
	testSquareCoor[2] = *(new pnt_F(3.0, 3.0));
	testSquareCoor[3] = *(new pnt_F(0.0, 0.0));
	RRE::Shape testSquare = *(new RRE::Shape(testSquareCoor));

	//Make a triangle
	pnt_F testTriangleCoor[3];
	testTriangleCoor[0] = *(new pnt_F(1.0, 1.0));
	testTriangleCoor[1] = *(new pnt_F(-2.0, 4.0));
	testTriangleCoor[2] = *(new pnt_F(4.0, 4.0));
	RRE::Shape testTriangle = *(new RRE::Shape(testTriangleCoor));

	//Make a rectangle
	pnt_F testRectangleCoor[4];
	testRectangleCoor[0] = *(new pnt_F(1.0, 3.5));
	testRectangleCoor[1] = *(new pnt_F(4.0, 3.5));
	testRectangleCoor[2] = *(new pnt_F(1.0, 5.0));
	testRectangleCoor[3] = *(new pnt_F(4.0, 5.0));
	RRE::Shape testRectangle = *(new RRE::Shape(testRectangleCoor));

	//Now, check the collisions
	std::ofstream outfile;
	outfile.open("test_output.txt");
	if(RRE::Shape::CheckCollision(testSquare, testTriangle)); //Should print True
		outfile << "Square and triangle intersect";
	if(RRE::Shape::CheckCollision(testTriangle, testRectangle)); //Should print True
		outfile << "Triangle and rectangle intersect";
	if(RRE::Shape::CheckCollision(testSquare, testRectangle)); //Should print False
		outfile << " Square and Rectangle intersect";
	
	outfile.close();
	
	return 0;
}