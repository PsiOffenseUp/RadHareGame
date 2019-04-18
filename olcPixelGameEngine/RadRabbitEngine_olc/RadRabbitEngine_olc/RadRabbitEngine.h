#define OLC_PGE_APPLICATION
#include "olcPixelGameEngine.h"

//MACROS
#define v2d_I RRE::Vector2_generic<int>
#define v2d_F RRE::Vector2_generic<float>
#define v2d_D RRE::Vector2_generic<double>

#define pnt_I RRE::Point_generic<int>
#define pnt_F RRE::Point_generic<float>
#define pnt_D RRE::Point_generic<double>

namespace RRE {

	class Shape;

	template <class T>
	struct Vector2_generic { //Used for movement/ vector based calculations

		T x, y;

		//Constructors
		Vector2_generic() {
			x = 0; y = 0;
		}

		Vector2_generic(T x, T y) {
			this->x = x; this->y = y;
		}

		//Operator Overloads
		inline Vector2_generic<T> operator + (Vector2_generic<T> const &vec) { return Vector2_generic<T>(this->x + vec.x, this->y + vec.y); }
		inline Vector2_generic<T> operator - (Vector2_generic<T> const &vec) { return Vector2_generic<T>(this->x - vec.x, this->y - vec.y); }
		inline Vector2_generic<T> operator * (Vector2_generic<T> const &vec) { return Vector2_generic<T>(this->x * vec.x, this->y * vec.y); }
		inline Vector2_generic<T> operator / (Vector2_generic<T> const &vec) { return Vector2_generic<T>(this->x / vec.x, this->y / vec.y); }

		inline Vector2_generic<T> operator += (Vector2_generic<T> const &vec) { return Vector2_generic<T>(this->x + vec.x, this->y + vec.y); }
		inline Vector2_generic<T> operator -= (Vector2_generic<T> const &vec) { return Vector2_generic<T>(this->x - vec.x, this->y - vec.y); }
		inline Vector2_generic<T> operator *= (Vector2_generic<T> const &vec) { return Vector2_generic<T>(this->x * vec.x, this->y * vec.y); }
		inline Vector2_generic<T> operator /= (Vector2_generic<T> const &vec) { return Vector2_generic<T>(this->x / vec.x, this->y / vec.y); }

		//Inline Member Functions
		inline float Normalized() { T length = sqrt((this->x * this->x) + (this->y * this->y)); return Vector2_generic<T>(this->x / length, this->y / length); }
	};

	template <class T>
	struct Point_generic { //Used for location based movements

		T x, y;

		//Constructors
		Point_generic() {
			x = 0; y = 0;
		}

		Point_generic(T X, T Y) {
			x = X; y = Y;
		}

		//Operator Overloads
		inline Point_generic<T> * operator + (Point_generic<T> const &p) { return Point_generic<T>(this->x + p.x, this->y + p.y); }
		inline Point_generic<T> * operator + (Vector2_generic<T> const &vec) { return Point_generic<T>(this->x + vec.x, this->y + vec.y); } //used for Points that move
		inline Point_generic<T> * operator - (Point_generic<T> const &p) { return Point_generic<T>(this->x - p.x, this->y - p.y); }
		inline Point_generic<T> * operator * (Point_generic<T> const &p) { return Point_generic<T>(this->x * p.x, this->y * p.y); }
		inline Point_generic<T> * operator / (Point_generic<T> const &p) { return Point_generic<T>(this->x / p.x, this->y / p.y); }

		inline Point_generic<T> * operator += (Point_generic<T> const &p) { return Point_generic<T>(this->x + p.x, this->y + p.y); }
		inline Point_generic<T> * operator += (Vector2_generic<T> const &vec) { return Point_generic<T>(this->x + vec.x, this->y + vec.y); } //used for Points that move
		inline Point_generic<T> * operator -= (Point_generic<T> const &p) { return Point_generic<T>(this->x - p.x, this->y - p.y); }
		inline Point_generic<T> * operator *= (Point_generic<T> const &p) { return Point_generic<T>(this->x * p.x, this->y * p.y); }
		inline Point_generic<T> * operator /= (Point_generic<T> const &p) { return Point_generic<T>(this->x / p.x, this->y / p.y); }

		//Public Memeber Functions
		static float GetSlope(const Point_generic<T> p1, const Point_generic<T> p2) {
			return (float)((p2.y - p1.y) / (p2.x - p1.x)); 
		}
	};

	class LineSegment
	{
	public:
		pnt_F start;
		pnt_F end;

