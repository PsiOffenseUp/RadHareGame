#include "RadRabbitEngine.h"

class Game : public olc::PixelGameEngine {
public:
	bool OnUserCreate() override {
		sAppName = "Rad Hare Game";
		return true;
	}

	bool OnUserUpdate(float fElapsedTime) override {

		pnt_F * p1 = new pnt_F(5.0f, 0.0f);

		std::cout << p1->x << std::endl;

		return true;
	}
};

int main()
{
	Game game;

	if (game.Construct(800, 600, 1, 1)) {
		game.Start();
	}
}
