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

	template <class T>
	struct Vector2_generic { //Used for movement/ vector based calculations

		T x, y;

		//Constructors
		Vector2_generic() {
			x = 0; y = 0;
		}

		Vector2_generic(T X, T Y) {
			x = X; y = Y;
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

	class Shape {

		pnt_F vertices[];

		Shape() {

		}
		~Shape() {
			delete[] vertices;
		}

		static bool ChackCollision(const Shape sh1, const Shape sh2) {
			//magic
			return false;
		}
	};
}