		//Member functions
		static bool IsIntersecting(const LineSegment& seg1, const LineSegment& seg2)
		{
			//This intersection method find the percent/fraction distance the intersection is away from the start point of each line.
			//If both percents are elements of [0,1], then it guarantees they intersect. Using this to avoid doing anything too specific.
			//For an explanation of the formulas, see this link: http://www.cs.swan.ac.uk/~cssimon/line_intersection.html
			float d1, d2; //ta and tb from the link above

			pnt_F p1 = seg1.start, p2 = seg1.end, p3 = seg2.start, p4 = seg2.end;
			float denominator = ((p4.x - p3.x)*(p1.y - p2.y) - (p1.x - p2.x)*(p4.y - p3.y)); //Both equations share a common denominator, so let's just calculate it once.

			d1 = ((p3.y - p4.y)*(p1.x - p3.x) + (p4.x - p3.x)*(p1.y - p3.y)) / denominator;
			d2 = ((p1.y - p2.y)*(p1.x - p3.x) + (p2.x - p1.x)*(p1.y - p3.y)) / denominator;

			return 0 <= d1 && d1 <= 1 && 0 <= d2 && d2 <= 1;
		}

		//Constructors
		LineSegment() {}
		LineSegment(const pnt_F p1, const pnt_F p2) { start = p1; end = p2; }

	};

	class Line {
		public:
			float slope;
			float yInt;

			//Public member functions
			static bool isIntersection(const Line line1, const Line line2) { return line1.slope != line2.slope || line1.yInt == line2.yInt; }

			static pnt_F FindIntersection(const Line line1, const Line line2) {
				pnt_F intersection; 
				try{
					intersection.x = (line2.yInt - line1.yInt) / (line1.slope - line2.slope);
					intersection.y = line1.slope*intersection.x + line1.yInt;
				}

				catch (...) { std::cout << "No intersection detected"; } //Should use checkIntersection function to make sure this catch isn't needed}

				return intersection;
			}

			//Returns a projection of a 2D shape and condenses it into a LineSegment. This will projected onto the calling Line.
			//Note that a projection of a 2D object is 1D, so this will be a finite line.
			LineSegment TakeProjection(const Shape shape)
			{
				//DEBUG If we update the LineSegment class to be only s

				//Project each point onto this line
				pnt_F* projectedPoints = new pnt_F[shape.vertexCount];
				float tempX;
				float slope = this->slope; //Let's get this now so we don't have to keep following the pointer

				for (int i = 0; i < shape.vertexCount; i++)
				{
					tempX = (shape.vertices[i].x + slope * shape.vertices[i].y - slope * this->yInt) / (slope*slope + 1); //Find the x coordinate of the projection
					projectedPoints[i] = *(new pnt_F(tempX, slope*tempX + this->yInt)); //Construct and dereference a point for this projected point
				}

				//All points should now be projected. For convenience, let's condense them into a single line segment
				//There may be a more eloquent way to check for vertical lines. Update if we find something.
				if (slope == INFINITY) //If we have a vertical line. Projected points fall on the calling line, so it should share the same slope and yInt
				{
					//Find the highest and lowest points of this vertical line (i.e. the end points)
					float minY = projectedPoints[0].y, maxY = projectedPoints[0].y;
					pnt_F minPoint = projectedPoints[0], maxPoint = projectedPoints[0];

					for (int i = 1; i < shape.vertexCount; i++)
					{
						if (projectedPoints[i].y > maxY)
						{
							maxY = projectedPoints[i].y;
							maxPoint = projectedPoints[i];
						}

						else if (projectedPoints[i].y < minY)
						{
							minY = projectedPoints[i].y;
							minPoint = projectedPoints[i];
						}
					}

					//Construct and dereference a LineSegment with an undefined slope with the end points we found
					return *(new LineSegment(minPoint, maxPoint)); 
				}

				else //If this is not a vertical line, then let's find the point furthest to the left and right and find the slope
				{
					float minX = projectedPoints[0].x, maxX = projectedPoints[0].x;
					pnt_F minPoint = projectedPoints[0], maxPoint = projectedPoints[0];

					for (int i = 1; i < shape.vertexCount; i++)
					{
						if (projectedPoints[i].x > maxX)
						{
							maxX = projectedPoints[i].x;
							maxPoint = projectedPoints[i];
						}

						else if (projectedPoints[i].x < minX)
						{
							minX = projectedPoints[i].x;
							minPoint = projectedPoints[i];
						}
					}

					return *(new LineSegment(minPoint, maxPoint));
				}


			}

			//Constructors
			Line() { this->slope = 0; this->yInt = 0; } //Default constructor
			Line(const pnt_F p1, const pnt_F p2) { this->slope = pnt_F::GetSlope(p1, p2); this->yInt = p1.y - this->slope*p1.x; } //If we get two points
			Line(const float slope, const float yInt) { this->slope = slope; this->yInt = yInt; } //If we get a slope and yInt
			~Line() {}
	};

	class Shape {

	public:
		pnt_F* vertices; //Had to change this to a pointer instead of an array because VS was failing to recognize it as a member.
		int vertexCount; //May need to keep track of this if size() function fails to work

		Shape() {
		}
		Shape(int vertexCount) { this->vertexCount = vertexCount; vertices = new pnt_F[vertexCount]; }
		~Shape() {
			delete[] vertices;
		}

		//Checks collision between sh1 and sh2. Does so by checking against one axis at a time; this way, we don't have to compute extra axes
		//in the event that there is no collision.
		static bool CheckCollision(const Shape sh1, const Shape sh2) {
			Line* axis; //Current axis being used
			LineSegment projection1, projection2; //Used to store 2 projections so we can check if they overlap later
			pnt_F secondPoint; //Used for getting a second point for each edge of each shape
			float tempSlope; //Used to temporarily store the perpendicular slope to make things easier.

			for (int i = 0; i < sh1.vertexCount; i++) //Check collision using the first shape
			{
				//Get the point adjacent to this one to come up with the axis
				if (i != sh1.vertexCount - 1)
					secondPoint = sh1.vertices[i + 1];
				else
					secondPoint = sh1.vertices[0];

				tempSlope = -1.0 / pnt_F::GetSlope(sh1.vertices[i], secondPoint);
				axis = new Line(tempSlope, sh1.vertices[i].y - tempSlope * sh1.vertices[i].x); //Construct a perpendicular line with the given slope and yInt

				//Now that we have an axis to project onto, let's project both shapes
				projection1 = axis->TakeProjection(sh1); // Project shape1 onto the axis
				projection2 = axis->TakeProjection(sh2); //Project shape2 onto the axis

				//Check intersection of the two LineSegment projections
				//DEBUG, finish writing the functions that do this
				if (!LineSegment::IsIntersecting(projection1, projection2)) //If projected lines don't intersect, these shapes aren't colliding
					return false;
			}

			//Check collision for the second shape
			for (int i = 0; i < sh2.vertexCount; i++) //Check collision using the first shape
			{
				//Get the point adjacent to this one to come up with the axis
				if (i != sh2.vertexCount - 1)
					secondPoint = sh2.vertices[i + 1];
				else
					secondPoint = sh2.vertices[0];

				tempSlope = -1.0 / pnt_F::GetSlope(sh2.vertices[i], secondPoint);
				axis = new Line(tempSlope, sh2.vertices[i].y - tempSlope * sh2.vertices[i].x); //Construct a perpendicular line with the given slope and yInt

				//Now that we have an axis to project onto, let's project both shapes
				projection1 = axis->TakeProjection(sh1); // Project shape1 onto the axis
				projection2 = axis->TakeProjection(sh2); //Project shape2 onto the axis

				//Check intersection of the two LineSegment projections
				if (!LineSegment::IsIntersecting(projection1, projection2)) //If projected lines don't intersect, these shapes aren't colliding
					return false;
			}

			return true;
		}
	};

	struct InputScheme { //Control scheme for key bindings.
		olc::HWButton Jump, Left, Right, Up, Down, Start;

		bool RebindKey(olc::HWButton input, olc::HWButton key) {
			//check to see if key is already used, if true (either unbind key, or dont allow binding of new key)
			//return true if key was successfully rebound
			return true;
		}
	};

	//Mostly how the Unity AnimatorController works
	struct Animation {
	public:
		olc::Sprite* Sprites;
		int Frames, CurrentFrame;
		bool Looping, ExitWhenDone; //ExitWhenDone is basically the same as "Has Exit Time" in Unity. When the AnimatorController wants 
		float FrameDuration, CurrentTime;

		olc::Sprite GetSprite(float DeltaTime) {
			CurrentTime += DeltaTime;
			float OverFlow = 0.0f;
			if (CurrentTime > FrameDuration) { 
				OverFlow = CurrentTime - FrameDuration;
				CurrentFrame = (CurrentFrame + 1 > Frames) ? 0 : CurrentFrame + 1;
				CurrentTime = OverFlow;
			}

			return this->Sprites[CurrentFrame];
		}

		Animation() {}
		Animation(olc::Sprite Sprites, int Frames) { this->Sprites = new olc::Sprite[Frames]; this->Frames = Frames; this->Sprites = &Sprites; this->CurrentFrame = 0; };
	};

	class AnimationController {
	public:
		static enum AnimationType {HUMANOID, OBJECT}; //Humanoids have walking/jumping/attacking/death animations. Objects just have their idle/interaction/etc. animations
		AnimationType animationType;
		Animation* Animations;
		Animation* CurrentAnimation;

		AnimationController() {}
		AnimationController(Animation* Animations, AnimationType animationType) { this->Animations = Animations; this->animationType = animationType; }
	};
	
	//TODO: create system for using screen dimensions to clip sprites that are outside the viewing box
}
